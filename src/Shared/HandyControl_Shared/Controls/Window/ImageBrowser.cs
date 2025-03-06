using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using HandyControl.Data;
using HandyControl.Interactivity;

namespace HandyControl.Controls;

/// <summary>
///     图片浏览器
/// </summary>
[TemplatePart(Name = ElementPanelTop, Type = typeof(Panel))]
[TemplatePart(Name = ElementImageViewer, Type = typeof(ImageViewer))]
public class ImageBrowser : Window
{
    #region Constants

    private const string ElementPanelTop = "PART_PanelTop";

    private const string ElementImageViewer = "PART_ImageViewer";

    #endregion Constants

    #region Data

    private Panel _panelTop;

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
        Topmost = true;
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
        if (_panelTop != null)
        {
            _panelTop.MouseLeftButtonDown -= PanelTopOnMouseLeftButtonDown;
        }

        if (_imageViewer != null)
        {
            _imageViewer.MouseLeftButtonDown -= ImageViewer_MouseLeftButtonDown;
        }

        base.OnApplyTemplate();

        _panelTop = GetTemplateChild(ElementPanelTop) as Panel;
        _imageViewer = GetTemplateChild(ElementImageViewer) as ImageViewer;

        if (_panelTop != null)
        {
            _panelTop.MouseLeftButtonDown += PanelTopOnMouseLeftButtonDown;
        }

        if (_imageViewer != null)
        {
            _imageViewer.MouseLeftButtonDown += ImageViewer_MouseLeftButtonDown;
        }
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        _imageViewer?.Dispose();
        base.OnClosing(e);
    }

    private void ButtonClose_OnClick(object sender, RoutedEventArgs e) => Close();

    private void PanelTopOnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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
