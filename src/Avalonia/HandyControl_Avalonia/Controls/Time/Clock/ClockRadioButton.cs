using Avalonia;
using Avalonia.Controls;

namespace HandyControl.Controls;

public class ClockRadioButton : RadioButton
{
    public static readonly StyledProperty<int> NumProperty =
        AvaloniaProperty.Register<ClockRadioButton, int>(nameof(Num));

    public int Num
    {
        get => GetValue(NumProperty);
        set => SetValue(NumProperty, value);
    }
}
