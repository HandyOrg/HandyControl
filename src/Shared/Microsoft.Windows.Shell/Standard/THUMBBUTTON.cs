using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000084 RID: 132
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 8)]
	internal struct THUMBBUTTON
	{
		// Token: 0x04000587 RID: 1415
		public const int THBN_CLICKED = 6144;

		// Token: 0x04000588 RID: 1416
		public THB dwMask;

		// Token: 0x04000589 RID: 1417
		public uint iId;

		// Token: 0x0400058A RID: 1418
		public uint iBitmap;

		// Token: 0x0400058B RID: 1419
		public IntPtr hIcon;

		// Token: 0x0400058C RID: 1420
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string szTip;

		// Token: 0x0400058D RID: 1421
		public THBF dwFlags;
	}
}
