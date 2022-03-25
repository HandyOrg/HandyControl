using System;
using System.Runtime.InteropServices;

namespace Standard;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
internal struct CREATESTRUCT
{
    public IntPtr lpCreateParams;

    public IntPtr hInstance;

    public IntPtr hMenu;

    public IntPtr hwndParent;

    public int cy;

    public int cx;

    public int y;

    public int x;

    public WS style;

    [MarshalAs(UnmanagedType.LPWStr)]
    public string lpszName;

    [MarshalAs(UnmanagedType.LPWStr)]
    public string lpszClass;

    public WS_EX dwExStyle;
}
