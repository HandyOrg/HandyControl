using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Standard
{
	// Token: 0x0200008B RID: 139
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("000214E6-0000-0000-C000-000000000046")]
	[ComImport]
	internal interface IShellFolder
	{
		// Token: 0x06000195 RID: 405
		void ParseDisplayName([In] IntPtr hwnd, [In] IBindCtx pbc, [MarshalAs(UnmanagedType.LPWStr)] [In] string pszDisplayName, [In] [Out] ref int pchEaten, out IntPtr ppidl, [In] [Out] ref uint pdwAttributes);

		// Token: 0x06000196 RID: 406
		IEnumIDList EnumObjects([In] IntPtr hwnd, [In] SHCONTF grfFlags);

		// Token: 0x06000197 RID: 407
		[return: MarshalAs(UnmanagedType.Interface)]
		object BindToObject([In] IntPtr pidl, [In] IBindCtx pbc, [In] ref Guid riid);

		// Token: 0x06000198 RID: 408
		[return: MarshalAs(UnmanagedType.Interface)]
		object BindToStorage([In] IntPtr pidl, [In] IBindCtx pbc, [In] ref Guid riid);

		// Token: 0x06000199 RID: 409
		[PreserveSig]
		HRESULT CompareIDs([In] IntPtr lParam, [In] IntPtr pidl1, [In] IntPtr pidl2);

		// Token: 0x0600019A RID: 410
		[return: MarshalAs(UnmanagedType.Interface)]
		object CreateViewObject([In] IntPtr hwndOwner, [In] ref Guid riid);

		// Token: 0x0600019B RID: 411
		void GetAttributesOf([In] uint cidl, [In] IntPtr apidl, [In] [Out] ref SFGAO rgfInOut);

		// Token: 0x0600019C RID: 412
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetUIObjectOf([In] IntPtr hwndOwner, [In] uint cidl, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysInt, SizeParamIndex = 2)] [In] IntPtr apidl, [In] ref Guid riid, [In] [Out] ref uint rgfReserved);

		// Token: 0x0600019D RID: 413
		void GetDisplayNameOf([In] IntPtr pidl, [In] SHGDN uFlags, out IntPtr pName);

		// Token: 0x0600019E RID: 414
		void SetNameOf([In] IntPtr hwnd, [In] IntPtr pidl, [MarshalAs(UnmanagedType.LPWStr)] [In] string pszName, [In] SHGDN uFlags, out IntPtr ppidlOut);
	}
}
