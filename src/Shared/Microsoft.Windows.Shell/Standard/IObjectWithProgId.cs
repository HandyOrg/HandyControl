using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000096 RID: 150
	[Guid("71e806fb-8dee-46fc-bf8c-7748a8a1ae13")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IObjectWithProgId
	{
		// Token: 0x060001EA RID: 490
		void SetProgID([MarshalAs(UnmanagedType.LPWStr)] string pszProgID);

		// Token: 0x060001EB RID: 491
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string GetProgID();
	}
}
