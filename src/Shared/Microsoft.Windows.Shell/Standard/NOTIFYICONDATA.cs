using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000057 RID: 87
	[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
	[StructLayout(LayoutKind.Sequential)]
	internal class NOTIFYICONDATA
	{
		// Token: 0x0400045E RID: 1118
		public int cbSize;

		// Token: 0x0400045F RID: 1119
		public IntPtr hWnd;

		// Token: 0x04000460 RID: 1120
		public int uID;

		// Token: 0x04000461 RID: 1121
		public NIF uFlags;

		// Token: 0x04000462 RID: 1122
		public int uCallbackMessage;

		// Token: 0x04000463 RID: 1123
		public IntPtr hIcon;

		// Token: 0x04000464 RID: 1124
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
		public char[] szTip = new char[128];

		// Token: 0x04000465 RID: 1125
		public uint dwState;

		// Token: 0x04000466 RID: 1126
		public uint dwStateMask;

		// Token: 0x04000467 RID: 1127
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		public char[] szInfo = new char[256];

		// Token: 0x04000468 RID: 1128
		public uint uVersion;

		// Token: 0x04000469 RID: 1129
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		public char[] szInfoTitle = new char[64];

		// Token: 0x0400046A RID: 1130
		public uint dwInfoFlags;

		// Token: 0x0400046B RID: 1131
		public Guid guidItem;

		// Token: 0x0400046C RID: 1132
		private IntPtr hBalloonIcon;
	}
}
