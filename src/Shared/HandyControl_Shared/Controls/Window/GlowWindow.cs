using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using HandyControl.Data;
using HandyControl.Expression.Drawing;
using HandyControl.Tools;
using HandyControl.Tools.Interop;

namespace HandyControl.Controls
{
    public class GlowWindow : Window
    {
        internal int DeferGlowChangesCount;

        private readonly GlowEdge[] _glowEdges = new GlowEdge[4];

        private DispatcherTimer _makeGlowVisibleTimer;

        private bool _isGlowVisible;

        private bool _useLogicalSizeForRestore;

        private bool _updatingZOrder;

        private Rect _logicalSizeForRestore = Rect.Empty;

        public static readonly DependencyProperty ActiveGlowColorProperty = DependencyProperty.Register(
            "ActiveGlowColor", typeof(Color), typeof(GlowWindow), new PropertyMetadata(default(Color), OnGlowColorChanged));

        public Color ActiveGlowColor
        {
            get => (Color)GetValue(ActiveGlowColorProperty);
            set => SetValue(ActiveGlowColorProperty, value);
        }

        public static readonly DependencyProperty InactiveGlowColorProperty = DependencyProperty.Register(
            "InactiveGlowColor", typeof(Color), typeof(GlowWindow), new PropertyMetadata(default(Color), OnGlowColorChanged));

        public Color InactiveGlowColor
        {
            get => (Color)GetValue(InactiveGlowColorProperty);
            set => SetValue(InactiveGlowColorProperty, value);
        }

        #region internal

        internal void EndDeferGlowChanges()
        {
            foreach (var current in LoadedGlowWindows) current.CommitChanges();
        }

        #endregion

        #region protected

        protected virtual bool ShouldShowGlow
        {
            get
            {
                var handle = this.GetHandle();
                return InteropMethods.IsWindowVisible(handle) && !InteropMethods.IsIconic(handle) &&
                       !InteropMethods.IsZoomed(handle) && ResizeMode != ResizeMode.NoResize;
            }
        }

        private IntPtr HwndSourceHook(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg <= InteropValues.WM_WINDOWPOSCHANGED)
            {
                if (msg == InteropValues.WM_ACTIVATE)
                {
                    return IntPtr.Zero;
                }

                if (msg != InteropValues.WM_QUIT)
                {
                    switch (msg)
                    {
                        case InteropValues.WM_WINDOWPOSCHANGING:
                            WmWindowPosChanging(lParam);
                            return IntPtr.Zero;
                        case InteropValues.WM_WINDOWPOSCHANGED:
                            WmWindowPosChanged(lParam);
                            return IntPtr.Zero;
                        default:
                            return IntPtr.Zero;
                    }
                }
            }
            else
            {
                if (msg <= InteropValues.WM_NCRBUTTONDBLCLK)
                {
                    switch (msg)
                    {
                        case InteropValues.WM_SETICON:
                            break;
                        case InteropValues.WM_NCCREATE:
                        case InteropValues.WM_NCDESTROY:
                            return IntPtr.Zero;
                        case InteropValues.WM_NCACTIVATE:
                            handled = true;
                            return WmNcActivate(hWnd, wParam);
                        default:
                            switch (msg)
                            {
                                case InteropValues.WM_NCRBUTTONDOWN:
                                case InteropValues.WM_NCRBUTTONUP:
                                case InteropValues.WM_NCRBUTTONDBLCLK:
                                    handled = true;
                                    return IntPtr.Zero;
                                default:
                                    return IntPtr.Zero;
                            }
                    }
                }
                else
                {
                    switch (msg)
                    {
                        case InteropValues.WM_NCUAHDRAWCAPTION:
                        case InteropValues.WM_NCUAHDRAWFRAME:
                            handled = true;
                            return IntPtr.Zero;
                        default:
                            if (msg != InteropValues.WM_SYSCOMMAND) return IntPtr.Zero;
                            WmSysCommand(hWnd, wParam);
                            return IntPtr.Zero;
                    }
                }
            }

            handled = true;
            return CallDefWindowProcWithoutVisibleStyle(hWnd, msg, wParam, lParam);
        }

        protected override void OnActivated(EventArgs e)
        {
            UpdateGlowActiveState();
            base.OnActivated(e);
        }

        protected override void OnDeactivated(EventArgs e)
        {
            UpdateGlowActiveState();
            base.OnDeactivated(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            StopTimer();
            DestroyGlowWindows();
            base.OnClosed(e);
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            var hwndSource = this.GetHwndSource();
            if (hwndSource != null)
            {
                hwndSource.AddHook(HwndSourceHook);
                CreateGlowWindowHandles();
            }

            base.OnSourceInitialized(e);
        }

        #endregion

        #region private

        private IEnumerable<GlowEdge> LoadedGlowWindows => from w in _glowEdges where w != null select w;

        private bool IsGlowVisible
        {
            get => _isGlowVisible;
            set
            {
                if (_isGlowVisible != value)
                {
                    _isGlowVisible = value;
                    for (var i = 0; i < _glowEdges.Length; i++)
                    {
                        GetOrCreateGlowWindow(i).IsVisible = value;
                    }
                }
            }
        }

        private GlowEdge GetOrCreateGlowWindow(int direction)
        {
            return _glowEdges[direction] ?? (_glowEdges[direction] = new GlowEdge(this, (Dock)direction)
            {
                ActiveGlowColor = ActiveGlowColor,
                InactiveGlowColor = InactiveGlowColor,
                IsActive = IsActive
            });
        }

        private static void OnResizeModeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var customChromeWindow = (GlowWindow)obj;
            customChromeWindow.UpdateGlowVisibility(false);
        }

        private static void OnGlowColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args) => ((GlowWindow)obj).UpdateGlowColors();

        private void UpdateGlowColors()
        {
            using (DeferGlowChanges())
            {
                foreach (var current in LoadedGlowWindows)
                {
                    current.ActiveGlowColor = ActiveGlowColor;
                    current.InactiveGlowColor = InactiveGlowColor;
                }
            }
        }

        private void UpdateGlowVisibility(bool delayIfNecessary)
        {
            var shouldShowGlow = ShouldShowGlow;
            if (shouldShowGlow != IsGlowVisible)
            {
                if (SystemParameters.MinimizeAnimation && shouldShowGlow && delayIfNecessary)
                {
                    if (_makeGlowVisibleTimer != null)
                    {
                        _makeGlowVisibleTimer.Stop();
                    }
                    else
                    {
                        _makeGlowVisibleTimer = new DispatcherTimer
                        {
                            Interval = TimeSpan.FromMilliseconds(200.0)
                        };
                        _makeGlowVisibleTimer.Tick += OnDelayedVisibilityTimerTick;
                    }

                    _makeGlowVisibleTimer.Start();
                    return;
                }

                StopTimer();
                IsGlowVisible = shouldShowGlow;
            }
        }

        private void StopTimer()
        {
            if (_makeGlowVisibleTimer != null)
            {
                _makeGlowVisibleTimer.Stop();
                _makeGlowVisibleTimer.Tick -= OnDelayedVisibilityTimerTick;
                _makeGlowVisibleTimer = null;
            }
        }

        private void OnDelayedVisibilityTimerTick(object sender, EventArgs e)
        {
            StopTimer();
            UpdateGlowWindowPositions(false);
        }

        private void UpdateGlowWindowPositions(bool delayIfNecessary)
        {
            using (DeferGlowChanges())
            {
                UpdateGlowVisibility(delayIfNecessary);
                foreach (var current in LoadedGlowWindows) current.UpdateWindowPos();
            }
        }

        private IDisposable DeferGlowChanges() => new ChangeScope(this);

        private void UpdateGlowActiveState()
        {
            using (DeferGlowChanges())
            {
                foreach (var current in LoadedGlowWindows)
                {
                    current.IsActive = IsActive;
                }
            }
        }

        private void DestroyGlowWindows()
        {
            for (var i = 0; i < _glowEdges.Length; i++)
            {
                using (_glowEdges[i])
                {
                    _glowEdges[i] = null;
                }
            }
        }

        private void WmWindowPosChanging(IntPtr lParam)
        {
            var windowpos = (InteropValues.WINDOWPOS)Marshal.PtrToStructure(lParam, typeof(InteropValues.WINDOWPOS));
            if ((windowpos.flags & 2u) == 0u && (windowpos.flags & 1u) == 0u && windowpos.cx > 0 && windowpos.cy > 0)
            {
                var rect = new Rect(windowpos.x, windowpos.y, windowpos.cx, windowpos.cy);
                rect = rect.DeviceToLogicalUnits();
                if (_useLogicalSizeForRestore)
                {
                    rect = _logicalSizeForRestore;
                    _logicalSizeForRestore = Rect.Empty;
                    _useLogicalSizeForRestore = false;
                }

                var logicalRect = GetOnScreenPosition(rect);
                logicalRect = logicalRect.LogicalToDeviceUnits();
                windowpos.x = (int)logicalRect.X;
                windowpos.y = (int)logicalRect.Y;
                Marshal.StructureToPtr(windowpos, lParam, true);
            }
        }

        private void UpdateZOrderOfOwner(IntPtr hwndOwner)
        {
            var lastOwnedWindow = IntPtr.Zero;
            InteropMethods.EnumThreadWindows(InteropMethods.GetCurrentThreadId(), delegate (IntPtr hwnd, IntPtr lParam)
            {
                if (InteropMethods.GetWindow(hwnd, 4) == hwndOwner) lastOwnedWindow = hwnd;
                return true;
            }, IntPtr.Zero);

            if (lastOwnedWindow != IntPtr.Zero && InteropMethods.GetWindow(hwndOwner, 3) != lastOwnedWindow)
                InteropMethods.SetWindowPos(hwndOwner, lastOwnedWindow, 0, 0, 0, 0, 19);
        }

        private void UpdateZOrderOfThisAndOwner()
        {
            if (_updatingZOrder) return;
            try
            {
                _updatingZOrder = true;
                var windowInteropHelper = new WindowInteropHelper(this);
                var handle = windowInteropHelper.Handle;
                foreach (var current in LoadedGlowWindows)
                {
                    var window = InteropMethods.GetWindow(current.Handle, 3);
                    if (window != handle) InteropMethods.SetWindowPos(current.Handle, handle, 0, 0, 0, 0, 19);
                    handle = current.Handle;
                }

                var owner = windowInteropHelper.Owner;
                if (owner != IntPtr.Zero) UpdateZOrderOfOwner(owner);
            }
            finally
            {
                _updatingZOrder = false;
            }
        }

        private void WmWindowPosChanged(IntPtr lParam)
        {
            try
            {
                var windowpos = (InteropValues.WINDOWPOS)Marshal.PtrToStructure(lParam, typeof(InteropValues.WINDOWPOS));

                UpdateGlowWindowPositions((windowpos.flags & 64u) == 0u);
                UpdateZOrderOfThisAndOwner();
            }
            catch
            {
                // ignored
            }
        }

        private Rect GetOnScreenPosition(Rect floatRect)
        {
            var result = floatRect;
            floatRect = floatRect.LogicalToDeviceUnits();
            ScreenHelper.FindMaximumSingleMonitorRectangle(floatRect, out _, out var rect2);
            if (!floatRect.IntersectsWith(rect2))
            {
                ScreenHelper.FindMonitorRectsFromPoint(InteropMethods.GetCursorPos(), out _, out rect2);
                rect2 = rect2.DeviceToLogicalUnits();
                if (result.Width > rect2.Width) result.Width = rect2.Width;
                if (result.Height > rect2.Height) result.Height = rect2.Height;
                if (rect2.Right <= result.X) result.X = rect2.Right - result.Width;
                if (rect2.Left > result.X + result.Width) result.X = rect2.Left;
                if (rect2.Bottom <= result.Y) result.Y = rect2.Bottom - result.Height;
                if (rect2.Top > result.Y + result.Height) result.Y = rect2.Top;
            }

            return result;
        }

        private static InteropValues.MONITORINFO MonitorInfoFromWindow(IntPtr hWnd)
        {
            var hMonitor = InteropMethods.MonitorFromWindow(hWnd, 2);
            var result = default(InteropValues.MONITORINFO);
            result.cbSize = (uint)Marshal.SizeOf(typeof(InteropValues.MONITORINFO));
            InteropMethods.GetMonitorInfo(hMonitor, ref result);
            return result;
        }

        private IntPtr WmNcActivate(IntPtr hWnd, IntPtr wParam) => InteropMethods.DefWindowProc(hWnd, InteropValues.WM_NCACTIVATE, wParam, InteropMethods.HRGN_NONE);

        private bool IsAeroSnappedToMonitor(IntPtr hWnd)
        {
            var monitorinfo = MonitorInfoFromWindow(hWnd);
            var logicalRect = new Rect(Left, Top, Width, Height);
            logicalRect = logicalRect.LogicalToDeviceUnits();
            return MathHelper.AreClose(monitorinfo.rcWork.Height, logicalRect.Height) && MathHelper.AreClose(monitorinfo.rcWork.Top, logicalRect.Top);
        }

        private void WmSysCommand(IntPtr hWnd, IntPtr wParam)
        {
            var num = InteropMethods.GET_SC_WPARAM(wParam);
            
            if (num == InteropValues.SC_MOVE)
            {
                InteropMethods.RedrawWindow(hWnd, IntPtr.Zero, IntPtr.Zero,
                    InteropValues.RedrawWindowFlags.Invalidate | InteropValues.RedrawWindowFlags.NoChildren |
                    InteropValues.RedrawWindowFlags.UpdateNow | InteropValues.RedrawWindowFlags.Frame);
            }

            if ((num == InteropValues.SC_MAXIMIZE || num == InteropValues.SC_MINIMIZE || num == InteropValues.SC_MOVE ||
                 num == InteropValues.SC_SIZE) && WindowState == WindowState.Normal && !IsAeroSnappedToMonitor(hWnd))
            {
                _logicalSizeForRestore = new Rect(Left, Top, Width, Height);
            }
            
            if (num == InteropValues.SC_MOVE && WindowState == WindowState.Maximized && _logicalSizeForRestore == Rect.Empty)
            {
                _logicalSizeForRestore = new Rect(Left, Top, Width, Height);
            }
            
            if (num == InteropValues.SC_RESTORE && WindowState != WindowState.Minimized && 
                _logicalSizeForRestore.Width > 0.0 && _logicalSizeForRestore.Height > 0.0)
            {
                Left = _logicalSizeForRestore.Left;
                Top = _logicalSizeForRestore.Top;
                Width = _logicalSizeForRestore.Width;
                Height = _logicalSizeForRestore.Height;
                _useLogicalSizeForRestore = true;
            }
        }

        private IntPtr CallDefWindowProcWithoutVisibleStyle(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam)
        {
            var flag = VisualHelper.ModifyStyle(hWnd, InteropValues.WS_VISIBLE, 0);
            var result = InteropMethods.DefWindowProc(hWnd, msg, wParam, lParam);
            if (flag) VisualHelper.ModifyStyle(hWnd, 0, InteropValues.WS_VISIBLE);
            return result;
        }

        private void CreateGlowWindowHandles()
        {
            for (var i = 0; i < _glowEdges.Length; i++) GetOrCreateGlowWindow(i).EnsureHandle();
        }

        #endregion

    }
}