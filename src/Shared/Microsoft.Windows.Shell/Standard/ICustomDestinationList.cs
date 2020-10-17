using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000094 RID: 148
	[Guid("6332debf-87b5-4670-90c0-5e57b408a49e")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ICustomDestinationList
	{
		// Token: 0x060001DF RID: 479
		void SetAppID([MarshalAs(UnmanagedType.LPWStr)] [In] string pszAppID);

		// Token: 0x060001E0 RID: 480
		[return: MarshalAs(UnmanagedType.Interface)]
		object BeginList(out uint pcMaxSlots, [In] ref Guid riid);

		// Token: 0x060001E1 RID: 481
		[PreserveSig]
		HRESULT AppendCategory([MarshalAs(UnmanagedType.LPWStr)] string pszCategory, IObjectArray poa);

		// Token: 0x060001E2 RID: 482
		void AppendKnownCategory(KDC category);

		// Token: 0x060001E3 RID: 483
		[PreserveSig]
		HRESULT AddUserTasks(IObjectArray poa);

		// Token: 0x060001E4 RID: 484
		void CommitList();

		// Token: 0x060001E5 RID: 485
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetRemovedDestinations([In] ref Guid riid);

		// Token: 0x060001E6 RID: 486
		void DeleteList([MarshalAs(UnmanagedType.LPWStr)] string pszAppID);

		// Token: 0x060001E7 RID: 487
		void AbortList();
	}
}
