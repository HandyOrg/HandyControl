using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Standard
{
	// Token: 0x0200008C RID: 140
	[Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IShellItem
	{
		// Token: 0x0600019F RID: 415
		[return: MarshalAs(UnmanagedType.Interface)]
		object BindToHandler(IBindCtx pbc, [In] ref Guid bhid, [In] ref Guid riid);

		// Token: 0x060001A0 RID: 416
		IShellItem GetParent();

		// Token: 0x060001A1 RID: 417
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string GetDisplayName(SIGDN sigdnName);

		// Token: 0x060001A2 RID: 418
		SFGAO GetAttributes(SFGAO sfgaoMask);

		// Token: 0x060001A3 RID: 419
		int Compare(IShellItem psi, SICHINT hint);
	}
}
