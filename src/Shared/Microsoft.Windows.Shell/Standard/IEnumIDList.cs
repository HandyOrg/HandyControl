using System;
using System.Runtime.InteropServices;

namespace Standard;

[Guid("000214F2-0000-0000-C000-000000000046")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
internal interface IEnumIDList
{
    [PreserveSig]
    HRESULT Next(uint celt, out IntPtr rgelt, out int pceltFetched);

    [PreserveSig]
    HRESULT Skip(uint celt);

    void Reset();

    void Clone([MarshalAs(UnmanagedType.Interface)] out IEnumIDList ppenum);
}
