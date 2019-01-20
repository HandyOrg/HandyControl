namespace Standard
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal class MONITORINFO
    {
        public int cbSize = Marshal.SizeOf(typeof(Standard.MONITORINFO));
        public Standard.RECT rcMonitor;
        public Standard.RECT rcWork;
        public int dwFlags;
    }
}

