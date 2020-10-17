using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Standard
{
	// Token: 0x0200006A RID: 106
	[BestFitMapping(false)]
	[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal class WIN32_FIND_DATAW
	{
		// Token: 0x040004BA RID: 1210
		public FileAttributes dwFileAttributes;

		// Token: 0x040004BB RID: 1211
		public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;

		// Token: 0x040004BC RID: 1212
		public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;

		// Token: 0x040004BD RID: 1213
		public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;

		// Token: 0x040004BE RID: 1214
		public int nFileSizeHigh;

		// Token: 0x040004BF RID: 1215
		public int nFileSizeLow;

		// Token: 0x040004C0 RID: 1216
		public int dwReserved0;

		// Token: 0x040004C1 RID: 1217
		public int dwReserved1;

		// Token: 0x040004C2 RID: 1218
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string cFileName;

		// Token: 0x040004C3 RID: 1219
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
		public string cAlternateFileName;
	}
}
