using System;
using System.Runtime.InteropServices;

namespace Standard;

[Guid("6332debf-87b5-4670-90c0-5e57b408a49e")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
internal interface ICustomDestinationList
{
    void SetAppID([MarshalAs(UnmanagedType.LPWStr)][In] string pszAppID);

    [return: MarshalAs(UnmanagedType.Interface)]
    object BeginList(out uint pcMaxSlots, [In] ref Guid riid);

    [PreserveSig]
    HRESULT AppendCategory([MarshalAs(UnmanagedType.LPWStr)] string pszCategory, IObjectArray poa);

    void AppendKnownCategory(KDC category);

    [PreserveSig]
    HRESULT AddUserTasks(IObjectArray poa);

    void CommitList();

    [return: MarshalAs(UnmanagedType.Interface)]
    object GetRemovedDestinations([In] ref Guid riid);

    void DeleteList([MarshalAs(UnmanagedType.LPWStr)] string pszAppID);

    void AbortList();
}
