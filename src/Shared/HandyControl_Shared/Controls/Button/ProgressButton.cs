using System.Windows;
using System.Windows.Controls.Primitives;
using HandyControl.Data;

namespace HandyControl.Controls;

public class ProgressButton : ToggleButton
{
    public static readonly DependencyProperty ProgressStyleProperty = DependencyProperty.Register(
        nameof(ProgressStyle), typeof(Style), typeof(ProgressButton), new PropertyMetadata(default(Style)));

    public Style ProgressStyle
    {
        get => (Style) GetValue(ProgressStyleProperty);
        set => SetValue(ProgressStyleProperty, value);
    }

    public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register(
        nameof(Progress), typeof(double), typeof(ProgressButton), new PropertyMetadata(ValueBoxes.Double0Box));

    public double Progress
    {
        get => (double) GetValue(ProgressProperty);
        set => SetValue(ProgressProperty, value);
    }
}
