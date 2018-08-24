using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace HandyControl.Tools
{
    public class ArithmeticHelper
    {
        /// <summary>
        ///     获取布局范围框
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        internal static Rect GetLayoutRect(FrameworkElement element)
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