using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Tools;
using HandyControl.Tools.Extension;

namespace HandyControl.Controls;

/// <summary>
///     页码
/// </summary>
[TemplatePart(Name = ElementButtonLeft, Type = typeof(Button))]
[TemplatePart(Name = ElementButtonRight, Type = typeof(Button))]
[TemplatePart(Name = ElementButtonFirst, Type = typeof(RadioButton))]
[TemplatePart(Name = ElementMoreLeft, Type = typeof(FrameworkElement))]
[TemplatePart(Name = ElementPanelMain, Type = typeof(Panel))]
[TemplatePart(Name = ElementMoreRight, Type = typeof(FrameworkElement))]
[TemplatePart(Name = ElementButtonLast, Type = typeof(RadioButton))]
[TemplatePart(Name = ElementJump, Type = typeof(NumericUpDown))]
public class Pagination : Control
{
    #region Constants

    private const string ElementButtonLeft = "PART_ButtonLeft";
    private const string ElementButtonRight = "PART_ButtonRight";
    private const string ElementButtonFirst = "PART_ButtonFirst";
    private const string ElementMoreLeft = "PART_MoreLeft";
    private const string ElementPanelMain = "PART_PanelMain";
    private const string ElementMoreRight = "PART_MoreRight";
    private const string ElementButtonLast = "PART_ButtonLast";
    private const string ElementJump = "PART_Jump";

    #endregion Constants

    #region Data

    private Button _buttonLeft;
    private Button _buttonRight;
    private RadioButton _buttonFirst;
    private FrameworkElement _moreLeft;
    private Panel _panelMain;
    private FrameworkElement _moreRight;
    private RadioButton _buttonLast;
    private NumericUpDown _jumpNumericUpDown;

    private bool _appliedTemplate;

    #endregion Data

    #region Public Events

    /// <summary>
    ///     页面更新事件
    /// </summary>
    public static readonly RoutedEvent PageUpdatedEvent =
        EventManager.RegisterRoutedEvent("PageUpdated", RoutingStrategy.Bubble,
            typeof(EventHandler<FunctionEventArgs<int>>), typeof(Pagination));

    /// <summary>
    ///     页面更新事件
    /// </summary>
    public event EventHandler<FunctionEventArgs<int>> PageUpdated
    {
        add => AddHandler(PageUpdatedEvent, value);
        remove => RemoveHandler(PageUpdatedEvent, value);
    }

    #endregion Public Events

    public Pagination()
    {
        CommandBindings.Add(new CommandBinding(ControlCommands.Prev, ButtonPrev_OnClick));
        CommandBindings.Add(new CommandBinding(ControlCommands.Next, ButtonNext_OnClick));
        CommandBindings.Add(new CommandBinding(ControlCommands.Selected, ToggleButton_OnChecked));
        CommandBindings.Add(new CommandBinding(ControlCommands.Jump, (s, e) => PageIndex = (int) _jumpNumericUpDown.Value));

        OnAutoHidingChanged(AutoHiding);
        Update();
    }

    #region Public Properties

    #region MaxPageCount

    /// <summary>
    ///     最大页数
    /// </summary>
    public static readonly DependencyProperty MaxPageCountProperty = DependencyProperty.Register(
        nameof(MaxPageCount), typeof(int), typeof(Pagination), new PropertyMetadata(ValueBoxes.Int1Box, OnMaxPageCountChanged, CoerceMaxPageCount), ValidateHelper.IsInRangeOfPosIntIncludeZero);

    private static object CoerceMaxPageCount(DependencyObject d, object basevalue)
    {
        var intValue = (int) basevalue;
        return intValue < 1 ? 1 : intValue;
    }

    private static void OnMaxPageCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Pagination pagination)
        {
            if (pagination.PageIndex > pagination.MaxPageCount)
            {
                pagination.PageIndex = pagination.MaxPageCount;
            }

            pagination.CoerceValue(PageIndexProperty);
            pagination.OnAutoHidingChanged(pagination.AutoHiding);
            pagination.Update();
        }
    }

    /// <summary>
    ///     最大页数
    /// </summary>
    public int MaxPageCount
    {
        get => (int) GetValue(MaxPageCountProperty);
        set => SetValue(MaxPageCountProperty, value);
    }

    #endregion MaxPageCount

    #region DataCountPerPage

    /// <summary>
    ///     每页的数据量
    /// </summary>
    public static readonly DependencyProperty DataCountPerPageProperty = DependencyProperty.Register(
        nameof(DataCountPerPage), typeof(int), typeof(Pagination), new PropertyMetadata(20, OnDataCountPerPageChanged),
        ValidateHelper.IsInRangeOfPosInt);

    private static void OnDataCountPerPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Pagination pagination)
        {
            pagination.Update();
        }
    }

    /// <summary>
    ///     每页的数据量
    /// </summary>
    public int DataCountPerPage
    {
        get => (int) GetValue(DataCountPerPageProperty);
        set => SetValue(DataCountPerPageProperty, value);
    }

    #endregion

    #region PageIndex

    /// <summary>
    ///     当前页
    /// </summary>
    public static readonly DependencyProperty PageIndexProperty = DependencyProperty.Register(
        nameof(PageIndex), typeof(int), typeof(Pagination), new PropertyMetadata(ValueBoxes.Int1Box, OnPageIndexChanged, CoercePageIndex), ValidateHelper.IsInRangeOfPosIntIncludeZero);

    private static object CoercePageIndex(DependencyObject d, object basevalue)
    {
        if (d is not Pagination pagination) return 1;

        var intValue = (int) basevalue;
        return intValue < 1
            ? 1
            : intValue > pagination.MaxPageCount
                ? pagination.MaxPageCount
                : intValue;
    }

    private static void OnPageIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Pagination pagination && e.NewValue is int value)
        {
            pagination.Update();
            pagination.RaiseEvent(new FunctionEventArgs<int>(PageUpdatedEvent, pagination)
            {
                Info = value
            });
        }
    }

    /// <summary>
    ///     当前页
    /// </summary>
    public int PageIndex
    {
        get => (int) GetValue(PageIndexProperty);
        set => SetValue(PageIndexProperty, value);
    }

    #endregion PageIndex

    #region MaxPageInterval

    /// <summary>
    ///     表示当前选中的按钮距离左右两个方向按钮的最大间隔（4表示间隔4个按钮，如果超过则用省略号表示）
    /// </summary>
    public static readonly DependencyProperty MaxPageIntervalProperty = DependencyProperty.Register(
        nameof(MaxPageInterval), typeof(int), typeof(Pagination), new PropertyMetadata(3, OnMaxPageIntervalChanged), ValidateHelper.IsInRangeOfPosIntIncludeZero);

    private static void OnMaxPageIntervalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Pagination pagination)
        {
            pagination.Update();
        }
    }

    /// <summary>
    ///     表示当前选中的按钮距离左右两个方向按钮的最大间隔（4表示间隔4个按钮，如果超过则用省略号表示）
    /// </summary>
    public int MaxPageInterval
    {
        get => (int) GetValue(MaxPageIntervalProperty);
        set => SetValue(MaxPageIntervalProperty, value);
    }

    #endregion MaxPageInterval

    #region IsJumpEnabled

    public static readonly DependencyProperty IsJumpEnabledProperty = DependencyProperty.Register(
        nameof(IsJumpEnabled), typeof(bool), typeof(Pagination), new PropertyMetadata(ValueBoxes.FalseBox));

    public bool IsJumpEnabled
    {
        get => (bool) GetValue(IsJumpEnabledProperty);
        set => SetValue(IsJumpEnabledProperty, ValueBoxes.BooleanBox(value));
    }

    #endregion

    #region AutoHiding

    public static readonly DependencyProperty AutoHidingProperty = DependencyProperty.Register(
        nameof(AutoHiding), typeof(bool), typeof(Pagination), new PropertyMetadata(ValueBoxes.TrueBox, OnAutoHidingChanged));

    private static void OnAutoHidingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Pagination pagination)
        {
            pagination.OnAutoHidingChanged((bool) e.NewValue);
        }
    }

    private void OnAutoHidingChanged(bool newValue) => this.Show(!newValue || MaxPageCount > 1);

    public bool AutoHiding
    {
        get => (bool) GetValue(AutoHidingProperty);
        set => SetValue(AutoHidingProperty, ValueBoxes.BooleanBox(value));
    }

    #endregion

    public static readonly DependencyProperty PaginationButtonStyleProperty = DependencyProperty.Register(
        nameof(PaginationButtonStyle), typeof(Style), typeof(Pagination), new PropertyMetadata(default(Style)));

    public Style PaginationButtonStyle
    {
        get => (Style) GetValue(PaginationButtonStyleProperty);
        set => SetValue(PaginationButtonStyleProperty, value);
    }

    #endregion

    #region Public Methods

    public override void OnApplyTemplate()
    {
        _appliedTemplate = false;
        base.OnApplyTemplate();

        _buttonLeft = GetTemplateChild(ElementButtonLeft) as Button;
        _buttonRight = GetTemplateChild(ElementButtonRight) as Button;
        _buttonFirst = GetTemplateChild(ElementButtonFirst) as RadioButton;
        _moreLeft = GetTemplateChild(ElementMoreLeft) as FrameworkElement;
        _panelMain = GetTemplateChild(ElementPanelMain) as Panel;
        _moreRight = GetTemplateChild(ElementMoreRight) as FrameworkElement;
        _buttonLast = GetTemplateChild(ElementButtonLast) as RadioButton;
        _jumpNumericUpDown = GetTemplateChild(ElementJump) as NumericUpDown;

        CheckNull();

        _appliedTemplate = true;
        Update();
    }

    #endregion Public Methods

    #region Private Methods

    private void CheckNull()
    {
        if (_buttonLeft == null || _buttonRight == null || _buttonFirst == null ||
            _moreLeft == null || _panelMain == null || _moreRight == null ||
            _buttonLast == null) throw new Exception();
    }

    /// <summary>
    ///     更新
    /// </summary>
    private void Update()
    {
        if (!_appliedTemplate) return;
        _buttonLeft.IsEnabled = PageIndex > 1;
        _buttonRight.IsEnabled = PageIndex < MaxPageCount;
        if (MaxPageInterval == 0)
        {
            _buttonFirst.Collapse();
            _buttonLast.Collapse();
            _moreLeft.Collapse();
            _moreRight.Collapse();
            _panelMain.Children.Clear();
            var selectButton = CreateButton(PageIndex);
            _panelMain.Children.Add(selectButton);
            selectButton.IsChecked = true;
            return;
        }
        _buttonFirst.Show();
        _buttonLast.Show();
        _moreLeft.Show();
        _moreRight.Show();

        //更新最后一页
        if (MaxPageCount == 1)
        {
            _buttonLast.Collapse();
        }
        else
        {
            _buttonLast.Show();
            _buttonLast.Content = MaxPageCount.ToString();
        }

        //更新省略号
        var right = MaxPageCount - PageIndex;
        var left = PageIndex - 1;
        _moreRight.Show(right > MaxPageInterval);
        _moreLeft.Show(left > MaxPageInterval);

        //更新中间部分
        _panelMain.Children.Clear();
        if (PageIndex > 1 && PageIndex < MaxPageCount)
        {
            var selectButton = CreateButton(PageIndex);
            _panelMain.Children.Add(selectButton);
            selectButton.IsChecked = true;
        }
        else if (PageIndex == 1)
        {
            _buttonFirst.IsChecked = true;
        }
        else
        {
            _buttonLast.IsChecked = true;
        }

        var sub = PageIndex;
        for (var i = 0; i < MaxPageInterval - 1; i++)
        {
            if (--sub > 1)
            {
                _panelMain.Children.Insert(0, CreateButton(sub));
            }
            else
            {
                break;
            }
        }
        var add = PageIndex;
        for (var i = 0; i < MaxPageInterval - 1; i++)
        {
            if (++add < MaxPageCount)
            {
                _panelMain.Children.Add(CreateButton(add));
            }
            else
            {
                break;
            }
        }
    }

    private void ButtonPrev_OnClick(object sender, RoutedEventArgs e) => PageIndex--;

    private void ButtonNext_OnClick(object sender, RoutedEventArgs e) => PageIndex++;

    private RadioButton CreateButton(int page)
    {
        return new()
        {
            Style = PaginationButtonStyle,
            Content = page.ToString()
        };
    }

    private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
    {
        if (e.OriginalSource is not RadioButton button) return;
        if (button.IsChecked == false) return;
        PageIndex = int.Parse(button.Content.ToString());
    }

    #endregion Private Methods
}
