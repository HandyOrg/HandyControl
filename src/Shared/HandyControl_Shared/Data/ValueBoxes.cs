using System;
using System.Windows;
using System.Windows.Controls;

namespace HandyControl.Data;

/// <summary>
///     装箱后的值类型（用于提高效率）
/// </summary>
internal static class ValueBoxes
{
    internal static object TrueBox = true;

    internal static object FalseBox = false;

    internal static object VerticalBox = Orientation.Vertical;

    internal static object HorizontalBox = Orientation.Horizontal;

    internal static object VisibleBox = Visibility.Visible;

    internal static object CollapsedBox = Visibility.Collapsed;

    internal static object HiddenBox = Visibility.Hidden;

    internal static object Double01Box = .1;

    internal static object Double0Box = .0;

    internal static object Double1Box = 1.0;

    internal static object Double10Box = 10.0;

    internal static object Double20Box = 20.0;

    internal static object Double100Box = 100.0;

    internal static object Double200Box = 200.0;

    internal static object Double300Box = 300.0;

    internal static object DoubleNeg1Box = -1.0;

    internal static object Int0Box = 0;

    internal static object Int1Box = 1;

    internal static object Int2Box = 2;

    internal static object Int5Box = 5;

    internal static object Int99Box = 99;

    internal static object BooleanBox(bool value) => value ? TrueBox : FalseBox;

    internal static object OrientationBox(Orientation value) =>
        value == Orientation.Horizontal ? HorizontalBox : VerticalBox;

    internal static object VisibilityBox(Visibility value)
    {
        return value switch
        {
            Visibility.Visible => VisibleBox,
            Visibility.Hidden => HiddenBox,
            Visibility.Collapsed => CollapsedBox,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }
}
