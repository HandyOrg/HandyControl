using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using HandyControl.Data;
using HandyControl.Tools;
using HandyControl.Tools.Extension;
using HandyControl.Tools.Interop;

namespace HandyControl.Controls
{
    [TemplatePart(Name = ElementCanvas, Type = typeof(InkCanvas))]
    [TemplatePart(Name = ElementMaskAreaLeft, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = ElementMaskAreaTop, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = ElementMaskAreaRight, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = ElementMaskAreaBottom, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = ElementTargetArea, Type = typeof(InkCanvas))]
    [TemplatePart(Name = ElementMagnifier, Type = typeof(FrameworkElement))]
    public class ScreenshotWindow : System.Windows.Window
    {
        #region fields

        private readonly Screenshot _screenshot;

        private VisualBrush _visualPreview;

        private Size _viewboxSize;

        private BitmapSource _imageSource;

        private static readonly Guid BmpGuid = new Guid("{b96b3cab-0728-11d3-9d7b-0000f81ef32e}");

        #region const

        private const int IntervalLength = 1;

        private const int IntervalBigLength = 10;

        private const int SnapLength = 4;

        #endregion

        #region IntPtr

        private IntPtr _desktopWindowHandle;

        private IntPtr _mouseOverWindowHandle;

        private readonly IntPtr _screenshotWindowHandle;

        #endregion

        #region status

        private InteropValues.RECT _desktopWindowRect;

        private InteropValues.RECT _targetWindowRect;

        private readonly int[] _flagArr = new int[4];

        private bool _isOut;

        private bool _canDrag;

        private bool _receiveMoveMsg = true;

        private Point _mousePointOld;

        private Point _pointFixed;

        private InteropValues.POINT _pointFloating;

        private bool _saveScreenshot;

        #endregion

        #endregion

        #region Elements

        internal InkCanvas Canvas { get; set; }

        internal FrameworkElement MaskAreaLeft { get; set; }

        internal FrameworkElement MaskAreaTop { get; set; }

        internal FrameworkElement MaskAreaRight { get; set; }

        internal FrameworkElement MaskAreaBottom { get; set; }

        internal FrameworkElement TargetArea { get; set; }

        private FrameworkElement _magnifier;

        #endregion

        #region const

        private const string ElementCanvas = "PART_Canvas";

        private const string ElementMaskAreaLeft = "PART_MaskAreaLeft";

        private const string ElementMaskAreaTop = "PART_MaskAreaTop";

        private const string ElementMaskAreaRight = "PART_MaskAreaRight";

        private const string ElementMaskAreaBottom = "PART_MaskAreaBottom";

        private const string ElementTargetArea = "PART_TargetArea";

        private const string ElementMagnifier = "PART_Magnifier";

        #endregion

        #region prop

        public static readonly DependencyProperty IsDrawingProperty = DependencyProperty.Register(
            "IsDrawing", typeof(bool), typeof(ScreenshotWindow), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool IsDrawing
        {
            get => (bool)GetValue(IsDrawingProperty);
            internal set => SetValue(IsDrawingProperty, value);
        }

        public static readonly DependencyProperty IsSelectingProperty = DependencyProperty.Register(
            "IsSelecting", typeof(bool), typeof(ScreenshotWindow), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool IsSelecting
        {
            get => (bool)GetValue(IsSelectingProperty);
            internal set => SetValue(IsSelectingProperty, value);
        }

        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
            "Size", typeof(Size), typeof(ScreenshotWindow), new PropertyMetadata(default(Size)));

        public Size Size
        {
            get => (Size) GetValue(SizeProperty);
            internal set => SetValue(SizeProperty, value);
        }

        public static readonly DependencyProperty SizeStrProperty = DependencyProperty.Register(
            "SizeStr", typeof(string), typeof(ScreenshotWindow), new PropertyMetadata(default(string)));

        public string SizeStr
        {
            get => (string) GetValue(SizeStrProperty);
            internal set => SetValue(SizeStrProperty, value);
        }

        public static readonly DependencyProperty PixelColorProperty = DependencyProperty.Register(
            "PixelColor", typeof(Color), typeof(ScreenshotWindow), new PropertyMetadata(default(Color)));

        public Color PixelColor
        {
            get => (Color) GetValue(PixelColorProperty);
            internal set => SetValue(PixelColorProperty, value);
        }

        public static readonly DependencyProperty PixelColorStrProperty = DependencyProperty.Register(
            "PixelColorStr", typeof(string), typeof(ScreenshotWindow), new PropertyMetadata(default(string)));

        public string PixelColorStr
        {
            get => (string) GetValue(PixelColorStrProperty);
            internal set => SetValue(PixelColorStrProperty, value);
        }

        public static readonly DependencyPropertyKey PreviewBrushPropertyKey = DependencyProperty.RegisterReadOnly(
            "PreviewBrush", typeof(Brush), typeof(ScreenshotWindow), new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty PreviewBrushProperty = PreviewBrushPropertyKey.DependencyProperty;

        public Brush PreviewBrush
        {
            get => (Brush) GetValue(PreviewBrushProperty);
            set => SetValue(PreviewBrushProperty, value);
        }

        #endregion

        public ScreenshotWindow(Screenshot screenshot)
        {
            Style = ResourceHelper.GetResource<Style>(ResourceToken.Window4ScreenshotStyle);
            _screenshot = screenshot;
            DataContext = this;

            _screenshotWindowHandle = this.GetHandle();
            InteropMethods.EnableWindow(_screenshotWindowHandle, false);

            Loaded += ScreenshotWindow_Loaded;
            Closed += ScreenshotWindow_Closed;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Canvas = GetTemplateChild(ElementCanvas) as InkCanvas;
            MaskAreaLeft = GetTemplateChild(ElementMaskAreaLeft) as FrameworkElement;
            MaskAreaTop = GetTemplateChild(ElementMaskAreaTop) as FrameworkElement;
            MaskAreaRight = GetTemplateChild(ElementMaskAreaRight) as FrameworkElement;
            MaskAreaBottom = GetTemplateChild(ElementMaskAreaBottom) as FrameworkElement;
            TargetArea = GetTemplateChild(ElementTargetArea) as FrameworkElement;
            _magnifier = GetTemplateChild(ElementMagnifier) as FrameworkElement;

            if (_magnifier != null)
            {
                _viewboxSize = new Size(29, 21);
            }

            _visualPreview = new VisualBrush(Canvas)
            {
                ViewboxUnits = BrushMappingMode.Absolute
            };
            SetValue(PreviewBrushPropertyKey, _visualPreview);
            _magnifier.Show();
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape) Close();

            if (IsDrawing)
            {
                switch (e.Key)
                {
                    case Key.Left:
                        {
                            MoveTargetArea(MoveRect(_targetWindowRect, -1, rightFlag: -1));
                        }
                        break;
                    case Key.Up:
                        {
                            MoveTargetArea(MoveRect(_targetWindowRect, bottomFlag: -1, topFlag: -1));
                        }
                        break;
                    case Key.Right:
                        {
                            MoveTargetArea(MoveRect(_targetWindowRect, 1, rightFlag: 1));
                        }
                        break;
                    case Key.Down:
                        {
                            MoveTargetArea(MoveRect(_targetWindowRect, bottomFlag: 1, topFlag: 1));
                        }
                        break;
                    case Key.Enter:
                        _saveScreenshot = true;
                        Close();
                        break;
                }
            }
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e) => _mousePointOld = e.GetPosition(this);

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e) => _magnifier.Collapse();

        protected override void OnPreviewMouseDoubleClick(MouseButtonEventArgs e)
        {
            _saveScreenshot = true;
            Close();
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                UpdateStatus(e.GetPosition(TargetArea));
                return;
            }

            var newPoint = Mouse.GetPosition(this);
            var offsetX = (int)(newPoint.X - _mousePointOld.X);
            var offsetY = (int)(newPoint.Y - _mousePointOld.Y);
            
            if (IsDrawing)
            {
                if (_isOut) return;
                var rect = _targetWindowRect;
                
                if (_canDrag)
                {
                    rect.Left += offsetX;
                    rect.Top += offsetY;
                    rect.Right += offsetX;
                    rect.Bottom += offsetY;
                }
                else
                {
                    var magnifierPos = new InteropValues.POINT((int) newPoint.X, (int) newPoint.Y);

                    if (_flagArr[0] > 0)
                    {
                        _pointFloating.X += offsetX * _flagArr[0];
                        magnifierPos.X = _pointFloating.X;
                    }
                    else if (_flagArr[2] > 0)
                    {
                        _pointFloating.X += offsetX * _flagArr[2];
                        magnifierPos.X = _pointFloating.X - 1;
                    }

                    if (_flagArr[1] > 0)
                    {
                        _pointFloating.Y += offsetY * _flagArr[1];
                        magnifierPos.Y = _pointFloating.Y;
                    }
                    else if (_flagArr[3] > 0)
                    {
                        _pointFloating.Y += offsetY * _flagArr[3];
                        magnifierPos.Y = _pointFloating.Y - 1;
                    }

                    rect.Left = (int)Math.Min(_pointFixed.X, _pointFloating.X);
                    rect.Top = (int)Math.Min(_pointFixed.Y, _pointFloating.Y);
                    rect.Right = (int)Math.Max(_pointFixed.X, _pointFloating.X);
                    rect.Bottom = (int)Math.Max(_pointFixed.Y, _pointFloating.Y);

                    _magnifier.Show();
                    MoveMagnifier(magnifierPos);
                }

                MoveTargetArea(rect);
                _mousePointOld = newPoint;
            }
            else if (IsSelecting)
            {
                var minX = (int)Math.Min(_mousePointOld.X, newPoint.X);
                var maxX = (int)Math.Max(_mousePointOld.X, newPoint.X);
                var minY = (int)Math.Min(_mousePointOld.Y, newPoint.Y);
                var maxY = (int)Math.Max(_mousePointOld.Y, newPoint.Y);

                MoveTargetArea(new InteropValues.RECT(minX, minY, maxX, maxY));
            }
            else if (!IsSelecting && offsetX > 0 && offsetY > 0)
            {
                IsSelecting = true;
            }
        }

        private void ScreenshotWindow_Closed(object sender, EventArgs e)
        {
            if (_saveScreenshot)
            {
                SaveScreenshot();
            }

            StopHooks();
            IsDrawing = false;

            Loaded -= ScreenshotWindow_Loaded;
            Closed -= ScreenshotWindow_Closed;
        }

        private void ScreenshotWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _imageSource = GetDesktopSnapshot();
            var image = new Image
            {
                Source = _imageSource
            };
            RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.NearestNeighbor);
            Canvas.Children.Add(image);
            StartHooks();
            InteropMethods.GetCursorPos(out var point);
            MoveElement(point);
            MoveMagnifier(point);
        }

        private void UpdateStatus(Point point)
        {
            Cursor cursor;

            var leftAbs = Math.Abs(point.X);
            var topAbs = Math.Abs(point.Y);
            var rightAbs = Math.Abs(point.X - _targetWindowRect.Width);
            var downAbs = Math.Abs(point.Y - _targetWindowRect.Height);

            _canDrag = false;
            _isOut = false;
            _flagArr[0] = 0;
            _flagArr[1] = 0;
            _flagArr[2] = 0;
            _flagArr[3] = 0;

            if (leftAbs <= SnapLength)
            {
                if (topAbs > SnapLength)
                {
                    if (downAbs > SnapLength)
                    {
                        // left
                        cursor = Cursors.SizeWE;
                        _pointFixed = new Point(_targetWindowRect.Right, _targetWindowRect.Top);
                        _pointFloating = new InteropValues.POINT(_targetWindowRect.Left, _targetWindowRect.Bottom);
                        _flagArr[0] = 1;
                    }
                    else
                    {
                        //left bottom
                        cursor = Cursors.SizeNESW;
                        _pointFixed = new Point(_targetWindowRect.Right, _targetWindowRect.Top);
                        _pointFloating = new InteropValues.POINT(_targetWindowRect.Left, _targetWindowRect.Bottom);
                        _flagArr[0] = 1;
                        _flagArr[3] = 1;
                    }
                }
                else
                {
                    // left top
                    cursor = Cursors.SizeNWSE;
                    _pointFixed = new Point(_targetWindowRect.Right, _targetWindowRect.Bottom);
                    _pointFloating = new InteropValues.POINT(_targetWindowRect.Left, _targetWindowRect.Top);
                    _flagArr[0] = 1;
                    _flagArr[1] = 1;
                }
            }
            else if (rightAbs > SnapLength)
            {
                if (topAbs > SnapLength)
                {
                    if (downAbs > SnapLength)
                    {
                        if (TargetArea.IsMouseOver)
                        {
                            //drag
                            cursor = Cursors.SizeAll;
                            _canDrag = true;
                        }
                        else
                        {
                            //out
                            cursor = Cursors.Arrow;
                            _isOut = true;
                        }
                    }
                    else
                    {
                        //bottom
                        cursor = Cursors.SizeNS;
                        _pointFixed = new Point(_targetWindowRect.Left, _targetWindowRect.Top);
                        _pointFloating = new InteropValues.POINT(_targetWindowRect.Right, _targetWindowRect.Bottom);
                        _flagArr[3] = 1;
                    }
                }
                else
                {
                    //top
                    cursor = Cursors.SizeNS;
                    _pointFixed = new Point(_targetWindowRect.Right, _targetWindowRect.Bottom);
                    _pointFloating = new InteropValues.POINT(_targetWindowRect.Left, _targetWindowRect.Top);
                    _flagArr[1] = 1;
                }
            }
            else if (rightAbs <= SnapLength)
            {
                if (topAbs > SnapLength)
                {
                    if (downAbs > SnapLength)
                    {
                        //right
                        cursor = Cursors.SizeWE;
                        _pointFixed = new Point(_targetWindowRect.Left, _targetWindowRect.Bottom);
                        _pointFloating = new InteropValues.POINT(_targetWindowRect.Right, _targetWindowRect.Top);
                        _flagArr[2] = 1;
                    }
                    else
                    {
                        //right bottom
                        cursor = Cursors.SizeNWSE;
                        _pointFixed = new Point(_targetWindowRect.Left, _targetWindowRect.Top);
                        _pointFloating = new InteropValues.POINT(_targetWindowRect.Right, _targetWindowRect.Bottom);
                        _flagArr[2] = 1;
                        _flagArr[3] = 1;
                    }
                }
                else
                {
                    // right top
                    cursor = Cursors.SizeNESW;
                    _pointFixed = new Point(_targetWindowRect.Left, _targetWindowRect.Bottom);
                    _pointFloating = new InteropValues.POINT(_targetWindowRect.Right, _targetWindowRect.Top);
                    _flagArr[1] = 1;
                    _flagArr[2] = 1;
                }
            }
            else
            {
                //out
                cursor = Cursors.Arrow;
                _isOut = true;
            }

            TargetArea.Cursor = cursor;
        }

        private void StopHooks()
        {
            MouseHook.Stop();
            MouseHook.StatusChanged -= MouseHook_StatusChanged;
        }

        private void StartHooks()
        {
            MouseHook.Start();
            MouseHook.StatusChanged += MouseHook_StatusChanged;
        }

        private void MouseHook_StatusChanged(object sender, MouseHookEventArgs e)
        {
            switch (e.MessageType)
            {
                case MouseHookMessageType.MouseMove:
                    MoveElement(e.Point);
                    MoveMagnifier(e.Point);
                    break;
                case MouseHookMessageType.LeftButtonDown:
                    _receiveMoveMsg = false;
                    _mousePointOld = new Point(e.Point.X, e.Point.Y);
                    InteropMethods.EnableWindow(_screenshotWindowHandle, true);
                    break;
                case MouseHookMessageType.RightButtonDown:
                    if (!IsDrawing) Close();
                    break;
                case MouseHookMessageType.LeftButtonUp:
                    StopHooks();
                    IsSelecting = false;
                    IsDrawing = true;
                    _magnifier.Collapse();
                    break;
            }
        }

        private void SaveScreenshot()
        {
            var cb = new CroppedBitmap(_imageSource, new Int32Rect(_targetWindowRect.Left, _targetWindowRect.Top, _targetWindowRect.Width, _targetWindowRect.Height));
            _screenshot.OnSnapped(cb);

            Close();
        }

        private BitmapSource GetDesktopSnapshot()
        {
            _desktopWindowHandle = InteropMethods.GetDesktopWindow();
            var hdcSrc = InteropMethods.GetWindowDC(_desktopWindowHandle);
            var hdcDest = InteropMethods.CreateCompatibleDC(hdcSrc);

            InteropMethods.GetWindowRect(_desktopWindowHandle, out _desktopWindowRect);
            var desktopWindowWidth = _desktopWindowRect.Right - _desktopWindowRect.Left;
            var desktopWindowHeight = _desktopWindowRect.Bottom - _desktopWindowRect.Top;

            var hbitmap = InteropMethods.CreateCompatibleBitmap(hdcSrc, desktopWindowWidth, desktopWindowHeight);
            var hOld = InteropMethods.SelectObject(hdcDest, hbitmap);
            InteropMethods.BitBlt(hdcDest, 0, 0, desktopWindowWidth, desktopWindowHeight, hdcSrc, 0, 0, InteropValues.SRCCOPY);
            InteropMethods.SelectObject(hdcDest, hOld);
            InteropMethods.DeleteDC(hdcDest);
            InteropMethods.ReleaseDC(_desktopWindowHandle, hdcSrc);

            var status = InteropMethods.Gdip.GdipCreateBitmapFromHBITMAP(new HandleRef(null, hbitmap), new HandleRef(null, IntPtr.Zero), out var bitmap);
            if (status != InteropMethods.Gdip.Ok) throw InteropMethods.Gdip.StatusException(status);

            using var ms = new MemoryStream();
            status = InteropMethods.Gdip.GdipGetImageEncodersSize(out var numEncoders, out var size);
            if (status != InteropMethods.Gdip.Ok) throw InteropMethods.Gdip.StatusException(status);

            var memory = Marshal.AllocHGlobal(size);
            try
            {
                status = InteropMethods.Gdip.GdipGetImageEncoders(numEncoders, size, memory);
                if (status != InteropMethods.Gdip.Ok) throw InteropMethods.Gdip.StatusException(status);

                var codecInfo = ImageCodecInfo.ConvertFromMemory(memory, numEncoders).FirstOrDefault(item => item.FormatID.Equals(BmpGuid));
                if (codecInfo == null) throw new Exception("ImageCodecInfo is null");

                var encoderParamsMemory = IntPtr.Zero;

                try
                {
                    var g = codecInfo.Clsid;
                    status = InteropMethods.Gdip.GdipSaveImageToStream(new HandleRef(this, bitmap),
                        new InteropValues.ComStreamFromDataStream(ms), ref g,
                        new HandleRef(null, encoderParamsMemory));
                }
                finally
                {
                    if (encoderParamsMemory != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(encoderParamsMemory);
                    }
                }

                if (status != InteropMethods.Gdip.Ok)
                {
                    throw InteropMethods.Gdip.StatusException(status);
                }
            }
            finally
            {
                Marshal.FreeHGlobal(memory);
            }

            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.StreamSource = ms;
            bitmapImage.EndInit();
            bitmapImage.Freeze();
            return bitmapImage;
        }

        private void MoveElement(InteropValues.POINT point)
        {
            if (!_receiveMoveMsg) return;

            var mouseOverWindowHandle = InteropMethods.ChildWindowFromPointEx(_desktopWindowHandle,
                new InteropValues.POINT
                {
                    X = point.X,
                    Y = point.Y
                }, 1 | 2);

            if (mouseOverWindowHandle != _mouseOverWindowHandle && mouseOverWindowHandle != IntPtr.Zero)
            {
                _mouseOverWindowHandle = mouseOverWindowHandle;

                InteropMethods.GetWindowRect(_mouseOverWindowHandle, out var windowRect);
                MoveTargetArea(windowRect);
            }
        }

        private static InteropValues.RECT MoveRect(InteropValues.RECT rect, int leftFlag = 0, int topFlag = 0, int rightFlag = 0, int bottomFlag = 0)
        {
            var moveLength = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)
                ? IntervalBigLength
                : IntervalLength;

            rect.Left += leftFlag * moveLength;
            rect.Top += topFlag * moveLength;
            rect.Right += rightFlag * moveLength;
            rect.Bottom += bottomFlag * moveLength;

            return rect;
        }

        private void MoveTargetArea(InteropValues.RECT rect)
        {
            if (rect.Left < 0)
            {
                rect.Right -= rect.Left;
                rect.Left = 0;
            }

            if (rect.Top < 0)
            {
                rect.Bottom -= rect.Top;
                rect.Top = 0;
            }

            if (rect.Right > _desktopWindowRect.Width)
            {
                rect.Left -= rect.Right - _desktopWindowRect.Width;
                rect.Right = _desktopWindowRect.Width;
            }

            if (rect.Bottom > _desktopWindowRect.Height)
            {
                rect.Top -= rect.Bottom - _desktopWindowRect.Height;
                rect.Bottom = _desktopWindowRect.Height;
            }

            rect.Left = Math.Max(0, rect.Left);
            rect.Top = Math.Max(0, rect.Top);

            var width = rect.Width;
            var height = rect.Height;
            var left = rect.Left;
            var top = rect.Top;

            TargetArea.Width = width;
            TargetArea.Height = height;
            TargetArea.Margin = new Thickness(left, top, 0, 0);

            _targetWindowRect = new InteropValues.RECT(left, top, left + width, top + height);
            Size = _targetWindowRect.Size;
            SizeStr = $"{_targetWindowRect.Width} x {_targetWindowRect.Height}";

            MoveMaskArea();
        }

        private void MoveMaskArea()
        {
            MaskAreaLeft.Width = TargetArea.Margin.Left;
            MaskAreaLeft.Height = _desktopWindowRect.Height;

            MaskAreaTop.Margin = new Thickness(TargetArea.Margin.Left, 0, 0, 0);
            MaskAreaTop.Width = TargetArea.Width;
            MaskAreaTop.Height = TargetArea.Margin.Top;

            MaskAreaRight.Margin = new Thickness(TargetArea.Width + TargetArea.Margin.Left, 0, 0, 0);
            MaskAreaRight.Width = _desktopWindowRect.Width - TargetArea.Margin.Left - TargetArea.Width;
            MaskAreaRight.Height = _desktopWindowRect.Height;

            MaskAreaBottom.Margin = new Thickness(TargetArea.Margin.Left, TargetArea.Height + TargetArea.Margin.Top, 0, 0);
            MaskAreaBottom.Width = TargetArea.Width;
            MaskAreaBottom.Height = _desktopWindowRect.Height - TargetArea.Height - TargetArea.Margin.Top;
        }

        private void MoveMagnifier(InteropValues.POINT point)
        {
            _magnifier.Margin = new Thickness(point.X + 4, point.Y + 26, 0, 0);
            _visualPreview.Viewbox = new Rect(new Point(point.X - _viewboxSize.Width / 2 + 0.5, point.Y - _viewboxSize.Height / 2 + 0.5), _viewboxSize);
        }
    }
}
