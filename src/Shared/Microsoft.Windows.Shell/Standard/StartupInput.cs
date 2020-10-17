using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000069 RID: 105
	[StructLayout(LayoutKind.Sequential)]
	internal class StartupInput
	{
		// Token: 0x040004B6 RID: 1206
		public int GdiplusVersion = 1;

		// Token: 0x040004B7 RID: 1207
		public IntPtr DebugEventCallback;

		// Token: 0x040004B8 RID: 1208
		public bool SuppressBackgroundThread;

		// Token: 0x040004B9 RID: 1209
		public bool SuppressExternalCodecs;
	}
}
