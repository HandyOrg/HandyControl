namespace Standard
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, Guid("3c594f9f-9f30-47a1-979a-c9e83d3d0a06"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IApplicationDocumentLists
    {
        void SetAppID([MarshalAs(UnmanagedType.LPWStr)] string pszAppID);
        [return: MarshalAs(UnmanagedType.IUnknown)]
        object GetList([In] Standard.APPDOCLISTTYPE listtype, [In] uint cItemsDesired, [In] ref Guid riid);
    }
}

