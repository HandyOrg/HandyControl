using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using HandyControl.Data;

namespace HandyControl.Controls;

public class TitleElement
{
    public static readonly AttachedProperty<string> TitleProperty =
        AvaloniaProperty.RegisterAttached<TitleElement, AvaloniaObject, string>("Title");

    public static void SetTitle(AvaloniaObject element, string value) => element.SetValue(TitleProperty, value);

    public static string GetTitle(AvaloniaObject element) => element.GetValue(TitleProperty);

    public static readonly AttachedProperty<IBrush?> BackgroundProperty =
        AvaloniaProperty.RegisterAttached<TitleElement, AvaloniaObject, IBrush?>("Background", inherits: true);

    public static void SetBackground(AvaloniaObject element, IBrush? value) =>
        element.SetValue(BackgroundProperty, value);

    public static IBrush? GetBackground(AvaloniaObject element) => element.GetValue(BackgroundProperty);

    public static readonly AttachedProperty<IBrush?> ForegroundProperty =
        AvaloniaProperty.RegisterAttached<TitleElement, AvaloniaObject, IBrush?>("Foreground", inherits: true);

    public static void SetForeground(AvaloniaObject element, IBrush? value) =>
        element.SetValue(ForegroundProperty, value);

    public static IBrush? GetForeground(AvaloniaObject element) => element.GetValue(ForegroundProperty);

    public static readonly AttachedProperty<IBrush?> BorderBrushProperty =
        AvaloniaProperty.RegisterAttached<TitleElement, AvaloniaObject, IBrush?>("BorderBrush", inherits: true);

    public static void SetBorderBrush(AvaloniaObject element, IBrush? value) =>
        element.SetValue(BorderBrushProperty, value);

    public static IBrush? GetBorderBrush(AvaloniaObject element) => element.GetValue(BorderBrushProperty);

    public static readonly AttachedProperty<TitlePlacementType> TitlePlacementProperty =
        AvaloniaProperty.RegisterAttached<TitleElement, AvaloniaObject, TitlePlacementType>("TitlePlacement",
            defaultValue: TitlePlacementType.Top, inherits: true);

    public static void SetTitlePlacement(AvaloniaObject element, TitlePlacementType value) =>
        element.SetValue(TitlePlacementProperty, value);

    public static TitlePlacementType GetTitlePlacement(AvaloniaObject element) =>
        element.GetValue(TitlePlacementProperty);

    public static readonly AttachedProperty<GridLength> TitleWidthProperty =
        AvaloniaProperty.RegisterAttached<TitleElement, AvaloniaObject, GridLength>("TitleWidth",
            defaultValue: GridLength.Auto, inherits: true);

    public static void SetTitleWidth(AvaloniaObject element, GridLength value) =>
        element.SetValue(TitleWidthProperty, value);

    public static GridLength GetTitleWidth(AvaloniaObject element) => element.GetValue(TitleWidthProperty);

    public static readonly AttachedProperty<HorizontalAlignment> HorizontalAlignmentProperty =
        AvaloniaProperty.RegisterAttached<TitleElement, AvaloniaObject, HorizontalAlignment>("HorizontalAlignment",
            inherits: true);

    public static void SetHorizontalAlignment(AvaloniaObject element, HorizontalAlignment value) =>
        element.SetValue(HorizontalAlignmentProperty, value);

    public static HorizontalAlignment GetHorizontalAlignment(AvaloniaObject element) =>
        element.GetValue(HorizontalAlignmentProperty);

    public static readonly AttachedProperty<VerticalAlignment> VerticalAlignmentProperty =
        AvaloniaProperty.RegisterAttached<TitleElement, AvaloniaObject, VerticalAlignment>("VerticalAlignment",
            inherits: true);

    public static void SetVerticalAlignment(AvaloniaObject element, VerticalAlignment value) =>
        element.SetValue(VerticalAlignmentProperty, value);

    public static VerticalAlignment GetVerticalAlignment(AvaloniaObject element) =>
        element.GetValue(VerticalAlignmentProperty);

    public static readonly AttachedProperty<Thickness> MarginOnTheLeftProperty =
        AvaloniaProperty.RegisterAttached<TitleElement, AvaloniaObject, Thickness>("MarginOnTheLeft", inherits: true);

    public static void SetMarginOnTheLeft(AvaloniaObject element, Thickness value) =>
        element.SetValue(MarginOnTheLeftProperty, value);

    public static Thickness GetMarginOnTheLeft(AvaloniaObject element) => element.GetValue(MarginOnTheLeftProperty);

    public static readonly AttachedProperty<Thickness> MarginOnTheTopProperty =
        AvaloniaProperty.RegisterAttached<TitleElement, AvaloniaObject, Thickness>("MarginOnTheTop", inherits: true);

    public static void SetMarginOnTheTop(AvaloniaObject element, Thickness value) =>
        element.SetValue(MarginOnTheTopProperty, value);

    public static Thickness GetMarginOnTheTop(AvaloniaObject element) => element.GetValue(MarginOnTheTopProperty);

    public static readonly AttachedProperty<double> MinHeightProperty =
        AvaloniaProperty.RegisterAttached<TitleElement, AvaloniaObject, double>("MinHeight", inherits: true);

    public static void SetMinHeight(AvaloniaObject element, double value) => element.SetValue(MinHeightProperty, value);

    public static double GetMinHeight(AvaloniaObject element) => element.GetValue(MinHeightProperty);

    public static readonly AttachedProperty<double> MinWidthProperty =
        AvaloniaProperty.RegisterAttached<TitleElement, AvaloniaObject, double>("MinWidth", inherits: true);

    public static void SetMinWidth(AvaloniaObject element, double value) => element.SetValue(MinWidthProperty, value);

    public static double GetMinWidth(AvaloniaObject element) => element.GetValue(MinWidthProperty);
}
