using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
namespace HandyControl.Interactivity
{
    public class ExtendedVisualStateManager
    {       
        internal static Rect GetLayoutRect(FrameworkElement element)
        {
            var actualWidth = element.ActualWidth;
            var actualHeight = element.ActualHeight;
            if (element is Image || element is MediaElement)
                if (element.Parent is Canvas)
                {
                    actualWidth = double.IsNaN(element.Width) ? actualWidth : element.Width;
                    actualHeight = double.IsNaN(element.Height) ? actualHeight : element.Height;
                }
                else
                {
                    actualWidth = element.RenderSize.Width;
                    actualHeight = element.RenderSize.Height;
                }
            actualWidth = element.Visibility == Visibility.Collapsed ? 0.0 : actualWidth;
            actualHeight = element.Visibility == Visibility.Collapsed ? 0.0 : actualHeight;
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
                    x = (layoutSlot.Left + margin.Left + layoutSlot.Right - margin.Right) / 2.0 - actualWidth / 2.0;
                    break;

                case HorizontalAlignment.Right:
                    x = layoutSlot.Right - margin.Right - actualWidth;
                    break;

                case HorizontalAlignment.Stretch:
                    x = Math.Max(layoutSlot.Left + margin.Left,
                        (layoutSlot.Left + margin.Left + layoutSlot.Right - margin.Right) / 2.0 - actualWidth / 2.0);
                    break;
            }
            switch (element.VerticalAlignment)
            {
                case VerticalAlignment.Top:
                    y = layoutSlot.Top + margin.Top;
                    break;

                case VerticalAlignment.Center:
                    y = (layoutSlot.Top + margin.Top + layoutSlot.Bottom - margin.Bottom) / 2.0 - actualHeight / 2.0;
                    break;

                case VerticalAlignment.Bottom:
                    y = layoutSlot.Bottom - margin.Bottom - actualHeight;
                    break;

                case VerticalAlignment.Stretch:
                    y = Math.Max(layoutSlot.Top + margin.Top,
                        (layoutSlot.Top + margin.Top + layoutSlot.Bottom - margin.Bottom) / 2.0 - actualHeight / 2.0);
                    break;
            }
            return new Rect(x, y, actualWidth, actualHeight);
        }        
    }
}