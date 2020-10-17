using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000050 RID: 80
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	internal struct BITMAPINFOHEADER
	{
		// Token: 0x04000425 RID: 1061
		public int biSize;

		// Token: 0x04000426 RID: 1062
		public int biWidth;

		// Token: 0x04000427 RID: 1063
		public int biHeight;

		// Token: 0x04000428 RID: 1064
		public short biPlanes;

		// Token: 0x04000429 RID: 1065
		public short biBitCount;

		// Token: 0x0400042A RID: 1066
		public BI biCompression;

		// Token: 0x0400042B RID: 1067
		public int biSizeImage;

		// Token: 0x0400042C RID: 1068
		public int biXPelsPerMeter;

		// Token: 0x0400042D RID: 1069
		public int biYPelsPerMeter;

		// Token: 0x0400042E RID: 1070
		public int biClrUsed;

		// Token: 0x0400042F RID: 1071
		public int biClrImportant;
	}
}
