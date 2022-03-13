using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Standard;

[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("B63EA76D-1F85-456F-A19C-48159EFA858B")]
[ComImport]
internal interface IShellItemArray
{
    [return: MarshalAs(UnmanagedType.Interface)]
    object BindToHandler(IBindCtx pbc, [In] ref Guid rbhid, [In] ref Guid riid);

    [return: MarshalAs(UnmanagedType.Interface)]
    object GetPropertyStore(int flags, [In] ref Guid riid);

    [return: MarshalAs(UnmanagedType.Interface)]
    object GetPropertyDescriptionList([In] ref PKEY keyType, [In] ref Guid riid);

    uint GetAttributes(SIATTRIBFLAGS dwAttribFlags, uint sfgaoMask);

    uint GetCount();

    IShellItem GetItemAt(uint dwIndex);

    [return: MarshalAs(UnmanagedType.Interface)]
    object EnumItems();
}
