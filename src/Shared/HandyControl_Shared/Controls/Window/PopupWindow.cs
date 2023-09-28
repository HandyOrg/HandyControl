using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Tools;

namespace HandyControl.Controls;

/// <summary>
///     弹出窗口
/// </summary>
[TemplatePart(Name = ElementMainBorder, Type = typeof(Border))]
[TemplatePart(Name = ElementTitleBlock, Type = typeof(TextBlock))]
public class PopupWindow : System.Windows.Window
{
    #region Constants

    private const string ElementMainBorder = "PART_MainBorder";

    private const string ElementTitleBlock = "PART_TitleBlock";

    #endregion Constants

    private Border _mainBorder;

    private TextBlock _titleBlock;

    private bool _showBackground = true;

    private FrameworkElement _targetElement;

    public override void OnApplyTemplate()
    {
        if (_titleBlock != null)
        {
            _titleBlock.MouseLeftButtonDown -= TitleBlock_OnMouseLeftButtonDown;
        }

        base.OnApplyTemplate();

        _mainBorder = GetTemplateChild(ElementMainBorder) as Border;
        _titleBlock = GetTemplateChild(ElementTitleBlock) as TextBlock;

        if (_titleBlock != null)
        {
            _titleBlock.MouseLeftButtonDown += TitleBlock_OnMouseLeftButtonDown;
        }

        if (PopupElement != null)
        {
            _mainBorder.Child = PopupElement;
        }
    }

    internal static readonly DependencyProperty ContentStrProperty = DependencyProperty.Register(
        nameof(ContentStr), typeof(string), typeof(PopupWindow), new PropertyMetadata(default(string)));

    internal string ContentStr
    {
        get => (string) GetValue(ContentStrProperty);
        set => SetValue(ContentStrProperty, value);
    }

    private bool IsDialog { get; set; }

    public PopupWindow()
    {
        CommandBindings.Add(new CommandBinding(ControlCommands.Close, CloseButton_OnClick));
        CommandBindings.Add(new CommandBinding(ControlCommands.Confirm, ButtonOk_OnClick));
        CommandBindings.Add(new CommandBinding(ControlCommands.Cancel, ButtonCancle_OnClick));

        Closing += (sender, args) =>
        {
            if (!IsDialog)
                Owner?.Focus();
        };
        Loaded += (s, e) =>
        {
            if (!_showBackground)
            {
                var point = ArithmeticHelper.CalSafePoint(_targetElement, PopupElement, BorderThickness);
                Left = point.X;
                Top = point.Y;
                Opacity = 1;
            }
        };
        try
        {
            Owner = Application.Current.MainWindow;
        }
        catch
        {
            // ignored
        }
    }

    public PopupWindow(System.Windows.Window owner) : this() => Owner = owner;

    private void CloseButton_OnClick(object sender, RoutedEventArgs e) => Close();

    public FrameworkElement PopupElement { get; set; }

    public static readonly DependencyProperty ShowTitleProperty = DependencyProperty.Register(
        nameof(ShowTitle), typeof(bool), typeof(PopupWindow), new PropertyMetadata(ValueBoxes.TrueBox));

    public bool ShowTitle
    {
        get => (bool) GetValue(ShowTitleProperty);
        set => SetValue(ShowTitleProperty, ValueBoxes.BooleanBox(value));
    }

    public static readonly DependencyProperty ShowCancelProperty = DependencyProperty.Register(
        nameof(ShowCancel), typeof(bool), typeof(PopupWindow), new PropertyMetadata(ValueBoxes.FalseBox));

    public bool ShowCancel
    {
        get => (bool) GetValue(ShowCancelProperty);
        set => SetValue(ShowCancelProperty, ValueBoxes.BooleanBox(value));
    }

    public static readonly DependencyProperty ShowBorderProperty = DependencyProperty.Register(
        nameof(ShowBorder), typeof(bool), typeof(PopupWindow), new PropertyMetadata(ValueBoxes.FalseBox));

    public bool ShowBorder
    {
        get => (bool) GetValue(ShowBorderProperty);
        set => SetValue(ShowBorderProperty, ValueBoxes.BooleanBox(value));
    }

    private void TitleBlock_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            DragMove();
        }
    }

    public void Show(FrameworkElement element, bool showBackground = true)
    {
        if (!showBackground)
        {
            Opacity = 0;
            AllowsTransparency = true;
            WindowStyle = WindowStyle.None;
            ShowTitle = false;
            MinWidth = 0;
            MinHeight = 0;
        }

        _showBackground = showBackground;
        _targetElement = element;
        Show();
    }

    public void ShowDialog(FrameworkElement element, bool showBackground = true)
    {
        if (!showBackground)
        {
            Opacity = 0;
            AllowsTransparency = true;
            WindowStyle = WindowStyle.None;
            ShowTitle = false;
            MinWidth = 0;
            MinHeight = 0;
        }

        _showBackground = showBackground;
        _targetElement = element;
        ShowDialog();
    }

    public void Show(System.Windows.Window element, Point point)
    {
        Left = element.Left + point.X;
        Top = element.Top + point.Y;
        Show();
    }

    public static void Show(string message)
    {
        var window = new PopupWindow
        {
            AllowsTransparency = true,
            WindowStyle = WindowStyle.None,
            ContentStr = message,
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            Background = ResourceHelper.GetResourceInternal<Brush>(ResourceToken.PrimaryBrush)
        };
        window.Show();
    }

    public static bool? ShowDialog(string message, string title = default, bool showCancel = false)
    {
        var window = new PopupWindow
        {
            AllowsTransparency = true,
            WindowStyle = WindowStyle.None,
            ContentStr = message,
            IsDialog = true,
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            ShowBorder = true,
            Title = string.IsNullOrEmpty(title) ? Properties.Langs.Lang.Tip : title,
            ShowCancel = showCancel
        };
        return window.ShowDialog();
    }

    private void ButtonOk_OnClick(object sender, RoutedEventArgs e)
    {
        if (IsDialog)
        {
            DialogResult = true;
        }
        Close();
    }

    private void ButtonCancle_OnClick(object sender, RoutedEventArgs e)
    {
        if (IsDialog)
        {
            DialogResult = false;
        }
        Close();
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        PopupElement = null;
    }
}
