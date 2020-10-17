using System;

namespace Standard
{
	// Token: 0x02000042 RID: 66
	[Flags]
	internal enum NIF : uint
	{
		// Token: 0x040003F3 RID: 1011
		MESSAGE = 1U,
		// Token: 0x040003F4 RID: 1012
		ICON = 2U,
		// Token: 0x040003F5 RID: 1013
		TIP = 4U,
		// Token: 0x040003F6 RID: 1014
		STATE = 8U,
		// Token: 0x040003F7 RID: 1015
		INFO = 16U,
		// Token: 0x040003F8 RID: 1016
		GUID = 32U,
		// Token: 0x040003F9 RID: 1017
		REALTIME = 64U,
		// Token: 0x040003FA RID: 1018
		SHOWTIP = 128U,
		// Token: 0x040003FB RID: 1019
		XP_MASK = 59U,
		// Token: 0x040003FC RID: 1020
		VISTA_MASK = 251U
	}
}
