using System;
using System.Runtime.InteropServices;

namespace Standard;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 8)]
internal struct THUMBBUTTON
{
    public const int THBN_CLICKED = 6144;

    public THB dwMask;

    public uint iId;

    public uint iBitmap;

    public IntPtr hIcon;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
    public string szTip;

    public THBF dwFlags;
}
