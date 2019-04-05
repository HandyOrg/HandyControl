namespace Standard
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct BITMAPINFO
    {
        public Standard.BITMAPINFOHEADER bmiHeader;
        public Standard.RGBQUAD bmiColors;
    }
}

