using System.Windows;
using System.Windows.Controls;
using HandyControl.Data;

namespace HandyControl.Controls;

public class Empty : ContentControl
{
    public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
        nameof(Description), typeof(object), typeof(Empty), new PropertyMetadata(default(object)));

    public object Description
    {
        get => GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public static readonly DependencyProperty LogoProperty = DependencyProperty.Register(
        nameof(Logo), typeof(object), typeof(Empty), new PropertyMetadata(default(object)));

    public object Logo
    {
        get => GetValue(LogoProperty);
        set => SetValue(LogoProperty, value);
    }

    public static readonly DependencyProperty ShowEmptyProperty = DependencyProperty.RegisterAttached(
        "ShowEmpty", typeof(bool), typeof(Empty), new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits));

    public static void SetShowEmpty(DependencyObject element, bool value)
        => element.SetValue(ShowEmptyProperty, ValueBoxes.BooleanBox(value));

    public static bool GetShowEmpty(DependencyObject element)
        => (bool) element.GetValue(ShowEmptyProperty);
}
