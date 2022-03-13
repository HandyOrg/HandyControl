using System;
using System.Runtime.InteropServices;

namespace Standard;

[Guid("2c1c7e2e-2d0e-4059-831e-1e6f82335c2e")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
internal interface IEnumObjects
{
    void Next(uint celt, [In] ref Guid riid, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.IUnknown, SizeParamIndex = 0)][Out] object[] rgelt, out uint pceltFetched);

    void Skip(uint celt);

    void Reset();

    IEnumObjects Clone();
}
