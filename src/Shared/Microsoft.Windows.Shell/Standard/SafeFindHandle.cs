using System;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;

namespace Standard
{
	// Token: 0x02000047 RID: 71
	internal sealed class SafeFindHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x060000A2 RID: 162 RVA: 0x000042AE File Offset: 0x000024AE
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		private SafeFindHandle() : base(true)
		{
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x000042B7 File Offset: 0x000024B7
		protected override bool ReleaseHandle()
		{
			return NativeMethods.FindClose(this.handle);
		}
	}
}
