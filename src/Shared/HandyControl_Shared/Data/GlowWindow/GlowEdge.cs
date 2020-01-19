using System;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using HandyControl.Controls;
using HandyControl.Tools.Interop;

namespace HandyControl.Data
{
    internal class GlowEdge : HwndWrapper
    {
        private const string GlowEdgeClassName = "HandyControlGlowEdge";
        
        private const int GlowDepth = 9;
        
        private const int CornerGripThickness = 18;

        private static ushort _sharedWindowClassAtom;
        
        // ReSharper disable once NotAccessedField.Local
        private static InteropValues.WndProc _sharedWndProc;

        private readonly GlowBitmap[] _activeGlowBitmaps = new GlowBitmap[16];
        
        private readonly GlowBitmap[] _inactiveGlowBitmaps = new GlowBitmap[16];
        
        private readonly Dock _orientation;
        
        private readonly GlowWindow _targetWindow;
        
        private Color _activeGlowColor = Colors.Transparent;
        
        private int _height;
        
        private Color _inactiveGlowColor = Colors.Transparent;
        
        private FieldInvalidationTypes _invalidatedValues;
        
        private bool _isActive;
        
        private bool _isVisible;
        
        private int _left;
        
        private bool _pendingDelayRender;
        
        private int _top;
        
        private int _width;

        internal static long CreatedGlowEdges { get; private set; }

        internal static long DisposedGlowEdges { get; private set; }

        internal GlowEdge(GlowWindow owner, Dock orientation)
        {
            _targetWindow = owner ?? throw new ArgumentNullException(nameof(owner));
            _orientation = orientation;
            CreatedGlowEdges += 1L;
        }

        private bool IsDeferringChanges => _targetWindow.DeferGlowChangesCount > 0;

        private static ushort SharedWindowClassAtom
        {
            get
            {
                if (_sharedWindowClassAtom == 0)
                {
                    var wndclass = default(InteropValues.WNDCLASS);
                    
                    wndclass.cbClsExtra = 0;
                    wndclass.cbWndExtra = 0;
                    wndclass.hbrBackground = IntPtr.Zero;
                    wndclass.hCursor = IntPtr.Zero;
                    wndclass.hIcon = IntPtr.Zero;
                    wndclass.lpfnWndProc = _sharedWndProc = InteropMethods.DefWindowProc;
                    wndclass.lpszClassName = GlowEdgeClassName;
                    wndclass.lpszMenuName = null;
                    wndclass.style = 0u;

                    _sharedWindowClassAtom = InteropMethods.RegisterClass(ref wndclass);
                }

                return _sharedWindowClassAtom;
            }
        }

        internal bool IsVisible
        {
            get => _isVisible;
            set => UpdateProperty(ref _isVisible, value, FieldInvalidationTypes.Render | FieldInvalidationTypes.Visibility);
        }

        internal int Left
        {
            get => _left;
            set => UpdateProperty(ref _left, value, FieldInvalidationTypes.Location);
        }

        internal int Top
        {
            get => _top;
            set => UpdateProperty(ref _top, value, FieldInvalidationTypes.Location);
        }

        internal int Width
        {
            get => _width;
            set => UpdateProperty( ref _width, value, FieldInvalidationTypes.Size | FieldInvalidationTypes.Render);
        }

        internal int Height
        {
            get => _height;
            set => UpdateProperty(ref _height, value, FieldInvalidationTypes.Size | FieldInvalidationTypes.Render);
        }

        internal bool IsActive
        {
            get => _isActive;
            set => UpdateProperty(ref _isActive, value, FieldInvalidationTypes.Render);
        }

        internal Color ActiveGlowColor
        {
            get => _activeGlowColor;
            set => UpdateProperty(ref _activeGlowColor, value, FieldInvalidationTypes.ActiveColor | FieldInvalidationTypes.Render);
        }

        internal Color InactiveGlowColor
        {
            get => _inactiveGlowColor;
            set => UpdateProperty(ref _inactiveGlowColor, value, FieldInvalidationTypes.InactiveColor | FieldInvalidationTypes.Render);
        }

        private IntPtr TargetWindowHandle => new WindowInteropHelper(_targetWindow).Handle;

        protected override bool IsWindowSubclassed => true;

        private bool IsPositionValid => (_invalidatedValues & (FieldInvalidationTypes.Location | FieldInvalidationTypes.Size | FieldInvalidationTypes.Visibility)) == FieldInvalidationTypes.None;

        private void UpdateProperty<T>(ref T field, T value, FieldInvalidationTypes invalidatedValues) where T : struct
        {
            if (!field.Equals(value))
            {
                field = value;
                _invalidatedValues |= invalidatedValues;
                if (!IsDeferringChanges) CommitChanges();
            }
        }

        protected override ushort CreateWindowClassCore() => SharedWindowClassAtom;

        protected override void DestroyWindowClassCore()
        {
        }

        protected override IntPtr CreateWindowCore()
        {
            return InteropMethods.CreateWindowEx(
                524416,
                new IntPtr(WindowClassAtom),
                string.Empty,
                -2046820352,
                0,
                0,
                0,
                0,
                new WindowInteropHelper(_targetWindow).Owner,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero);
        }

        internal void ChangeOwner(IntPtr newOwner) => InteropMethods.SetWindowLongPtr(Handle, InteropValues.GWLP.HWNDPARENT, newOwner);

        protected override IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam)
        {
            if (msg <= 70)
            {
                if (msg == 6) return IntPtr.Zero;
                if (msg == 70)
                {
                    var windowpos = (InteropValues.WINDOWPOS)Marshal.PtrToStructure(lParam, typeof(InteropValues.WINDOWPOS));
                    windowpos.flags |= 16u;
                    Marshal.StructureToPtr(windowpos, lParam, true);
                }
            }
            else
            {
                if (msg != 126)
                {
                    if (msg == 132) return new IntPtr(WmNcHitTest(lParam));
                    switch (msg)
                    {
                        case 161:
                        case 163:
                        case 164:
                        case 166:
                        case 167:
                        case 169:
                        case 171:
                        case 173:
                            {
                                var targetWindowHandle = TargetWindowHandle;
                                InteropMethods.SendMessage(targetWindowHandle, 6, new IntPtr(2), IntPtr.Zero);
                                InteropMethods.SendMessage(targetWindowHandle, msg, wParam, IntPtr.Zero);
                                return IntPtr.Zero;
                            }
                    }
                }
                else
                {
                    if (IsVisible) RenderLayeredWindow();
                }
            }

            return base.WndProc(hwnd, msg, wParam, lParam);
        }

        private int WmNcHitTest(IntPtr lParam)
        {
            var xLParam = InteropMethods.GetXLParam(lParam.ToInt32());
            var yLParam = InteropMethods.GetYLParam(lParam.ToInt32());
            InteropMethods.GetWindowRect(Handle, out var rect);
            switch (_orientation)
            {
                case Dock.Left:
                    if (yLParam - CornerGripThickness < rect.Top) return 13;
                    if (yLParam + CornerGripThickness > rect.Bottom) return 16;
                    return 10;
                case Dock.Top:
                    if (xLParam - CornerGripThickness < rect.Left) return 13;
                    if (xLParam + CornerGripThickness > rect.Right) return 14;
                    return 12;
                case Dock.Right:
                    if (yLParam - CornerGripThickness < rect.Top) return 14;
                    if (yLParam + CornerGripThickness > rect.Bottom) return 17;
                    return 11;
                default:
                    if (xLParam - CornerGripThickness < rect.Left) return 16;
                    if (xLParam + CornerGripThickness > rect.Right) return 17;
                    return 15;
            }
        }

        internal void CommitChanges()
        {
            InvalidateCachedBitmaps();
            UpdateWindowPosCore();
            UpdateLayeredWindowCore();
            _invalidatedValues = FieldInvalidationTypes.None;
        }

        private void InvalidateCachedBitmaps()
        {
            if (_invalidatedValues.HasFlag(FieldInvalidationTypes.ActiveColor)) ClearCache(_activeGlowBitmaps);
            if (_invalidatedValues.HasFlag(FieldInvalidationTypes.InactiveColor)) ClearCache(_inactiveGlowBitmaps);
        }

        private void UpdateWindowPosCore()
        {
            if (_invalidatedValues.HasFlag(FieldInvalidationTypes.Location) ||
                _invalidatedValues.HasFlag(FieldInvalidationTypes.Size) ||
                _invalidatedValues.HasFlag(FieldInvalidationTypes.Visibility))
            {
                var num = 532;
                if (_invalidatedValues.HasFlag(FieldInvalidationTypes.Visibility))
                {
                    if (IsVisible)
                        num |= 64;
                    else
                        num |= 131;
                }

                if (!_invalidatedValues.HasFlag(FieldInvalidationTypes.Location)) num |= 2;
                if (!_invalidatedValues.HasFlag(FieldInvalidationTypes.Size)) num |= 1;
                InteropMethods.SetWindowPos(Handle, IntPtr.Zero, Left, Top, Width, Height, num);
            }
        }

        private void UpdateLayeredWindowCore()
        {
            if (IsVisible && _invalidatedValues.HasFlag(FieldInvalidationTypes.Render))
            {
                if (IsPositionValid)
                {
                    BeginDelayedRender();
                    return;
                }

                CancelDelayedRender();
                RenderLayeredWindow();
            }
        }

        private void BeginDelayedRender()
        {
            if (!_pendingDelayRender)
            {
                _pendingDelayRender = true;
                CompositionTarget.Rendering += CommitDelayedRender;
            }
        }

        private void CancelDelayedRender()
        {
            if (_pendingDelayRender)
            {
                _pendingDelayRender = false;
                CompositionTarget.Rendering -= CommitDelayedRender;
            }
        }

        private void CommitDelayedRender(object sender, EventArgs e)
        {
            CancelDelayedRender();
            if (IsVisible) RenderLayeredWindow();
        }

        private void RenderLayeredWindow()
        {
            using var glowDrawingContext = new GlowDrawingContext(Width, Height);
            if (glowDrawingContext.IsInitialized)
            {
                switch (_orientation)
                {
                    case Dock.Left:
                        DrawLeft(glowDrawingContext);
                        break;
                    case Dock.Top:
                        DrawTop(glowDrawingContext);
                        break;
                    case Dock.Right:
                        DrawRight(glowDrawingContext);
                        break;
                    default:
                        DrawBottom(glowDrawingContext);
                        break;
                }

                var point = new InteropValues.POINT
                {
                    X = Left,
                    Y = Top
                };
                var size = new InteropValues.SIZE
                {
                    cx = Width,
                    cy = Height
                };
                var point2 = new InteropValues.POINT
                {
                    X = 0,
                    Y = 0
                };

                InteropMethods.UpdateLayeredWindow(
                    Handle,
                    glowDrawingContext.ScreenDC,
                    ref point,
                    ref size,
                    glowDrawingContext.WindowDC,
                    ref point2,
                    0u,
                    ref glowDrawingContext.Blend,
                    2u);
            }
        }

        private GlowBitmap GetOrCreateBitmap(GlowDrawingContext drawingContext, GlowBitmapPart bitmapPart)
        {
            GlowBitmap[] array;
            Color color;
            if (IsActive)
            {
                array = _activeGlowBitmaps;
                color = ActiveGlowColor;
            }
            else
            {
                array = _inactiveGlowBitmaps;
                color = InactiveGlowColor;
            }

            return array[(int) bitmapPart] ?? (array[(int) bitmapPart] = GlowBitmap.Create(drawingContext, bitmapPart, color));
        }

        private void ClearCache(GlowBitmap[] cache)
        {
            for (var i = 0; i < cache.Length; i++)
                using (cache[i])
                {
                    cache[i] = null;
                }
        }

        protected override void DisposeManagedResources()
        {
            ClearCache(_activeGlowBitmaps);
            ClearCache(_inactiveGlowBitmaps);
        }

        protected override void DisposeNativeResources()
        {
            base.DisposeNativeResources();
            DisposedGlowEdges += 1L;
        }

        private void DrawLeft(GlowDrawingContext drawingContext)
        {
            var orCreateBitmap = GetOrCreateBitmap(drawingContext, GlowBitmapPart.CornerTopLeft);
            var orCreateBitmap2 = GetOrCreateBitmap(drawingContext, GlowBitmapPart.LeftTop);
            var orCreateBitmap3 = GetOrCreateBitmap(drawingContext, GlowBitmapPart.Left);
            var orCreateBitmap4 = GetOrCreateBitmap(drawingContext, GlowBitmapPart.LeftBottom);
            var orCreateBitmap5 = GetOrCreateBitmap(drawingContext, GlowBitmapPart.CornerBottomLeft);
            var height = orCreateBitmap.Height;
            var num = height + orCreateBitmap2.Height;
            var num2 = drawingContext.Height - orCreateBitmap5.Height;
            var num3 = num2 - orCreateBitmap4.Height;
            var num4 = num3 - num;
            InteropMethods.SelectObject(drawingContext.BackgroundDC, orCreateBitmap.Handle);
            InteropMethods.AlphaBlend(drawingContext.WindowDC, 0, 0, orCreateBitmap.Width, orCreateBitmap.Height,
                drawingContext.BackgroundDC, 0, 0, orCreateBitmap.Width, orCreateBitmap.Height,
                drawingContext.Blend);
            InteropMethods.SelectObject(drawingContext.BackgroundDC, orCreateBitmap2.Handle);
            InteropMethods.AlphaBlend(drawingContext.WindowDC, 0, height, orCreateBitmap2.Width,
                orCreateBitmap2.Height, drawingContext.BackgroundDC, 0, 0, orCreateBitmap2.Width,
                orCreateBitmap2.Height, drawingContext.Blend);
            if (num4 > 0)
            {
                InteropMethods.SelectObject(drawingContext.BackgroundDC, orCreateBitmap3.Handle);
                InteropMethods.AlphaBlend(drawingContext.WindowDC, 0, num, orCreateBitmap3.Width, num4,
                    drawingContext.BackgroundDC, 0, 0, orCreateBitmap3.Width, orCreateBitmap3.Height,
                    drawingContext.Blend);
            }

            InteropMethods.SelectObject(drawingContext.BackgroundDC, orCreateBitmap4.Handle);
            InteropMethods.AlphaBlend(drawingContext.WindowDC, 0, num3, orCreateBitmap4.Width,
                orCreateBitmap4.Height, drawingContext.BackgroundDC, 0, 0, orCreateBitmap4.Width,
                orCreateBitmap4.Height, drawingContext.Blend);
            InteropMethods.SelectObject(drawingContext.BackgroundDC, orCreateBitmap5.Handle);
            InteropMethods.AlphaBlend(drawingContext.WindowDC, 0, num2, orCreateBitmap5.Width,
                orCreateBitmap5.Height, drawingContext.BackgroundDC, 0, 0, orCreateBitmap5.Width,
                orCreateBitmap5.Height, drawingContext.Blend);
        }

        private void DrawRight(GlowDrawingContext drawingContext)
        {
            var orCreateBitmap = GetOrCreateBitmap(drawingContext, GlowBitmapPart.CornerTopRight);
            var orCreateBitmap2 = GetOrCreateBitmap(drawingContext, GlowBitmapPart.RightTop);
            var orCreateBitmap3 = GetOrCreateBitmap(drawingContext, GlowBitmapPart.Right);
            var orCreateBitmap4 = GetOrCreateBitmap(drawingContext, GlowBitmapPart.RightBottom);
            var orCreateBitmap5 = GetOrCreateBitmap(drawingContext, GlowBitmapPart.CornerBottomRight);
            var height = orCreateBitmap.Height;
            var num = height + orCreateBitmap2.Height;
            var num2 = drawingContext.Height - orCreateBitmap5.Height;
            var num3 = num2 - orCreateBitmap4.Height;
            var num4 = num3 - num;
            InteropMethods.SelectObject(drawingContext.BackgroundDC, orCreateBitmap.Handle);
            InteropMethods.AlphaBlend(drawingContext.WindowDC, 0, 0, orCreateBitmap.Width, orCreateBitmap.Height,
                drawingContext.BackgroundDC, 0, 0, orCreateBitmap.Width, orCreateBitmap.Height,
                drawingContext.Blend);
            InteropMethods.SelectObject(drawingContext.BackgroundDC, orCreateBitmap2.Handle);
            InteropMethods.AlphaBlend(drawingContext.WindowDC, 0, height, orCreateBitmap2.Width,
                orCreateBitmap2.Height, drawingContext.BackgroundDC, 0, 0, orCreateBitmap2.Width,
                orCreateBitmap2.Height, drawingContext.Blend);
            if (num4 > 0)
            {
                InteropMethods.SelectObject(drawingContext.BackgroundDC, orCreateBitmap3.Handle);
                InteropMethods.AlphaBlend(drawingContext.WindowDC, 0, num, orCreateBitmap3.Width, num4,
                    drawingContext.BackgroundDC, 0, 0, orCreateBitmap3.Width, orCreateBitmap3.Height,
                    drawingContext.Blend);
            }

            InteropMethods.SelectObject(drawingContext.BackgroundDC, orCreateBitmap4.Handle);
            InteropMethods.AlphaBlend(drawingContext.WindowDC, 0, num3, orCreateBitmap4.Width,
                orCreateBitmap4.Height, drawingContext.BackgroundDC, 0, 0, orCreateBitmap4.Width,
                orCreateBitmap4.Height, drawingContext.Blend);
            InteropMethods.SelectObject(drawingContext.BackgroundDC, orCreateBitmap5.Handle);
            InteropMethods.AlphaBlend(drawingContext.WindowDC, 0, num2, orCreateBitmap5.Width,
                orCreateBitmap5.Height, drawingContext.BackgroundDC, 0, 0, orCreateBitmap5.Width,
                orCreateBitmap5.Height, drawingContext.Blend);
        }

        private void DrawTop(GlowDrawingContext drawingContext)
        {
            var orCreateBitmap = GetOrCreateBitmap(drawingContext, GlowBitmapPart.TopLeft);
            var orCreateBitmap2 = GetOrCreateBitmap(drawingContext, GlowBitmapPart.Top);
            var orCreateBitmap3 = GetOrCreateBitmap(drawingContext, GlowBitmapPart.TopRight);
            var num2 = GlowDepth + orCreateBitmap.Width;
            var num3 = drawingContext.Width - GlowDepth - orCreateBitmap3.Width;
            var num4 = num3 - num2;
            InteropMethods.SelectObject(drawingContext.BackgroundDC, orCreateBitmap.Handle);
            InteropMethods.AlphaBlend(drawingContext.WindowDC, GlowDepth, 0, orCreateBitmap.Width, orCreateBitmap.Height,
                drawingContext.BackgroundDC, 0, 0, orCreateBitmap.Width, orCreateBitmap.Height,
                drawingContext.Blend);
            if (num4 > 0)
            {
                InteropMethods.SelectObject(drawingContext.BackgroundDC, orCreateBitmap2.Handle);
                InteropMethods.AlphaBlend(drawingContext.WindowDC, num2, 0, num4, orCreateBitmap2.Height,
                    drawingContext.BackgroundDC, 0, 0, orCreateBitmap2.Width, orCreateBitmap2.Height,
                    drawingContext.Blend);
            }

            InteropMethods.SelectObject(drawingContext.BackgroundDC, orCreateBitmap3.Handle);
            InteropMethods.AlphaBlend(drawingContext.WindowDC, num3, 0, orCreateBitmap3.Width,
                orCreateBitmap3.Height, drawingContext.BackgroundDC, 0, 0, orCreateBitmap3.Width,
                orCreateBitmap3.Height, drawingContext.Blend);
        }

        private void DrawBottom(GlowDrawingContext drawingContext)
        {
            var orCreateBitmap = GetOrCreateBitmap(drawingContext, GlowBitmapPart.BottomLeft);
            var orCreateBitmap2 = GetOrCreateBitmap(drawingContext, GlowBitmapPart.Bottom);
            var orCreateBitmap3 = GetOrCreateBitmap(drawingContext, GlowBitmapPart.BottomRight);
            var num2 = GlowDepth + orCreateBitmap.Width;
            var num3 = drawingContext.Width - GlowDepth - orCreateBitmap3.Width;
            var num4 = num3 - num2;
            InteropMethods.SelectObject(drawingContext.BackgroundDC, orCreateBitmap.Handle);
            InteropMethods.AlphaBlend(drawingContext.WindowDC, GlowDepth, 0, orCreateBitmap.Width, orCreateBitmap.Height,
                drawingContext.BackgroundDC, 0, 0, orCreateBitmap.Width, orCreateBitmap.Height,
                drawingContext.Blend);
            if (num4 > 0)
            {
                InteropMethods.SelectObject(drawingContext.BackgroundDC, orCreateBitmap2.Handle);
                InteropMethods.AlphaBlend(drawingContext.WindowDC, num2, 0, num4, orCreateBitmap2.Height,
                    drawingContext.BackgroundDC, 0, 0, orCreateBitmap2.Width, orCreateBitmap2.Height,
                    drawingContext.Blend);
            }

            InteropMethods.SelectObject(drawingContext.BackgroundDC, orCreateBitmap3.Handle);
            InteropMethods.AlphaBlend(drawingContext.WindowDC, num3, 0, orCreateBitmap3.Width,
                orCreateBitmap3.Height, drawingContext.BackgroundDC, 0, 0, orCreateBitmap3.Width,
                orCreateBitmap3.Height, drawingContext.Blend);
        }

        internal void UpdateWindowPos()
        {
            var targetWindowHandle = TargetWindowHandle;
            InteropMethods.GetWindowRect(targetWindowHandle, out var rect);
            InteropMethods.GetWindowPlacement(targetWindowHandle);
            if (IsVisible)
                switch (_orientation)
                {
                    case Dock.Left:
                        Left = rect.Left - GlowDepth;
                        Top = rect.Top - GlowDepth;
                        Width = GlowDepth;
                        Height = rect.Height + CornerGripThickness;
                        return;
                    case Dock.Top:
                        Left = rect.Left - GlowDepth;
                        Top = rect.Top - GlowDepth;
                        Width = rect.Width + CornerGripThickness;
                        Height = GlowDepth;
                        return;
                    case Dock.Right:
                        Left = rect.Right;
                        Top = rect.Top - GlowDepth;
                        Width = GlowDepth;
                        Height = rect.Height + CornerGripThickness;
                        return;
                    default:
                        Left = rect.Left - GlowDepth;
                        Top = rect.Bottom;
                        Width = rect.Width + CornerGripThickness;
                        Height = GlowDepth;
                        break;
                }
        }

        [Flags]
        private enum FieldInvalidationTypes
        {
            None = 0,
            Location = 1,
            Size = 2,
            ActiveColor = 4,
            InactiveColor = 8,
            Render = 16,
            Visibility = 32
        }
    }
}
