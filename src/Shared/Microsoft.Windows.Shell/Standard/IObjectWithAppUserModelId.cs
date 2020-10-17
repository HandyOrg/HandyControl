using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000095 RID: 149
	[Guid("36db0196-9665-46d1-9ba7-d3709eecf9ed")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IObjectWithAppUserModelId
	{
		// Token: 0x060001E8 RID: 488
		void SetAppID([MarshalAs(UnmanagedType.LPWStr)] string pszAppID);

		// Token: 0x060001E9 RID: 489
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string GetAppID();
	}
}
