using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.ConstrainedExecution;
using Microsoft.Win32.SafeHandles;

namespace Standard
{
	// Token: 0x0200004B RID: 75
	internal sealed class SafeGdiplusStartupToken : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x060000B3 RID: 179 RVA: 0x0000449C File Offset: 0x0000269C
		private SafeGdiplusStartupToken() : base(true)
		{
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x000044A8 File Offset: 0x000026A8
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		protected override bool ReleaseHandle()
		{
			Status status = NativeMethods.GdiplusShutdown(this.handle);
			return status == Status.Ok;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x000044C8 File Offset: 0x000026C8
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
		[SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
		public static SafeGdiplusStartupToken Startup()
		{
			SafeGdiplusStartupToken safeGdiplusStartupToken = new SafeGdiplusStartupToken();
			IntPtr handle;
			StartupOutput startupOutput;
			if (NativeMethods.GdiplusStartup(out handle, new StartupInput(), out startupOutput) == Status.Ok)
			{
				safeGdiplusStartupToken.handle = handle;
				return safeGdiplusStartupToken;
			}
			safeGdiplusStartupToken.Dispose();
			throw new Exception("Unable to initialize GDI+");
		}
	}
}
