using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000088 RID: 136
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("92CA9DCD-5622-4bba-A805-5E9F541BD8C9")]
	[ComImport]
	internal interface IObjectArray
	{
		// Token: 0x06000188 RID: 392
		uint GetCount();

		// Token: 0x06000189 RID: 393
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object GetAt([In] uint uiIndex, [In] ref Guid riid);
	}
}
