using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using HandyControl.Data;
using HandyControl.Interactivity;

namespace HandyControl.Controls
{
    /// <summary>
    ///     图片浏览器
    /// </summary>
    [TemplatePart(Name = ElementGridTop, Type = typeof(Grid))]
    [TemplatePart(Name = ElementImageViewer, Type = typeof(ImageViewer))]
    public class ImageBrowser : Window
    {
        #region Constants

        private const string ElementGridTop = "PART_GridTop";

        private const string ElementImageViewer = "PART_ImageViewer";

        #endregion Constants

        #region Data

        private Grid _gridTop;

        private ImageViewer _imageViewer;

        #endregion Data

        static ImageBrowser()
        {
            IsFullScreenProperty.AddOwner(typeof(ImageBrowser), new PropertyMetadata(ValueBoxes.FalseBox));
        }    

        public ImageBrowser()
        {
            CommandBindings.Add(new CommandBinding(ControlCommands.Close, ButtonClose_OnClick));

            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            WindowStyle = WindowStyle.None;
            AllowsTransparency = true;
        }

        /// <summary>
        ///     带一个图片Uri的构造函数
        /// </summary>
        /// <param name="uri"></param>
        public ImageBrowser(Uri uri) : this()
        {
            Loaded += (s, e) =>
            {
                try
                {
                    _imageViewer.ImageSource = BitmapFrame.Create(uri);
                    _imageViewer.ImgPath = uri.AbsolutePath;
                    if (File.Exists(_imageViewer.ImgPath))
                    {
                        var info = new FileInfo(_imageViewer.ImgPath);
                        _imageViewer.ImgSize = info.Length;
                    }
                }
                catch
                {
                    MessageBox.Show(Properties.Langs.Lang.ErrorImgPath);
                }
            };
        }

        /// <summary>
        ///     带一个图片路径的构造函数
        /// </summary>
        /// <param name="path"></param>
        public ImageBrowser(string path) : this(new Uri(path))
        {

        }

        public override void OnApplyTemplate()
        {
            if (_gridTop != null)
            {
                _gridTop.MouseLeftButtonDown -= GridTop_OnMouseLeftButtonDown;
            }

            if (_imageViewer != null)
            {
                _imageViewer.MouseLeftButtonDown -= ImageViewer_MouseLeftButtonDown;
            }

            base.OnApplyTemplate();

            _gridTop = GetTemplateChild(ElementGridTop) as Grid;
            _imageViewer = GetTemplateChild(ElementImageViewer) as ImageViewer;

            if (_gridTop != null)
            {
                _gridTop.MouseLeftButtonDown += GridTop_OnMouseLeftButtonDown;
            }

            if (_imageViewer != null)
            {
                _imageViewer.MouseLeftButtonDown += ImageViewer_MouseLeftButtonDown;
            }
        }

        private void ButtonClose_OnClick(object sender, RoutedEventArgs e) => Close();

        private void GridTop_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void ImageViewer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && !(_imageViewer.ImageWidth > ActualWidth || _imageViewer.ImageHeight > ActualHeight))
            {
                DragMove();
            }
        }
    }
}