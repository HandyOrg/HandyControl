using System;

namespace Standard;

internal delegate IntPtr WndProcHook(IntPtr hwnd, WM uMsg, IntPtr wParam, IntPtr lParam, ref bool handled);
