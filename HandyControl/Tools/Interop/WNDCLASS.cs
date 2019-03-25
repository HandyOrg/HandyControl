using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace HandyControl.Tools.Interop
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    internal class WNDCLASS
    {
        public int style;
        public ExternDllHelper.WndProc lpfnWndProc;
        public int cbClsExtra = 0;
        public int cbWndExtra = 0;
        public IntPtr hInstance = IntPtr.Zero;
        public IntPtr hIcon = IntPtr.Zero;
        public IntPtr hCursor = IntPtr.Zero;
        public IntPtr hbrBackground = IntPtr.Zero;
        public string lpszMenuName = null;
        public string lpszClassName = null;
    }
}