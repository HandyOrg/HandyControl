using System.Windows;
using HandyControl.Data;

namespace HandyControl.Controls;

public class EdgeElement
{
    public static readonly DependencyProperty LeftContentProperty = DependencyProperty.RegisterAttached(
        "LeftContent", typeof(object), typeof(EdgeElement), new PropertyMetadata(default(object), OnEdgeContentChanged));

    public static void SetLeftContent(DependencyObject element, object value) => element.SetValue(LeftContentProperty, value);

    public static object GetLeftContent(DependencyObject element) => element.GetValue(LeftContentProperty);

    public static readonly DependencyProperty TopContentProperty = DependencyProperty.RegisterAttached(
        "TopContent", typeof(object), typeof(EdgeElement), new PropertyMetadata(default(object), OnEdgeContentChanged));

    public static void SetTopContent(DependencyObject element, object value) => element.SetValue(TopContentProperty, value);

    public static object GetTopContent(DependencyObject element) => element.GetValue(TopContentProperty);

    public static readonly DependencyProperty RightContentProperty = DependencyProperty.RegisterAttached(
        "RightContent", typeof(object), typeof(EdgeElement), new PropertyMetadata(default(object), OnEdgeContentChanged));

    public static void SetRightContent(DependencyObject element, object value) => element.SetValue(RightContentProperty, value);

    public static object GetRightContent(DependencyObject element) => element.GetValue(RightContentProperty);

    public static readonly DependencyProperty BottomContentProperty = DependencyProperty.RegisterAttached(
        "BottomContent", typeof(object), typeof(EdgeElement), new PropertyMetadata(default(object), OnEdgeContentChanged));

    public static void SetBottomContent(DependencyObject element, object value) => element.SetValue(BottomContentProperty, value);

    public static object GetBottomContent(DependencyObject element) => element.GetValue(BottomContentProperty);

    public static readonly DependencyProperty ShowEdgeContentProperty = DependencyProperty.RegisterAttached(
        "ShowEdgeContent", typeof(bool), typeof(EdgeElement), new PropertyMetadata(ValueBoxes.FalseBox));

    public static void SetShowEdgeContent(DependencyObject element, bool value) => element.SetValue(ShowEdgeContentProperty, ValueBoxes.BooleanBox(value));

    public static bool GetShowEdgeContent(DependencyObject element) => (bool) element.GetValue(ShowEdgeContentProperty);

    private static void OnEdgeContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
        SetShowEdgeContent(d, !(GetLeftContent(d) == null && GetTopContent(d) == null &&
                                GetRightContent(d) == null && GetBottomContent(d) == null));
}
