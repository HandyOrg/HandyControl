using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Media3D;

namespace HandyControl.Tools;

/// <summary>
///     包含内部使用的一些简单算法
/// </summary>
internal class ArithmeticHelper
{
    /// <summary>
    ///     平分一个整数到一个数组中
    /// </summary>
    /// <param name="num"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static int[] DivideInt2Arr(int num, int count)
    {
        var arr = new int[count];
        var div = num / count;
        var rest = num % count;
        for (var i = 0; i < count; i++)
        {
            arr[i] = div;
        }
        for (var i = 0; i < rest; i++)
        {
            arr[i] += 1;
        }
        return arr;
    }

    /// <summary>
    ///     计算控件在窗口中的可见坐标
    /// </summary>
    public static Point CalSafePoint(FrameworkElement element, FrameworkElement showElement, Thickness thickness = default)
    {
        if (element == null || showElement == null) return default;
        var point = element.PointToScreen(new Point(0, 0));

        if (point.X < 0) point.X = 0;
        if (point.Y < 0) point.Y = 0;

        var maxLeft = SystemParameters.WorkArea.Width -
                      ((double.IsNaN(showElement.Width) ? showElement.ActualWidth : showElement.Width) +
                       thickness.Left + thickness.Right);
        var maxTop = SystemParameters.WorkArea.Height -
                     ((double.IsNaN(showElement.Height) ? showElement.ActualHeight : showElement.Height) +
                      thickness.Top + thickness.Bottom);
        return new Point(maxLeft > point.X ? point.X : maxLeft, maxTop > point.Y ? point.Y : maxTop);
    }

    /// <summary>
    ///     获取布局范围框
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static Rect GetLayoutRect(FrameworkElement element)
    {
        var num1 = element.ActualWidth;
        var num2 = element.ActualHeight;
        if (element is Image || element is MediaElement)
            if (element.Parent is Canvas)
            {
                num1 = double.IsNaN(element.Width) ? num1 : element.Width;
                num2 = double.IsNaN(element.Height) ? num2 : element.Height;
            }
            else
            {
                num1 = element.RenderSize.Width;
                num2 = element.RenderSize.Height;
            }
        var width = element.Visibility == Visibility.Collapsed ? 0.0 : num1;
        var height = element.Visibility == Visibility.Collapsed ? 0.0 : num2;
        var margin = element.Margin;
        var layoutSlot = LayoutInformation.GetLayoutSlot(element);
        var x = 0.0;
        var y = 0.0;
        x = element.HorizontalAlignment switch
        {
            HorizontalAlignment.Left => layoutSlot.Left + margin.Left,
            HorizontalAlignment.Center => (layoutSlot.Left + margin.Left + layoutSlot.Right - margin.Right) / 2.0 -
                                          width / 2.0,
            HorizontalAlignment.Right => layoutSlot.Right - margin.Right - width,
            HorizontalAlignment.Stretch => Math.Max(layoutSlot.Left + margin.Left,
                (layoutSlot.Left + margin.Left + layoutSlot.Right - margin.Right) / 2.0 - width / 2.0),
            _ => x
        };
        y = element.VerticalAlignment switch
        {
            VerticalAlignment.Top => layoutSlot.Top + margin.Top,
            VerticalAlignment.Center => (layoutSlot.Top + margin.Top + layoutSlot.Bottom - margin.Bottom) / 2.0 -
                                        height / 2.0,
            VerticalAlignment.Bottom => layoutSlot.Bottom - margin.Bottom - height,
            VerticalAlignment.Stretch => Math.Max(layoutSlot.Top + margin.Top,
                (layoutSlot.Top + margin.Top + layoutSlot.Bottom - margin.Bottom) / 2.0 - height / 2.0),
            _ => y
        };
        return new Rect(x, y, width, height);
    }

    /// <summary>
    ///     计算两点的连线和x轴的夹角
    /// </summary>
    /// <param name="center"></param>
    /// <param name="p"></param>
    /// <returns></returns>
    public static double CalAngle(Point center, Point p) => Math.Atan2(p.Y - center.Y, p.X - center.X) * 180 / Math.PI;

    /// <summary>
    ///     计算法线
    /// </summary>
    /// <param name="p0"></param>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <returns></returns>
    public static Vector3D CalNormal(Point3D p0, Point3D p1, Point3D p2)
    {
        var v0 = new Vector3D(p1.X - p0.X, p1.Y - p0.Y, p1.Z - p0.Z);
        var v1 = new Vector3D(p2.X - p1.X, p2.Y - p1.Y, p2.Z - p1.Z);
        return Vector3D.CrossProduct(v0, v1);
    }
}
