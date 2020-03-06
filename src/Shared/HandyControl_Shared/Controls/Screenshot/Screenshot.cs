using System;
using System.Windows.Media;

namespace HandyControl.Controls
{
    public class Screenshot
    {
        public event EventHandler Snapped;

        public ImageSource Source { get; set; }

        public void Start() => new ScreenshotWindow(this).Show();

        internal void OnSnapped() => Snapped?.Invoke(this, EventArgs.Empty);
    }
}
