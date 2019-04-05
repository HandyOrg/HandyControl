namespace Standard
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct MINMAXINFO
    {
        public Standard.POINT ptReserved;
        public Standard.POINT ptMaxSize;
        public Standard.POINT ptMaxPosition;
        public Standard.POINT ptMinTrackSize;
        public Standard.POINT ptMaxTrackSize;
    }
}

