using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Standard;

[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
[StructLayout(LayoutKind.Sequential, Pack = 4)]
internal class SHARDAPPIDINFOIDLIST
{
    private IntPtr pidl;

    [MarshalAs(UnmanagedType.LPWStr)]
    private string pszAppID;
}
