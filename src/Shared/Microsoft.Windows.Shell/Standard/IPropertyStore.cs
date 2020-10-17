using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x0200008A RID: 138
	[Guid("886d8eeb-8cf2-4446-8d02-cdba1dbdcf99")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IPropertyStore
	{
		// Token: 0x06000190 RID: 400
		uint GetCount();

		// Token: 0x06000191 RID: 401
		PKEY GetAt(uint iProp);

		// Token: 0x06000192 RID: 402
		void GetValue([In] ref PKEY pkey, [In] [Out] PROPVARIANT pv);

		// Token: 0x06000193 RID: 403
		void SetValue([In] ref PKEY pkey, PROPVARIANT pv);

		// Token: 0x06000194 RID: 404
		void Commit();
	}
}
