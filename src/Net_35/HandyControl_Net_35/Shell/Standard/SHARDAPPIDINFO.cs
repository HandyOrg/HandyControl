namespace Standard
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4), SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class SHARDAPPIDINFO
    {
        [MarshalAs(UnmanagedType.Interface)]
        private object psi;
        [MarshalAs(UnmanagedType.LPWStr)]
        private string pszAppID;
    }
}

