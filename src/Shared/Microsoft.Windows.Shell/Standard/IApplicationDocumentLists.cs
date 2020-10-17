using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000093 RID: 147
	[Guid("3c594f9f-9f30-47a1-979a-c9e83d3d0a06")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IApplicationDocumentLists
	{
		// Token: 0x060001DD RID: 477
		void SetAppID([MarshalAs(UnmanagedType.LPWStr)] string pszAppID);

		// Token: 0x060001DE RID: 478
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object GetList([In] APPDOCLISTTYPE listtype, [In] uint cItemsDesired, [In] ref Guid riid);
	}
}
