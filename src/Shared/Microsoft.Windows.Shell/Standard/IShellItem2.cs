using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Standard
{
	// Token: 0x0200008E RID: 142
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("7e9fb0d3-919f-4307-ab2e-9b1860310c93")]
	[ComImport]
	internal interface IShellItem2 : IShellItem
	{
		// Token: 0x060001AB RID: 427
		[return: MarshalAs(UnmanagedType.Interface)]
		object BindToHandler([In] IBindCtx pbc, [In] ref Guid bhid, [In] ref Guid riid);

		// Token: 0x060001AC RID: 428
		IShellItem GetParent();

		// Token: 0x060001AD RID: 429
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string GetDisplayName(SIGDN sigdnName);

		// Token: 0x060001AE RID: 430
		SFGAO GetAttributes(SFGAO sfgaoMask);

		// Token: 0x060001AF RID: 431
		int Compare(IShellItem psi, SICHINT hint);

		// Token: 0x060001B0 RID: 432
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetPropertyStore(GPS flags, [In] ref Guid riid);

		// Token: 0x060001B1 RID: 433
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetPropertyStoreWithCreateObject(GPS flags, [MarshalAs(UnmanagedType.IUnknown)] object punkCreateObject, [In] ref Guid riid);

		// Token: 0x060001B2 RID: 434
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetPropertyStoreForKeys(IntPtr rgKeys, uint cKeys, GPS flags, [In] ref Guid riid);

		// Token: 0x060001B3 RID: 435
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetPropertyDescriptionList(IntPtr keyType, [In] ref Guid riid);

		// Token: 0x060001B4 RID: 436
		void Update(IBindCtx pbc);

		// Token: 0x060001B5 RID: 437
		PROPVARIANT GetProperty(IntPtr key);

		// Token: 0x060001B6 RID: 438
		Guid GetCLSID(IntPtr key);

		// Token: 0x060001B7 RID: 439
		System.Runtime.InteropServices.ComTypes.FILETIME GetFileTime(IntPtr key);

		// Token: 0x060001B8 RID: 440
		int GetInt32(IntPtr key);

		// Token: 0x060001B9 RID: 441
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string GetString(IntPtr key);

		// Token: 0x060001BA RID: 442
		uint GetUInt32(IntPtr key);

		// Token: 0x060001BB RID: 443
		ulong GetUInt64(IntPtr key);

		// Token: 0x060001BC RID: 444
		[return: MarshalAs(UnmanagedType.Bool)]
		void GetBool(IntPtr key);
	}
}
