namespace Standard
{
    using System;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("B63EA76D-1F85-456F-A19C-48159EFA858B")]
    internal interface IShellItemArray
    {
        [return: MarshalAs(UnmanagedType.Interface)]
        object BindToHandler(IBindCtx pbc, [In] ref Guid rbhid, [In] ref Guid riid);
        [return: MarshalAs(UnmanagedType.Interface)]
        object GetPropertyStore(int flags, [In] ref Guid riid);
        [return: MarshalAs(UnmanagedType.Interface)]
        object GetPropertyDescriptionList([In] ref Standard.PKEY keyType, [In] ref Guid riid);
        uint GetAttributes(Standard.SIATTRIBFLAGS dwAttribFlags, uint sfgaoMask);
        uint GetCount();
        Standard.IShellItem GetItemAt(uint dwIndex);
        [return: MarshalAs(UnmanagedType.Interface)]
        object EnumItems();
    }
}

