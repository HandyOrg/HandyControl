using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000085 RID: 133
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	internal struct PKEY
	{
		// Token: 0x0600017E RID: 382 RVA: 0x00005264 File Offset: 0x00003464
		public PKEY(Guid fmtid, uint pid)
		{
			this._fmtid = fmtid;
			this._pid = pid;
		}

		// Token: 0x0400058E RID: 1422
		private readonly Guid _fmtid;

		// Token: 0x0400058F RID: 1423
		private readonly uint _pid;

		// Token: 0x04000590 RID: 1424
		public static readonly PKEY Title = new PKEY(new Guid("F29F85E0-4FF9-1068-AB91-08002B27B3D9"), 2U);

		// Token: 0x04000591 RID: 1425
		public static readonly PKEY AppUserModel_ID = new PKEY(new Guid("9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3"), 5U);

		// Token: 0x04000592 RID: 1426
		public static readonly PKEY AppUserModel_IsDestListSeparator = new PKEY(new Guid("9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3"), 6U);

		// Token: 0x04000593 RID: 1427
		public static readonly PKEY AppUserModel_RelaunchCommand = new PKEY(new Guid("9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3"), 2U);

		// Token: 0x04000594 RID: 1428
		public static readonly PKEY AppUserModel_RelaunchDisplayNameResource = new PKEY(new Guid("9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3"), 4U);

		// Token: 0x04000595 RID: 1429
		public static readonly PKEY AppUserModel_RelaunchIconResource = new PKEY(new Guid("9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3"), 3U);
	}
}
