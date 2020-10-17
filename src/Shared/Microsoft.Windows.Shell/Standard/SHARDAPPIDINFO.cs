using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x0200005A RID: 90
	[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	internal class SHARDAPPIDINFO
	{
		// Token: 0x04000472 RID: 1138
		[MarshalAs(UnmanagedType.Interface)]
		private object psi;

		// Token: 0x04000473 RID: 1139
		[MarshalAs(UnmanagedType.LPWStr)]
		private string pszAppID;
	}
}
