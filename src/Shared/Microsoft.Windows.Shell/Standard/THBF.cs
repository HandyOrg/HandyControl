using System;

namespace Standard
{
	// Token: 0x0200007B RID: 123
	[Flags]
	internal enum THBF : uint
	{
		// Token: 0x04000528 RID: 1320
		ENABLED = 0U,
		// Token: 0x04000529 RID: 1321
		DISABLED = 1U,
		// Token: 0x0400052A RID: 1322
		DISMISSONCLICK = 2U,
		// Token: 0x0400052B RID: 1323
		NOBACKGROUND = 4U,
		// Token: 0x0400052C RID: 1324
		HIDDEN = 8U,
		// Token: 0x0400052D RID: 1325
		NONINTERACTIVE = 16U
	}
}
