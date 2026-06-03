using System.Windows;
using System.Windows.Media;
using HandyControl.Data;

namespace HandyControl.Controls;

public class VisualElement
{
    public static readonly DependencyProperty HighlightBrushProperty = DependencyProperty.RegisterAttached(
        "HighlightBrush", typeof(Brush), typeof(VisualElement),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

    public static void SetHighlightBrush(DependencyObject element, Brush value)
        => element.SetValue(HighlightBrushProperty, value);

    public static Brush GetHighlightBrush(DependencyObject element)
        => (Brush) element.GetValue(HighlightBrushProperty);

    public static readonly DependencyProperty HighlightBackgroundProperty = DependencyProperty.RegisterAttached(
        "HighlightBackground", typeof(Brush), typeof(VisualElement),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

    public static void SetHighlightBackground(DependencyObject element, Brush value)
        => element.SetValue(HighlightBackgroundProperty, value);

    public static Brush GetHighlightBackground(DependencyObject element)
        => (Brush) element.GetValue(HighlightBackgroundProperty);

    public static readonly DependencyProperty HighlightBorderBrushProperty = DependencyProperty.RegisterAttached(
        "HighlightBorderBrush", typeof(Brush), typeof(VisualElement),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

    public static void SetHighlightBorderBrush(DependencyObject element, Brush value)
        => element.SetValue(HighlightBorderBrushProperty, value);

    public static Brush GetHighlightBorderBrush(DependencyObject element)
        => (Brush) element.GetValue(HighlightBorderBrushProperty);

    public static readonly DependencyProperty HighlightForegroundProperty = DependencyProperty.RegisterAttached(
        "HighlightForeground", typeof(Brush), typeof(VisualElement),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

    public static void SetHighlightForeground(DependencyObject element, Brush value)
        => element.SetValue(HighlightForegroundProperty, value);

    public static Brush GetHighlightForeground(DependencyObject element)
        => (Brush) element.GetValue(HighlightForegroundProperty);

    public static readonly DependencyProperty TextProperty = DependencyProperty.RegisterAttached(
        "Text", typeof(string), typeof(VisualElement), new PropertyMetadata(default(string)));

    public static void SetText(DependencyObject element, string value)
        => element.SetValue(TextProperty, value);

    public static string GetText(DependencyObject element)
        => (string) element.GetValue(TextProperty);

    public static readonly DependencyProperty StaysHighlightedProperty = DependencyProperty.RegisterAttached(
        "StaysHighlighted", typeof(bool), typeof(VisualElement),
        new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits));

    public static void SetStaysHighlighted(DependencyObject element, bool value)
        => element.SetValue(StaysHighlightedProperty, value);

    public static bool GetStaysHighlighted(DependencyObject element)
        => (bool) element.GetValue(StaysHighlightedProperty);
}
