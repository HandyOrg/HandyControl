using System;
using HandyControl.Controls;
using HandyControl.Tools;

namespace HandyControl.Data
{
    public class GifImageInfo
    {
        private const int PropertyTagFrameDelay = 0x5100;

        private int _frame;

        private readonly int[] _frameDelay;

        public int FrameCount { get; }

        internal GifImage Image { get; }

        public bool Animated { get; }

        public EventHandler FrameChangedHandler { get; set; }

        internal int FrameTimer { get; set; }

        public bool FrameDirty { get; private set; }

        public int Frame
        {
            get => _frame;
            set
            {
                if (_frame != value)
                {
                    if (value < 0 || value >= FrameCount)
                    {
                        throw new ArgumentException("InvalidFrame");
                    }

                    if (Animated)
                    {
                        _frame = value;
                        FrameDirty = true;

                        OnFrameChanged(EventArgs.Empty);
                    }
                }
            }
        }

        public GifImageInfo(GifImage image)
        {
            Image = image;
            Animated = ImageAnimator.CanAnimate(image);

            if (Animated)
            {
                FrameCount = image.GetFrameCount(GifFrameDimension.Time);

                var frameDelayItem = image.GetPropertyItem(PropertyTagFrameDelay);

                // If the image does not have a frame delay, we just return 0.                                     
                //
                if (frameDelayItem != null)
                {
                    // Convert the frame delay from byte[] to int
                    //
                    var values = frameDelayItem.Value;
                    _frameDelay = new int[FrameCount];
                    for (var i = 0; i < FrameCount; ++i)
                    {
                        _frameDelay[i] = values[i * 4] + 256 * values[i * 4 + 1] + 256 * 256 * values[i * 4 + 2] + 256 * 256 * 256 * values[i * 4 + 3];
                    }
                }
            }
            else
            {
                FrameCount = 1;
            }

            _frameDelay ??= new int[FrameCount];
        }

        public int FrameDelay(int frame) => _frameDelay[frame];

        internal void UpdateFrame()
        {
            if (FrameDirty)
            {
                Image.SelectActiveFrame(GifFrameDimension.Time, Frame);
                FrameDirty = false;
            }
        }

        protected void OnFrameChanged(EventArgs e) => FrameChangedHandler?.Invoke(Image, e);
    }
}