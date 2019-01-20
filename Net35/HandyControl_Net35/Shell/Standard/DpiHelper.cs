namespace Standard
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Media;

    internal static class DpiHelper
    {
        private static Matrix _transformToDevice;
        private static Matrix _transformToDip;

        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
        static DpiHelper()
        {
            using (Standard.SafeDC edc = Standard.SafeDC.GetDesktop())
            {
                int deviceCaps = Standard.NativeMethods.GetDeviceCaps(edc, Standard.DeviceCap.LOGPIXELSX);
                int num2 = Standard.NativeMethods.GetDeviceCaps(edc, Standard.DeviceCap.LOGPIXELSY);
                _transformToDip = Matrix.Identity;
                _transformToDip.Scale(96.0 / ((double) deviceCaps), 96.0 / ((double) num2));
                _transformToDevice = Matrix.Identity;
                _transformToDevice.Scale(((double) deviceCaps) / 96.0, ((double) num2) / 96.0);
            }
        }

        public static Point DevicePixelsToLogical(Point devicePoint)
        {
            return _transformToDip.Transform(devicePoint);
        }

        public static Rect DeviceRectToLogical(Rect deviceRectangle)
        {
            Point point = DevicePixelsToLogical(new Point(deviceRectangle.Left, deviceRectangle.Top));
            return new Rect(point, DevicePixelsToLogical(new Point(deviceRectangle.Right, deviceRectangle.Bottom)));
        }

        public static Size DeviceSizeToLogical(Size deviceSize)
        {
            Point point = DevicePixelsToLogical(new Point(deviceSize.Width, deviceSize.Height));
            return new Size(point.X, point.Y);
        }

        public static Point LogicalPixelsToDevice(Point logicalPoint)
        {
            return _transformToDevice.Transform(logicalPoint);
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static Rect LogicalRectToDevice(Rect logicalRectangle)
        {
            Point point = LogicalPixelsToDevice(new Point(logicalRectangle.Left, logicalRectangle.Top));
            return new Rect(point, LogicalPixelsToDevice(new Point(logicalRectangle.Right, logicalRectangle.Bottom)));
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static Size LogicalSizeToDevice(Size logicalSize)
        {
            Point point = LogicalPixelsToDevice(new Point(logicalSize.Width, logicalSize.Height));
            return new Size { Width = point.X, Height = point.Y };
        }
    }
}

