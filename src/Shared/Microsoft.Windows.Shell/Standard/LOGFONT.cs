using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x0200005D RID: 93
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct LOGFONT
	{
		// Token: 0x04000478 RID: 1144
		public int lfHeight;

		// Token: 0x04000479 RID: 1145
		public int lfWidth;

		// Token: 0x0400047A RID: 1146
		public int lfEscapement;

		// Token: 0x0400047B RID: 1147
		public int lfOrientation;

		// Token: 0x0400047C RID: 1148
		public int lfWeight;

		// Token: 0x0400047D RID: 1149
		public byte lfItalic;

		// Token: 0x0400047E RID: 1150
		public byte lfUnderline;

		// Token: 0x0400047F RID: 1151
		public byte lfStrikeOut;

		// Token: 0x04000480 RID: 1152
		public byte lfCharSet;

		// Token: 0x04000481 RID: 1153
		public byte lfOutPrecision;

		// Token: 0x04000482 RID: 1154
		public byte lfClipPrecision;

		// Token: 0x04000483 RID: 1155
		public byte lfQuality;

		// Token: 0x04000484 RID: 1156
		public byte lfPitchAndFamily;

		// Token: 0x04000485 RID: 1157
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string lfFaceName;
	}
}
