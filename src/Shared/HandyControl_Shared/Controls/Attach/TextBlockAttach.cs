using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HandyControl.Data;
using HandyControl.Tools.Helper;

namespace HandyControl.Controls;

public class TextBlockAttach
{
    public static readonly DependencyProperty AutoTooltipProperty = DependencyProperty.RegisterAttached(
        "AutoTooltip", typeof(bool), typeof(TextBlockAttach), new PropertyMetadata(ValueBoxes.FalseBox, OnAutoTooltipChanged));

    private static void OnAutoTooltipChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is TextBlock ctl)
        {
            if ((bool) e.NewValue)
            {
                UpdateTooltip(ctl);
                ctl.SizeChanged += TextBlock_SizeChanged;
            }
            else
            {
                ctl.SizeChanged -= TextBlock_SizeChanged;
            }
        }
    }

    public static void SetAutoTooltip(DependencyObject element, bool value)
        => element.SetValue(AutoTooltipProperty, ValueBoxes.BooleanBox(value));

    public static bool GetAutoTooltip(DependencyObject element)
        => (bool) element.GetValue(AutoTooltipProperty);

    private static void TextBlock_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (sender is TextBlock textBlock)
        {
            UpdateTooltip(textBlock);
        }
    }

    private static void UpdateTooltip(TextBlock textBlock)
    {
        textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        var width = textBlock.DesiredSize.Width - textBlock.Margin.Left - textBlock.Margin.Right;

        // the code Math.Abs(CalcTextWidth(textBlock) - width) > 1 is not elegant end even bugly,
        // while it does solve the problem of Tooltip failure in some cases
        if (textBlock.RenderSize.Width > width || textBlock.ActualWidth < width || Math.Abs(CalcTextWidth(textBlock) - width) > 1)
        {
            ToolTipService.SetToolTip(textBlock, textBlock.Text);
        }
        else
        {
            ToolTipService.SetToolTip(textBlock, null);
        }
    }

    private static double CalcTextWidth(TextBlock textBlock)
    {
        var formattedText = TextHelper.CreateFormattedText(textBlock.Text, textBlock.FlowDirection,
            new Typeface(textBlock.FontFamily, textBlock.FontStyle, textBlock.FontWeight, textBlock.FontStretch),
            textBlock.FontSize);

        return formattedText.WidthIncludingTrailingWhitespace;
    }
}
