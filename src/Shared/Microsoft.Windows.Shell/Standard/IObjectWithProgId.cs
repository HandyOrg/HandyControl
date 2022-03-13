using System;
using System.Runtime.InteropServices;

namespace Standard;

[Guid("71e806fb-8dee-46fc-bf8c-7748a8a1ae13")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
internal interface IObjectWithProgId
{
    void SetProgID([MarshalAs(UnmanagedType.LPWStr)] string pszProgID);

    [return: MarshalAs(UnmanagedType.LPWStr)]
    string GetProgID();
}
