using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000053 RID: 83
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct CREATESTRUCT
	{
		// Token: 0x04000434 RID: 1076
		public IntPtr lpCreateParams;

		// Token: 0x04000435 RID: 1077
		public IntPtr hInstance;

		// Token: 0x04000436 RID: 1078
		public IntPtr hMenu;

		// Token: 0x04000437 RID: 1079
		public IntPtr hwndParent;

		// Token: 0x04000438 RID: 1080
		public int cy;

		// Token: 0x04000439 RID: 1081
		public int cx;

		// Token: 0x0400043A RID: 1082
		public int y;

		// Token: 0x0400043B RID: 1083
		public int x;

		// Token: 0x0400043C RID: 1084
		public WS style;

		// Token: 0x0400043D RID: 1085
		[MarshalAs(UnmanagedType.LPWStr)]
		public string lpszName;

		// Token: 0x0400043E RID: 1086
		[MarshalAs(UnmanagedType.LPWStr)]
		public string lpszClass;

		// Token: 0x0400043F RID: 1087
		public WS_EX dwExStyle;
	}
}
