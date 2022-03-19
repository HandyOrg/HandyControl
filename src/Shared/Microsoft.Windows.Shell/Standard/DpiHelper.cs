using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Media;

namespace Standard;

internal static class DpiHelper
{
    [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
    static DpiHelper()
    {
        using (SafeDC desktop = SafeDC.GetDesktop())
        {
            int deviceCaps = NativeMethods.GetDeviceCaps(desktop, DeviceCap.LOGPIXELSX);
            int deviceCaps2 = NativeMethods.GetDeviceCaps(desktop, DeviceCap.LOGPIXELSY);
            DpiHelper._transformToDip = Matrix.Identity;
            DpiHelper._transformToDip.Scale(96.0 / (double) deviceCaps, 96.0 / (double) deviceCaps2);
            DpiHelper._transformToDevice = Matrix.Identity;
            DpiHelper._transformToDevice.Scale((double) deviceCaps / 96.0, (double) deviceCaps2 / 96.0);
        }
    }

    public static Point LogicalPixelsToDevice(Point logicalPoint)
    {
        return DpiHelper._transformToDevice.Transform(logicalPoint);
    }

    public static Point DevicePixelsToLogical(Point devicePoint)
    {
        return DpiHelper._transformToDip.Transform(devicePoint);
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static Rect LogicalRectToDevice(Rect logicalRectangle)
    {
        Point point = DpiHelper.LogicalPixelsToDevice(new Point(logicalRectangle.Left, logicalRectangle.Top));
        Point point2 = DpiHelper.LogicalPixelsToDevice(new Point(logicalRectangle.Right, logicalRectangle.Bottom));
        return new Rect(point, point2);
    }

    public static Rect DeviceRectToLogical(Rect deviceRectangle)
    {
        Point point = DpiHelper.DevicePixelsToLogical(new Point(deviceRectangle.Left, deviceRectangle.Top));
        Point point2 = DpiHelper.DevicePixelsToLogical(new Point(deviceRectangle.Right, deviceRectangle.Bottom));
        return new Rect(point, point2);
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static Size LogicalSizeToDevice(Size logicalSize)
    {
        Point point = DpiHelper.LogicalPixelsToDevice(new Point(logicalSize.Width, logicalSize.Height));
        return new Size
        {
            Width = point.X,
            Height = point.Y
        };
    }

    public static Size DeviceSizeToLogical(Size deviceSize)
    {
        Point point = DpiHelper.DevicePixelsToLogical(new Point(deviceSize.Width, deviceSize.Height));
        return new Size(point.X, point.Y);
    }

    private static Matrix _transformToDevice;

    private static Matrix _transformToDip;
}
