namespace Standard
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, Guid("36db0196-9665-46d1-9ba7-d3709eecf9ed"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IObjectWithAppUserModelId
    {
        void SetAppID([MarshalAs(UnmanagedType.LPWStr)] string pszAppID);
        [return: MarshalAs(UnmanagedType.LPWStr)]
        string GetAppID();
    }
}

