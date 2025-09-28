using Avalonia;

namespace HandyControl.Controls;

public class InfoElement
{
    public static readonly AttachedProperty<bool> NecessaryProperty =
        AvaloniaProperty.RegisterAttached<InfoElement, AvaloniaObject, bool>("Necessary", inherits: true);

    public static void SetNecessary(AvaloniaObject element, bool value) => element.SetValue(NecessaryProperty, value);

    public static bool GetNecessary(AvaloniaObject element) => element.GetValue(NecessaryProperty);

    public static readonly AttachedProperty<object> SymbolProperty =
        AvaloniaProperty.RegisterAttached<InfoElement, AvaloniaObject, object>("Symbol", inherits: true);

    public static void SetSymbol(AvaloniaObject element, object value) => element.SetValue(SymbolProperty, value);

    public static object GetSymbol(AvaloniaObject element) => element.GetValue(SymbolProperty);

    public static readonly AttachedProperty<double> ContentHeightProperty =
        AvaloniaProperty.RegisterAttached<InfoElement, AvaloniaObject, double>("ContentHeight", defaultValue: 28,
            inherits: true);

    public static void SetContentHeight(AvaloniaObject element, double value) =>
        element.SetValue(ContentHeightProperty, value);

    public static double GetContentHeight(AvaloniaObject element) => element.GetValue(ContentHeightProperty);

    public static readonly AttachedProperty<double> MinContentHeightProperty =
        AvaloniaProperty.RegisterAttached<InfoElement, AvaloniaObject, double>("MinContentHeight", defaultValue: 28,
            inherits: true);

    public static void SetMinContentHeight(AvaloniaObject element, double value) =>
        element.SetValue(MinContentHeightProperty, value);

    public static double GetMinContentHeight(AvaloniaObject element) => element.GetValue(MinContentHeightProperty);

    public static readonly AttachedProperty<double> MaxContentHeightProperty =
        AvaloniaProperty.RegisterAttached<InfoElement, AvaloniaObject, double>("MaxContentHeight",
            defaultValue: double.PositiveInfinity, inherits: true);

    public static void SetMaxContentHeight(AvaloniaObject element, double value) =>
        element.SetValue(MaxContentHeightProperty, value);

    public static double GetMaxContentHeight(AvaloniaObject element) => element.GetValue(MaxContentHeightProperty);

    public static readonly AttachedProperty<string> RegexPatternProperty =
        AvaloniaProperty.RegisterAttached<InfoElement, AvaloniaObject, string>("RegexPattern");

    public static void SetRegexPattern(AvaloniaObject element, string value) =>
        element.SetValue(RegexPatternProperty, value);

    public static string GetRegexPattern(AvaloniaObject element) => element.GetValue(RegexPatternProperty);

    public static readonly AttachedProperty<bool> ShowClearButtonProperty =
        AvaloniaProperty.RegisterAttached<InfoElement, AvaloniaObject, bool>("ShowClearButton", inherits: true);

    public static void SetShowClearButton(AvaloniaObject element, bool value) =>
        element.SetValue(ShowClearButtonProperty, value);

    public static bool GetShowClearButton(AvaloniaObject element) => element.GetValue(ShowClearButtonProperty);
}
