using System.Windows;
using HandyControl.Data;

namespace HandyControl.Controls;

/// <summary>
///     规则ItemsControl
/// </summary>
/// <remarks>
///     该类的每一项都具有相同的大小和外边距
/// </remarks>
public class RegularItemsControl : SimpleItemsControl
{
    public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register(
        nameof(ItemWidth), typeof(double), typeof(RegularItemsControl), new PropertyMetadata(ValueBoxes.Double200Box));

    public double ItemWidth
    {
        get => (double) GetValue(ItemWidthProperty);
        set => SetValue(ItemWidthProperty, value);
    }

    public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register(
        nameof(ItemHeight), typeof(double), typeof(RegularItemsControl), new PropertyMetadata(ValueBoxes.Double200Box));

    public double ItemHeight
    {
        get => (double) GetValue(ItemHeightProperty);
        set => SetValue(ItemHeightProperty, value);
    }

    public static readonly DependencyProperty ItemMarginProperty = DependencyProperty.Register(
        nameof(ItemMargin), typeof(Thickness), typeof(RegularItemsControl), new PropertyMetadata(default(Thickness)));

    public Thickness ItemMargin
    {
        get => (Thickness) GetValue(ItemMarginProperty);
        set => SetValue(ItemMarginProperty, value);
    }
}
