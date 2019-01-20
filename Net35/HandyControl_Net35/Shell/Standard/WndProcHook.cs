namespace Standard
{
    using System;
    using System.Runtime.CompilerServices;

    internal delegate IntPtr WndProcHook(IntPtr hwnd, Standard.WM uMsg, IntPtr wParam, IntPtr lParam, ref bool handled);
}

