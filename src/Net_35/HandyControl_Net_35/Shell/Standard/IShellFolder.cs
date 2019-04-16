namespace Standard
{
    using System;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("000214E6-0000-0000-C000-000000000046")]
    internal interface IShellFolder
    {
        void ParseDisplayName([In] IntPtr hwnd, [In] IBindCtx pbc, [In, MarshalAs(UnmanagedType.LPWStr)] string pszDisplayName, [In, Out] ref int pchEaten, out IntPtr ppidl, [In, Out] ref uint pdwAttributes);
        Standard.IEnumIDList EnumObjects([In] IntPtr hwnd, [In] Standard.SHCONTF grfFlags);
        [return: MarshalAs(UnmanagedType.Interface)]
        object BindToObject([In] IntPtr pidl, [In] IBindCtx pbc, [In] ref Guid riid);
        [return: MarshalAs(UnmanagedType.Interface)]
        object BindToStorage([In] IntPtr pidl, [In] IBindCtx pbc, [In] ref Guid riid);
        [PreserveSig]
        Standard.HRESULT CompareIDs([In] IntPtr lParam, [In] IntPtr pidl1, [In] IntPtr pidl2);
        [return: MarshalAs(UnmanagedType.Interface)]
        object CreateViewObject([In] IntPtr hwndOwner, [In] ref Guid riid);
        void GetAttributesOf([In] uint cidl, [In] IntPtr apidl, [In, Out] ref Standard.SFGAO rgfInOut);
        [return: MarshalAs(UnmanagedType.Interface)]
        object GetUIObjectOf([In] IntPtr hwndOwner, [In] uint cidl, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.SysInt, SizeParamIndex=2)] IntPtr apidl, [In] ref Guid riid, [In, Out] ref uint rgfReserved);
        void GetDisplayNameOf([In] IntPtr pidl, [In] Standard.SHGDN uFlags, out IntPtr pName);
        void SetNameOf([In] IntPtr hwnd, [In] IntPtr pidl, [In, MarshalAs(UnmanagedType.LPWStr)] string pszName, [In] Standard.SHGDN uFlags, out IntPtr ppidlOut);
    }
}

