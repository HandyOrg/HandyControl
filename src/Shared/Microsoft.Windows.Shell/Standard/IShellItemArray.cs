using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Standard
{
	// Token: 0x0200008D RID: 141
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("B63EA76D-1F85-456F-A19C-48159EFA858B")]
	[ComImport]
	internal interface IShellItemArray
	{
		// Token: 0x060001A4 RID: 420
		[return: MarshalAs(UnmanagedType.Interface)]
		object BindToHandler(IBindCtx pbc, [In] ref Guid rbhid, [In] ref Guid riid);

		// Token: 0x060001A5 RID: 421
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetPropertyStore(int flags, [In] ref Guid riid);

		// Token: 0x060001A6 RID: 422
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetPropertyDescriptionList([In] ref PKEY keyType, [In] ref Guid riid);

		// Token: 0x060001A7 RID: 423
		uint GetAttributes(SIATTRIBFLAGS dwAttribFlags, uint sfgaoMask);

		// Token: 0x060001A8 RID: 424
		uint GetCount();

		// Token: 0x060001A9 RID: 425
		IShellItem GetItemAt(uint dwIndex);

		// Token: 0x060001AA RID: 426
		[return: MarshalAs(UnmanagedType.Interface)]
		object EnumItems();
	}
}
