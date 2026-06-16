using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace HandyControl.Controls;

[TemplatePart(PartCloseButton, typeof(Button))]
public class Tag : HeaderedContentControl
{
    public const string PartCloseButton = "PART_CloseButton";

    private Button? _closeButton;

    public static readonly StyledProperty<bool> ShowCloseButtonProperty =
        AvaloniaProperty.Register<Tag, bool>(nameof(ShowCloseButton), true);

    public bool ShowCloseButton
    {
        get => GetValue(ShowCloseButtonProperty);
        set => SetValue(ShowCloseButtonProperty, value);
    }

    public static readonly StyledProperty<bool> SelectableProperty =
        AvaloniaProperty.Register<Tag, bool>(nameof(Selectable));

    public bool Selectable
    {
        get => GetValue(SelectableProperty);
        set => SetValue(SelectableProperty, value);
    }

    public static readonly StyledProperty<bool> IsSelectedProperty =
        AvaloniaProperty.Register<Tag, bool>(nameof(IsSelected));

    public bool IsSelected
    {
        get => GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    public static readonly RoutedEvent<RoutedEventArgs> SelectedEvent =
        RoutedEvent.Register<Tag, RoutedEventArgs>(nameof(Selected), RoutingStrategies.Bubble);

    public event System.EventHandler<RoutedEventArgs> Selected
    {
        add => AddHandler(SelectedEvent, value);
        remove => RemoveHandler(SelectedEvent, value);
    }

    public static readonly RoutedEvent<CancelRoutedEventArgs> ClosingEvent =
        RoutedEvent.Register<Tag, CancelRoutedEventArgs>(nameof(Closing), RoutingStrategies.Bubble);

    public event System.EventHandler<CancelRoutedEventArgs> Closing
    {
        add => AddHandler(ClosingEvent, value);
        remove => RemoveHandler(ClosingEvent, value);
    }

    public static readonly RoutedEvent<RoutedEventArgs> ClosedEvent =
        RoutedEvent.Register<Tag, RoutedEventArgs>(nameof(Closed), RoutingStrategies.Bubble);

    public event System.EventHandler<RoutedEventArgs> Closed
    {
        add => AddHandler(ClosedEvent, value);
        remove => RemoveHandler(ClosedEvent, value);
    }

    static Tag()
    {
        IsSelectedProperty.Changed.AddClassHandler<Tag>((tag, e) => tag.OnIsSelectedChanged(e));
    }

    private void OnIsSelectedChanged(AvaloniaPropertyChangedEventArgs e)
    {
        var newValue = e.GetNewValue<bool>();
        PseudoClasses.Set(":selected", newValue);
        RaiseEvent(new RoutedEventArgs(SelectedEvent, this));
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        if (_closeButton != null)
        {
            _closeButton.Click -= CloseButton_Click;
        }

        _closeButton = e.NameScope.Find<Button>(PartCloseButton);

        if (_closeButton != null)
        {
            _closeButton.Click += CloseButton_Click;
        }
    }

    private void CloseButton_Click(object? sender, RoutedEventArgs e)
    {
        RequestClose();
    }

    internal void RequestClose()
    {
        var argsClosing = new CancelRoutedEventArgs(ClosingEvent, this);
        RaiseEvent(argsClosing);
        if (argsClosing.Cancel)
        {
            return;
        }

        RaiseEvent(new RoutedEventArgs(ClosedEvent, this));
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        if (Selectable && e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            IsSelected = true;
        }
    }

    public void Hide()
    {
        IsVisible = false;
    }
}
