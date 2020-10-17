using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x0200006D RID: 109
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct WNDCLASSEX
	{
		// Token: 0x040004D1 RID: 1233
		public int cbSize;

		// Token: 0x040004D2 RID: 1234
		public CS style;

		// Token: 0x040004D3 RID: 1235
		public WndProc lpfnWndProc;

		// Token: 0x040004D4 RID: 1236
		public int cbClsExtra;

		// Token: 0x040004D5 RID: 1237
		public int cbWndExtra;

		// Token: 0x040004D6 RID: 1238
		public IntPtr hInstance;

		// Token: 0x040004D7 RID: 1239
		public IntPtr hIcon;

		// Token: 0x040004D8 RID: 1240
		public IntPtr hCursor;

		// Token: 0x040004D9 RID: 1241
		public IntPtr hbrBackground;

		// Token: 0x040004DA RID: 1242
		[MarshalAs(UnmanagedType.LPWStr)]
		public string lpszMenuName;

		// Token: 0x040004DB RID: 1243
		[MarshalAs(UnmanagedType.LPWStr)]
		public string lpszClassName;

		// Token: 0x040004DC RID: 1244
		public IntPtr hIconSm;
	}
}
