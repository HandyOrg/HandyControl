using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using HandyControl.Data;
using HandyControl.Tools;
using HandyControl.Tools.Interop;

namespace HandyControl.Controls
{
    public class NotifyIcon : FrameworkElement, IDisposable
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

        private int _wmTaskbarCreated;

        private IntPtr _messageWindowHandle;

        private readonly WndProc _callback;

        private Popup _contextContent;

        private bool _doubleClick;

        static NotifyIcon()
        {
            VisibilityProperty.OverrideMetadata(typeof(NotifyIcon), new PropertyMetadata(Visibility.Visible, OnVisibilityChanged));
        }

        private static void OnVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (NotifyIcon) d;
            var v = (Visibility)e.NewValue;

            if (v == Visibility.Visible)
            {
                if (ctl._currentSmallIconHandle == null)
                {
                    ctl.OnIconChanged();
                }
                ctl.UpdateIcon(true);
            }
            else if(ctl._currentSmallIconHandle !=null)
            {
                ctl.UpdateIcon(false);
            }
        }

        public NotifyIcon()
        {
            _id = ++NextId;
            _callback = Callback;

            Loaded += (s, e) =>
            {
                RegisterClass();
                if (Visibility == Visibility.Visible)
                {
                    OnIconChanged();
                    UpdateIcon(true);
                }
            };

            if (Application.Current != null) Application.Current.Exit += (s, e) => Dispose();
        }

        ~NotifyIcon()
        {
            Dispose(false);
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(NotifyIcon), new PropertyMetadata(default(string)));

        public string Text
        {
            get => (string) GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            "Icon", typeof(ImageSource), typeof(NotifyIcon), new PropertyMetadata(default(ImageSource), OnIconChanged));

        private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (NotifyIcon)d;
            ctl._icon = (ImageSource)e.NewValue;
            ctl.OnIconChanged();
        }

        public static readonly DependencyProperty ContextContentProperty = DependencyProperty.Register(
            "ContextContent", typeof(object), typeof(NotifyIcon), new PropertyMetadata(default(object)));

        public object ContextContent
        {
            get => GetValue(ContextContentProperty);
            set => SetValue(ContextContentProperty, value);
        }

        public static readonly DependencyProperty IsBlinkProperty = DependencyProperty.Register(
            "IsBlink", typeof(bool), typeof(NotifyIcon), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool IsBlink
        {
            get => (bool) GetValue(IsBlinkProperty);
            set => SetValue(IsBlinkProperty, value);
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
                    uTimeoutOrVersion = 10,
                    uID = _id,
                    dwInfoFlags = NativeMethods.NIF_TIP,
                    hIcon = _currentSmallIconHandle.CriticalGetHandle(),
                    szTip = Text
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
                lpszMenuName = "",
                lpszClassName = _windowClassName
            };

            UnsafeNativeMethods.RegisterClass(wndclass);
            _wmTaskbarCreated = NativeMethods.RegisterWindowMessage("TaskbarCreated");
            _messageWindowHandle = UnsafeNativeMethods.CreateWindowEx(0, _windowClassName, "", 0, 0, 0, 1, 1, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
        }

        private IntPtr Callback(IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam)
        {
            if (IsLoaded)
            {
                if (msg == _wmTaskbarCreated)
                {
                    UpdateIcon(true);
                }
                else
                {
                    switch (lparam.ToInt32())
                    {
                        case NativeMethods.WM_LBUTTONDBLCLK:
                            WmMouseDown(MouseButton.Left, 2);
                            break;
                        case NativeMethods.WM_LBUTTONDOWN:
                            WmMouseDown(MouseButton.Left, 1);
                            break;
                        case NativeMethods.WM_LBUTTONUP:
                            WmMouseUp(MouseButton.Left);
                            break;
                        case NativeMethods.WM_MBUTTONDBLCLK:
                            WmMouseDown(MouseButton.Middle, 2);
                            break;
                        case NativeMethods.WM_MBUTTONDOWN:
                            WmMouseDown(MouseButton.Middle, 1);
                            break;
                        case NativeMethods.WM_MBUTTONUP:
                            WmMouseUp(MouseButton.Middle);
                            break;
                        case NativeMethods.WM_MOUSEMOVE:
                            WmMouseMove();
                            break;
                        case NativeMethods.WM_RBUTTONDBLCLK:
                            WmMouseDown(MouseButton.Right, 2);
                            break;
                        case NativeMethods.WM_RBUTTONDOWN:
                            WmMouseDown(MouseButton.Right, 1);
                            break;
                        case NativeMethods.WM_RBUTTONUP:
                            ShowContextMenu();
                            WmMouseUp(MouseButton.Right);
                            break;
                    }
                }
            }

            return UnsafeNativeMethods.DefWindowProc(hWnd, msg, wparam, lparam);
        }

        private void WmMouseDown(MouseButton button, int clicks)
        {
            if (clicks == 2)
            {
                RaiseEvent(new MouseButtonEventArgs(Mouse.PrimaryDevice, Environment.TickCount, button)
                {
                    RoutedEvent = MouseDoubleClickEvent
                });
                _doubleClick = true;
            }
            RaiseEvent(new MouseButtonEventArgs(Mouse.PrimaryDevice, Environment.TickCount, button)
            {
                RoutedEvent = MouseDownEvent
            });
        }

        private void WmMouseUp(MouseButton button)
        {
            RaiseEvent(new MouseButtonEventArgs(Mouse.PrimaryDevice, Environment.TickCount, button)
            {
                RoutedEvent = MouseUpEvent
            });

            if (!_doubleClick)
            {
                RaiseEvent(new MouseButtonEventArgs(Mouse.PrimaryDevice, Environment.TickCount, button)
                {
                    RoutedEvent = ClickEvent
                });
            }
            _doubleClick = false;
        }

        private void WmMouseMove()
        {
            RaiseEvent(new MouseEventArgs(Mouse.PrimaryDevice, Environment.TickCount)
            {
                RoutedEvent = MouseMoveEvent
            });
        }

        private void ShowContextMenu()
        {
            var point = new POINT();
            UnsafeNativeMethods.GetCursorPos(point);

            if (ContextContent != null)
            {
                if (_contextContent == null)
                {
                    _contextContent = new Popup
                    {
                        Placement = PlacementMode.AbsolutePoint,
                        AllowsTransparency = true,
                        StaysOpen = false
                    };
                }

                _contextContent.HorizontalOffset = point.x;
                _contextContent.VerticalOffset = point.y;
                _contextContent.Child = new ContentControl
                {
                    Content = ContextContent
                };
                _contextContent.IsOpen = true;
                var handle = IntPtr.Zero;
                var hwndSource = (HwndSource)PresentationSource.FromVisual(_contextContent.Child);
                if (hwndSource != null)
                {
                    handle = hwndSource.Handle;
                }
                UnsafeNativeMethods.SetForegroundWindow(handle);
            }
            else if (ContextMenu != null)
            {
                ContextMenu.Placement = PlacementMode.AbsolutePoint;
                ContextMenu.HorizontalOffset = point.x;
                ContextMenu.VerticalOffset = point.y;
                ContextMenu.IsOpen = true;

                var handle = IntPtr.Zero;
                var hwndSource = (HwndSource)PresentationSource.FromVisual(ContextMenu);
                if (hwndSource != null)
                {
                    handle = hwndSource.Handle;
                }
                UnsafeNativeMethods.SetForegroundWindow(handle);
            }
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

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                UpdateIcon(false);
                _defaultLargeIconHandle?.Dispose();
                _defaultSmallIconHandle?.Dispose();
                _currentLargeIconHandle?.Dispose();
                _currentSmallIconHandle?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}