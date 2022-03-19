using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Standard;

[Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
internal interface IShellItem
{
    [return: MarshalAs(UnmanagedType.Interface)]
    object BindToHandler(IBindCtx pbc, [In] ref Guid bhid, [In] ref Guid riid);

    IShellItem GetParent();

    [return: MarshalAs(UnmanagedType.LPWStr)]
    string GetDisplayName(SIGDN sigdnName);

    SFGAO GetAttributes(SFGAO sfgaoMask);

    int Compare(IShellItem psi, SICHINT hint);
}
