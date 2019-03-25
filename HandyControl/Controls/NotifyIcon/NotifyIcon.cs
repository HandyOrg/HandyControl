using System.Windows;
using System.Windows.Media;
using HandyControl.Data;
using HandyControl.Tools;
using HandyControl.Tools.Interop;

namespace HandyControl.Controls
{
    public class NotifyIcon : FrameworkElement
    {
        private bool _added;

        private readonly object _syncObj = new object();

        private WindowClass _windowClass;

        private readonly int _id;

        private static int NextId;

        private ImageSource _icon;

        private IconHandle _defaultLargeIconHandle;

        private IconHandle _defaultSmallIconHandle;

        private IconHandle _currentLargeIconHandle;

        private IconHandle _currentSmallIconHandle;

        private const int WmTrayMouseMessage = NativeMethods.WM_USER + 1024;

        public NotifyIcon()
        {
            _id = ++NextId;
            _windowClass = new WindowClass();
        }

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            "Icon", typeof(ImageSource), typeof(NotifyIcon), new PropertyMetadata(default(ImageSource), OnIconChanged));

        private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (NotifyIcon) d;
            ctl._icon = (ImageSource) e.NewValue;
            ctl.UpdateIcon();
        }

        public ImageSource Icon
        {
            get => (ImageSource) GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        private void UpdateIcon()
        {
            IconHandle largeIconHandle;
            IconHandle smallIconHandle;

            if (_icon != null)
            {
                IconHelper.GetIconHandlesFromImageSource(_icon, out largeIconHandle, out smallIconHandle);
            }
            else
            {
                if (_defaultLargeIconHandle == null && _defaultSmallIconHandle == null)
                {
                    IconHelper.GetDefaultIconHandles(out largeIconHandle, out smallIconHandle);
                    _defaultLargeIconHandle = largeIconHandle;
                    _defaultSmallIconHandle = smallIconHandle;
                }
                else
                {
                    largeIconHandle = _defaultLargeIconHandle;
                    smallIconHandle = _defaultSmallIconHandle;
                }
            }

            if (_currentLargeIconHandle != null && _currentLargeIconHandle != _defaultLargeIconHandle)
            {
                _currentLargeIconHandle.Dispose();
            }

            if (_currentSmallIconHandle != null && _currentSmallIconHandle != _defaultSmallIconHandle)
            {
                _currentSmallIconHandle.Dispose();
            }

            _currentLargeIconHandle = largeIconHandle;
            _currentSmallIconHandle = smallIconHandle;
        }

        public static readonly DependencyProperty BalloonTipIconProperty = DependencyProperty.Register(
            "BalloonTipIcon", typeof(ToolTipIcon), typeof(NotifyIcon), new PropertyMetadata(default(ToolTipIcon)));

        public ToolTipIcon BalloonTipIcon
        {
            get => (ToolTipIcon) GetValue(BalloonTipIconProperty);
            set => SetValue(BalloonTipIconProperty, value);
        }

        public static readonly DependencyProperty BalloonTipTitleProperty = DependencyProperty.Register(
            "BalloonTipTitle", typeof(string), typeof(NotifyIcon), new PropertyMetadata(default(string)));

        public string BalloonTipTitle
        {
            get => (string) GetValue(BalloonTipTitleProperty);
            set => SetValue(BalloonTipTitleProperty, value);
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(NotifyIcon), new PropertyMetadata(default(string)));

        public string Text
        {
            get => (string) GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public void ShowBalloonTip(int timeout, string tipTitle, string tipText, ToolTipIcon tipIcon)
        {
            if (timeout < 0) return;
            if (string.IsNullOrEmpty(tipTitle)) return;

            if (_added)
            {
                if (DesignerHelper.IsInDesignMode) return;

                IntSecurity.UnrestrictedWindows.Demand();

                var data = new NOTIFYICONDATA
                {
                    hWnd = _windowClass.MessageWindowHandle,
                    uID = _id,
                    uFlags = NativeMethods.NIF_INFO,
                    uTimeoutOrVersion = timeout,
                    szInfoTitle = tipTitle,
                    szInfo = tipText
                };

                switch (tipIcon)
                {
                    case ToolTipIcon.Info: data.dwInfoFlags = NativeMethods.NIIF_INFO; break;
                    case ToolTipIcon.Warning: data.dwInfoFlags = NativeMethods.NIIF_WARNING; break;
                    case ToolTipIcon.Error: data.dwInfoFlags = NativeMethods.NIIF_ERROR; break;
                    case ToolTipIcon.None: data.dwInfoFlags = NativeMethods.NIIF_NONE; break;
                }
                UnsafeNativeMethods.Shell_NotifyIcon(NativeMethods.NIM_MODIFY, data);
            }
        }
    }
}