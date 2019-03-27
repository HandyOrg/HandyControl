using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace HandyControl.Tools.Interop
{
    public delegate IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    internal class NativeMethods
    {
        public const int
            WS_CHILD = 0x40000000,
            BITSPIXEL = 12,
            PLANES = 14,
            BI_RGB = 0,
            DIB_RGB_COLORS = 0,
            NIF_MESSAGE = 0x00000001,
            NIF_ICON = 0x00000002,
            NIF_TIP = 0x00000004,
            NIF_INFO = 0x00000010,
            NIM_ADD = 0x00000000,
            NIM_MODIFY = 0x00000001,
            NIM_DELETE = 0x00000002,
            NIIF_NONE = 0x00000000,
            NIIF_INFO = 0x00000001,
            NIIF_WARNING = 0x00000002,
            NIIF_ERROR = 0x00000003,
            WM_MOUSEMOVE = 0x0200,
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_LBUTTONDBLCLK = 0x0203,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205,
            WM_RBUTTONDBLCLK = 0x0206,
            WM_MBUTTONDOWN = 0x0207,
            WM_MBUTTONUP = 0x0208,
            WM_MBUTTONDBLCLK = 0x0209,
            WM_USER = 0x0400;

        [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        public static extern int RegisterWindowMessage(string msg);
    }
}