using System.Windows;
using HandyControl.Data;

namespace HandyControl.Controls;

public class InfoElement : TitleElement
{
    /// <summary>
    ///     占位符
    /// </summary>
    public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.RegisterAttached(
        "Placeholder", typeof(string), typeof(InfoElement), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.Inherits));

    public static void SetPlaceholder(DependencyObject element, string value) => element.SetValue(PlaceholderProperty, value);

    public static string GetPlaceholder(DependencyObject element) => (string) element.GetValue(PlaceholderProperty);

    /// <summary>
    ///     是否必填
    /// </summary>
    public static readonly DependencyProperty NecessaryProperty = DependencyProperty.RegisterAttached(
        "Necessary", typeof(bool), typeof(InfoElement), new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits));

    public static void SetNecessary(DependencyObject element, bool value) => element.SetValue(NecessaryProperty, ValueBoxes.BooleanBox(value));

    public static bool GetNecessary(DependencyObject element) => (bool) element.GetValue(NecessaryProperty);

    /// <summary>
    ///     标记
    /// </summary>
    public static readonly DependencyProperty SymbolProperty = DependencyProperty.RegisterAttached(
        "Symbol", typeof(string), typeof(InfoElement), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.Inherits));

    public static void SetSymbol(DependencyObject element, string value) => element.SetValue(SymbolProperty, value);

    public static string GetSymbol(DependencyObject element) => (string) element.GetValue(SymbolProperty);

    /// <summary>
    ///     内容高度
    /// </summary>
    public static readonly DependencyProperty ContentHeightProperty = DependencyProperty.RegisterAttached(
        "ContentHeight", typeof(double), typeof(InfoElement), new FrameworkPropertyMetadata(30.0, FrameworkPropertyMetadataOptions.Inherits));

    public static void SetContentHeight(DependencyObject element, double value) => element.SetValue(ContentHeightProperty, value);

    public static double GetContentHeight(DependencyObject element) => (double) element.GetValue(ContentHeightProperty);

    /// <summary>
    ///     最小内容高度
    /// </summary>
    public static readonly DependencyProperty MinContentHeightProperty = DependencyProperty.RegisterAttached(
        "MinContentHeight", typeof(double), typeof(InfoElement), new PropertyMetadata(30.0));

    public static void SetMinContentHeight(DependencyObject element, double value)
        => element.SetValue(MinContentHeightProperty, value);

    public static double GetMinContentHeight(DependencyObject element)
        => (double) element.GetValue(MinContentHeightProperty);

    /// <summary>
    ///     最大内容高度
    /// </summary>
    public static readonly DependencyProperty MaxContentHeightProperty = DependencyProperty.RegisterAttached(
        "MaxContentHeight", typeof(double), typeof(InfoElement), new PropertyMetadata(double.PositiveInfinity));

    public static void SetMaxContentHeight(DependencyObject element, double value)
        => element.SetValue(MaxContentHeightProperty, value);

    public static double GetMaxContentHeight(DependencyObject element)
        => (double) element.GetValue(MaxContentHeightProperty);

    /// <summary>
    ///     正则表达式
    /// </summary>
    public static readonly DependencyProperty RegexPatternProperty = DependencyProperty.RegisterAttached(
        "RegexPattern", typeof(string), typeof(InfoElement), new PropertyMetadata(default(string)));

    public static void SetRegexPattern(DependencyObject element, string value)
        => element.SetValue(RegexPatternProperty, value);

    public static string GetRegexPattern(DependencyObject element)
        => (string) element.GetValue(RegexPatternProperty);

    public static readonly DependencyProperty ShowClearButtonProperty = DependencyProperty.RegisterAttached(
        "ShowClearButton", typeof(bool), typeof(InfoElement), new PropertyMetadata(ValueBoxes.FalseBox));

    public static void SetShowClearButton(DependencyObject element, bool value)
        => element.SetValue(ShowClearButtonProperty, ValueBoxes.BooleanBox(value));

    public static bool GetShowClearButton(DependencyObject element)
        => (bool) element.GetValue(ShowClearButtonProperty);
}
