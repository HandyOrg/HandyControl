using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x0200005C RID: 92
	[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	internal class SHARDAPPIDINFOLINK
	{
		// Token: 0x04000476 RID: 1142
		private IntPtr psl;

		// Token: 0x04000477 RID: 1143
		[MarshalAs(UnmanagedType.LPWStr)]
		private string pszAppID;
	}
}
