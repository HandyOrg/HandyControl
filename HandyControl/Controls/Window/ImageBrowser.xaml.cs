using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using HandyControl.Tools;
using Microsoft.Win32;

// ReSharper disable once CheckNamespace
namespace HandyControl.Controls
{
    /// <summary>
    ///     图片浏览器
    /// </summary>
    public partial class ImageBrowser
    {
        /// <summary>
        ///     缩放比间隔
        /// </summary>
        private const double ScaleInternal = 0.2;

        static ImageBrowser()
        {
            IsFullScreenProperty.AddOwner(typeof(ImageBrowser), new PropertyMetadata(default(bool)));
        }

        private static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(
            "ImageSource", typeof(BitmapFrame), typeof(ImageBrowser), new PropertyMetadata(default(BitmapFrame)));

        private static readonly DependencyProperty ImageMarginProperty = DependencyProperty.Register(
            "ImageMargin", typeof(Thickness), typeof(ImageBrowser), new PropertyMetadata(default(Thickness)));

        private static readonly DependencyProperty ImageWidthProperty = DependencyProperty.Register(
            "ImageWidth", typeof(double), typeof(ImageBrowser), new PropertyMetadata(default(double)));

        private static readonly DependencyProperty ImageHeightProperty = DependencyProperty.Register(
            "ImageHeight", typeof(double), typeof(ImageBrowser), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty ImageScaleProperty = DependencyProperty.Register(
            "ImageScale", typeof(double), typeof(ImageBrowser), new PropertyMetadata(1.0, OnImageScaleChanged));

        private static readonly DependencyProperty ScaleStrProperty = DependencyProperty.Register(
            "ScaleStr", typeof(string), typeof(ImageBrowser), new PropertyMetadata("100%"));

        private static readonly DependencyProperty ImageRotateProperty = DependencyProperty.Register(
            "ImageRotate", typeof(double), typeof(ImageBrowser), new PropertyMetadata(default(double)));

        private static readonly DependencyProperty ShowSmallImgProperty = DependencyProperty.Register(
            "ShowSmallImg", typeof(bool), typeof(ImageBrowser), new PropertyMetadata(false));

        /// <summary>
        ///     图片保存对话框
        /// </summary>
        private static readonly SaveFileDialog SaveFileDialog = new SaveFileDialog
        {
            Filter = $"{Properties.Langs.Lang.PngImg}|*.png"
        };

        public static readonly DependencyProperty ImgPathProperty = DependencyProperty.Register(
            "ImgPath", typeof(string), typeof(ImageBrowser), new PropertyMetadata(default(string)));

        public string ImgPath
        {
            get => (string)GetValue(ImgPathProperty);
            set => SetValue(ImgPathProperty, value);
        }

        public static readonly DependencyProperty ImgSizeProperty = DependencyProperty.Register(
            "ImgSize", typeof(long), typeof(ImageBrowser), new PropertyMetadata(-1L));

        public long ImgSize
        {
            get => (long)GetValue(ImgSizeProperty);
            set => SetValue(ImgSizeProperty, value);
        }

        /// <summary>
        ///     右下角小图片是否加载过
        /// </summary>
        private bool _borderSmallIsLoaded;

        /// <summary>
        ///     图片实际位置
        /// </summary>
        private Thickness _imgActualMargin;

        /// <summary>
        ///     图片实际旋转角度
        /// </summary>
        private double _imgActualRotate;

        /// <summary>
        ///     图片实际旋缩放比
        /// </summary>
        private double _imgActualScale = 1;

        /// <summary>
        ///     在图片上鼠标移动时的即时位置
        /// </summary>
        private Point _imgCurrentPoint;

        /// <summary>
        ///     在小图片上鼠标移动时的即时位置
        /// </summary>
        private Point _imgSmallCurrentPoint;

        /// <summary>
        ///     鼠标是否在图片上按下左键
        /// </summary>
        private bool _imgIsMouseLeftButtonDown;

        /// <summary>
        ///     鼠标是否在小图片上按下左键
        /// </summary>
        private bool _imgSmallIsMouseLeftButtonDown;

        /// <summary>
        ///     在图片上按下时图片的位置
        /// </summary>
        private Thickness _imgMouseDownMargin;

        /// <summary>
        ///     在小图片上按下时图片的位置
        /// </summary>
        private Thickness _imgSmallMouseDownMargin;

        /// <summary>
        ///     在图片上按下时鼠标的位置
        /// </summary>
        private Point _imgMouseDownPoint;

        /// <summary>
        ///     在小图片上按下时鼠标的位置
        /// </summary>
        private Point _imgSmallMouseDownPoint;

        /// <summary>
        ///     图片长宽比
        /// </summary>
        private double _imgWidHeiScale;

        /// <summary>
        ///     是否已经初始化
        /// </summary>
        private bool _isLoaded;

        /// <summary>
        ///     缩放高度间隔
        /// </summary>
        private double _scaleInternalHeight;

        /// <summary>
        ///     缩放宽度间隔
        /// </summary>
        private double _scaleInternalWidth;

        /// <summary>
        ///     底部BorderBottom（包含一些图片操作）是否显示中
        /// </summary>
        private bool _showBorderBottom;

        /// <summary>
        ///     关闭按钮是否显示中
        /// </summary>
        private bool _showCloseButton;

        /// <summary>
        ///     图片是否可以在x轴方向移动
        /// </summary>
        private bool _canMoveX;

        /// <summary>
        ///     图片是否可以在y轴方向移动
        /// </summary>
        private bool _canMoveY;

        /// <summary>
        ///     图片是否倾斜
        /// </summary>
        private bool _isOblique;

        public ImageBrowser() => InitializeComponent();

        /// <summary>
        ///     带一个图片路径的构造函数
        /// </summary>
        /// <param name="path"></param>
        public ImageBrowser(string path) : this(new Uri(path))
        {

        }

        /// <summary>
        ///     带一个图片Uri的构造函数
        /// </summary>
        /// <param name="uri"></param>
        public ImageBrowser(Uri uri) : this()
        {
            try
            {
                ImageSource = BitmapFrame.Create(uri);
                ImgPath = uri.AbsolutePath;
                if (File.Exists(ImgPath))
                {
                    var info = new FileInfo(ImgPath);
                    ImgSize = info.Length;
                }
            }
            catch
            {
                PopupWindow.ShowDialog(Properties.Langs.Lang.ErrorImgPath);
            }
        }

        private BitmapFrame ImageSource
        {
            get => (BitmapFrame)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        private Thickness ImageMargin
        {
            get => (Thickness)GetValue(ImageMarginProperty);
            set => SetValue(ImageMarginProperty, value);
        }

        private double ImageWidth
        {
            get => (double)GetValue(ImageWidthProperty);
            set => SetValue(ImageWidthProperty, value);
        }

        private double ImageHeight
        {
            get => (double)GetValue(ImageHeightProperty);
            set => SetValue(ImageHeightProperty, value);
        }

        public double ImageScale
        {
            get => (double)GetValue(ImageScaleProperty);
            set => SetValue(ImageScaleProperty, value);
        }

        private string ScaleStr
        {
            get => (string)GetValue(ScaleStrProperty);
            set => SetValue(ScaleStrProperty, value);
        }

        private double ImageRotate
        {
            get => (double)GetValue(ImageRotateProperty);
            set => SetValue(ImageRotateProperty, value);
        }

        private bool ShowSmallImg
        {
            get => (bool)GetValue(ShowSmallImgProperty);
            set => SetValue(ShowSmallImgProperty, value);
        }

        /// <summary>
        ///     图片原始宽度
        /// </summary>
        private double ImageOriWidth { get; set; }

        /// <summary>
        ///     图片原始高度
        /// </summary>
        private double ImageOriHeight { get; set; }

        /// <summary>
        ///     关闭按钮是否显示中
        /// </summary>
        public bool ShowCloseButton
        {
            get => _showCloseButton;
            set
            {
                if (_showCloseButton == value) return;
                GridTop.BeginAnimation(OpacityProperty,
                    value ? AnimationHelper.CreateAnimation(1, 100) : AnimationHelper.CreateAnimation(0, 400));
                _showCloseButton = value;
            }
        }

        /// <summary>
        ///     底部BorderBottom（包含一些图片操作）是否显示中
        /// </summary>
        public bool ShowBorderBottom
        {
            get => _showBorderBottom;
            set
            {
                if (_showBorderBottom == value) return;
                BorderBottom.BeginAnimation(OpacityProperty,
                    value ? AnimationHelper.CreateAnimation(1, 100) : AnimationHelper.CreateAnimation(0, 400));
                _showBorderBottom = value;
            }
        }

        private static void OnImageScaleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ImageBrowser imageBrowser && e.NewValue is double newValue)
            {
                imageBrowser.ImageWidth = imageBrowser.ImageOriWidth * newValue;
                imageBrowser.ImageHeight = imageBrowser.ImageOriHeight * newValue;
                imageBrowser.ScaleStr = $"{newValue * 100:#0}%";
            }
        }

        /// <summary>
        ///     初始化
        /// </summary>
        private void Init()
        {
            if (ImageSource == null) return;

            double width;
            double height;

            if (!_isOblique)
            {
                width = ImageSource.Width;
                height = ImageSource.Height;
            }
            else
            {
                width = ImageSource.Height;
                height = ImageSource.Width;
            }

            ImageWidth = width;
            ImageHeight = height;
            ImageOriWidth = width;
            ImageOriHeight = height;
            _scaleInternalWidth = ImageOriWidth * ScaleInternal;
            _scaleInternalHeight = ImageOriHeight * ScaleInternal;

            if (Math.Abs(height - 0) < 0.001 || Math.Abs(width - 0) < 0.001)
            {
                PopupWindow.ShowDialog(Properties.Langs.Lang.ErrorImgSize);
                return;
            }
            _imgWidHeiScale = width / height;
            var scaleWindow = ActualWidth / ActualHeight;
            if (_imgWidHeiScale > scaleWindow)
            {
                if (width > ActualWidth)
                    ImageScale = ActualWidth / width;
            }
            else if (height > ActualHeight)
            {
                ImageScale = ActualHeight / height;
            }
            ImageMargin = new Thickness((ActualWidth - ImageWidth) / 2, (ActualHeight - ImageHeight) / 2, 0, 0);

            _imgActualScale = ImageScale;
            _imgActualMargin = ImageMargin;
        }

        private void ButtonActual_OnClick(object sender, RoutedEventArgs e)
        {
            var scaleAnimation = AnimationHelper.CreateAnimation(1);
            scaleAnimation.FillBehavior = FillBehavior.Stop;
            _imgActualScale = 1;
            scaleAnimation.Completed += (s, e1) =>
            {
                ImageScale = 1;
                _canMoveX = ImageWidth > ActualWidth;
                _canMoveY = ImageHeight > ActualHeight;
                BorderSmallShowSwitch();
            };
            var thickness = new Thickness((ActualWidth - ImageOriWidth) / 2, (ActualHeight - ImageOriHeight) / 2, 0, 0);
            var marginAnimation = AnimationHelper.CreateAnimation(thickness);
            marginAnimation.FillBehavior = FillBehavior.Stop;
            _imgActualMargin = thickness;
            marginAnimation.Completed += (s, e1) => { ImageMargin = thickness; };

            BeginAnimation(ImageScaleProperty, scaleAnimation);
            BeginAnimation(ImageMarginProperty, marginAnimation);
        }

        private void ButtonReduce_OnClick(object sender, RoutedEventArgs e) => ScaleImg(false);

        private void ButtonEnlarge_OnClick(object sender, RoutedEventArgs e) => ScaleImg(true);

        private void ButtonRotateLeft_OnClick(object sender, RoutedEventArgs e) => RotateImg(_imgActualRotate - 90);

        private void ButtonRotateRight_OnClick(object sender, RoutedEventArgs e) => RotateImg(_imgActualRotate + 90);

        private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog.FileName = $"{DateTime.Now:yyyy-M-d-h-m-s.fff}";
            if (SaveFileDialog.ShowDialog() == true)
            {
                var path = SaveFileDialog.FileName;
                using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(ImageSource));
                    encoder.Save(fileStream);
                }
            }
        }

        private void ButtonWindowsOpen_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(ImgPath);
            }
            catch (Exception exception)
            {
                PopupWindow.ShowDialog(exception.Message);
            }
        }

        private void ButtonScreenChange_OnClick(object sender, RoutedEventArgs e) => WindowState = IsFullScreen ? WindowState.Maximized : WindowState.Normal;

        private void ButtonClose_OnClick(object sender, RoutedEventArgs e) => Close();

        private void CanvasBack_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch
            {
                // ignored
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            MoveImg();
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            _imgIsMouseLeftButtonDown = false;
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            ScaleImg(e.Delta > 0);
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            if (!_isLoaded) return;

            if (ImageWidth < 0.001 || ImageHeight < 0.001) return;
            var scaleX = (ImageMargin.Left + ImageWidth * .5) / sizeInfo.PreviousSize.Width;
            var scaleY = (ImageMargin.Top + ImageHeight * .5) / sizeInfo.PreviousSize.Height;

            var marginX = scaleX * ActualWidth - ImageWidth * .5;
            var marginY = scaleY * ActualHeight - ImageHeight * .5;

            _canMoveX = true;
            _canMoveY = true;
            if (ImageWidth <= ActualWidth)
            {
                _canMoveX = false;
                marginX = (ActualWidth - ImageWidth) / 2;
            }
            else if (Math.Abs(ImageMargin.Left) < 0.001)
            {
                marginX = 0;
            }
            else
            {
                var right = Math.Abs(BorderMove.Width - CanvasSmallImg.ActualWidth + BorderMove.Margin.Left);
                if (right < 0.001)
                {
                    marginX = ActualWidth - ImageWidth;
                }
            }
            if (ImageHeight <= ActualHeight)
            {
                _canMoveY = false;
                marginY = (ActualHeight - ImageHeight) / 2;
            }
            else if (Math.Abs(ImageMargin.Top) < 0.001)
            {
                marginY = 0;
            }
            else
            {
                var top = Math.Abs(BorderMove.Height - CanvasSmallImg.ActualHeight + BorderMove.Margin.Top);
                if (top < 0.001)
                {
                    marginY = ActualHeight - ImageHeight;
                }
            }

            ImageMargin = new Thickness(marginX, marginY, 0, 0);

            BorderSmallShowSwitch();
        }

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            if (e.GetPosition(GridTop).Y > GridTop.ActualHeight) return;
            IsFullScreen = !IsFullScreen;
            ButtonScreenChange_OnClick(null, null);
        }

        private void ImageBrowser_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (_isLoaded) return;
            Init();
            _isLoaded = true;
        }

        private void ImageMain_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!(ImageWidth > ActualWidth || ImageHeight > ActualHeight))
            {
                DragMove();
                return;
            }
            _imgMouseDownPoint = Mouse.GetPosition(CanvasMain);
            _imgMouseDownMargin = ImageMargin;
            _imgIsMouseLeftButtonDown = true;
            e.Handled = true;
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonUp(e);
            _imgIsMouseLeftButtonDown = false;
        }

        /// <summary>
        ///     右下角小图片显示切换
        /// </summary>
        private void BorderSmallShowSwitch()
        {
            if (_canMoveX || _canMoveY)
            {
                if (!_borderSmallIsLoaded)
                {
                    CanvasSmallImg.Background = new VisualBrush(ImageMain);
                    InitBorderSmall();
                    _borderSmallIsLoaded = true;
                }
                ShowSmallImg = true;
                UpdateBorderSmall();
            }
            else
            {
                ShowSmallImg = false;
            }
        }

        /// <summary>
        ///     初始化右下角小图片
        /// </summary>
        private void InitBorderSmall()
        {
            var scaleWindow = CanvasSmallImg.MaxWidth / CanvasSmallImg.MaxHeight;
            if (_imgWidHeiScale > scaleWindow)
            {
                CanvasSmallImg.Width = CanvasSmallImg.MaxWidth;
                CanvasSmallImg.Height = CanvasSmallImg.Width / _imgWidHeiScale;
            }
            else
            {
                CanvasSmallImg.Width = CanvasSmallImg.MaxHeight * _imgWidHeiScale;
                CanvasSmallImg.Height = CanvasSmallImg.MaxHeight;
            }
        }

        /// <summary>
        ///     更新右下角小图片
        /// </summary>
        private void UpdateBorderSmall()
        {
            if (!ShowSmallImg) return;

            var widthMin = Math.Min(ImageWidth, ActualWidth);
            var heightMin = Math.Min(ImageHeight, ActualHeight);

            BorderMove.Width = widthMin / ImageWidth * CanvasSmallImg.Width;
            BorderMove.Height = heightMin / ImageHeight * CanvasSmallImg.Height;

            var marginX = -ImageMargin.Left / ImageWidth * CanvasSmallImg.Width;
            var marginY = -ImageMargin.Top / ImageHeight * CanvasSmallImg.Height;

            var marginXMax = CanvasSmallImg.Width - BorderMove.Width;
            var marginYMax = CanvasSmallImg.Height - BorderMove.Height;

            marginX = Math.Max(0, marginX);
            marginX = Math.Min(marginXMax, marginX);
            marginY = Math.Max(0, marginY);
            marginY = Math.Min(marginYMax, marginY);

            BorderMove.Margin = new Thickness(marginX, marginY, 0, 0);
        }

        /// <summary>
        ///     缩放图片
        /// </summary>
        /// <param name="isEnlarge"></param>
        private void ScaleImg(bool isEnlarge)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed) return;
            var oldImageWidth = ImageWidth;
            var olgImageHeight = ImageHeight;

            var tempScale = isEnlarge ? _imgActualScale + ScaleInternal : _imgActualScale - ScaleInternal;
            if (Math.Abs(tempScale) < ScaleInternal || Math.Abs(tempScale - 50) < 0.001)
                return;

            ImageScale = tempScale;

            var posCanvas = Mouse.GetPosition(CanvasMain);
            var posImg = new Point(posCanvas.X - _imgActualMargin.Left, posCanvas.Y - _imgActualMargin.Top);

            var marginX = .5 * _scaleInternalWidth;
            var marginY = .5 * _scaleInternalHeight;

            if (ImageWidth > ActualWidth)
            {
                _canMoveX = true;
                if (ImageHeight > ActualHeight)
                {
                    _canMoveY = true;
                    marginX = posImg.X / oldImageWidth * _scaleInternalWidth;
                    marginY = posImg.Y / olgImageHeight * _scaleInternalHeight;
                }
                else
                {
                    _canMoveY = false;
                }
            }
            else
            {
                _canMoveY = ImageHeight > ActualHeight;
                _canMoveX = false;
            }

            Thickness thickness;
            if (isEnlarge)
            {
                thickness = new Thickness(_imgActualMargin.Left - marginX, _imgActualMargin.Top - marginY, 0, 0);
            }
            else
            {
                var marginActualX = _imgActualMargin.Left + marginX;
                var marginActualY = _imgActualMargin.Top + marginY;
                var subX = ImageWidth - ActualWidth;
                var subY = ImageHeight - ActualHeight;

                var right = Math.Abs(BorderMove.Width - CanvasSmallImg.ActualWidth + BorderMove.Margin.Left);
                var top = Math.Abs(BorderMove.Height - CanvasSmallImg.ActualHeight + BorderMove.Margin.Top);
                if (Math.Abs(ImageMargin.Left) < 0.001 || right < 0.001)
                {
                    marginActualX = _imgActualMargin.Left + BorderMove.Margin.Left / (CanvasSmallImg.ActualWidth - BorderMove.Width) * _scaleInternalWidth;
                }
                if (Math.Abs(ImageMargin.Top) < 0.001 || top < 0.001)
                {
                    marginActualY = _imgActualMargin.Top + BorderMove.Margin.Top / (CanvasSmallImg.ActualHeight - BorderMove.Height) * _scaleInternalHeight;
                }
                if (subX < 0.001)
                {
                    marginActualX = (ActualWidth - ImageWidth) / 2;
                }
                if (subY < 0.001)
                {
                    marginActualY = (ActualHeight - ImageHeight) / 2;
                }
                thickness = new Thickness(marginActualX, marginActualY, 0, 0);
            }

            ImageMargin = thickness;
            _imgActualScale = tempScale;
            _imgActualMargin = thickness;
            BorderSmallShowSwitch();
        }

        /// <summary>
        ///     旋转图片
        /// </summary>
        /// <param name="rotate"></param>
        private void RotateImg(double rotate)
        {
            _imgActualRotate = rotate;

            _isOblique = ((int)_imgActualRotate - 90) % 180 == 0;
            ShowSmallImg = false;
            Init();
            InitBorderSmall();

            var animation = AnimationHelper.CreateAnimation(rotate);
            animation.Completed += (s, e1) => { ImageRotate = rotate; };
            animation.FillBehavior = FillBehavior.Stop;
            BeginAnimation(ImageRotateProperty, animation);
        }

        /// <summary>
        ///     移动图片
        /// </summary>
        private void MoveImg()
        {
            _imgCurrentPoint = Mouse.GetPosition(CanvasMain);
            ShowCloseButton = _imgCurrentPoint.Y < 200;
            ShowBorderBottom = _imgCurrentPoint.Y > ActualHeight - 200;

            if (_imgIsMouseLeftButtonDown)
            {
                var subX = _imgCurrentPoint.X - _imgMouseDownPoint.X;
                var subY = _imgCurrentPoint.Y - _imgMouseDownPoint.Y;

                var marginX = _imgMouseDownMargin.Left;
                if (ImageWidth > ActualWidth)
                {
                    marginX = _imgMouseDownMargin.Left + subX;
                    if (marginX >= 0)
                    {
                        marginX = 0;
                    }
                    else if (-marginX + ActualWidth >= ImageWidth)
                    {
                        marginX = ActualWidth - ImageWidth;
                    }
                    _canMoveX = true;
                }

                var marginY = _imgMouseDownMargin.Top;
                if (ImageHeight > ActualHeight)
                {
                    marginY = _imgMouseDownMargin.Top + subY;
                    if (marginY >= 0)
                    {
                        marginY = 0;
                    }
                    else if (-marginY + ActualHeight >= ImageHeight)
                    {
                        marginY = ActualHeight - ImageHeight;
                    }
                    _canMoveY = true;
                }

                ImageMargin = new Thickness(marginX, marginY, 0, 0);
                _imgActualMargin = ImageMargin;

                UpdateBorderSmall();
            }
        }

        /// <summary>
        ///     移动小图片
        /// </summary>
        private void MoveSmallImg()
        {
            if (_imgSmallIsMouseLeftButtonDown)
            {
                _imgSmallCurrentPoint = Mouse.GetPosition(CanvasSmallImg);

                var subX = _imgSmallCurrentPoint.X - _imgSmallMouseDownPoint.X;
                var subY = _imgSmallCurrentPoint.Y - _imgSmallMouseDownPoint.Y;

                var marginX = _imgSmallMouseDownMargin.Left + subX;
                if (marginX < 0)
                {
                    marginX = 0;
                }
                else if (marginX + BorderMove.Width >= CanvasSmallImg.ActualWidth)
                {
                    marginX = CanvasSmallImg.ActualWidth - BorderMove.Width;
                }

                var marginY = _imgSmallMouseDownMargin.Top + subY;
                if (marginY < 0)
                {
                    marginY = 0;
                }
                else if (marginY + BorderMove.Height >= CanvasSmallImg.ActualHeight)
                {
                    marginY = CanvasSmallImg.ActualHeight - BorderMove.Height;
                }
                BorderMove.Margin = new Thickness(marginX, marginY, 0, 0);

                var marginActualX = (ActualWidth - ImageWidth) / 2;
                var marginActualY = (ActualHeight - ImageHeight) / 2;

                if (_canMoveX)
                {
                    marginActualX = -marginX / CanvasSmallImg.ActualWidth * ImageWidth;
                }
                if (_canMoveY)
                {
                    marginActualY = -marginY / CanvasSmallImg.ActualHeight * ImageHeight;
                }

                ImageMargin = new Thickness(marginActualX, marginActualY, 0, 0);
                _imgActualMargin = ImageMargin;
            }
        }

        private void CanvasSmallImg_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _imgSmallMouseDownPoint = Mouse.GetPosition(CanvasSmallImg);
            _imgSmallMouseDownMargin = BorderMove.Margin;
            _imgSmallIsMouseLeftButtonDown = true;
            e.Handled = true;
        }

        private void CanvasSmallImg_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e) => _imgSmallIsMouseLeftButtonDown = false;

        private void CanvasSmallImg_OnMouseMove(object sender, MouseEventArgs e) => MoveSmallImg();

        private void CanvasSmallImg_OnMouseLeave(object sender, MouseEventArgs e) => _imgSmallIsMouseLeftButtonDown = false;
    }
}