using System.Windows;

namespace HandyControl.Tools.Extension;

public static class FrameworkElementExtension
{

    public static double GetValidWidth(this FrameworkElement element)
    {
        if (!double.IsNaN(element.Width))
        {
            if (element.Width > 0)
            {
                return element.Width;
            }
        }
        else
        {
            if (element.ActualWidth > 0)
            {
                return element.ActualWidth;
            }

            if (element.DesiredSize.Width > 0)
            {
                return element.DesiredSize.Width;
            }
        }

        return 0;
    }

    public static double GetValidHeight(this FrameworkElement element)
    {
        if (!double.IsNaN(element.Height))
        {
            if (element.Height > 0)
            {
                return element.Height;
            }
        }
        else
        {
            if (element.ActualHeight > 0)
            {
                return element.ActualHeight;
            }

            if (element.DesiredSize.Height > 0)
            {
                return element.DesiredSize.Height;
            }
        }

        return 0;
    }
}
