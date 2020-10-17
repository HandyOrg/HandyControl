using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000062 RID: 98
	[StructLayout(LayoutKind.Sequential)]
	internal class MONITORINFO
	{
		// Token: 0x040004A2 RID: 1186
		public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));

		// Token: 0x040004A3 RID: 1187
		public RECT rcMonitor;

		// Token: 0x040004A4 RID: 1188
		public RECT rcWork;

		// Token: 0x040004A5 RID: 1189
		public int dwFlags;
	}
}
