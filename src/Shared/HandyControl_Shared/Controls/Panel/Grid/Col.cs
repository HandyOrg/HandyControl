using System;
using System.Windows;
using System.Windows.Controls;
using HandyControl.Data;
using HandyControl.Tools.Extension;

namespace HandyControl.Controls;

public class Col : ContentControl
{
    public static readonly DependencyProperty LayoutProperty = DependencyProperty.Register(
        nameof(Layout), typeof(ColLayout), typeof(Col), new PropertyMetadata(default(ColLayout)));

    public ColLayout Layout
    {
        get => (ColLayout) GetValue(LayoutProperty);
        set => SetValue(LayoutProperty, value);
    }

    public static readonly DependencyProperty OffsetProperty = DependencyProperty.Register(
        nameof(Offset), typeof(int), typeof(Col), new PropertyMetadata(ValueBoxes.Int0Box));

    public int Offset
    {
        get => (int) GetValue(OffsetProperty);
        set => SetValue(OffsetProperty, value);
    }

    public static readonly DependencyProperty SpanProperty = DependencyProperty.Register(
        nameof(Span), typeof(int), typeof(Col), new PropertyMetadata(24), OnSpanValidate);

    private static bool OnSpanValidate(object value)
    {
        var v = (int) value;
        return v is >= 1 and <= 24;
    }

    public int Span
    {
        get => (int) GetValue(SpanProperty);
        set => SetValue(SpanProperty, value);
    }

    public static readonly DependencyProperty IsFixedProperty = DependencyProperty.Register(
        nameof(IsFixed), typeof(bool), typeof(Col), new PropertyMetadata(ValueBoxes.FalseBox));

    public bool IsFixed
    {
        get => (bool) GetValue(IsFixedProperty);
        set => SetValue(IsFixedProperty, ValueBoxes.BooleanBox(value));
    }

    internal int GetLayoutCellCount(ColLayoutStatus status)
    {
        var result = 0;

        if (Layout != null)
        {
            if (!IsFixed)
            {
                switch (status)
                {
                    case ColLayoutStatus.Xs:
                        result = Layout.Xs;
                        break;
                    case ColLayoutStatus.Sm:
                        result = Layout.Sm;
                        break;
                    case ColLayoutStatus.Md:
                        result = Layout.Md;
                        break;
                    case ColLayoutStatus.Lg:
                        result = Layout.Lg;
                        break;
                    case ColLayoutStatus.Xl:
                        result = Layout.Xl;
                        break;
                    case ColLayoutStatus.Xxl:
                        result = Layout.Xxl;
                        break;
                    case ColLayoutStatus.Auto:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(status), status, null);
                }
            }
        }
        else
        {
            result = Span;
        }

        return result;
    }
}
