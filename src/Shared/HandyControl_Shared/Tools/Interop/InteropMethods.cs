using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;
using System.Text;

namespace HandyControl.Tools.Interop
{
    internal class InteropMethods
    {
        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        public static extern int RegisterWindowMessage(string msg);

        [DllImport(InteropValues.ExternDll.User32, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int MapWindowPoints(IntPtr hWndFrom, IntPtr hWndTo, ref InteropValues.RECT lpPoints, uint cPoints);

        [DllImport(InteropValues.ExternDll.Kernel32, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, out InteropValues.TBBUTTON lpBuffer,
            int dwSize, out int lpNumberOfBytesRead);

        [DllImport(InteropValues.ExternDll.Kernel32, SetLastError = true)]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, out InteropValues.RECT lpBuffer,
            int dwSize, out int lpNumberOfBytesRead);

        [DllImport(InteropValues.ExternDll.Kernel32, SetLastError = true)]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, out InteropValues.TRAYDATA lpBuffer,
            int dwSize, out int lpNumberOfBytesRead);

        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Auto)]
        public static extern uint SendMessage(IntPtr hWnd, uint Msg, uint wParam, IntPtr lParam);

        [DllImport(InteropValues.ExternDll.User32, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport(InteropValues.ExternDll.Kernel32, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr OpenProcess(InteropValues.ProcessAccess dwDesiredAccess,
            [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, uint dwProcessId);

        [DllImport(InteropValues.ExternDll.Kernel32, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, int dwSize,
            InteropValues.AllocationType flAllocationType, InteropValues.MemoryProtection flProtect);

        [DllImport(InteropValues.ExternDll.Kernel32, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int CloseHandle(IntPtr hObject);

        [DllImport(InteropValues.ExternDll.Kernel32, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, int dwSize, InteropValues.FreeType dwFreeType);

        [DllImport(InteropValues.ExternDll.User32, SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport(InteropValues.ExternDll.User32, SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass,
            string lpszWindow);

        [DllImport(InteropValues.ExternDll.User32)]
        public static extern int GetWindowRect(IntPtr hwnd, out InteropValues.RECT lpRect);

        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Auto)]
        public static extern bool GetCursorPos(out InteropValues.POINT pt);

        [DllImport(InteropValues.ExternDll.User32)]
        public static extern IntPtr GetDesktopWindow();

        [DllImport(InteropValues.ExternDll.User32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AddClipboardFormatListener(IntPtr hwnd);

        [DllImport(InteropValues.ExternDll.User32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool RemoveClipboardFormatListener(IntPtr hwnd);

        [DllImport(InteropValues.ExternDll.User32)]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport(InteropValues.ExternDll.User32)]
        public static extern bool InsertMenu(IntPtr hMenu, int wPosition, int wFlags, int wIDNewItem, string lpNewItem);

        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport(InteropValues.ExternDll.User32, SetLastError = true, ExactSpelling = true, EntryPoint = nameof(GetDC),
            CharSet = CharSet.Auto)]
        public static extern IntPtr IntGetDC(HandleRef hWnd);

        [SecurityCritical]
        public static IntPtr GetDC(HandleRef hWnd)
        {
            var hDc = IntGetDC(hWnd);
            if (hDc == IntPtr.Zero) throw new Win32Exception();

            return HandleCollector.Add(hDc, CommonHandles.HDC);
        }

        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport(InteropValues.ExternDll.User32, ExactSpelling = true, EntryPoint = nameof(ReleaseDC), CharSet = CharSet.Auto)]
        public static extern int IntReleaseDC(HandleRef hWnd, HandleRef hDC);

        [SecurityCritical]
        public static int ReleaseDC(HandleRef hWnd, HandleRef hDC)
        {
            HandleCollector.Remove((IntPtr) hDC, CommonHandles.HDC);
            return IntReleaseDC(hWnd, hDC);
        }

        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport(InteropValues.ExternDll.Gdi32, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern int GetDeviceCaps(HandleRef hDC, int nIndex);

        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport(InteropValues.ExternDll.User32)]
        public static extern int GetSystemMetrics(InteropValues.SM nIndex);

        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport(InteropValues.ExternDll.User32, EntryPoint = nameof(DestroyIcon), CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool IntDestroyIcon(IntPtr hIcon);

        [SecurityCritical]
        public static bool DestroyIcon(IntPtr hIcon)
        {
            var result = IntDestroyIcon(hIcon);
            return result;
        }

        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport(InteropValues.ExternDll.Gdi32, EntryPoint = nameof(DeleteObject), CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool IntDeleteObject(IntPtr hObject);

        [SecurityCritical]
        public static bool DeleteObject(IntPtr hObject)
        {
            var result = IntDeleteObject(hObject);
            return result;
        }

        [SecurityCritical]
        public static BitmapHandle CreateDIBSection(HandleRef hdc, ref InteropValues.BITMAPINFO bitmapInfo, int iUsage,
            ref IntPtr ppvBits, SafeFileMappingHandle hSection, int dwOffset)
        {
            if (hSection == null) hSection = new SafeFileMappingHandle(IntPtr.Zero);

            var hBitmap = PrivateCreateDIBSection(hdc, ref bitmapInfo, iUsage, ref ppvBits, hSection, dwOffset);
            return hBitmap;
        }

        [DllImport(InteropValues.ExternDll.Kernel32, EntryPoint = "CloseHandle", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool IntCloseHandle(HandleRef handle);

        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport(InteropValues.ExternDll.Gdi32, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Auto,
            EntryPoint = nameof(CreateDIBSection))]
        private static extern BitmapHandle PrivateCreateDIBSection(HandleRef hdc, ref InteropValues.BITMAPINFO bitmapInfo, int iUsage,
            ref IntPtr ppvBits, SafeFileMappingHandle hSection, int dwOffset);

        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport(InteropValues.ExternDll.User32, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Auto,
            EntryPoint = nameof(CreateIconIndirect))]
        private static extern IconHandle PrivateCreateIconIndirect([In] [MarshalAs(UnmanagedType.LPStruct)]
            InteropValues.ICONINFO iconInfo);

        [SecurityCritical]
        public static IconHandle CreateIconIndirect([In] [MarshalAs(UnmanagedType.LPStruct)]
            InteropValues.ICONINFO iconInfo)
        {
            var hIcon = PrivateCreateIconIndirect(iconInfo);
            return hIcon;
        }

        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport(InteropValues.ExternDll.Gdi32, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Auto,
            EntryPoint = nameof(CreateBitmap))]
        private static extern BitmapHandle PrivateCreateBitmap(int width, int height, int planes, int bitsPerPixel,
            byte[] lpvBits);

        [SecurityCritical]
        internal static BitmapHandle CreateBitmap(int width, int height, int planes, int bitsPerPixel, byte[] lpvBits)
        {
            var hBitmap = PrivateCreateBitmap(width, height, planes, bitsPerPixel, lpvBits);
            return hBitmap;
        }

        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport(InteropValues.ExternDll.Kernel32, EntryPoint = "GetModuleFileName", CharSet = CharSet.Unicode,
            SetLastError = true)]
        private static extern int IntGetModuleFileName(HandleRef hModule, StringBuilder buffer, int length);

        [SecurityCritical]
        internal static string GetModuleFileName(HandleRef hModule)
        {
            var buffer = new StringBuilder(InteropValues.Win32Constant.MAX_PATH);
            while (true)
            {
                var size = IntGetModuleFileName(hModule, buffer, buffer.Capacity);
                if (size == 0) throw new Win32Exception();

                if (size == buffer.Capacity)
                {
                    buffer.EnsureCapacity(buffer.Capacity * 2);
                    continue;
                }

                return buffer.ToString();
            }
        }

        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport(InteropValues.ExternDll.Shell32, CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int ExtractIconEx(string szExeFileName, int nIconIndex, out IconHandle phiconLarge,
            out IconHandle phiconSmall, int nIcons);

        [DllImport(InteropValues.ExternDll.Shell32, CharSet = CharSet.Auto)]
        public static extern int Shell_NotifyIcon(int message, InteropValues.NOTIFYICONDATA pnid);

        [SecurityCritical]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DllImport(InteropValues.ExternDll.User32, SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "CreateWindowExW")]
        public static extern IntPtr CreateWindowEx(
            int dwExStyle,
            [MarshalAs(UnmanagedType.LPWStr)] string lpClassName,
            [MarshalAs(UnmanagedType.LPWStr)] string lpWindowName,
            int dwStyle,
            int x,
            int y,
            int nWidth,
            int nHeight,
            IntPtr hWndParent,
            IntPtr hMenu,
            IntPtr hInstance,
            IntPtr lpParam);

        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Unicode, SetLastError = true, BestFitMapping = false)]
        public static extern short RegisterClass(InteropValues.WNDCLASS wc);

        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Auto)]
        public static extern IntPtr DefWindowProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport(InteropValues.ExternDll.User32, ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport(InteropValues.ExternDll.Kernel32, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook, InteropValues.HookProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport(InteropValues.ExternDll.User32, SetLastError = true)]
        public static extern IntPtr GetWindowDC(IntPtr window);

        [DllImport(InteropValues.ExternDll.Gdi32, SetLastError = true)]
        public static extern uint GetPixel(IntPtr dc, int x, int y);

        [DllImport(InteropValues.ExternDll.User32, SetLastError = true)]
        public static extern int ReleaseDC(IntPtr window, IntPtr dc);

        [DllImport(InteropValues.ExternDll.Gdi32, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Auto)]
        public static extern IntPtr GetDC(IntPtr ptr);
    }
}