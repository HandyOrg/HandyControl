namespace Standard
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode, Pack=8)]
    internal struct THUMBBUTTON
    {
        public const int THBN_CLICKED = 0x1800;
        public Standard.THB dwMask;
        public uint iId;
        public uint iBitmap;
        public IntPtr hIcon;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=260)]
        public string szTip;
        public Standard.THBF dwFlags;
    }
}

