using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using HandyControl.Tools;

namespace HandyControl.Controls
{
    public class NotifyIcon : FrameworkElement
    {
        private Popup _trayPopup;

        private readonly object _syncObj = new object();

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            "Icon", typeof(ImageSource), typeof(NotifyIcon), new PropertyMetadata(default(ImageSource)));

        public ImageSource Icon
        {
            get => (ImageSource) GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        private void UpdateIcon(bool showIconInTray)
        {
            lock (_syncObj)
            {
                if (DesignerHelper.IsInDesignMode) return;
            }
        }
    }
}