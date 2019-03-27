using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
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

        private Popup _contextMenu;

        private bool _doubleClick;

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
            var ctl = (NotifyIcon)d;
            ctl._icon = (ImageSource)e.NewValue;
            ctl.OnIconChanged();
        }

        public ImageSource Icon
        {
            get => (ImageSource)GetValue(IconProperty);
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
            if (IsLoaded)
            {
                if (msg == WmTaskbarcreated)
                {
                    UpdateIcon(true);
                }
                else
                {
                    //switch (lparam.ToInt32())
                    //{
                    //    case NativeMethods.WM_LBUTTONDBLCLK:
                    //        WmMouseDown(MouseButton.Left, 2);
                    //        break;
                    //    case NativeMethods.WM_LBUTTONDOWN:
                    //        WmMouseDown(MouseButton.Left, 1);
                    //        break;
                    //    case NativeMethods.WM_LBUTTONUP:
                    //        WmMouseUp(MouseButton.Left);
                    //        break;
                    //    case NativeMethods.WM_MBUTTONDBLCLK:
                    //        WmMouseDown(MouseButton.Middle, 2);
                    //        break;
                    //    case NativeMethods.WM_MBUTTONDOWN:
                    //        WmMouseDown(MouseButton.Middle, 1);
                    //        break;
                    //    case NativeMethods.WM_MBUTTONUP:
                    //        WmMouseUp(MouseButton.Middle);
                    //        break;
                    //    case NativeMethods.WM_MOUSEMOVE:
                    //        WmMouseMove();
                    //        break;
                    //    case NativeMethods.WM_RBUTTONDBLCLK:
                    //        WmMouseDown(MouseButton.Right, 2);
                    //        break;
                    //    case NativeMethods.WM_RBUTTONDOWN:
                    //        WmMouseDown(MouseButton.Right, 1);
                    //        break;
                    //    case NativeMethods.WM_RBUTTONUP:
                    //        ShowContextMenu();
                    //        WmMouseUp(MouseButton.Right);
                    //        break;
                    //}
                }

                var point = new POINT();
                UnsafeNativeMethods.GetCursorPos(point);
                ShowToolTip(point);
            }

            return UnsafeNativeMethods.DefWindowProc(hWnd, msg, wparam, lparam);
        }

        private void WmMouseDown(MouseButton button, int clicks)
        {
            if (clicks == 2)
            {
                OnMouseDoubleClick(new MouseButtonEventArgs(Mouse.PrimaryDevice, Environment.TickCount, button)
                {
                    Source = this,
                    RoutedEvent = MouseDoubleClickEvent
                });
                _doubleClick = true;
            }
            OnMouseDown(new MouseButtonEventArgs(Mouse.PrimaryDevice, Environment.TickCount, button)
            {
                Source = this,
                RoutedEvent = MouseDownEvent
            });
        }

        private void WmMouseUp(MouseButton button)
        {
            OnMouseUp(new MouseButtonEventArgs(Mouse.PrimaryDevice, Environment.TickCount, button)
            {
                Source = this,
                RoutedEvent = MouseUpEvent
            });

            if (!_doubleClick)
            {
                OnClick(new MouseButtonEventArgs(Mouse.PrimaryDevice, Environment.TickCount, button)
                {
                    Source = this,
                    RoutedEvent = ClickEvent
                });
            }
            _doubleClick = false;
        }

        private void WmMouseMove()
        {
            OnMouseMove(new MouseEventArgs(Mouse.PrimaryDevice, Environment.TickCount)
            {
                RoutedEvent = MouseMoveEvent,
                Source = this,
            });
        }

        private void ShowContextMenu()
        {
            if (ContextMenu != null) ContextMenu.IsOpen = true;
        }

        private void ShowToolTip(POINT point)
        {
            if (_toolTip != null)
            {
                _toolTip.HorizontalOffset = point.x;
                _toolTip.VerticalOffset = point.x;
                _toolTip.IsOpen = true;
            }
        }

        private void CreateToolTip()
        {
            _toolTip = new ToolTip
            {
                Placement = PlacementMode.AbsolutePoint,
                HasDropShadow = false,
                BorderThickness = new Thickness(0),
                Background = Brushes.Transparent
            };

            _toolTip.SetBinding(ContentControl.ContentProperty, new Binding(ToolTipProperty.Name) { Source = this });
            _toolTip.SetBinding(DataContextProperty, new Binding(DataContextProperty.Name) { Source = this });
        }

        public static readonly RoutedEvent ClickEvent =
            EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(NotifyIcon));

        public event RoutedEventHandler Click
        {
            add => AddHandler(ClickEvent, value);
            remove => RemoveHandler(ClickEvent, value);
        }

        public static readonly RoutedEvent MouseDoubleClickEvent =
            EventManager.RegisterRoutedEvent("MouseDoubleClick", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(NotifyIcon));

        public event RoutedEventHandler MouseDoubleClick
        {
            add => AddHandler(MouseDoubleClickEvent, value);
            remove => RemoveHandler(MouseDoubleClickEvent, value);
        }

        public static readonly RoutedEvent BalloonTipShownEvent =
            EventManager.RegisterRoutedEvent("BalloonTipShown", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(NotifyIcon));

        public event RoutedEventHandler BalloonTipShown
        {
            add => AddHandler(BalloonTipShownEvent, value);
            remove => RemoveHandler(BalloonTipShownEvent, value);
        }

        public static readonly RoutedEvent BalloonTipClosedEvent =
            EventManager.RegisterRoutedEvent("BalloonTipClosed", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(NotifyIcon));

        public event RoutedEventHandler BalloonTipClosed
        {
            add => AddHandler(BalloonTipClosedEvent, value);
            remove => RemoveHandler(BalloonTipClosedEvent, value);
        }

        protected virtual void OnClick(MouseButtonEventArgs e) => RaiseEvent(e);

        protected virtual void OnDoubleClick(MouseButtonEventArgs e) => RaiseEvent(e);

        protected virtual void OnMouseDoubleClick(MouseButtonEventArgs e) => RaiseEvent(e);

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            RaiseEvent(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            RaiseEvent(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            RaiseEvent(e);
        }
    }
}