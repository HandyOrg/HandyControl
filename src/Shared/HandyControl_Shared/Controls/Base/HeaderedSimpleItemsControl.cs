using System.Windows;

namespace HandyControl.Controls;

public class HeaderedSimpleItemsControl : SimpleItemsControl
{
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        nameof(Header), typeof(object), typeof(HeaderedSimpleItemsControl), new PropertyMetadata(default(object)));

    public object Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }
}
