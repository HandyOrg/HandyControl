namespace Standard
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, Guid("71e806fb-8dee-46fc-bf8c-7748a8a1ae13"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IObjectWithProgId
    {
        void SetProgID([MarshalAs(UnmanagedType.LPWStr)] string pszProgID);
        [return: MarshalAs(UnmanagedType.LPWStr)]
        string GetProgID();
    }
}

