using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Standard;

[BestFitMapping(false)]
[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
internal class WIN32_FIND_DATAW
{
    public FileAttributes dwFileAttributes;

    public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;

    public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;

    public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;

    public int nFileSizeHigh;

    public int nFileSizeLow;

    public int dwReserved0;

    public int dwReserved1;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
    public string cFileName;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
    public string cAlternateFileName;
}
