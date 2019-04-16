namespace Standard
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, Guid("6332debf-87b5-4670-90c0-5e57b408a49e"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface ICustomDestinationList
    {
        void SetAppID([In, MarshalAs(UnmanagedType.LPWStr)] string pszAppID);
        [return: MarshalAs(UnmanagedType.Interface)]
        object BeginList(out uint pcMaxSlots, [In] ref Guid riid);
        [PreserveSig]
        Standard.HRESULT AppendCategory([MarshalAs(UnmanagedType.LPWStr)] string pszCategory, Standard.IObjectArray poa);
        void AppendKnownCategory(Standard.KDC category);
        [PreserveSig]
        Standard.HRESULT AddUserTasks(Standard.IObjectArray poa);
        void CommitList();
        [return: MarshalAs(UnmanagedType.Interface)]
        object GetRemovedDestinations([In] ref Guid riid);
        void DeleteList([MarshalAs(UnmanagedType.LPWStr)] string pszAppID);
        void AbortList();
    }
}

