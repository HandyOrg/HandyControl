using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000060 RID: 96
	[StructLayout(LayoutKind.Explicit)]
	internal struct WTA_OPTIONS
	{
		// Token: 0x0400049B RID: 1179
		public const uint Size = 8U;

		// Token: 0x0400049C RID: 1180
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Used by native code.")]
		[FieldOffset(0)]
		public WTNCA dwFlags;

		// Token: 0x0400049D RID: 1181
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Used by native code.")]
		[FieldOffset(4)]
		public WTNCA dwMask;
	}
}
