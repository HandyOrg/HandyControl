using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000054 RID: 84
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct SHFILEOPSTRUCT
	{
		// Token: 0x04000440 RID: 1088
		public IntPtr hwnd;

		// Token: 0x04000441 RID: 1089
		[MarshalAs(UnmanagedType.U4)]
		public FO wFunc;

		// Token: 0x04000442 RID: 1090
		public string pFrom;

		// Token: 0x04000443 RID: 1091
		public string pTo;

		// Token: 0x04000444 RID: 1092
		[MarshalAs(UnmanagedType.U2)]
		public FOF fFlags;

		// Token: 0x04000445 RID: 1093
		[MarshalAs(UnmanagedType.Bool)]
		public int fAnyOperationsAborted;

		// Token: 0x04000446 RID: 1094
		public IntPtr hNameMappings;

		// Token: 0x04000447 RID: 1095
		public string lpszProgressTitle;
	}
}
