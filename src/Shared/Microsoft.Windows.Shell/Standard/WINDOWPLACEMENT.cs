using System;
using System.Runtime.InteropServices;

namespace Standard;

[StructLayout(LayoutKind.Sequential)]
internal class WINDOWPLACEMENT
{
    public int length = Marshal.SizeOf(typeof(WINDOWPLACEMENT));

    public int flags;

    public SW showCmd;

    public POINT ptMinPosition;

    public POINT ptMaxPosition;

    public RECT rcNormalPosition;
}
