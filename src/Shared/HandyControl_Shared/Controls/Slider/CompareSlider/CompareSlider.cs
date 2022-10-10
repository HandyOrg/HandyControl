using System.Windows;
using System.Windows.Controls;

namespace HandyControl.Controls;

public class CompareSlider : Slider
{
    public static readonly DependencyProperty TargetContentProperty = DependencyProperty.Register(
        nameof(TargetContent), typeof(object), typeof(CompareSlider), new PropertyMetadata(default(object)));

    public static readonly DependencyProperty SourceContentProperty = DependencyProperty.Register(
        nameof(SourceContent), typeof(object), typeof(CompareSlider), new PropertyMetadata(default(object)));

    public object TargetContent
    {
        get => GetValue(TargetContentProperty);
        set => SetValue(TargetContentProperty, value);
    }

    public object SourceContent
    {
        get => GetValue(SourceContentProperty);
        set => SetValue(SourceContentProperty, value);
    }
}
