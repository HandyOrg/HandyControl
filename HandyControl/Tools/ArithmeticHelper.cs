using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace HandyControl.Tools
{
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
            for (int i = 0; i < count; i++)
            {
                arr[i] = div;
            }
            for (int i = 0; i < rest; i++)
            {
                arr[i] += 1;
            }
            return arr;
        }

        /// <summary>
        ///     计算控件在窗口中的可见坐标
        /// </summary>
        /// <param name="element"></param>
        /// <param name="showElement"></param>
        /// <returns></returns>
        public static Point CalSafePoint(FrameworkElement element, FrameworkElement showElement)
        {
            if (element == null || showElement == null) return default(Point);
            var point = element.PointToScreen(new Point(0, 0));

            if (point.X < 0) point.X = 0;
            if (point.Y < 0) point.Y = 0;

            var maxLeft = SystemParameters.PrimaryScreenWidth -
                          (double.IsNaN(showElement.Width) ? showElement.ActualWidth : showElement.Width);
            var maxTop = SystemParameters.PrimaryScreenHeight -
                         (double.IsNaN(showElement.Height) ? showElement.ActualHeight : showElement.Height);
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
            switch (element.HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    x = layoutSlot.Left + margin.Left;
                    break;
                case HorizontalAlignment.Center:
                    x = (layoutSlot.Left + margin.Left + layoutSlot.Right - margin.Right) / 2.0 - width / 2.0;
                    break;
                case HorizontalAlignment.Right:
                    x = layoutSlot.Right - margin.Right - width;
                    break;
                case HorizontalAlignment.Stretch:
                    x = Math.Max(layoutSlot.Left + margin.Left,
                        (layoutSlot.Left + margin.Left + layoutSlot.Right - margin.Right) / 2.0 - width / 2.0);
                    break;
            }
            switch (element.VerticalAlignment)
            {
                case VerticalAlignment.Top:
                    y = layoutSlot.Top + margin.Top;
                    break;
                case VerticalAlignment.Center:
                    y = (layoutSlot.Top + margin.Top + layoutSlot.Bottom - margin.Bottom) / 2.0 - height / 2.0;
                    break;
                case VerticalAlignment.Bottom:
                    y = layoutSlot.Bottom - margin.Bottom - height;
                    break;
                case VerticalAlignment.Stretch:
                    y = Math.Max(layoutSlot.Top + margin.Top,
                        (layoutSlot.Top + margin.Top + layoutSlot.Bottom - margin.Bottom) / 2.0 - height / 2.0);
                    break;
            }
            return new Rect(x, y, width, height);
        }
    }
}