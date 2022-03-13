using System;
using System.Runtime.InteropServices;

namespace Standard;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
internal struct WNDCLASSEX
{
    public int cbSize;

    public CS style;

    public WndProc lpfnWndProc;

    public int cbClsExtra;

    public int cbWndExtra;

    public IntPtr hInstance;

    public IntPtr hIcon;

    public IntPtr hCursor;

    public IntPtr hbrBackground;

    [MarshalAs(UnmanagedType.LPWStr)]
    public string lpszMenuName;

    [MarshalAs(UnmanagedType.LPWStr)]
    public string lpszClassName;

    public IntPtr hIconSm;
}
