using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Styling;
using HandyControl.Data;

namespace HandyControl.Controls;

[TemplatePart(Name = ElementButtonLeft, Type = typeof(Button))]
[TemplatePart(Name = ElementButtonRight, Type = typeof(Button))]
[TemplatePart(Name = ElementButtonFirst, Type = typeof(RadioButton))]
[TemplatePart(Name = ElementMoreLeft, Type = typeof(Control))]
[TemplatePart(Name = ElementPanelMain, Type = typeof(Panel))]
[TemplatePart(Name = ElementMoreRight, Type = typeof(Control))]
[TemplatePart(Name = ElementButtonLast, Type = typeof(RadioButton))]
[TemplatePart(Name = ElementJump, Type = typeof(NumericUpDown))]
[TemplatePart(Name = ElementButtonJump, Type = typeof(Button))]
public class Pagination : TemplatedControl
{
    private const string ElementButtonLeft = "PART_ButtonLeft";
    private const string ElementButtonRight = "PART_ButtonRight";
    private const string ElementButtonFirst = "PART_ButtonFirst";
    private const string ElementMoreLeft = "PART_MoreLeft";
    private const string ElementPanelMain = "PART_PanelMain";
    private const string ElementMoreRight = "PART_MoreRight";
    private const string ElementButtonLast = "PART_ButtonLast";
    private const string ElementJump = "PART_Jump";
    private const string ElementButtonJump = "PART_ButtonJump";

    public static readonly RoutedEvent<FunctionEventArgs<int>> PageUpdatedEvent =
        RoutedEvent.Register<Pagination, FunctionEventArgs<int>>(nameof(PageUpdated), RoutingStrategies.Bubble);

    public static readonly StyledProperty<int> MaxPageCountProperty =
        AvaloniaProperty.Register<Pagination, int>(nameof(MaxPageCount), 1);

    public static readonly StyledProperty<int> DataCountPerPageProperty =
        AvaloniaProperty.Register<Pagination, int>(nameof(DataCountPerPage), 20);

    public static readonly StyledProperty<int> PageIndexProperty =
        AvaloniaProperty.Register<Pagination, int>(nameof(PageIndex), 1);

    public static readonly StyledProperty<int> MaxPageIntervalProperty =
        AvaloniaProperty.Register<Pagination, int>(nameof(MaxPageInterval), 3);

    public static readonly StyledProperty<bool> IsJumpEnabledProperty =
        AvaloniaProperty.Register<Pagination, bool>(nameof(IsJumpEnabled));

    public static readonly StyledProperty<bool> AutoHidingProperty =
        AvaloniaProperty.Register<Pagination, bool>(nameof(AutoHiding), true);

    public static readonly StyledProperty<ControlTheme?> PaginationButtonThemeProperty =
        AvaloniaProperty.Register<Pagination, ControlTheme?>(nameof(PaginationButtonTheme));

    private readonly string _groupName = Guid.NewGuid().ToString("N");

    private Button? _buttonLeft;
    private Button? _buttonRight;
    private RadioButton? _buttonFirst;
    private Control? _moreLeft;
    private Panel? _panelMain;
    private Control? _moreRight;
    private RadioButton? _buttonLast;
    private NumericUpDown? _jumpNumericUpDown;
    private Button? _buttonJump;

    private bool _appliedTemplate;
    private bool _isUpdating;

    static Pagination()
    {
        MaxPageCountProperty.Changed.AddClassHandler<Pagination>((pagination, e) => pagination.OnMaxPageCountChanged(e));
        PageIndexProperty.Changed.AddClassHandler<Pagination>((pagination, e) => pagination.OnPageIndexChanged(e));
        MaxPageIntervalProperty.Changed.AddClassHandler<Pagination>((pagination, _) => pagination.Update());
        DataCountPerPageProperty.Changed.AddClassHandler<Pagination>((pagination, _) => pagination.Update());
        AutoHidingProperty.Changed.AddClassHandler<Pagination>((pagination, _) => pagination.OnAutoHidingChanged());
    }

    public event EventHandler<FunctionEventArgs<int>> PageUpdated
    {
        add => AddHandler(PageUpdatedEvent, value);
        remove => RemoveHandler(PageUpdatedEvent, value);
    }

    public int MaxPageCount
    {
        get => GetValue(MaxPageCountProperty);
        set => SetValue(MaxPageCountProperty, value);
    }

    public int DataCountPerPage
    {
        get => GetValue(DataCountPerPageProperty);
        set => SetValue(DataCountPerPageProperty, value);
    }

    public int PageIndex
    {
        get => GetValue(PageIndexProperty);
        set => SetValue(PageIndexProperty, value);
    }

    public int MaxPageInterval
    {
        get => GetValue(MaxPageIntervalProperty);
        set => SetValue(MaxPageIntervalProperty, value);
    }

    public bool IsJumpEnabled
    {
        get => GetValue(IsJumpEnabledProperty);
        set => SetValue(IsJumpEnabledProperty, value);
    }

    public bool AutoHiding
    {
        get => GetValue(AutoHidingProperty);
        set => SetValue(AutoHidingProperty, value);
    }

    public ControlTheme? PaginationButtonTheme
    {
        get => GetValue(PaginationButtonThemeProperty);
        set => SetValue(PaginationButtonThemeProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        _appliedTemplate = false;

        if (_buttonLeft != null)
        {
            _buttonLeft.Click -= ButtonPrevOnClick;
        }

        if (_buttonRight != null)
        {
            _buttonRight.Click -= ButtonNextOnClick;
        }

        if (_buttonFirst != null)
        {
            _buttonFirst.Checked -= ToggleButtonOnChecked;
        }

        if (_buttonLast != null)
        {
            _buttonLast.Checked -= ToggleButtonOnChecked;
        }

        if (_buttonJump != null)
        {
            _buttonJump.Click -= ButtonJumpOnClick;
        }

        base.OnApplyTemplate(e);

        _buttonLeft = e.NameScope.Find<Button>(ElementButtonLeft);
        _buttonRight = e.NameScope.Find<Button>(ElementButtonRight);
        _buttonFirst = e.NameScope.Find<RadioButton>(ElementButtonFirst);
        _moreLeft = e.NameScope.Find<Control>(ElementMoreLeft);
        _panelMain = e.NameScope.Find<Panel>(ElementPanelMain);
        _moreRight = e.NameScope.Find<Control>(ElementMoreRight);
        _buttonLast = e.NameScope.Find<RadioButton>(ElementButtonLast);
        _jumpNumericUpDown = e.NameScope.Find<NumericUpDown>(ElementJump);
        _buttonJump = e.NameScope.Find<Button>(ElementButtonJump);

        if (_buttonLeft == null || _buttonRight == null || _buttonFirst == null ||
            _moreLeft == null || _panelMain == null || _moreRight == null || _buttonLast == null)
        {
            return;
        }

        _buttonLeft.Click += ButtonPrevOnClick;
        _buttonRight.Click += ButtonNextOnClick;
        _buttonFirst.Checked += ToggleButtonOnChecked;
        _buttonLast.Checked += ToggleButtonOnChecked;
        if (_buttonJump != null)
        {
            _buttonJump.Click += ButtonJumpOnClick;
        }

        _buttonFirst.Content = "1";
        _buttonFirst.GroupName = _groupName;
        _buttonLast.GroupName = _groupName;

        _appliedTemplate = true;
        OnAutoHidingChanged();
        Update();
    }

    private void OnMaxPageCountChanged(AvaloniaPropertyChangedEventArgs e)
    {
        var newValue = Math.Max(e.GetNewValue<int>(), 1);
        if (newValue != MaxPageCount)
        {
            SetCurrentValue(MaxPageCountProperty, newValue);
            return;
        }

        if (PageIndex > newValue)
        {
            SetCurrentValue(PageIndexProperty, newValue);
        }

        OnAutoHidingChanged();
        Update();
    }

    private void OnPageIndexChanged(AvaloniaPropertyChangedEventArgs e)
    {
        var newValue = e.GetNewValue<int>();
        var clamped = Math.Clamp(newValue, 1, Math.Max(MaxPageCount, 1));
        if (clamped != newValue)
        {
            SetCurrentValue(PageIndexProperty, clamped);
            return;
        }

        if (_isUpdating)
        {
            return;
        }

        Update();
        RaiseEvent(new FunctionEventArgs<int>(PageUpdatedEvent, this) { Info = clamped });
    }

    private void OnAutoHidingChanged()
    {
        IsVisible = !AutoHiding || MaxPageCount > 1;
    }

    private void ButtonPrevOnClick(object? sender, RoutedEventArgs e)
    {
        if (PageIndex > 1)
        {
            PageIndex--;
        }
    }

    private void ButtonNextOnClick(object? sender, RoutedEventArgs e)
    {
        if (PageIndex < MaxPageCount)
        {
            PageIndex++;
        }
    }

    private void ButtonJumpOnClick(object? sender, RoutedEventArgs e)
    {
        if (_jumpNumericUpDown?.Value is null)
        {
            return;
        }

        PageIndex = (int)Math.Round(_jumpNumericUpDown.Value.Value);
    }

    private void ToggleButtonOnChecked(object? sender, RoutedEventArgs e)
    {
        if (_isUpdating)
        {
            return;
        }

        if (e.Source is not RadioButton button || button.IsChecked != true)
        {
            return;
        }

        if (int.TryParse(button.Content?.ToString(), out var page))
        {
            PageIndex = page;
        }
    }

    private RadioButton CreateButton(int page)
    {
        var button = new RadioButton
        {
            Theme = PaginationButtonTheme,
            Content = page.ToString(),
            GroupName = _groupName,
            Margin = new Thickness(-1, 0, 0, 0)
        };
        button.Checked += ToggleButtonOnChecked;
        return button;
    }

    private void Update()
    {
        if (!_appliedTemplate || _buttonLeft == null || _buttonRight == null || _buttonFirst == null ||
            _buttonLast == null || _panelMain == null || _moreLeft == null || _moreRight == null)
        {
            return;
        }

        _isUpdating = true;

        _buttonLeft.IsEnabled = PageIndex > 1;
        _buttonRight.IsEnabled = PageIndex < MaxPageCount;

        if (_jumpNumericUpDown != null)
        {
            _jumpNumericUpDown.Minimum = 1;
            _jumpNumericUpDown.Maximum = MaxPageCount;
            _jumpNumericUpDown.Value = PageIndex;
        }

        if (MaxPageInterval <= 0)
        {
            _buttonFirst.IsVisible = false;
            _buttonLast.IsVisible = false;
            _moreLeft.IsVisible = false;
            _moreRight.IsVisible = false;
            _panelMain.Children.Clear();

            var selected = CreateButton(PageIndex);
            selected.IsChecked = true;
            _panelMain.Children.Add(selected);

            _isUpdating = false;
            return;
        }

        _buttonFirst.IsVisible = true;
        _buttonLast.IsVisible = MaxPageCount > 1;
        _buttonLast.Content = MaxPageCount.ToString();

        var right = MaxPageCount - PageIndex;
        var left = PageIndex - 1;
        _moreRight.IsVisible = right > MaxPageInterval;
        _moreLeft.IsVisible = left > MaxPageInterval;

        // 先把所有按钮加到面板，最后才设 IsChecked，
        // 确保 RadioButton 组在同级视觉树中正确互斥。
        _panelMain.Children.Clear();
        RadioButton? selectedButton = null;

        if (PageIndex > 1 && PageIndex < MaxPageCount)
        {
            selectedButton = CreateButton(PageIndex);
            _panelMain.Children.Add(selectedButton);
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
            sub--;
            if (sub <= 1)
            {
                break;
            }

            _panelMain.Children.Insert(0, CreateButton(sub));
        }

        var add = PageIndex;
        for (var i = 0; i < MaxPageInterval - 1; i++)
        {
            add++;
            if (add >= MaxPageCount)
            {
                break;
            }

            _panelMain.Children.Add(CreateButton(add));
        }

        // 所有按钮就位后再勾选，确保 RadioButton 组正确
        if (selectedButton != null)
        {
            selectedButton.IsChecked = true;
        }

        _isUpdating = false;
    }
}
