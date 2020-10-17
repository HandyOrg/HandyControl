using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000071 RID: 113
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct DWM_TIMING_INFO
	{
		// Token: 0x040004E7 RID: 1255
		public int cbSize;

		// Token: 0x040004E8 RID: 1256
		public UNSIGNED_RATIO rateRefresh;

		// Token: 0x040004E9 RID: 1257
		public ulong qpcRefreshPeriod;

		// Token: 0x040004EA RID: 1258
		public UNSIGNED_RATIO rateCompose;

		// Token: 0x040004EB RID: 1259
		public ulong qpcVBlank;

		// Token: 0x040004EC RID: 1260
		public ulong cRefresh;

		// Token: 0x040004ED RID: 1261
		public uint cDXRefresh;

		// Token: 0x040004EE RID: 1262
		public ulong qpcCompose;

		// Token: 0x040004EF RID: 1263
		public ulong cFrame;

		// Token: 0x040004F0 RID: 1264
		public uint cDXPresent;

		// Token: 0x040004F1 RID: 1265
		public ulong cRefreshFrame;

		// Token: 0x040004F2 RID: 1266
		public ulong cFrameSubmitted;

		// Token: 0x040004F3 RID: 1267
		public uint cDXPresentSubmitted;

		// Token: 0x040004F4 RID: 1268
		public ulong cFrameConfirmed;

		// Token: 0x040004F5 RID: 1269
		public uint cDXPresentConfirmed;

		// Token: 0x040004F6 RID: 1270
		public ulong cRefreshConfirmed;

		// Token: 0x040004F7 RID: 1271
		public uint cDXRefreshConfirmed;

		// Token: 0x040004F8 RID: 1272
		public ulong cFramesLate;

		// Token: 0x040004F9 RID: 1273
		public uint cFramesOutstanding;

		// Token: 0x040004FA RID: 1274
		public ulong cFrameDisplayed;

		// Token: 0x040004FB RID: 1275
		public ulong qpcFrameDisplayed;

		// Token: 0x040004FC RID: 1276
		public ulong cRefreshFrameDisplayed;

		// Token: 0x040004FD RID: 1277
		public ulong cFrameComplete;

		// Token: 0x040004FE RID: 1278
		public ulong qpcFrameComplete;

		// Token: 0x040004FF RID: 1279
		public ulong cFramePending;

		// Token: 0x04000500 RID: 1280
		public ulong qpcFramePending;

		// Token: 0x04000501 RID: 1281
		public ulong cFramesDisplayed;

		// Token: 0x04000502 RID: 1282
		public ulong cFramesComplete;

		// Token: 0x04000503 RID: 1283
		public ulong cFramesPending;

		// Token: 0x04000504 RID: 1284
		public ulong cFramesAvailable;

		// Token: 0x04000505 RID: 1285
		public ulong cFramesDropped;

		// Token: 0x04000506 RID: 1286
		public ulong cFramesMissed;

		// Token: 0x04000507 RID: 1287
		public ulong cRefreshNextDisplayed;

		// Token: 0x04000508 RID: 1288
		public ulong cRefreshNextPresented;

		// Token: 0x04000509 RID: 1289
		public ulong cRefreshesDisplayed;

		// Token: 0x0400050A RID: 1290
		public ulong cRefreshesPresented;

		// Token: 0x0400050B RID: 1291
		public ulong cRefreshStarted;

		// Token: 0x0400050C RID: 1292
		public ulong cPixelsReceived;

		// Token: 0x0400050D RID: 1293
		public ulong cPixelsDrawn;

		// Token: 0x0400050E RID: 1294
		public ulong cBuffersEmpty;
	}
}
