using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x0200006B RID: 107
	[StructLayout(LayoutKind.Sequential)]
	internal class WINDOWPLACEMENT
	{
		// Token: 0x040004C4 RID: 1220
		public int length = Marshal.SizeOf(typeof(WINDOWPLACEMENT));

		// Token: 0x040004C5 RID: 1221
		public int flags;

		// Token: 0x040004C6 RID: 1222
		public SW showCmd;

		// Token: 0x040004C7 RID: 1223
		public POINT ptMinPosition;

		// Token: 0x040004C8 RID: 1224
		public POINT ptMaxPosition;

		// Token: 0x040004C9 RID: 1225
		public RECT rcNormalPosition;
	}
}
