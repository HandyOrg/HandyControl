using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using HandyControl.Controls;
using HandyControl.Data;

namespace HandyControl.Tools
{
    internal class ImageAnimator
    {
        private static List<GifImageInfo> ImageInfoList;

        private static readonly ReaderWriterLock RwImgListLock = new ReaderWriterLock();

        private static Thread AnimationThread;

        private static bool AnyFrameDirty;

        [ThreadStatic]
        private static int ThreadWriterLockWaitCount;

        public static bool CanAnimate(GifImage image)
        {
            if (image == null)
            {
                return false;
            }

            lock (image)
            {
                var dimensions = image.FrameDimensionsList;

                if (dimensions.Select(guid => new GifFrameDimension(guid)).Contains(GifFrameDimension.Time))
                {
                    return image.GetFrameCount(GifFrameDimension.Time) > 1;
                }
            }

            return false;
        }

        public static void Animate(GifImage image, EventHandler onFrameChangedHandler)
        {
            if (image == null)
            {
                return;
            }

            GifImageInfo imageInfo;

            // See comment in the class header about locking the image ref.
            lock (image)
            {
                // could we avoid creating an ImageInfo object if FrameCount == 1 ?
                imageInfo = new GifImageInfo(image);
            }

            // If the image is already animating, stop animating it
            StopAnimate(image, onFrameChangedHandler);

            // Acquire a writer lock to modify the image info list.  If the thread has a reader lock we need to upgrade
            // it to a writer lock; acquiring a reader lock in this case would block the thread on itself.  
            // If the thread already has a writer lock its ref count will be incremented w/o placing the request in the 
            // writer queue.  See ReaderWriterLock.AcquireWriterLock method in the MSDN.

            var readerLockHeld = RwImgListLock.IsReaderLockHeld;
            var lockDowngradeCookie = new LockCookie();

            ThreadWriterLockWaitCount++;

            try
            {
                if (readerLockHeld)
                {
                    lockDowngradeCookie = RwImgListLock.UpgradeToWriterLock(Timeout.Infinite);
                }
                else
                {
                    RwImgListLock.AcquireWriterLock(Timeout.Infinite);
                }
            }
            finally
            {
                ThreadWriterLockWaitCount--;
            }

            try
            {
                if (imageInfo.Animated)
                {
                    // Construct the image array
                    //                               
                    ImageInfoList ??= new List<GifImageInfo>();

                    // Add the new image
                    //
                    imageInfo.FrameChangedHandler = onFrameChangedHandler;
                    ImageInfoList.Add(imageInfo);

                    // Construct a new timer thread if we haven't already
                    //
                    if (AnimationThread == null)
                    {
                        AnimationThread = new Thread(AnimateImages50Ms)
                        {
                            Name = nameof(ImageAnimator),
                            IsBackground = true
                        };
                        AnimationThread.Start();
                    }
                }
            }
            finally
            {
                if (readerLockHeld)
                {
                    RwImgListLock.DowngradeFromWriterLock(ref lockDowngradeCookie);
                }
                else
                {
                    RwImgListLock.ReleaseWriterLock();
                }
            }
        }

        public static void StopAnimate(GifImage image, EventHandler onFrameChangedHandler)
        {
            // Make sure we have a list of images                       
            if (image == null || ImageInfoList == null)
            {
                return;
            }

            // Acquire a writer lock to modify the image info list - See comments on Animate() about this locking.

            var readerLockHeld = RwImgListLock.IsReaderLockHeld;
            var lockDowngradeCookie = new LockCookie();

            ThreadWriterLockWaitCount++;

            try
            {
                if (readerLockHeld)
                {
                    lockDowngradeCookie = RwImgListLock.UpgradeToWriterLock(Timeout.Infinite);
                }
                else
                {
                    RwImgListLock.AcquireWriterLock(Timeout.Infinite);
                }
            }
            finally
            {
                ThreadWriterLockWaitCount--;
            }

            try
            {
                // Find the corresponding reference and remove it
                for (var i = 0; i < ImageInfoList.Count; i++)
                {
                    var imageInfo = ImageInfoList[i];

                    if (Equals(image, imageInfo.Image))
                    {
                        if (onFrameChangedHandler == imageInfo.FrameChangedHandler || onFrameChangedHandler != null && onFrameChangedHandler.Equals(imageInfo.FrameChangedHandler))
                        {
                            ImageInfoList.Remove(imageInfo);
                        }
                        break;
                    }
                }

                if (!ImageInfoList.Any())
                {
                    AnimationThread = null;
                }
            }
            finally
            {
                if (readerLockHeld)
                {
                    RwImgListLock.DowngradeFromWriterLock(ref lockDowngradeCookie);
                }
                else
                {
                    RwImgListLock.ReleaseWriterLock();
                }
            }
        }

        public static void UpdateFrames()
        {
            if (!AnyFrameDirty || ImageInfoList == null)
            {
                return;
            }
            if (ThreadWriterLockWaitCount > 0)
            {
                // Cannot acquire reader lock at this time, frames update will be missed.
                return;
            }

            RwImgListLock.AcquireReaderLock(Timeout.Infinite);

            try
            {
                foreach (var imageInfo in ImageInfoList)
                {
                    // See comment in the class header about locking the image ref.
                    lock (imageInfo.Image)
                    {
                        imageInfo.UpdateFrame();
                    }
                }
                AnyFrameDirty = false;
            }
            finally
            {
                RwImgListLock.ReleaseReaderLock();
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals")]
        private static void AnimateImages50Ms()
        {
            while (ImageInfoList.Any())
            {
                // Acquire reader-lock to access imageInfoList, elemens in the list can be modified w/o needing a writer-lock.
                // Observe that we don't need to check if the thread is waiting or a writer lock here since the thread this 
                // method runs in never acquires a writer lock.
                RwImgListLock.AcquireReaderLock(Timeout.Infinite);
                try
                {
                    foreach (var imageInfo in ImageInfoList)
                    {
                        // Frame delay is measured in 1/100ths of a second. This thread
                        // sleeps for 50 ms = 5/100ths of a second between frame updates,
                        // so we increase the frame delay count 5/100ths of a second
                        // at a time.
                        //
                        imageInfo.FrameTimer += 5;
                        if (imageInfo.FrameTimer >= imageInfo.FrameDelay(imageInfo.Frame))
                        {
                            imageInfo.FrameTimer = 0;

                            if (imageInfo.Frame + 1 < imageInfo.FrameCount)
                            {
                                imageInfo.Frame++;
                            }
                            else
                            {
                                imageInfo.Frame = 0;
                            }

                            if (imageInfo.FrameDirty)
                            {
                                AnyFrameDirty = true;
                            }
                        }
                    }
                }
                finally
                {
                    RwImgListLock.ReleaseReaderLock();
                }

                Thread.Sleep(50);
            }
        }
    }
}