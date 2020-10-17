using System;

namespace Standard
{
	// Token: 0x02000021 RID: 33
	[Flags]
	internal enum ErrorModes
	{
		// Token: 0x040000EF RID: 239
		Default = 0,
		// Token: 0x040000F0 RID: 240
		FailCriticalErrors = 1,
		// Token: 0x040000F1 RID: 241
		NoGpFaultErrorBox = 2,
		// Token: 0x040000F2 RID: 242
		NoAlignmentFaultExcept = 4,
		// Token: 0x040000F3 RID: 243
		NoOpenFileErrorBox = 32768
	}
}
