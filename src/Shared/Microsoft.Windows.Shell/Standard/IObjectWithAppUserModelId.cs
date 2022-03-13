using System;
using System.Runtime.InteropServices;

namespace Standard;

[Guid("36db0196-9665-46d1-9ba7-d3709eecf9ed")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
internal interface IObjectWithAppUserModelId
{
    void SetAppID([MarshalAs(UnmanagedType.LPWStr)] string pszAppID);

    [return: MarshalAs(UnmanagedType.LPWStr)]
    string GetAppID();
}
