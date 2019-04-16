namespace Standard
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal delegate IntPtr MessageHandler(Standard.WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled);
}

