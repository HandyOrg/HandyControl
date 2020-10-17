using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000092 RID: 146
	[Guid("12337d35-94c6-48a0-bce7-6a9c69d4d600")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IApplicationDestinations
	{
		// Token: 0x060001DA RID: 474
		void SetAppID([MarshalAs(UnmanagedType.LPWStr)] [In] string pszAppID);

		// Token: 0x060001DB RID: 475
		void RemoveDestination([MarshalAs(UnmanagedType.IUnknown)] object punk);

		// Token: 0x060001DC RID: 476
		void RemoveAllDestinations();
	}
}
