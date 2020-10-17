using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Media;

namespace Standard
{
	// Token: 0x02000011 RID: 17
	internal static class DpiHelper
	{
		// Token: 0x06000078 RID: 120 RVA: 0x00003778 File Offset: 0x00001978
		[SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
		static DpiHelper()
		{
			using (SafeDC desktop = SafeDC.GetDesktop())
			{
				int deviceCaps = NativeMethods.GetDeviceCaps(desktop, DeviceCap.LOGPIXELSX);
				int deviceCaps2 = NativeMethods.GetDeviceCaps(desktop, DeviceCap.LOGPIXELSY);
				DpiHelper._transformToDip = Matrix.Identity;
				DpiHelper._transformToDip.Scale(96.0 / (double)deviceCaps, 96.0 / (double)deviceCaps2);
				DpiHelper._transformToDevice = Matrix.Identity;
				DpiHelper._transformToDevice.Scale((double)deviceCaps / 96.0, (double)deviceCaps2 / 96.0);
			}
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00003814 File Offset: 0x00001A14
		public static Point LogicalPixelsToDevice(Point logicalPoint)
		{
			return DpiHelper._transformToDevice.Transform(logicalPoint);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003821 File Offset: 0x00001A21
		public static Point DevicePixelsToLogical(Point devicePoint)
		{
			return DpiHelper._transformToDip.Transform(devicePoint);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00003830 File Offset: 0x00001A30
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static Rect LogicalRectToDevice(Rect logicalRectangle)
		{
			Point point = DpiHelper.LogicalPixelsToDevice(new Point(logicalRectangle.Left, logicalRectangle.Top));
			Point point2 = DpiHelper.LogicalPixelsToDevice(new Point(logicalRectangle.Right, logicalRectangle.Bottom));
			return new Rect(point, point2);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003878 File Offset: 0x00001A78
		public static Rect DeviceRectToLogical(Rect deviceRectangle)
		{
			Point point = DpiHelper.DevicePixelsToLogical(new Point(deviceRectangle.Left, deviceRectangle.Top));
			Point point2 = DpiHelper.DevicePixelsToLogical(new Point(deviceRectangle.Right, deviceRectangle.Bottom));
			return new Rect(point, point2);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000038C0 File Offset: 0x00001AC0
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

		// Token: 0x0600007E RID: 126 RVA: 0x0000390C File Offset: 0x00001B0C
		public static Size DeviceSizeToLogical(Size deviceSize)
		{
			Point point = DpiHelper.DevicePixelsToLogical(new Point(deviceSize.Width, deviceSize.Height));
			return new Size(point.X, point.Y);
		}

		// Token: 0x0400003F RID: 63
		private static Matrix _transformToDevice;

		// Token: 0x04000040 RID: 64
		private static Matrix _transformToDip;
	}
}
