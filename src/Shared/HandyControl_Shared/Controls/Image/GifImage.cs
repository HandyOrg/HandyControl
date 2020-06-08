using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using HandyControl.Data;
using HandyControl.Tools;
using HandyControl.Tools.Interop;

namespace HandyControl.Controls
{
    public class GifImage : Image, IDisposable
    {
        private static readonly Guid GifGuid = new Guid("{b96b3caa-0728-11d3-9d7b-0000f81ef32e}");

        internal IntPtr NativeImage;

        private byte[] _rawData;

        private bool _isStart;

        private bool _isLoaded;

        static GifImage()
        {
            VisibilityProperty.OverrideMetadata(typeof(GifImage), new PropertyMetadata(default(Visibility), OnVisibilityChanged));
        }

        public static readonly DependencyProperty UriProperty = DependencyProperty.Register(
            "Uri", typeof(Uri), typeof(GifImage), new PropertyMetadata(default(Uri), OnUriChanged));

        private static void OnUriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (GifImage) d;
            ctl.StopAnimate();

            if (e.NewValue != null)
            {
                var v = (Uri)e.NewValue;
                ctl.GetGifStreamFromPack(v);
                ctl.StartAnimate();
            }
            else
            {
                ctl.Source = null;
            }
        }

        public Uri Uri
        {
            get => (Uri) GetValue(UriProperty);
            set => SetValue(UriProperty, value);
        }

        private static void OnVisibilityChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (GifImage)s;
            var v = (Visibility)e.NewValue;
            if (v != Visibility.Visible)
            {
                ctl.StopAnimate();
            }
            else if (!ctl._isStart)
            {
                ctl.StartAnimate();
            }
        }

        ~GifImage() => Dispose(false);

        public GifImage()
        {
            Loaded += (s, e) =>
            {
                if (DesignerProperties.GetIsInDesignMode(this)) return;
                if (_isLoaded) return;
                _isLoaded = true;
                if (Uri != null)
                {
                    GetGifStreamFromPack(Uri);
                    StartAnimate();
                }
                else if (Source is BitmapImage image)
                {
                    GetGifStreamFromPack(image.UriSource);
                    StartAnimate();
                }
            };

            Unloaded += (s, e) => Dispose();
        }

        public GifImage(string filename)
        {
            Source = null;
            CreateSourceFromFile(filename);
            StartAnimate();
        }

        public GifImage(Stream stream)
        {
            Source = null;
            CreateSourceFromStream(stream);
            StartAnimate();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (NativeImage == IntPtr.Zero) return;
            try
            {
                StopAnimate();
                InteropMethods.Gdip.GdipDisposeImage(new HandleRef(this, NativeImage));
            }
            catch
            {
                // ignored
            }
            finally
            {
                NativeImage = IntPtr.Zero;
            }
        }

        private void CreateSourceFromFile(string filename)
        {
            filename = Path.GetFullPath(filename);

            var status = InteropMethods.Gdip.GdipCreateBitmapFromFile(filename, out var bitmap);

            if (status != InteropMethods.Gdip.Ok)
                throw InteropMethods.Gdip.StatusException(status);

            status = InteropMethods.Gdip.GdipImageForceValidation(new HandleRef(null, bitmap));

            if (status != InteropMethods.Gdip.Ok)
            {
                InteropMethods.Gdip.GdipDisposeImage(new HandleRef(null, bitmap));
                throw InteropMethods.Gdip.StatusException(status);
            }

            SetNativeImage(bitmap);

            EnsureSave(this, filename, null);
        }

        private void CreateSourceFromStream(Stream stream)
        {
            if (stream == null)
                throw new ArgumentException("stream null");

            var status = InteropMethods.Gdip.GdipCreateBitmapFromStream(new GPStream(stream), out var bitmap);

            if (status != InteropMethods.Gdip.Ok)
                throw InteropMethods.Gdip.StatusException(status);

            status = InteropMethods.Gdip.GdipImageForceValidation(new HandleRef(null, bitmap));

            if (status != InteropMethods.Gdip.Ok)
            {
                InteropMethods.Gdip.GdipDisposeImage(new HandleRef(null, bitmap));
                throw InteropMethods.Gdip.StatusException(status);
            }

            SetNativeImage(bitmap);

            EnsureSave(this, null, stream);
        }

        private void GetGifStreamFromPack(Uri uri)
        {
            try
            {
                StreamResourceInfo streamInfo;

                if (!uri.IsAbsoluteUri)
                {
                    streamInfo = Application.GetContentStream(uri) ?? Application.GetResourceStream(uri);
                }
                else
                {
                    if (uri.GetLeftPart(UriPartial.Authority).Contains("siteoforigin"))
                    {
                        streamInfo = Application.GetRemoteStream(uri);
                    }
                    else
                    {
                        streamInfo = Application.GetContentStream(uri) ?? Application.GetResourceStream(uri);
                    }
                }
                if (streamInfo == null)
                    throw new FileNotFoundException("Resource not found.", uri.ToString());
                CreateSourceFromStream(streamInfo.Stream);
            }
            catch
            {
                // ignored
            }
        }

        [ResourceExposure(ResourceScope.None)]
        [ResourceConsumption(ResourceScope.Machine, ResourceScope.Machine)]
        internal static void EnsureSave(GifImage image, string filename, Stream dataStream)
        {
            if (!image.RawGuid.Equals(GifGuid)) return;
            var animatedGif = false;

            var dimensions = image.FrameDimensionsList;
            if (dimensions.Select(guid => new GifFrameDimension(guid)).Contains(GifFrameDimension.Time))
            {
                animatedGif = image.GetFrameCount(GifFrameDimension.Time) > 1;
            }

            if (!animatedGif) return;

            try
            {
                Stream created = null;
                long lastPos = 0;
                if (dataStream != null)
                {
                    lastPos = dataStream.Position;
                    dataStream.Position = 0;
                }

                try
                {
                    if (dataStream == null)
                    {
                        created = dataStream = File.OpenRead(filename);
                    }

                    image._rawData = new byte[(int)dataStream.Length];
                    dataStream.Read(image._rawData, 0, (int)dataStream.Length);
                }
                finally
                {
                    if (created != null)
                    {
                        created.Close();
                    }
                    else
                    {
                        // ReSharper disable once PossibleNullReferenceException
                        dataStream.Position = lastPos;
                    }
                }
            }
            // possible exceptions for reading the filename
            catch (UnauthorizedAccessException)
            {
            }
            catch (DirectoryNotFoundException)
            {
            }
            catch (IOException)
            {
            }
            // possible exceptions for setting/getting the position inside dataStream
            catch (NotSupportedException)
            {
            }
            catch (ObjectDisposedException)
            {
            }
            // possible exception when reading stuff into dataStream
            catch (ArgumentException)
            {
            }
        }

        internal void SetNativeImage(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
                throw new ArgumentException("NativeHandle0");

            NativeImage = handle;
        }

        internal Guid RawGuid
        {
            get
            {
                var guid = new Guid();

                var status = InteropMethods.Gdip.GdipGetImageRawFormat(new HandleRef(this, NativeImage), ref guid);

                if (status != InteropMethods.Gdip.Ok)
                    throw InteropMethods.Gdip.StatusException(status);

                return guid;
            }
        }

        internal void StartAnimate()
        {
            ImageAnimator.Animate(this, OnFrameChanged);
            _isStart = true;
        }

        internal void StopAnimate()
        {
            ImageAnimator.StopAnimate(this, OnFrameChanged);
            _isStart = false;
        }

        internal Guid[] FrameDimensionsList
        {
            get
            {
                var status = InteropMethods.Gdip.GdipImageGetFrameDimensionsCount(new HandleRef(this, NativeImage), out var count);

                if (status != InteropMethods.Gdip.Ok)
                {
                    throw InteropMethods.Gdip.StatusException(status);
                }

                if (count <= 0)
                {
                    return new Guid[0];
                }

                var size = Marshal.SizeOf(typeof(Guid));

                var buffer = Marshal.AllocHGlobal(checked(size * count));
                if (buffer == IntPtr.Zero)
                {
                    throw InteropMethods.Gdip.StatusException(InteropMethods.Gdip.OutOfMemory);
                }

                status = InteropMethods.Gdip.GdipImageGetFrameDimensionsList(new HandleRef(this, NativeImage), buffer, count);

                if (status != InteropMethods.Gdip.Ok)
                {
                    Marshal.FreeHGlobal(buffer);
                    throw InteropMethods.Gdip.StatusException(status);
                }

                var guids = new Guid[count];

                try
                {
                    for (var i = 0; i < count; i++)
                    {
                        guids[i] = (Guid)InteropMethods.PtrToStructure((IntPtr)((long)buffer + size * i), typeof(Guid));
                    }
                }
                finally
                {
                    Marshal.FreeHGlobal(buffer);
                }

                return guids;
            }
        }

        internal int GetFrameCount(GifFrameDimension dimension)
        {
            var count = new[] { 0 };

            var dimensionId = dimension.Guid;
            var status = InteropMethods.Gdip.GdipImageGetFrameCount(new HandleRef(this, NativeImage), ref dimensionId, count);

            if (status != InteropMethods.Gdip.Ok)
                throw InteropMethods.Gdip.StatusException(status);

            return count[0];
        }

        internal GifPropertyItem GetPropertyItem(int propid)
        {
            GifPropertyItem propitem;

            var status = InteropMethods.Gdip.GdipGetPropertyItemSize(new HandleRef(this, NativeImage), propid, out var size);

            if (status != InteropMethods.Gdip.Ok)
                throw InteropMethods.Gdip.StatusException(status);

            if (size == 0)
                return null;

            var propdata = Marshal.AllocHGlobal(size);

            if (propdata == IntPtr.Zero)
                throw InteropMethods.Gdip.StatusException(InteropMethods.Gdip.OutOfMemory);

            status = InteropMethods.Gdip.GdipGetPropertyItem(new HandleRef(this, NativeImage), propid, size, propdata);

            try
            {
                if (status != InteropMethods.Gdip.Ok)
                {
                    throw InteropMethods.Gdip.StatusException(status);
                }

                propitem = GifPropertyItemInternal.ConvertFromMemory(propdata, 1)[0];
            }
            finally
            {
                Marshal.FreeHGlobal(propdata);
            }

            return propitem;
        }

        internal int SelectActiveFrame(GifFrameDimension dimension, int frameIndex)
        {
            var count = new[] { 0 };

            var dimensionId = dimension.Guid;
            var status = InteropMethods.Gdip.GdipImageSelectActiveFrame(new HandleRef(this, NativeImage), ref dimensionId, frameIndex);

            if (status != InteropMethods.Gdip.Ok)
                throw InteropMethods.Gdip.StatusException(status);

            return count[0];
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [ResourceExposure(ResourceScope.Machine)]
        [ResourceConsumption(ResourceScope.Machine)]
        internal IntPtr GetHbitmap() => GetHbitmap(Colors.LightGray);

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [ResourceExposure(ResourceScope.Machine)]
        [ResourceConsumption(ResourceScope.Machine)]
        internal IntPtr GetHbitmap(Color background)
        {
            var status = InteropMethods.Gdip.GdipCreateHBITMAPFromBitmap(new HandleRef(this, NativeImage), out var hBitmap,
                ColorHelper.ToWin32(background));
            if (status == 2 && (Width >= short.MaxValue || Height >= short.MaxValue))
            {
                throw new ArgumentException("GdiplusInvalidSize");
            }

            if (status != InteropMethods.Gdip.Ok && NativeImage != IntPtr.Zero)
            {
                throw InteropMethods.Gdip.StatusException(status);
            }

            return hBitmap;
        }

        private void GetBitmapSource()
        {
            var handle = IntPtr.Zero;

            try
            {
                handle = GetHbitmap();
                Source = Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
            catch
            {
                // ignored
            }
            finally
            {
                if (handle != IntPtr.Zero)
                {
                    InteropMethods.DeleteObject(handle);
                }
            }
        }

        private void OnFrameChanged(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                ImageAnimator.UpdateFrames();
                Source?.Freeze();
                GetBitmapSource();
                InvalidateVisual();
            }));
        }
    }
}