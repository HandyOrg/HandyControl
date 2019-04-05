namespace Standard
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct HIGHCONTRAST
    {
        public int cbSize;
        public Standard.HCF dwFlags;
        public IntPtr lpszDefaultScheme;
    }
}

