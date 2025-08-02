using Avalonia;
using Avalonia.Controls;

namespace HandyControl.Controls;

public class TextBlockAttach
{
    public static readonly AttachedProperty<bool> AutoTooltipProperty =
        AvaloniaProperty.RegisterAttached<TextBlockAttach, AvaloniaObject, bool>("AutoTooltip", defaultValue: false);

    public static void SetAutoTooltip(AvaloniaObject element, bool value) =>
        element.SetValue(AutoTooltipProperty, value);

    public static bool GetAutoTooltip(AvaloniaObject element) => element.GetValue(AutoTooltipProperty);

    static TextBlockAttach()
    {
        AutoTooltipProperty.Changed.AddClassHandler<AvaloniaObject>(OnAutoTooltipChanged);
    }

    private static void OnAutoTooltipChanged(AvaloniaObject d, AvaloniaPropertyChangedEventArgs e)
    {
        if (d is not TextBlock ctl)
        {
            return;
        }

        if (e.GetNewValue<bool>())
        {
            UpdateTooltip(ctl);
            ctl.SizeChanged += TextBlock_SizeChanged;
        }
        else
        {
            ctl.SizeChanged -= TextBlock_SizeChanged;
        }
    }

    private static void TextBlock_SizeChanged(object? sender, SizeChangedEventArgs e)
    {
        if (sender is TextBlock textBlock)
        {
            UpdateTooltip(textBlock);
        }
    }

    private static void UpdateTooltip(TextBlock textBlock)
    {
        textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        double width = textBlock.DesiredSize.Width - textBlock.Margin.Left - textBlock.Margin.Right;
        ToolTip.SetTip(textBlock, textBlock.Bounds.Width > width ? textBlock.Text : null);
    }
}
