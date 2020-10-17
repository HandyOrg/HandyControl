using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Standard
{
	// Token: 0x02000048 RID: 72
	internal sealed class SafeDC : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x1700001B RID: 27
		// (set) Token: 0x060000A4 RID: 164 RVA: 0x000042C4 File Offset: 0x000024C4
		public IntPtr Hwnd
		{
			set
			{
				this._hwnd = new IntPtr?(value);
			}
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x000042D2 File Offset: 0x000024D2
		private SafeDC() : base(true)
		{
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000042DC File Offset: 0x000024DC
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		protected override bool ReleaseHandle()
		{
			if (this._created)
			{
				return SafeDC.NativeMethods.DeleteDC(this.handle);
			}
			return this._hwnd == null || this._hwnd.Value == IntPtr.Zero || SafeDC.NativeMethods.ReleaseDC(this._hwnd.Value, this.handle) == 1;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x0000433C File Offset: 0x0000253C
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
		public static SafeDC CreateDC(string deviceName)
		{
			SafeDC safeDC = null;
			try
			{
				safeDC = SafeDC.NativeMethods.CreateDC(deviceName, null, IntPtr.Zero, IntPtr.Zero);
			}
			finally
			{
				if (safeDC != null)
				{
					safeDC._created = true;
				}
			}
			if (safeDC.IsInvalid)
			{
				safeDC.Dispose();
				throw new SystemException("Unable to create a device context from the specified device information.");
			}
			return safeDC;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00004394 File Offset: 0x00002594
		[SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static SafeDC CreateCompatibleDC(SafeDC hdc)
		{
			SafeDC safeDC = null;
			try
			{
				IntPtr hdc2 = IntPtr.Zero;
				if (hdc != null)
				{
					hdc2 = hdc.handle;
				}
				safeDC = SafeDC.NativeMethods.CreateCompatibleDC(hdc2);
				if (safeDC == null)
				{
					HRESULT.ThrowLastError();
				}
			}
			finally
			{
				if (safeDC != null)
				{
					safeDC._created = true;
				}
			}
			if (safeDC.IsInvalid)
			{
				safeDC.Dispose();
				throw new SystemException("Unable to create a device context from the specified device information.");
			}
			return safeDC;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000043FC File Offset: 0x000025FC
		public static SafeDC GetDC(IntPtr hwnd)
		{
			SafeDC safeDC = null;
			try
			{
				safeDC = SafeDC.NativeMethods.GetDC(hwnd);
			}
			finally
			{
				if (safeDC != null)
				{
					safeDC.Hwnd = hwnd;
				}
			}
			if (safeDC.IsInvalid)
			{
				HRESULT.E_FAIL.ThrowIfFailed();
			}
			return safeDC;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00004448 File Offset: 0x00002648
		public static SafeDC GetDesktop()
		{
			return SafeDC.GetDC(IntPtr.Zero);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00004454 File Offset: 0x00002654
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
		public static SafeDC WrapDC(IntPtr hdc)
		{
			return new SafeDC
			{
				handle = hdc,
				_created = false,
				_hwnd = new IntPtr?(IntPtr.Zero)
			};
		}

		// Token: 0x04000417 RID: 1047
		private IntPtr? _hwnd;

		// Token: 0x04000418 RID: 1048
		private bool _created;

		// Token: 0x02000049 RID: 73
		private static class NativeMethods
		{
			// Token: 0x060000AC RID: 172
			[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
			[DllImport("user32.dll")]
			public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

			// Token: 0x060000AD RID: 173
			[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
			[DllImport("user32.dll")]
			public static extern SafeDC GetDC(IntPtr hwnd);

			// Token: 0x060000AE RID: 174
			[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
			[DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
			public static extern SafeDC CreateDC([MarshalAs(UnmanagedType.LPWStr)] string lpszDriver, [MarshalAs(UnmanagedType.LPWStr)] string lpszDevice, IntPtr lpszOutput, IntPtr lpInitData);

			// Token: 0x060000AF RID: 175
			[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
			[DllImport("gdi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
			public static extern SafeDC CreateCompatibleDC(IntPtr hdc);

			// Token: 0x060000B0 RID: 176
			[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
			[DllImport("gdi32.dll")]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool DeleteDC(IntPtr hdc);
		}
	}
}
