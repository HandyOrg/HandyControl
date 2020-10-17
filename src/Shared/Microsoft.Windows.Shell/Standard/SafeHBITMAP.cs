using System;
using System.Runtime.ConstrainedExecution;
using Microsoft.Win32.SafeHandles;

namespace Standard
{
	// Token: 0x0200004A RID: 74
	internal sealed class SafeHBITMAP : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x060000B1 RID: 177 RVA: 0x00004486 File Offset: 0x00002686
		private SafeHBITMAP() : base(true)
		{
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x0000448F File Offset: 0x0000268F
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		protected override bool ReleaseHandle()
		{
			return NativeMethods.DeleteObject(this.handle);
		}
	}
}
