namespace Standard
{
    using System;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;

    [ComImport, Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IShellItem
    {
        [return: MarshalAs(UnmanagedType.Interface)]
        object BindToHandler(IBindCtx pbc, [In] ref Guid bhid, [In] ref Guid riid);
        Standard.IShellItem GetParent();
        [return: MarshalAs(UnmanagedType.LPWStr)]
        string GetDisplayName(Standard.SIGDN sigdnName);
        Standard.SFGAO GetAttributes(Standard.SFGAO sfgaoMask);
        int Compare(Standard.IShellItem psi, Standard.SICHINT hint);
    }
}

