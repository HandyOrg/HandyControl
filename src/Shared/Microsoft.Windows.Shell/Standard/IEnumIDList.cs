using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000086 RID: 134
	[Guid("000214F2-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IEnumIDList
	{
		// Token: 0x06000180 RID: 384
		[PreserveSig]
		HRESULT Next(uint celt, out IntPtr rgelt, out int pceltFetched);

		// Token: 0x06000181 RID: 385
		[PreserveSig]
		HRESULT Skip(uint celt);

		// Token: 0x06000182 RID: 386
		void Reset();

		// Token: 0x06000183 RID: 387
		void Clone([MarshalAs(UnmanagedType.Interface)] out IEnumIDList ppenum);
	}
}
