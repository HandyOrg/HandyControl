using Avalonia.Layout;
using System;
using System.Windows;

namespace HandyControl.Data;


internal static class ValueBoxes
{
    public static object TrueBox = true;

    public static object FalseBox = false;

    public static object VerticalBox = Orientation.Vertical;

    public static object HorizontalBox = Orientation.Horizontal;

    public static object Double01Box = .1;

    public static object Double0Box = .0;

    public static object Double1Box = 1.0;

    public static object Double10Box = 10.0;

    public static object Double20Box = 20.0;

    public static object Double100Box = 100.0;

    public static object Double200Box = 200.0;

    public static object Double300Box = 300.0;

    public static object DoubleNeg1Box = -1.0;

    public static object Int0Box = 0;

    public static object Int1Box = 1;

    public static object Int2Box = 2;

    public static object Int5Box = 5;

    public static object Int99Box = 99;

    public static object BooleanBox(bool value) => value ? TrueBox : FalseBox;

    public static object OrientationBox(Orientation value) =>
        value == Orientation.Horizontal ? HorizontalBox : VerticalBox;

 
}
