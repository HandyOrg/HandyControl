using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x0200005F RID: 95
	internal struct NONCLIENTMETRICS
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00004714 File Offset: 0x00002914
		[SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
		public static NONCLIENTMETRICS VistaMetricsStruct
		{
			get
			{
				return new NONCLIENTMETRICS
				{
					cbSize = Marshal.SizeOf(typeof(NONCLIENTMETRICS))
				};
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00004740 File Offset: 0x00002940
		[SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
		public static NONCLIENTMETRICS XPMetricsStruct
		{
			get
			{
				return new NONCLIENTMETRICS
				{
					cbSize = Marshal.SizeOf(typeof(NONCLIENTMETRICS)) - 4
				};
			}
		}

		// Token: 0x0400048B RID: 1163
		public int cbSize;

		// Token: 0x0400048C RID: 1164
		public int iBorderWidth;

		// Token: 0x0400048D RID: 1165
		public int iScrollWidth;

		// Token: 0x0400048E RID: 1166
		public int iScrollHeight;

		// Token: 0x0400048F RID: 1167
		public int iCaptionWidth;

		// Token: 0x04000490 RID: 1168
		public int iCaptionHeight;

		// Token: 0x04000491 RID: 1169
		public LOGFONT lfCaptionFont;

		// Token: 0x04000492 RID: 1170
		public int iSmCaptionWidth;

		// Token: 0x04000493 RID: 1171
		public int iSmCaptionHeight;

		// Token: 0x04000494 RID: 1172
		public LOGFONT lfSmCaptionFont;

		// Token: 0x04000495 RID: 1173
		public int iMenuWidth;

		// Token: 0x04000496 RID: 1174
		public int iMenuHeight;

		// Token: 0x04000497 RID: 1175
		public LOGFONT lfMenuFont;

		// Token: 0x04000498 RID: 1176
		public LOGFONT lfStatusFont;

		// Token: 0x04000499 RID: 1177
		public LOGFONT lfMessageFont;

		// Token: 0x0400049A RID: 1178
		public int iPaddedBorderWidth;
	}
}
