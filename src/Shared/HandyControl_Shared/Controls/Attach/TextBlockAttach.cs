﻿using System.Windows;
using System.Windows.Controls;
using HandyControl.Data;

namespace HandyControl.Controls
{
    public class TextBlockAttach
    {
        public static readonly DependencyProperty AutoTooltipProperty = DependencyProperty.RegisterAttached(
            "AutoTooltip", typeof(bool), typeof(TextBlockAttach), new PropertyMetadata(ValueBoxes.FalseBox, OnAutoTooltipChanged));

        private static void OnAutoTooltipChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBlock ctl)
            {
                if ((bool)e.NewValue)
                {
                    ctl.TextTrimming = TextTrimming.WordEllipsis;
                    UpdateTooltip(ctl);
                    ctl.SizeChanged += TextBlock_SizeChanged;
                }
                else
                {
                    ctl.ClearValue(TextBlock.TextTrimmingProperty);
                    ctl.SizeChanged -= TextBlock_SizeChanged;
                }
            }
        }

        public static void SetAutoTooltip(DependencyObject element, bool value)
            => element.SetValue(AutoTooltipProperty, value);

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

            ToolTipService.SetToolTip(textBlock,
                textBlock.RenderSize.Width > width || textBlock.ActualWidth < width ? textBlock.Text : null);
        }
    }
}