using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x0200005B RID: 91
	[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	internal class SHARDAPPIDINFOIDLIST
	{
		// Token: 0x04000474 RID: 1140
		private IntPtr pidl;

		// Token: 0x04000475 RID: 1141
		[MarshalAs(UnmanagedType.LPWStr)]
		private string pszAppID;
	}
}
