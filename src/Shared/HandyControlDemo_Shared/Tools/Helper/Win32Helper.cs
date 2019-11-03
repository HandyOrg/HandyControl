using System;
using System.Runtime.InteropServices;

namespace HandyControlDemo.Tools
{
    internal class Win32Helper
    {
        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
    }
}
