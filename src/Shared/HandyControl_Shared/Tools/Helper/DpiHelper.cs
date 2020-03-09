using System;
using System.Windows;
using System.Windows.Media;
using HandyControl.Tools.Interop;

namespace HandyControl.Tools
{
    internal static class DpiHelper
    {
        private const double LogicalDpi = 96.0;

        [ThreadStatic] 
        private static Matrix _transformToDip;

        static DpiHelper()
        {
            var dC = InteropMethods.GetDC(IntPtr.Zero);
            if (dC != IntPtr.Zero)
            {
                DeviceDpiX = InteropMethods.GetDeviceCaps(dC, 88);
                DeviceDpiY = InteropMethods.GetDeviceCaps(dC, 90);
                InteropMethods.ReleaseDC(IntPtr.Zero, dC);
            }
            else
            {
                DeviceDpiX = LogicalDpi;
                DeviceDpiY = LogicalDpi;
            }

            var identity = Matrix.Identity;
            var identity2 = Matrix.Identity;
            identity.Scale(DeviceDpiX / LogicalDpi, DeviceDpiY / LogicalDpi);
            identity2.Scale(LogicalDpi / DeviceDpiX, LogicalDpi / DeviceDpiY);
            TransformFromDevice = new MatrixTransform(identity2);
            TransformFromDevice.Freeze();
            TransformToDevice = new MatrixTransform(identity);
            TransformToDevice.Freeze();
        }

        public static MatrixTransform TransformFromDevice { get; }

        public static MatrixTransform TransformToDevice { get; }

        public static double DeviceDpiX { get; }

        public static double DeviceDpiY { get; }

        public static double LogicalToDeviceUnitsScalingFactorX => TransformToDevice.Matrix.M11;

        public static double LogicalToDeviceUnitsScalingFactorY => TransformToDevice.Matrix.M22;

        public static Point DevicePixelsToLogical(Point devicePoint, double dpiScaleX, double dpiScaleY)
        {
            _transformToDip = Matrix.Identity;
            _transformToDip.Scale(1d / dpiScaleX, 1d / dpiScaleY);
            return _transformToDip.Transform(devicePoint);
        }

        public static Size DeviceSizeToLogical(Size deviceSize, double dpiScaleX, double dpiScaleY)
        {
            var pt = DevicePixelsToLogical(new Point(deviceSize.Width, deviceSize.Height), dpiScaleX, dpiScaleY);
            return new Size(pt.X, pt.Y);
        }

        public static Rect LogicalToDeviceUnits(this Rect logicalRect)
        {
            var result = logicalRect;
            result.Transform(TransformToDevice.Matrix);
            return result;
        }

        public static Rect DeviceToLogicalUnits(this Rect deviceRect)
        {
            var result = deviceRect;
            result.Transform(TransformFromDevice.Matrix);
            return result;
        }
    }
}