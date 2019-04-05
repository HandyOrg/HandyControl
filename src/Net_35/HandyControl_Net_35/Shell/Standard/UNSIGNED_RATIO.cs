namespace Standard
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct UNSIGNED_RATIO
    {
        public uint uiNumerator;
        public uint uiDenominator;
    }
}

