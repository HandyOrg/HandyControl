using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;

namespace HandyControl.Controls;

[TemplatePart(ElementButtonConfirm, typeof(Button))]
public abstract class ClockBase : TemplatedControl
{
    protected const string ElementButtonConfirm = "PART_ButtonConfirm";

    protected Button? ButtonConfirm;

    protected bool AppliedTemplate;

    public event Action? Confirmed;

    public event EventHandler<DateTime>? DisplayTimeChanged;

    public event EventHandler<DateTime?>? SelectedTimeChanged;

    public static readonly StyledProperty<string> TimeFormatProperty =
        AvaloniaProperty.Register<ClockBase, string>(nameof(TimeFormat), "HH:mm:ss");

    public string TimeFormat
    {
        get => GetValue(TimeFormatProperty);
        set => SetValue(TimeFormatProperty, value);
    }

    public static readonly StyledProperty<DateTime?> SelectedTimeProperty =
        AvaloniaProperty.Register<ClockBase, DateTime?>(nameof(SelectedTime),
            defaultBindingMode: BindingMode.TwoWay);

    public DateTime? SelectedTime
    {
        get => GetValue(SelectedTimeProperty);
        set => SetValue(SelectedTimeProperty, value);
    }

    public static readonly StyledProperty<DateTime> DisplayTimeProperty =
        AvaloniaProperty.Register<ClockBase, DateTime>(nameof(DisplayTime), DateTime.Now,
            defaultBindingMode: BindingMode.TwoWay);

    public DateTime DisplayTime
    {
        get => GetValue(DisplayTimeProperty);
        set => SetValue(DisplayTimeProperty, value);
    }

    internal static readonly StyledProperty<bool> ShowConfirmButtonProperty =
        AvaloniaProperty.Register<ClockBase, bool>(nameof(ShowConfirmButton));

    internal bool ShowConfirmButton
    {
        get => GetValue(ShowConfirmButtonProperty);
        set => SetValue(ShowConfirmButtonProperty, value);
    }

    static ClockBase()
    {
        SelectedTimeProperty.Changed.AddClassHandler<ClockBase>((o, e) => o.OnSelectedTimePropertyChanged(e));
        DisplayTimeProperty.Changed.AddClassHandler<ClockBase>((o, e) => o.OnDisplayTimePropertyChanged(e));
    }

    private void OnSelectedTimePropertyChanged(AvaloniaPropertyChangedEventArgs e)
    {
        var v = (DateTime?) e.NewValue;
        DisplayTime = v ?? DateTime.Now;
        SelectedTimeChanged?.Invoke(this, v);
    }

    private void OnDisplayTimePropertyChanged(AvaloniaPropertyChangedEventArgs e)
    {
        var v = (DateTime) (e.NewValue ?? DateTime.Now);
        Update(v);
        DisplayTimeChanged?.Invoke(this, v);
    }

    protected void ButtonConfirm_OnClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        SelectedTime = DisplayTime;
        Confirmed?.Invoke();
    }

    internal abstract void Update(DateTime time);

    public virtual void OnClockClosed()
    {
    }

    public virtual void OnClockOpened()
    {
    }
}
