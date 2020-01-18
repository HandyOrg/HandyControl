using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using HandyControl.Tools.Extension;
using HandyControl.Tools.Interop;

namespace HandyControl.Controls
{
    public class GlowWindow : System.Windows.Window
    {
        internal int DeferGlowChangesCount;

        private IntPtr _ownerForActivate;

        private readonly GlowEdge[] _glowEdges = new GlowEdge[4];

        private DispatcherTimer _makeGlowVisibleTimer;

        private bool _isGlowVisible;

        private bool _useLogicalSizeForRestore;

        private bool _isNonClientStripVisible;

        private bool _updatingZOrder;

        private int _lastWindowPlacement;

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

        static GlowWindow()
        {
            StyleProperty.OverrideMetadata(typeof(GlowWindow), new FrameworkPropertyMetadata(Application.Current.FindResource(ResourceToken.WindowGlow)));
            ResizeModeProperty.OverrideMetadata(typeof(GlowWindow), new FrameworkPropertyMetadata(OnResizeModeChanged));
        }

        public void ChangeOwnerForActivate(IntPtr newOwner)
        {
            _ownerForActivate = newOwner;
        }

        public void ChangeOwner(IntPtr newOwner)
        {
            var _ = new WindowInteropHelper(this)
            {
                Owner = newOwner
            };
            foreach (var current in LoadedGlowWindows) current.ChangeOwner(newOwner);
            UpdateZOrderOfThisAndOwner();
        }

        private static int PressedMouseButtons
        {
            get
            {
                var num = 0;
                if (InteropMethods.IsKeyPressed(1)) num |= 1;
                if (InteropMethods.IsKeyPressed(2)) num |= 2;
                if (InteropMethods.IsKeyPressed(4)) num |= 16;
                if (InteropMethods.IsKeyPressed(5)) num |= 32;
                if (InteropMethods.IsKeyPressed(6)) num |= 64;
                return num;
            }
        }

        private IEnumerable<GlowEdge> LoadedGlowWindows => from w in _glowEdges where w != null select w;

        protected virtual bool ShouldShowGlow
        {
            get
            {
                var handle = this.GetHandle();
                return InteropMethods.IsWindowVisible(handle) && !InteropMethods.IsIconic(handle) &&
                       !InteropMethods.IsZoomed(handle) && ResizeMode != ResizeMode.NoResize;
            }
        }

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

        private static void OnGlowColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            ((GlowWindow)obj).UpdateGlowColors();
        }

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

        internal void EndDeferGlowChanges()
        {
            foreach (var current in LoadedGlowWindows) current.CommitChanges();
        }

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

        private void WmActivate(IntPtr wParam, IntPtr lParam)
        {
            if (_ownerForActivate != IntPtr.Zero)
                InteropMethods.SendMessage(_ownerForActivate, InteropMethods.NOTIFYOWNERACTIVATE, wParam, lParam);
        }

        private void WmWindowPosChanging(IntPtr hwnd, IntPtr lParam)
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

        private static InteropValues.RECT GetClientRectRelativeToWindowRect(IntPtr hWnd)
        {
            InteropMethods.GetWindowRect(hWnd, out var rect);
            InteropMethods.GetClientRect(hWnd, out var result);
            var point = new InteropValues.POINT
            {
                X = 0,
                Y = 0
            };
            InteropMethods.ClientToScreen(hWnd, ref point);
            result.Offset(point.X - rect.Left, point.Y - rect.Top);
            return result;
        }

        private void UpdateMaximizedClipRegion(IntPtr hWnd)
        {
            var clientRectRelativeToWindowRect = GetClientRectRelativeToWindowRect(hWnd);
            if (_isNonClientStripVisible) clientRectRelativeToWindowRect.Bottom++;

            var hRgn = InteropMethods.CreateRectRgnIndirect(ref clientRectRelativeToWindowRect);
            InteropMethods.SetWindowRgn(hWnd, hRgn, InteropMethods.IsWindowVisible(hWnd));
        }

        private IntPtr ComputeRoundRectRegion(int left, int top, int width, int height, int cornerRadius)
        {
            var nWidthEllipse = (int)(2 * cornerRadius * DpiHelper.LogicalToDeviceUnitsScalingFactorX);
            var nHeightEllipse = (int)(2 * cornerRadius * DpiHelper.LogicalToDeviceUnitsScalingFactorY);
            return InteropMethods.CreateRoundRectRgn(left, top, left + width + 1, top + height + 1, nWidthEllipse,
                nHeightEllipse);
        }

        protected void SetRoundRect(IntPtr hWnd, int width, int height)
        {
            var hRgn = ComputeRoundRectRegion(0, 0, width, height, 0);
            InteropMethods.SetWindowRgn(hWnd, hRgn, InteropMethods.IsWindowVisible(hWnd));
        }

        protected virtual bool UpdateClipRegionCore(IntPtr hWnd, int showCmd, ClipRegionChangeType changeType, Int32Rect currentBounds)
        {
            if (showCmd == 3)
            {
                UpdateMaximizedClipRegion(hWnd);
                return true;
            }

            if (changeType == ClipRegionChangeType.FromSize || changeType == ClipRegionChangeType.FromPropertyChange ||
                _lastWindowPlacement != showCmd)
            {
                SetRoundRect(hWnd, currentBounds.Width, currentBounds.Height);
                return true;
            }

            return false;
        }

        private void UpdateClipRegion(IntPtr hWnd, InteropValues.WINDOWPLACEMENT placement, ClipRegionChangeType changeType,
            InteropValues.RECT currentBounds)
        {
            UpdateClipRegionCore(hWnd, placement.showCmd, changeType, currentBounds.ToInt32Rect());
            _lastWindowPlacement = placement.showCmd;
        }

        protected virtual void OnWindowPosChanged(IntPtr hWnd, int showCmd, Int32Rect rcNormalPosition)
        {
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

        private void WmWindowPosChanged(IntPtr hWnd, IntPtr lParam)
        {
            try
            {
                var windowpos = (InteropValues.WINDOWPOS)Marshal.PtrToStructure(lParam, typeof(InteropValues.WINDOWPOS));
                var windowPlacement = InteropMethods.GetWindowPlacement(hWnd);
                var currentBounds = new InteropValues.RECT(windowpos.x, windowpos.y, windowpos.x + windowpos.cx,
                    windowpos.y + windowpos.cy);
                if ((windowpos.flags & 1u) != 1u)
                {
                    UpdateClipRegion(hWnd, windowPlacement, ClipRegionChangeType.FromSize, currentBounds);
                }
                else
                {
                    if ((windowpos.flags & 2u) != 2u)
                        UpdateClipRegion(hWnd, windowPlacement, ClipRegionChangeType.FromPosition, currentBounds);
                }

                OnWindowPosChanged(hWnd, windowPlacement.showCmd, windowPlacement.rcNormalPosition.ToInt32Rect());
                UpdateGlowWindowPositions((windowpos.flags & 64u) == 0u);
                UpdateZOrderOfThisAndOwner();
            }
            catch (Win32Exception)
            {
            }
        }

        internal Rect GetOnScreenPosition(Rect floatRect)
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

        private InteropValues.WINDOWINFO GetWindowInfo(IntPtr hWnd)
        {
            var windowInfo = default(InteropValues.WINDOWINFO);
            windowInfo.cbSize = Marshal.SizeOf(windowInfo);
            InteropMethods.GetWindowInfo(hWnd, ref windowInfo);
            return windowInfo;
        }

        private IntPtr WmNcCalcSize(IntPtr hWnd, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            _isNonClientStripVisible = false;
            var windowPlacement = InteropMethods.GetWindowPlacement(hWnd);
            var flag = windowPlacement.showCmd == 3;
            if (flag)
            {
                var rect = (InteropValues.RECT)Marshal.PtrToStructure(lParam, typeof(InteropValues.RECT));
                InteropMethods.DefWindowProc(hWnd, 131, wParam, lParam);
                var rect2 = (InteropValues.RECT)Marshal.PtrToStructure(lParam, typeof(InteropValues.RECT));
                var monitorinfo = MonitorInfoFromWindow(hWnd);
                if (monitorinfo.rcMonitor.Height == monitorinfo.rcWork.Height &&
                    monitorinfo.rcMonitor.Width == monitorinfo.rcWork.Width)
                {
                    _isNonClientStripVisible = true;
                    rect2.Bottom--;
                }

                rect2.Top = rect.Top + (int)GetWindowInfo(hWnd).cyWindowBorders;
                Marshal.StructureToPtr(rect2, lParam, true);
            }

            handled = true;
            return IntPtr.Zero;
        }

        private IntPtr WmNcHitTest(IntPtr hWnd, IntPtr lParam, ref bool handled)
        {
            if (PresentationSource.FromDependencyObject(this) == null) return new IntPtr(0);

            var point = new Point(InteropMethods.GetXLParam(lParam.ToInt32()),
                InteropMethods.GetYLParam(lParam.ToInt32()));
            var point2 = PointFromScreen(point);
            DependencyObject visualHit = null;
            VisualHelper.HitTestVisibleElements(this, delegate (HitTestResult target)
            {
                visualHit = target.VisualHit;
                return HitTestResultBehavior.Stop;
            }, new PointHitTestParameters(point2));

            var num = 0;
            while (visualHit != null)
            {
                //var nonClientArea = visualHit as INonClientArea;
                //if (nonClientArea != null)
                //{
                //    num = nonClientArea.HitTest(point);
                //    if (num != 0) break;
                //}

                visualHit = visualHit.GetVisualOrLogicalParent();
            }

            if (num == 0) num = 1;
            handled = true;
            return new IntPtr(num);
        }

        private IntPtr WmNcActivate(IntPtr hWnd, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            handled = true;
            return InteropMethods.DefWindowProc(hWnd, 134, wParam, InteropMethods.HRGN_NONE);
        }

        private static void RaiseNonClientMouseMessageAsClient(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam)
        {
            var point = new InteropValues.POINT
            {
                X = InteropMethods.GetXLParam(lParam.ToInt32()),
                Y = InteropMethods.GetYLParam(lParam.ToInt32())
            };

            InteropMethods.ScreenToClient(hWnd, ref point);
            InteropMethods.SendMessage(hWnd, msg + 513 - 161, new IntPtr(PressedMouseButtons),
                InteropMethods.MakeParam(point.X, point.Y));
        }

        private bool IsAeroSnappedToMonitor(IntPtr hWnd)
        {
            var monitorinfo = MonitorInfoFromWindow(hWnd);
            var logicalRect = new Rect(Left, Top, Width, Height);
            logicalRect = logicalRect.LogicalToDeviceUnits();
            return MathHelper.AreClose(monitorinfo.rcWork.Height, logicalRect.Height) && MathHelper.AreClose(monitorinfo.rcWork.Top, logicalRect.Top);
        }

        private void WmSysCommand(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
        {
            var num = InteropMethods.GET_SC_WPARAM(wParam);
            if (num == 61456)
                InteropMethods.RedrawWindow(hWnd, IntPtr.Zero, IntPtr.Zero,
                    InteropValues.RedrawWindowFlags.Invalidate | InteropValues.RedrawWindowFlags.NoChildren |
                    InteropValues.RedrawWindowFlags.UpdateNow | InteropValues.RedrawWindowFlags.Frame);
            if ((num == 61488 || num == 61472 || num == 61456 || num == 61440) && WindowState == WindowState.Normal &&
                !IsAeroSnappedToMonitor(hWnd)) _logicalSizeForRestore = new Rect(Left, Top, Width, Height);
            if (num == 61456 && WindowState == WindowState.Maximized && _logicalSizeForRestore == Rect.Empty)
                _logicalSizeForRestore = new Rect(Left, Top, Width, Height);
            if (num == 61728 && WindowState != WindowState.Minimized && _logicalSizeForRestore.Width > 0.0 &&
                _logicalSizeForRestore.Height > 0.0)
            {
                Left = _logicalSizeForRestore.Left;
                Top = _logicalSizeForRestore.Top;
                Width = _logicalSizeForRestore.Width;
                Height = _logicalSizeForRestore.Height;
                _useLogicalSizeForRestore = true;
            }
        }

        private IntPtr CallDefWindowProcWithoutVisibleStyle(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            var flag = VisualHelper.ModifyStyle(hWnd, 268435456, 0);
            var result = InteropMethods.DefWindowProc(hWnd, msg, wParam, lParam);
            if (flag) VisualHelper.ModifyStyle(hWnd, 0, 268435456);
            handled = true;
            return result;
        }

        protected virtual IntPtr HwndSourceHook(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg <= 71)
            {
                if (msg == 6)
                {
                    WmActivate(wParam, lParam);
                    goto IL_11C;
                }

                if (msg != 12)
                    switch (msg)
                    {
                        case 70:
                            WmWindowPosChanging(hWnd, lParam);
                            goto IL_11C;
                        case 71:
                            WmWindowPosChanged(hWnd, lParam);
                            goto IL_11C;
                        default:
                            goto IL_11C;
                    }
            }
            else
            {
                if (msg <= 166)
                    switch (msg)
                    {
                        case 128:
                            break;
                        case 129:
                        case 130:
                            goto IL_11C;
                        case 131:
                            return WmNcCalcSize(hWnd, wParam, lParam, ref handled);
                        case 132:
                            return WmNcHitTest(hWnd, lParam, ref handled);
                        //case 133:
                        //    return WmNcPaint(hWnd, wParam, lParam, ref handled);
                        case 134:
                            return WmNcActivate(hWnd, wParam, lParam, ref handled);
                        default:
                            switch (msg)
                            {
                                case 164:
                                case 165:
                                case 166:
                                    RaiseNonClientMouseMessageAsClient(hWnd, msg, wParam, lParam);
                                    handled = true;
                                    goto IL_11C;
                                default:
                                    goto IL_11C;
                            }
                    }
                else
                    switch (msg)
                    {
                        case 174:
                        case 175:
                            handled = true;
                            goto IL_11C;
                        default:
                            if (msg != 274) goto IL_11C;
                            WmSysCommand(hWnd, wParam, lParam);
                            goto IL_11C;
                    }
            }

            return CallDefWindowProcWithoutVisibleStyle(hWnd, msg, wParam, lParam, ref handled);
        IL_11C:
            return IntPtr.Zero;
        }

        private void CreateGlowWindowHandles()
        {
            for (var i = 0; i < _glowEdges.Length; i++) GetOrCreateGlowWindow(i).EnsureHandle();
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
            var hwndSource = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
            if (hwndSource != null)
            {
                hwndSource.AddHook(HwndSourceHook);
                CreateGlowWindowHandles();
            }

            base.OnSourceInitialized(e);
        }

        protected enum ClipRegionChangeType
        {
            FromSize,
            FromPosition,
            FromPropertyChange,
            FromUndockSingleTab
        }
    }
}