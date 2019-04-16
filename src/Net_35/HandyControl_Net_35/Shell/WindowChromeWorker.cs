using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using Standard;

namespace Microsoft.Windows.Shell
{
    using HANDLE_MESSAGE = KeyValuePair<WM, MessageHandler>;

    internal class WindowChromeWorker : DependencyObject
    {
        private const SWP _SwpFlags =
            SWP.NOOWNERZORDER | SWP.DRAWFRAME | SWP.NOACTIVATE | SWP.NOZORDER | SWP.NOMOVE | SWP.NOSIZE;

        [SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId =
            "Member")] private static readonly HT[,] _HitTestBorders =
        {
            {HT.TOPLEFT, HT.TOP, HT.TOPRIGHT}, {HT.LEFT, HT.CLIENT, HT.RIGHT},
            {HT.BOTTOMLEFT, HT.BOTTOM, HT.BOTTOMRIGHT}
        };

        public static readonly DependencyProperty WindowChromeWorkerProperty =
            DependencyProperty.RegisterAttached("WindowChromeWorker", typeof(WindowChromeWorker),
                typeof(WindowChromeWorker), new PropertyMetadata(null, _OnChromeWorkerChanged));

        private readonly List<KeyValuePair<WM, MessageHandler>> _messageTable;
        private int _blackGlassFixupAttemptCount;
        private WindowChrome _chromeInfo;
        private bool _hasUserMovedWindow;
        private IntPtr _hwnd;
        private HwndSource _hwndSource;
        private bool _isFixedUp;
        private bool _isGlassEnabled;
        private bool _isHooked;
        private bool _isUserResizing;
        private WindowState _lastMenuState;
        private WindowState _lastRoundingState;
        private Window _window;
        private Point _windowPosAtStartOfUserMove;

        public WindowChromeWorker()
        {
            _messageTable = new List<HANDLE_MESSAGE>
            {
                new HANDLE_MESSAGE(WM.SETTEXT, _HandleSetTextOrIcon),
                new HANDLE_MESSAGE(WM.SETICON, _HandleSetTextOrIcon),
                new HANDLE_MESSAGE(WM.NCACTIVATE, _HandleNCActivate),
                new HANDLE_MESSAGE(WM.NCCALCSIZE, _HandleNCCalcSize),
                new HANDLE_MESSAGE(WM.NCHITTEST, _HandleNCHitTest),
                new HANDLE_MESSAGE(WM.NCRBUTTONUP, _HandleNCRButtonUp),
                new HANDLE_MESSAGE(WM.SIZE, _HandleSize),
                new HANDLE_MESSAGE(WM.WINDOWPOSCHANGED, _HandleWindowPosChanged),
                new HANDLE_MESSAGE(WM.DWMCOMPOSITIONCHANGED, _HandleDwmCompositionChanged)
            };
            if (Utility.IsPresentationFrameworkVersionLessThan4)
            {
                KeyValuePair<WM, MessageHandler>[] collection =
                {
                    new KeyValuePair<WM, MessageHandler>(WM.WININICHANGE, _HandleSettingChange),
                    new KeyValuePair<WM, MessageHandler>(WM.ENTERSIZEMOVE, _HandleEnterSizeMove),
                    new KeyValuePair<WM, MessageHandler>(WM.EXITSIZEMOVE, _HandleExitSizeMove),
                    new KeyValuePair<WM, MessageHandler>(WM.MOVE, _HandleMove)
                };
                _messageTable.AddRange(collection);
            }
        }

        private bool _IsWindowDocked
        {
            get
            {
                if (_window.WindowState != WindowState.Normal)
                    return false;
                var rcWindow = new RECT
                {
                    Bottom = 100,
                    Right = 100
                };
                var rect = _GetAdjustedWindowRect(rcWindow);
                var point = new Point(_window.Left, _window.Top);
                point -= (Vector) DpiHelper.DevicePixelsToLogical(new Point(rect.Left, rect.Top));
                return _window.RestoreBounds.Location != point;
            }
        }

        private void _ApplyNewCustomChrome()
        {
            if (_hwnd != IntPtr.Zero)
                if (_chromeInfo == null)
                {
                    _RestoreStandardChromeState(false);
                }
                else
                {
                    if (!_isHooked)
                    {
                        _hwndSource.AddHook(_WndProc);
                        _isHooked = true;
                    }
                    _FixupFrameworkIssues();
                    _UpdateSystemMenu(_window.WindowState);
                    _UpdateFrameState(true);
                    NativeMethods.SetWindowPos(_hwnd, IntPtr.Zero, 0, 0, 0, 0,
                        SWP.NOOWNERZORDER | SWP.DRAWFRAME | SWP.NOACTIVATE | SWP.NOZORDER | SWP.NOMOVE | SWP.NOSIZE);
                }
        }

        private void _ClearRoundingRegion()
        {
            NativeMethods.SetWindowRgn(_hwnd, IntPtr.Zero, NativeMethods.IsWindowVisible(_hwnd));
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "HRGNs")]
        private static void _CreateAndCombineRoundRectRgn(IntPtr hrgnSource, Rect region, double radius)
        {
            var zero = IntPtr.Zero;
            try
            {
                zero = _CreateRoundRectRgn(region, radius);
                if (NativeMethods.CombineRgn(hrgnSource, hrgnSource, zero, RGN.OR) == CombineRgnResult.ERROR)
                    throw new InvalidOperationException("Unable to combine two HRGNs.");
            }
            catch
            {
                Utility.SafeDeleteObject(ref zero);
                throw;
            }
        }

        private static IntPtr _CreateRoundRectRgn(Rect region, double radius)
        {
            if (DoubleUtilities.AreClose(0.0, radius))
                return NativeMethods.CreateRectRgn((int) Math.Floor(region.Left), (int) Math.Floor(region.Top),
                    (int) Math.Ceiling(region.Right), (int) Math.Ceiling(region.Bottom));
            return NativeMethods.CreateRoundRectRgn((int) Math.Floor(region.Left), (int) Math.Floor(region.Top),
                (int) Math.Ceiling(region.Right) + 1, (int) Math.Ceiling(region.Bottom) + 1, (int) Math.Ceiling(radius),
                (int) Math.Ceiling(radius));
        }

        private void _ExtendGlassFrame()
        {
            if (Utility.IsOSVistaOrNewer && IntPtr.Zero != _hwnd)
                if (!NativeMethods.DwmIsCompositionEnabled())
                {
                    _hwndSource.CompositionTarget.BackgroundColor = SystemColors.WindowColor;
                }
                else
                {
                    _hwndSource.CompositionTarget.BackgroundColor = Colors.Transparent;
                    var point = DpiHelper.LogicalPixelsToDevice(new Point(_chromeInfo.GlassFrameThickness.Left,
                        _chromeInfo.GlassFrameThickness.Top));
                    var point2 = DpiHelper.LogicalPixelsToDevice(new Point(_chromeInfo.GlassFrameThickness.Right,
                        _chromeInfo.GlassFrameThickness.Bottom));
                    var pMarInset = new MARGINS
                    {
                        cxLeftWidth = (int) Math.Ceiling(point.X),
                        cxRightWidth = (int) Math.Ceiling(point2.X),
                        cyTopHeight = (int) Math.Ceiling(point.Y),
                        cyBottomHeight = (int) Math.Ceiling(point2.Y)
                    };
                    NativeMethods.DwmExtendFrameIntoClientArea(_hwnd, ref pMarInset);
                }
        }

        private void _FixupFrameworkIssues()
        {
            if (Utility.IsPresentationFrameworkVersionLessThan4 && _window.Template != null)
                if (VisualTreeHelper.GetChildrenCount(_window) == 0)
                {
                    _window.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new _Action(_FixupFrameworkIssues));
                }
                else
                {
                    var child = (FrameworkElement) VisualTreeHelper.GetChild(_window, 0);
                    var windowRect = NativeMethods.GetWindowRect(_hwnd);
                    var rect2 = _GetAdjustedWindowRect(windowRect);
                    var rect3 = DpiHelper.DeviceRectToLogical(new Rect(windowRect.Left, windowRect.Top,
                        windowRect.Width, windowRect.Height));
                    var rect4 = DpiHelper.DeviceRectToLogical(
                        new Rect(rect2.Left, rect2.Top, rect2.Width, rect2.Height));
                    var thickness = new Thickness(rect3.Left - rect4.Left, rect3.Top - rect4.Top,
                        rect4.Right - rect3.Right, rect4.Bottom - rect3.Bottom);
                    child.Margin = new Thickness(0.0, 0.0, -(thickness.Left + thickness.Right),
                        -(thickness.Top + thickness.Bottom));
                    if (_window.FlowDirection == FlowDirection.RightToLeft)
                        child.RenderTransform =
                            new MatrixTransform(1.0, 0.0, 0.0, 1.0, -(thickness.Left + thickness.Right), 0.0);
                    else
                        child.RenderTransform = null;
                    if (!_isFixedUp)
                    {
                        _hasUserMovedWindow = false;
                        _window.StateChanged += _FixupRestoreBounds;
                        _isFixedUp = true;
                    }
                }
        }

        private void _FixupRestoreBounds(object sender, EventArgs e)
        {
            if ((_window.WindowState == WindowState.Maximized || _window.WindowState == WindowState.Minimized) &&
                _hasUserMovedWindow)
            {
                _hasUserMovedWindow = false;
                var windowPlacement = NativeMethods.GetWindowPlacement(_hwnd);
                var rcWindow = new RECT
                {
                    Bottom = 100,
                    Right = 100
                };
                var rect = _GetAdjustedWindowRect(rcWindow);
                var point = DpiHelper.DevicePixelsToLogical(new Point(windowPlacement.rcNormalPosition.Left - rect.Left,
                    windowPlacement.rcNormalPosition.Top - rect.Top));
                _window.Top = point.Y;
                _window.Left = point.X;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void _FixupWindows7Issues()
        {
            if (_blackGlassFixupAttemptCount <= 5 && Utility.IsOSWindows7OrNewer &&
                NativeMethods.DwmIsCompositionEnabled())
            {
                _blackGlassFixupAttemptCount++;
                var hasValue = false;
                try
                {
                    hasValue = NativeMethods.DwmGetCompositionTimingInfo(_hwnd).HasValue;
                }
                catch (Exception)
                {
                }
                if (!hasValue)
                    Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new _Action(_FixupWindows7Issues));
                else
                    _blackGlassFixupAttemptCount = 0;
            }
        }

        private RECT _GetAdjustedWindowRect(RECT rcWindow)
        {
            var windowLongPtr = (WS) (int) NativeMethods.GetWindowLongPtr(_hwnd, GWL.STYLE);
            var dwExStyle = (WS_EX) (int) NativeMethods.GetWindowLongPtr(_hwnd, GWL.EXSTYLE);
            return NativeMethods.AdjustWindowRectEx(rcWindow, windowLongPtr, false, dwExStyle);
        }

        private WindowState _GetHwndState()
        {
            switch (NativeMethods.GetWindowPlacement(_hwnd).showCmd)
            {
                case SW.SHOWMINIMIZED:
                    return WindowState.Minimized;

                case SW.SHOWMAXIMIZED:
                    return WindowState.Maximized;
            }
            return WindowState.Normal;
        }

        private Rect _GetWindowRect()
        {
            var windowRect = NativeMethods.GetWindowRect(_hwnd);
            return new Rect(windowRect.Left, windowRect.Top, windowRect.Width, windowRect.Height);
        }

        private IntPtr _HandleDwmCompositionChanged(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
        {
            _UpdateFrameState(false);
            handled = false;
            return IntPtr.Zero;
        }

        private IntPtr _HandleEnterSizeMove(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
        {
            _isUserResizing = true;
            if (_window.WindowState != WindowState.Maximized && !_IsWindowDocked)
                _windowPosAtStartOfUserMove = new Point(_window.Left, _window.Top);
            handled = false;
            return IntPtr.Zero;
        }

        private IntPtr _HandleExitSizeMove(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
        {
            _isUserResizing = false;
            if (_window.WindowState == WindowState.Maximized)
            {
                _window.Top = _windowPosAtStartOfUserMove.Y;
                _window.Left = _windowPosAtStartOfUserMove.X;
            }
            handled = false;
            return IntPtr.Zero;
        }

        private IntPtr _HandleMove(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
        {
            if (_isUserResizing)
                _hasUserMovedWindow = true;
            handled = false;
            return IntPtr.Zero;
        }

        private IntPtr _HandleNCActivate(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
        {
            var ptr = NativeMethods.DefWindowProc(_hwnd, WM.NCACTIVATE, wParam, new IntPtr(-1));
            handled = true;
            return ptr;
        }

        private IntPtr _HandleNCCalcSize(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
        {
            handled = true;
            return new IntPtr(0x300);
        }

        private IntPtr _HandleNCHitTest(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
        {
            var zero = IntPtr.Zero;
            handled = false;
            if (Utility.IsOSVistaOrNewer && _chromeInfo.GlassFrameThickness != new Thickness() && _isGlassEnabled)
                handled = NativeMethods.DwmDefWindowProc(_hwnd, uMsg, wParam, lParam, out zero);
            if (!(IntPtr.Zero == zero))
                return zero;
            var devicePoint = new Point(Utility.GET_X_LPARAM(lParam), Utility.GET_Y_LPARAM(lParam));
            var deviceRectangle = _GetWindowRect();
            var cLIENT = _HitTestNca(DpiHelper.DeviceRectToLogical(deviceRectangle),
                DpiHelper.DevicePixelsToLogical(devicePoint));
            if (cLIENT != HT.CLIENT)
            {
                var point2 = devicePoint;
                point2.Offset(-deviceRectangle.X, -deviceRectangle.Y);
                point2 = DpiHelper.DevicePixelsToLogical(point2);
                var inputElement = _window.InputHitTest(point2);
                if (inputElement != null && WindowChrome.GetIsHitTestVisibleInChrome(inputElement))
                    cLIENT = HT.CLIENT;
            }
            handled = true;
            return new IntPtr((int) cLIENT);
        }

        private IntPtr _HandleNCRButtonUp(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
        {
            if (2 == wParam.ToInt32())
                SystemCommands.ShowSystemMenuPhysicalCoordinates(_window,
                    new Point(Utility.GET_X_LPARAM(lParam), Utility.GET_Y_LPARAM(lParam)));
            handled = false;
            return IntPtr.Zero;
        }

        private IntPtr _HandleSetTextOrIcon(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
        {
            var flag = _ModifyStyle(WS.OVERLAPPED | WS.VISIBLE, WS.OVERLAPPED);
            var ptr = NativeMethods.DefWindowProc(_hwnd, uMsg, wParam, lParam);
            if (flag)
                _ModifyStyle(WS.OVERLAPPED, WS.OVERLAPPED | WS.VISIBLE);
            handled = true;
            return ptr;
        }

        private IntPtr _HandleSettingChange(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
        {
            _FixupFrameworkIssues();
            handled = false;
            return IntPtr.Zero;
        }

        private IntPtr _HandleSize(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
        {
            WindowState? assumeState = null;
            if (wParam.ToInt32() == 2)
                assumeState = WindowState.Maximized;
            _UpdateSystemMenu(assumeState);
            handled = false;
            return IntPtr.Zero;
        }

        private IntPtr _HandleWindowPosChanged(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
        {
            _UpdateSystemMenu(null);
            if (!_isGlassEnabled)
            {
                var windowpos = (WINDOWPOS) Marshal.PtrToStructure(lParam, typeof(WINDOWPOS));
                _SetRoundingRegion(windowpos);
            }
            handled = false;
            return IntPtr.Zero;
        }

        private HT _HitTestNca(Rect windowPosition, Point mousePosition)
        {
            var num = 1;
            var num2 = 1;
            var flag = false;
            if (mousePosition.Y >= windowPosition.Top && mousePosition.Y <
                windowPosition.Top + _chromeInfo.ResizeBorderThickness.Top + _chromeInfo.CaptionHeight)
            {
                flag = mousePosition.Y < windowPosition.Top + _chromeInfo.ResizeBorderThickness.Top;
                num = 0;
            }
            else if (mousePosition.Y < windowPosition.Bottom && mousePosition.Y >=
                     windowPosition.Bottom - (int) _chromeInfo.ResizeBorderThickness.Bottom)
            {
                num = 2;
            }
            if (mousePosition.X >= windowPosition.Left &&
                mousePosition.X < windowPosition.Left + (int) _chromeInfo.ResizeBorderThickness.Left)
                num2 = 0;
            else if (mousePosition.X < windowPosition.Right &&
                     mousePosition.X >= windowPosition.Right - _chromeInfo.ResizeBorderThickness.Right)
                num2 = 2;
            if (num == 0 && num2 != 1 && !flag)
                num = 1;
            var cAPTION = _HitTestBorders[num, num2];
            if (cAPTION == HT.TOP && !flag)
                cAPTION = HT.CAPTION;
            return cAPTION;
        }

        private static bool _IsUniform(CornerRadius cornerRadius)
        {
            if (!DoubleUtilities.AreClose(cornerRadius.BottomLeft, cornerRadius.BottomRight))
                return false;
            if (!DoubleUtilities.AreClose(cornerRadius.TopLeft, cornerRadius.TopRight))
                return false;
            if (!DoubleUtilities.AreClose(cornerRadius.BottomLeft, cornerRadius.TopRight))
                return false;
            return true;
        }

        private bool _ModifyStyle(WS removeStyle, WS addStyle)
        {
            var ws = (WS) NativeMethods.GetWindowLongPtr(_hwnd, GWL.STYLE).ToInt32();
            var ws2 = (ws & ~removeStyle) | addStyle;
            if (ws == ws2)
                return false;
            NativeMethods.SetWindowLongPtr(_hwnd, GWL.STYLE, new IntPtr((int) ws2));
            return true;
        }

        private void _OnChromePropertyChangedThatRequiresRepaint(object sender, EventArgs e)
        {
            _UpdateFrameState(true);
        }

        private static void _OnChromeWorkerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = (Window) d;
            ((WindowChromeWorker) e.NewValue)._SetWindow(window);
        }

        private void _OnWindowPropertyChangedThatRequiresTemplateFixup(object sender, EventArgs e)
        {
            if (_chromeInfo != null && _hwnd != IntPtr.Zero)
                _window.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new _Action(_FixupFrameworkIssues));
        }

        private void _RestoreFrameworkIssueFixups()
        {
            if (Utility.IsPresentationFrameworkVersionLessThan4)
            {
                var child = (FrameworkElement) VisualTreeHelper.GetChild(_window, 0);
                child.Margin = new Thickness();
                _window.StateChanged -= _FixupRestoreBounds;
                _isFixedUp = false;
            }
        }

        private void _RestoreGlassFrame()
        {
            if (Utility.IsOSVistaOrNewer && _hwnd != IntPtr.Zero)
            {
                _hwndSource.CompositionTarget.BackgroundColor = SystemColors.WindowColor;
                if (NativeMethods.DwmIsCompositionEnabled())
                {
                    var pMarInset = new MARGINS();
                    NativeMethods.DwmExtendFrameIntoClientArea(_hwnd, ref pMarInset);
                }
            }
        }

        private void _RestoreHrgn()
        {
            _ClearRoundingRegion();
            NativeMethods.SetWindowPos(_hwnd, IntPtr.Zero, 0, 0, 0, 0,
                SWP.NOOWNERZORDER | SWP.DRAWFRAME | SWP.NOACTIVATE | SWP.NOZORDER | SWP.NOMOVE | SWP.NOSIZE);
        }

        private void _RestoreStandardChromeState(bool isClosing)
        {
            VerifyAccess();
            _UnhookCustomChrome();
            if (!isClosing)
            {
                _RestoreFrameworkIssueFixups();
                _RestoreGlassFrame();
                _RestoreHrgn();
                _window.InvalidateMeasure();
            }
        }

        private void _SetRoundingRegion(WINDOWPOS? wp)
        {
            if (NativeMethods.GetWindowPlacement(_hwnd).showCmd == SW.SHOWMAXIMIZED)
            {
                int x;
                int y;
                if (wp.HasValue)
                {
                    x = wp.Value.x;
                    y = wp.Value.y;
                }
                else
                {
                    var rect = _GetWindowRect();
                    x = (int) rect.Left;
                    y = (int) rect.Top;
                }
                var rcWork = NativeMethods.GetMonitorInfo(NativeMethods.MonitorFromWindow(_hwnd, 2)).rcWork;
                rcWork.Offset(-x, -y);
                var zero = IntPtr.Zero;
                try
                {
                    zero = NativeMethods.CreateRectRgnIndirect(rcWork);
                    NativeMethods.SetWindowRgn(_hwnd, zero, NativeMethods.IsWindowVisible(_hwnd));
                    zero = IntPtr.Zero;
                }
                finally
                {
                    Utility.SafeDeleteObject(ref zero);
                }
            }
            else
            {
                Size size;
                if (wp.HasValue && !Utility.IsFlagSet(wp.Value.flags, 1))
                {
                    size = new Size(wp.Value.cx, wp.Value.cy);
                }
                else
                {
                    if (wp.HasValue && _lastRoundingState == _window.WindowState)
                        return;
                    size = _GetWindowRect().Size;
                }
                _lastRoundingState = _window.WindowState;
                var hrgnSource = IntPtr.Zero;
                try
                {
                    var num3 = Math.Min(size.Width, size.Height);
                    var radius =
                        Math.Min(DpiHelper.LogicalPixelsToDevice(new Point(_chromeInfo.CornerRadius.TopLeft, 0.0)).X,
                            num3 / 2.0);
                    if (_IsUniform(_chromeInfo.CornerRadius))
                    {
                        hrgnSource = _CreateRoundRectRgn(new Rect(size), radius);
                    }
                    else
                    {
                        hrgnSource =
                            _CreateRoundRectRgn(
                                new Rect(0.0, 0.0, size.Width / 2.0 + radius, size.Height / 2.0 + radius), radius);
                        var num5 = Math.Min(
                            DpiHelper.LogicalPixelsToDevice(new Point(_chromeInfo.CornerRadius.TopRight, 0.0)).X,
                            num3 / 2.0);
                        var region = new Rect(0.0, 0.0, size.Width / 2.0 + num5, size.Height / 2.0 + num5);
                        region.Offset(size.Width / 2.0 - num5, 0.0);
                        _CreateAndCombineRoundRectRgn(hrgnSource, region, num5);
                        var num6 = Math.Min(
                            DpiHelper.LogicalPixelsToDevice(new Point(_chromeInfo.CornerRadius.BottomLeft, 0.0)).X,
                            num3 / 2.0);
                        var rect4 = new Rect(0.0, 0.0, size.Width / 2.0 + num6, size.Height / 2.0 + num6);
                        rect4.Offset(0.0, size.Height / 2.0 - num6);
                        _CreateAndCombineRoundRectRgn(hrgnSource, rect4, num6);
                        var num7 = Math.Min(
                            DpiHelper.LogicalPixelsToDevice(new Point(_chromeInfo.CornerRadius.BottomRight, 0.0)).X,
                            num3 / 2.0);
                        var rect5 = new Rect(0.0, 0.0, size.Width / 2.0 + num7, size.Height / 2.0 + num7);
                        rect5.Offset(size.Width / 2.0 - num7, size.Height / 2.0 - num7);
                        _CreateAndCombineRoundRectRgn(hrgnSource, rect5, num7);
                    }
                    NativeMethods.SetWindowRgn(_hwnd, hrgnSource, NativeMethods.IsWindowVisible(_hwnd));
                    hrgnSource = IntPtr.Zero;
                }
                finally
                {
                    Utility.SafeDeleteObject(ref hrgnSource);
                }
            }
        }

        private void _SetWindow(Window window)
        {
            EventHandler handler = null;
            _window = window;
            _hwnd = new WindowInteropHelper(_window).Handle;
            if (Utility.IsPresentationFrameworkVersionLessThan4)
            {
                Utility.AddDependencyPropertyChangeListener(_window, Control.TemplateProperty,
                    _OnWindowPropertyChangedThatRequiresTemplateFixup);
                Utility.AddDependencyPropertyChangeListener(_window, FrameworkElement.FlowDirectionProperty,
                    _OnWindowPropertyChangedThatRequiresTemplateFixup);
            }
            _window.Closed += _UnsetWindow;
            if (IntPtr.Zero != _hwnd)
            {
                _hwndSource = HwndSource.FromHwnd(_hwnd);
                _window.ApplyTemplate();
                if (_chromeInfo != null)
                    _ApplyNewCustomChrome();
            }
            else
            {
                if (handler == null)
                    handler = delegate
                    {
                        _hwnd = new WindowInteropHelper(_window).Handle;
                        _hwndSource = HwndSource.FromHwnd(_hwnd);
                        if (_chromeInfo != null)
                            _ApplyNewCustomChrome();
                    };
                _window.SourceInitialized += handler;
            }
        }

        private void _UnhookCustomChrome()
        {
            if (_isHooked)
            {
                _hwndSource.RemoveHook(_WndProc);
                _isHooked = false;
            }
        }

        private void _UnsetWindow(object sender, EventArgs e)
        {
            if (Utility.IsPresentationFrameworkVersionLessThan4)
            {
                Utility.RemoveDependencyPropertyChangeListener(_window, Control.TemplateProperty,
                    _OnWindowPropertyChangedThatRequiresTemplateFixup);
                Utility.RemoveDependencyPropertyChangeListener(_window, FrameworkElement.FlowDirectionProperty,
                    _OnWindowPropertyChangedThatRequiresTemplateFixup);
            }
            if (_chromeInfo != null)
                _chromeInfo.PropertyChangedThatRequiresRepaint -= _OnChromePropertyChangedThatRequiresRepaint;
            _RestoreStandardChromeState(true);
        }

        private void _UpdateFrameState(bool force)
        {
            if (IntPtr.Zero != _hwnd)
            {
                var flag = NativeMethods.DwmIsCompositionEnabled();
                if (force || flag != _isGlassEnabled)
                {
                    _isGlassEnabled = flag && _chromeInfo.GlassFrameThickness != new Thickness();
                    if (!_isGlassEnabled)
                    {
                        _SetRoundingRegion(null);
                    }
                    else
                    {
                        _ClearRoundingRegion();
                        _ExtendGlassFrame();
                        _FixupWindows7Issues();
                    }
                    NativeMethods.SetWindowPos(_hwnd, IntPtr.Zero, 0, 0, 0, 0,
                        SWP.NOOWNERZORDER | SWP.DRAWFRAME | SWP.NOACTIVATE | SWP.NOZORDER | SWP.NOMOVE | SWP.NOSIZE);
                }
            }
        }

        private void _UpdateSystemMenu(WindowState? assumeState)
        {
            var nullable = assumeState;
            var state = nullable.HasValue ? nullable.GetValueOrDefault() : _GetHwndState();
            if (!assumeState.HasValue && _lastMenuState == state)
                return;
            _lastMenuState = state;
            var flag = _ModifyStyle(WS.OVERLAPPED | WS.VISIBLE, WS.OVERLAPPED);
            var systemMenu = NativeMethods.GetSystemMenu(_hwnd, false);
            if (IntPtr.Zero != systemMenu)
            {
                var ws = (WS) NativeMethods.GetWindowLongPtr(_hwnd, GWL.STYLE).ToInt32();
                var flag2 = Utility.IsFlagSet((int) ws, 0x20000);
                var flag3 = Utility.IsFlagSet((int) ws, 0x10000);
                var flag4 = Utility.IsFlagSet((int) ws, 0x40000);
                switch (state)
                {
                    case WindowState.Minimized:
                        NativeMethods.EnableMenuItem(systemMenu, SC.RESTORE, MF.BYCOMMAND);
                        NativeMethods.EnableMenuItem(systemMenu, SC.MOVE, MF.BYCOMMAND | MF.DISABLED | MF.GRAYED);
                        NativeMethods.EnableMenuItem(systemMenu, SC.SIZE, MF.BYCOMMAND | MF.DISABLED | MF.GRAYED);
                        NativeMethods.EnableMenuItem(systemMenu, SC.MINIMIZE, MF.BYCOMMAND | MF.DISABLED | MF.GRAYED);
                        NativeMethods.EnableMenuItem(systemMenu, SC.MAXIMIZE,
                            flag3 ? MF.BYCOMMAND : MF.BYCOMMAND | MF.DISABLED | MF.GRAYED);
                        goto Label_01A6;

                    case WindowState.Maximized:
                        NativeMethods.EnableMenuItem(systemMenu, SC.RESTORE, MF.BYCOMMAND);
                        NativeMethods.EnableMenuItem(systemMenu, SC.MOVE, MF.BYCOMMAND | MF.DISABLED | MF.GRAYED);
                        NativeMethods.EnableMenuItem(systemMenu, SC.SIZE, MF.BYCOMMAND | MF.DISABLED | MF.GRAYED);
                        NativeMethods.EnableMenuItem(systemMenu, SC.MINIMIZE,
                            flag2 ? MF.BYCOMMAND : MF.BYCOMMAND | MF.DISABLED | MF.GRAYED);
                        NativeMethods.EnableMenuItem(systemMenu, SC.MAXIMIZE, MF.BYCOMMAND | MF.DISABLED | MF.GRAYED);
                        goto Label_01A6;
                }
                NativeMethods.EnableMenuItem(systemMenu, SC.RESTORE, MF.BYCOMMAND | MF.DISABLED | MF.GRAYED);
                NativeMethods.EnableMenuItem(systemMenu, SC.MOVE, MF.BYCOMMAND);
                NativeMethods.EnableMenuItem(systemMenu, SC.SIZE,
                    flag4 ? MF.BYCOMMAND : MF.BYCOMMAND | MF.DISABLED | MF.GRAYED);
                NativeMethods.EnableMenuItem(systemMenu, SC.MINIMIZE,
                    flag2 ? MF.BYCOMMAND : MF.BYCOMMAND | MF.DISABLED | MF.GRAYED);
                NativeMethods.EnableMenuItem(systemMenu, SC.MAXIMIZE,
                    flag3 ? MF.BYCOMMAND : MF.BYCOMMAND | MF.DISABLED | MF.GRAYED);
            }
            Label_01A6:
            if (flag)
                _ModifyStyle(WS.OVERLAPPED, WS.OVERLAPPED | WS.VISIBLE);
        }

        private IntPtr _WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            var uMsg = (WM) msg;
            foreach (var pair in _messageTable)
                if (pair.Key == uMsg)
                    return pair.Value(uMsg, wParam, lParam, out handled);
            return IntPtr.Zero;
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static WindowChromeWorker GetWindowChromeWorker(Window window)
        {
            Verify.IsNotNull(window, "window");
            return (WindowChromeWorker) window.GetValue(WindowChromeWorkerProperty);
        }

        public void SetWindowChrome(WindowChrome newChrome)
        {
            VerifyAccess();
            if (newChrome != _chromeInfo)
            {
                if (_chromeInfo != null)
                    _chromeInfo.PropertyChangedThatRequiresRepaint -= _OnChromePropertyChangedThatRequiresRepaint;
                _chromeInfo = newChrome;
                if (_chromeInfo != null)
                    _chromeInfo.PropertyChangedThatRequiresRepaint += _OnChromePropertyChangedThatRequiresRepaint;
                _ApplyNewCustomChrome();
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static void SetWindowChromeWorker(Window window, WindowChromeWorker chrome)
        {
            Verify.IsNotNull(window, "window");
            window.SetValue(WindowChromeWorkerProperty, chrome);
        }

        private delegate void _Action();
    }
}