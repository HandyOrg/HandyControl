using System;
using System.Runtime.InteropServices;

namespace Standard;

[Guid("12337d35-94c6-48a0-bce7-6a9c69d4d600")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
internal interface IApplicationDestinations
{
    void SetAppID([MarshalAs(UnmanagedType.LPWStr)][In] string pszAppID);

    void RemoveDestination([MarshalAs(UnmanagedType.IUnknown)] object punk);

    void RemoveAllDestinations();
}
