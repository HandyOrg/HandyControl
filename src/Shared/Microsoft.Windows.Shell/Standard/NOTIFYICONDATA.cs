using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Standard;

[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
[StructLayout(LayoutKind.Sequential)]
internal class NOTIFYICONDATA
{
    public int cbSize;

    public IntPtr hWnd;

    public int uID;

    public NIF uFlags;

    public int uCallbackMessage;

    public IntPtr hIcon;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
    public char[] szTip = new char[128];

    public uint dwState;

    public uint dwStateMask;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    public char[] szInfo = new char[256];

    public uint uVersion;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    public char[] szInfoTitle = new char[64];

    public uint dwInfoFlags;

    public Guid guidItem;

    private IntPtr hBalloonIcon;
}
