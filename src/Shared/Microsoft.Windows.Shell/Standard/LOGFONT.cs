using System;
using System.Runtime.InteropServices;

namespace Standard;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
internal struct LOGFONT
{
    public int lfHeight;

    public int lfWidth;

    public int lfEscapement;

    public int lfOrientation;

    public int lfWeight;

    public byte lfItalic;

    public byte lfUnderline;

    public byte lfStrikeOut;

    public byte lfCharSet;

    public byte lfOutPrecision;

    public byte lfClipPrecision;

    public byte lfQuality;

    public byte lfPitchAndFamily;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string lfFaceName;
}
