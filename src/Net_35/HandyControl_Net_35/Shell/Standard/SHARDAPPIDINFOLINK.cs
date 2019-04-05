namespace Standard
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4), SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class SHARDAPPIDINFOLINK
    {
        private IntPtr psl;
        [MarshalAs(UnmanagedType.LPWStr)]
        private string pszAppID;
    }
}

