using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000087 RID: 135
	[Guid("2c1c7e2e-2d0e-4059-831e-1e6f82335c2e")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IEnumObjects
	{
		// Token: 0x06000184 RID: 388
		void Next(uint celt, [In] ref Guid riid, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.IUnknown, SizeParamIndex = 0)] [Out] object[] rgelt, out uint pceltFetched);

		// Token: 0x06000185 RID: 389
		void Skip(uint celt);

		// Token: 0x06000186 RID: 390
		void Reset();

		// Token: 0x06000187 RID: 391
		IEnumObjects Clone();
	}
}
