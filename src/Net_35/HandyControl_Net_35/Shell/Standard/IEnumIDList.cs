namespace Standard
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, Guid("000214F2-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IEnumIDList
    {
        [PreserveSig]
        Standard.HRESULT Next(uint celt, out IntPtr rgelt, out int pceltFetched);
        [PreserveSig]
        Standard.HRESULT Skip(uint celt);
        void Reset();
        void Clone([MarshalAs(UnmanagedType.Interface)] out Standard.IEnumIDList ppenum);
    }
}

