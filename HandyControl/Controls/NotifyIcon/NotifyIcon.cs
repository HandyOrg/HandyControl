using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using HandyControl.Tools;
using HandyControl.Tools.Interop;

namespace HandyControl.Controls
{
    public class NotifyIcon : FrameworkElement
    {
        private bool _added;

        private readonly object _syncObj = new object();

        private readonly int _id;

        private static int NextId;

        private ImageSource _icon;

        private IconHandle _defaultLargeIconHandle;

        private IconHandle _defaultSmallIconHandle;

        private IconHandle _currentLargeIconHandle;

        private IconHandle _currentSmallIconHandle;

        private const int WmTrayMouseMessage = NativeMethods.WM_USER + 1024;

        private string _windowClassName;

        private static readonly int WmTaskbarcreated = NativeMethods.RegisterWindowMessage("TaskbarCreated");

        private IntPtr _messageWindowHandle;

        private readonly WndProc _callback;

        private ToolTip _toolTip;

        public NotifyIcon()
        {
            _id = ++NextId;
            _callback = Callback;
            CreateToolTip();
            RegisterClass();
            OnIconChanged();
            UpdateIcon(true);
        }

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            "Icon", typeof(ImageSource), typeof(NotifyIcon), new PropertyMetadata(default(ImageSource), OnIconChanged));

        private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (NotifyIcon) d;
            ctl._icon = (ImageSource) e.NewValue;
            ctl.OnIconChanged();
        }

        public ImageSource Icon
        {
            get => (ImageSource) GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        private void OnIconChanged()
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

        private void UpdateIcon(bool showIconInTray)
        {
            lock (_syncObj)
            {
                if (DesignerHelper.IsInDesignMode) return;

                var data = new NOTIFYICONDATA
                {
                    uCallbackMessage = WmTrayMouseMessage,
                    uFlags = NativeMethods.NIF_MESSAGE | NativeMethods.NIF_ICON | NativeMethods.NIF_TIP,
                    hWnd = _messageWindowHandle,
                    uID = _id,
                    dwInfoFlags = NativeMethods.NIIF_INFO,
                    hIcon = _currentSmallIconHandle.CriticalGetHandle()
                };

                if (showIconInTray)
                {
                    if (!_added)
                    {
                        UnsafeNativeMethods.Shell_NotifyIcon(NativeMethods.NIM_ADD, data);
                        _added = true;
                    }
                    else
                    {
                        UnsafeNativeMethods.Shell_NotifyIcon(NativeMethods.NIM_MODIFY, data);
                    }
                }
                else if (_added)
                {
                    UnsafeNativeMethods.Shell_NotifyIcon(NativeMethods.NIM_DELETE, data);
                    _added = false;
                }
            }
        }

        private void RegisterClass()
        {
            _windowClassName = $"HandyControl.Controls.NotifyIcon{Guid.NewGuid()}";
            var wndclass = new WNDCLASS
            {
                style = 0,
                lpfnWndProc = _callback,
                cbClsExtra = 0,
                cbWndExtra = 0,
                hInstance = IntPtr.Zero,
                hIcon = IntPtr.Zero,
                hCursor = IntPtr.Zero,
                hbrBackground = IntPtr.Zero,
                lpszMenuName = string.Empty,
                lpszClassName = _windowClassName
            };

            UnsafeNativeMethods.RegisterClass(wndclass);
            _messageWindowHandle = UnsafeNativeMethods.CreateWindowEx(0, _windowClassName, "", 0, 0, 0, 1, 1, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
        }

        private IntPtr Callback(IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam)
        {
            if (msg == WmTaskbarcreated)
            {
                UpdateIcon(true);
            }

            return UnsafeNativeMethods.DefWindowProc(hWnd, msg, wparam, lparam);
        }

        private void CreateToolTip()
        {
            _toolTip = new ToolTip
            {
                Placement = PlacementMode.Mouse,
                HasDropShadow = false,
                BorderThickness = new Thickness(0),
                Background = Brushes.Transparent
            };

            _toolTip.SetBinding(ContentControl.ContentProperty, new Binding(ToolTipProperty.Name) {Source = this});
            _toolTip.SetBinding(DataContextProperty, new Binding(DataContextProperty.Name) { Source = this });
        }
    }
}