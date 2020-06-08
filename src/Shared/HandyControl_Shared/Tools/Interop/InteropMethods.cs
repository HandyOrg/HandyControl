using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;

namespace HandyControl.Tools.Interop
{
    internal class InteropMethods
    {
        #region common

        internal const int E_FAIL = unchecked((int)0x80004005);

        internal static readonly IntPtr HRGN_NONE = new IntPtr(-1);

        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        internal static extern int RegisterWindowMessage(string msg);

        [DllImport(InteropValues.ExternDll.Kernel32, SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, out InteropValues.TBBUTTON lpBuffer,
            int dwSize, out int lpNumberOfBytesRead);

        [DllImport(InteropValues.ExternDll.Kernel32, SetLastError = true)]
        internal static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, out InteropValues.RECT lpBuffer,
            int dwSize, out int lpNumberOfBytesRead);

        [DllImport(InteropValues.ExternDll.Kernel32, SetLastError = true)]
        internal static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, out InteropValues.TRAYDATA lpBuffer,
            int dwSize, out int lpNumberOfBytesRead);

        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Auto)]
        internal static extern uint SendMessage(IntPtr hWnd, uint Msg, uint wParam, IntPtr lParam);

        [DllImport(InteropValues.ExternDll.User32, SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport(InteropValues.ExternDll.Kernel32, SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern IntPtr OpenProcess(InteropValues.ProcessAccess dwDesiredAccess,
            [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, uint dwProcessId);

        [DllImport(InteropValues.ExternDll.Kernel32, SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, int dwSize,
            InteropValues.AllocationType flAllocationType, InteropValues.MemoryProtection flProtect);

        [DllImport(InteropValues.ExternDll.Kernel32, SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern int CloseHandle(IntPtr hObject);

        [DllImport(InteropValues.ExternDll.Kernel32, SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, int dwSize, InteropValues.FreeType dwFreeType);

        [DllImport(InteropValues.ExternDll.User32, SetLastError = true)]
        internal static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport(InteropValues.ExternDll.User32, SetLastError = true)]
        internal static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass,
            string lpszWindow);

        [DllImport(InteropValues.ExternDll.User32)]
        internal static extern int GetWindowRect(IntPtr hwnd, out InteropValues.RECT lpRect);

        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Auto)]
        internal static extern bool GetCursorPos(out InteropValues.POINT pt);

        [DllImport(InteropValues.ExternDll.User32)]
        internal static extern IntPtr GetDesktopWindow();

        [DllImport(InteropValues.ExternDll.User32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool AddClipboardFormatListener(IntPtr hwnd);

        [DllImport(InteropValues.ExternDll.User32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool RemoveClipboardFormatListener(IntPtr hwnd);

        [DllImport(InteropValues.ExternDll.User32)]
        internal static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport(InteropValues.ExternDll.User32)]
        internal static extern bool InsertMenu(IntPtr hMenu, int wPosition, int wFlags, int wIDNewItem, string lpNewItem);

        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport(InteropValues.ExternDll.User32, SetLastError = true, ExactSpelling = true, EntryPoint = nameof(GetDC),
            CharSet = CharSet.Auto)]
        internal static extern IntPtr IntGetDC(HandleRef hWnd);

        [SecurityCritical]
        internal static IntPtr GetDC(HandleRef hWnd)
        {
            var hDc = IntGetDC(hWnd);
            if (hDc == IntPtr.Zero) throw new Win32Exception();

            return HandleCollector.Add(hDc, CommonHandles.HDC);
        }

        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport(InteropValues.ExternDll.User32, ExactSpelling = true, EntryPoint = nameof(ReleaseDC), CharSet = CharSet.Auto)]
        internal static extern int IntReleaseDC(HandleRef hWnd, HandleRef hDC);

        [SecurityCritical]
        internal static int ReleaseDC(HandleRef hWnd, HandleRef hDC)
        {
            HandleCollector.Remove((IntPtr)hDC, CommonHandles.HDC);
            return IntReleaseDC(hWnd, hDC);
        }

        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport(InteropValues.ExternDll.Gdi32, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Auto)]
        internal static extern int GetDeviceCaps(HandleRef hDC, int nIndex);

        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport(InteropValues.ExternDll.User32)]
        internal static extern int GetSystemMetrics(InteropValues.SM nIndex);

        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport(InteropValues.ExternDll.User32, EntryPoint = nameof(DestroyIcon), CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool IntDestroyIcon(IntPtr hIcon);

        [SecurityCritical]
        internal static bool DestroyIcon(IntPtr hIcon)
        {
            var result = IntDestroyIcon(hIcon);
            return result;
        }

        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport(InteropValues.ExternDll.Gdi32, EntryPoint = nameof(DeleteObject), CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool IntDeleteObject(IntPtr hObject);

        [SecurityCritical]
        internal static bool DeleteObject(IntPtr hObject)
        {
            var result = IntDeleteObject(hObject);
            return result;
        }

        [SecurityCritical]
        internal static BitmapHandle CreateDIBSection(HandleRef hdc, ref InteropValues.BITMAPINFO bitmapInfo, int iUsage,
            ref IntPtr ppvBits, SafeFileMappingHandle hSection, int dwOffset)
        {
            if (hSection == null) hSection = new SafeFileMappingHandle(IntPtr.Zero);

            var hBitmap = PrivateCreateDIBSection(hdc, ref bitmapInfo, iUsage, ref ppvBits, hSection, dwOffset);
            return hBitmap;
        }

        [DllImport(InteropValues.ExternDll.Kernel32, EntryPoint = "CloseHandle", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool IntCloseHandle(HandleRef handle);

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
        internal static IconHandle CreateIconIndirect([In] [MarshalAs(UnmanagedType.LPStruct)]
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
        internal static extern int Shell_NotifyIcon(int message, InteropValues.NOTIFYICONDATA pnid);

        [SecurityCritical]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DllImport(InteropValues.ExternDll.User32, SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "CreateWindowExW")]
        internal static extern IntPtr CreateWindowEx(
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
        internal static extern short RegisterClass(InteropValues.WNDCLASS4ICON wc);

        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Auto)]
        internal static extern IntPtr DefWindowProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport(InteropValues.ExternDll.User32, ExactSpelling = true, CharSet = CharSet.Auto)]
        internal static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport(InteropValues.ExternDll.Kernel32, CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr SetWindowsHookEx(int idHook, InteropValues.HookProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport(InteropValues.ExternDll.User32, SetLastError = true)]
        internal static extern IntPtr GetWindowDC(IntPtr window);

        [DllImport(InteropValues.ExternDll.Gdi32, SetLastError = true)]
        internal static extern uint GetPixel(IntPtr dc, int x, int y);

        [DllImport(InteropValues.ExternDll.User32, SetLastError = true)]
        internal static extern int ReleaseDC(IntPtr window, IntPtr dc);

        [DllImport(InteropValues.ExternDll.Gdi32, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Auto)]
        internal static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Auto)]
        internal static extern IntPtr GetDC(IntPtr ptr);

        [DllImport(InteropValues.ExternDll.User32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowPlacement(IntPtr hwnd, InteropValues.WINDOWPLACEMENT lpwndpl);

        internal static InteropValues.WINDOWPLACEMENT GetWindowPlacement(IntPtr hwnd)
        {
            InteropValues.WINDOWPLACEMENT wINDOWPLACEMENT = new InteropValues.WINDOWPLACEMENT();
            if (GetWindowPlacement(hwnd, wINDOWPLACEMENT))
            {
                return wINDOWPLACEMENT;
            }
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        internal static int GetXLParam(int lParam) => LoWord(lParam);

        internal static int GetYLParam(int lParam) => HiWord(lParam);

        internal static int HiWord(int value) => (short)(value >> 16);

        internal static int LoWord(int value) => (short)(value & 65535);

        [DllImport(InteropValues.ExternDll.User32)]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

        [DllImport(InteropValues.ExternDll.User32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool EnumThreadWindows(uint dwThreadId, InteropValues.EnumWindowsProc lpfn, IntPtr lParam);

        [DllImport(InteropValues.ExternDll.Gdi32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DeleteDC(IntPtr hdc);

        [DllImport(InteropValues.ExternDll.Gdi32, SetLastError = true)]
        internal static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport(InteropValues.ExternDll.Gdi32, ExactSpelling = true, SetLastError = true)]
        internal static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, int nMsg, IntPtr wParam, IntPtr lParam);

        [DllImport(InteropValues.ExternDll.User32)]
        internal static extern IntPtr MonitorFromPoint(InteropValues.POINT pt, int flags);

        [DllImport(InteropValues.ExternDll.User32)]
        internal static extern IntPtr GetWindow(IntPtr hwnd, int nCmd);

        [DllImport(InteropValues.ExternDll.User32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool IsWindowVisible(IntPtr hwnd);

        [DllImport(InteropValues.ExternDll.User32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool IsIconic(IntPtr hwnd);

        [DllImport(InteropValues.ExternDll.User32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool IsZoomed(IntPtr hwnd);

        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Auto, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int flags);

        internal static System.Windows.Point GetCursorPos()
        {
            var result = default(System.Windows.Point);
            if (GetCursorPos(out var point))
            {
                result.X = point.X;
                result.Y = point.Y;
            }
            return result;
        }

        [DllImport(InteropValues.ExternDll.User32)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        internal static int GetWindowLong(IntPtr hWnd, InteropValues.GWL nIndex) => GetWindowLong(hWnd, (int)nIndex);

        internal static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            if (IntPtr.Size == 4)
            {
                return SetWindowLongPtr32(hWnd, nIndex, dwNewLong);
            }
            return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
        }

        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Auto, EntryPoint = "SetWindowLong")]
        internal static extern IntPtr SetWindowLongPtr32(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Auto, EntryPoint = "SetWindowLongPtr")]
        internal static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Unicode)]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Unicode)]
        private static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        internal static IntPtr SetWindowLongPtr(IntPtr hWnd, InteropValues.GWLP nIndex, IntPtr dwNewLong)
        {
            if (IntPtr.Size == 8)
            {
                return SetWindowLongPtr(hWnd, (int)nIndex, dwNewLong);
            }
            return new IntPtr(SetWindowLong(hWnd, (int)nIndex, dwNewLong.ToInt32()));
        }

        internal static int SetWindowLong(IntPtr hWnd, InteropValues.GWL nIndex, int dwNewLong) => SetWindowLong(hWnd, (int)nIndex, dwNewLong);

        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Unicode)]
        internal static extern ushort RegisterClass(ref InteropValues.WNDCLASS lpWndClass);

        [DllImport(InteropValues.ExternDll.Kernel32)]
        internal static extern uint GetCurrentThreadId();

        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr CreateWindowEx(int dwExStyle, IntPtr classAtom, string lpWindowName, int dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

        [DllImport(InteropValues.ExternDll.User32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DestroyWindow(IntPtr hwnd);

        [DllImport(InteropValues.ExternDll.User32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool UnregisterClass(IntPtr classAtom, IntPtr hInstance);

        [DllImport(InteropValues.ExternDll.User32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDest, ref InteropValues.POINT pptDest, ref InteropValues.SIZE psize, IntPtr hdcSrc, ref InteropValues.POINT pptSrc, uint crKey, [In] ref InteropValues.BLENDFUNCTION pblend, uint dwFlags);

        [DllImport(InteropValues.ExternDll.User32)]
        internal static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, InteropValues.RedrawWindowFlags flags);

        [DllImport(InteropValues.ExternDll.User32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, InteropValues.EnumMonitorsDelegate lpfnEnum, IntPtr dwData);

        [DllImport(InteropValues.ExternDll.User32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool IntersectRect(out InteropValues.RECT lprcDst, [In] ref InteropValues.RECT lprcSrc1, [In] ref InteropValues.RECT lprcSrc2);

        [DllImport(InteropValues.ExternDll.User32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, ref InteropValues.MONITORINFO monitorInfo);

        [DllImport(InteropValues.ExternDll.Gdi32, SetLastError = true)]
        internal static extern IntPtr CreateDIBSection(IntPtr hdc, ref InteropValues.BITMAPINFO pbmi, uint iUsage, out IntPtr ppvBits, IntPtr hSection, uint dwOffset);

        [DllImport(InteropValues.ExternDll.MsImg)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool AlphaBlend(IntPtr hdcDest, int xoriginDest, int yoriginDest, int wDest, int hDest, IntPtr hdcSrc, int xoriginSrc, int yoriginSrc, int wSrc, int hSrc, InteropValues.BLENDFUNCTION pfn);

        internal static int GET_SC_WPARAM(IntPtr wParam) => (int)wParam & 65520;

        [DllImport(InteropValues.ExternDll.User32)]
        internal static extern IntPtr ChildWindowFromPointEx(IntPtr hwndParent, InteropValues.POINT pt, int uFlags);

        [DllImport(InteropValues.ExternDll.Gdi32)]
        internal static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int width, int height);

        [DllImport(InteropValues.ExternDll.Gdi32)]
        internal static extern bool BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        [DllImport(InteropValues.ExternDll.User32)]
        [ResourceExposure(ResourceScope.None)]
        internal static extern bool EnableWindow(IntPtr hWnd, bool enable);

        [ReflectionPermission(SecurityAction.Assert, Unrestricted = true), SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
        internal static object PtrToStructure(IntPtr lparam, Type cls) => Marshal.PtrToStructure(lparam, cls);

        [ReflectionPermission(SecurityAction.Assert, Unrestricted = true),
         SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
        internal static void PtrToStructure(IntPtr lparam, object data) => Marshal.PtrToStructure(lparam, data);

        [DllImport(InteropValues.ExternDll.Shell32, CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SHAppBarMessage(int dwMessage, ref InteropValues.APPBARDATA pData);

        #endregion

        internal class Gdip
        {
            private const string ThreadDataSlotName = "system.drawing.threaddata";

            private static IntPtr InitToken;

            private static bool Initialized => InitToken != IntPtr.Zero;

            internal const int 
                Ok = 0,
                GenericError = 1,
                InvalidParameter = 2,
                OutOfMemory = 3,
                ObjectBusy = 4,
                InsufficientBuffer = 5,
                NotImplemented = 6,
                Win32Error = 7,
                WrongState = 8,
                Aborted = 9,
                FileNotFound = 10,
                ValueOverflow = 11,
                AccessDenied = 12,
                UnknownImageFormat = 13,
                FontFamilyNotFound = 14,
                FontStyleNotFound = 15,
                NotTrueTypeFont = 16,
                UnsupportedGdiplusVersion = 17,
                GdiplusNotInitialized = 18,
                PropertyNotFound = 19,
                PropertyNotSupported = 20,
                E_UNEXPECTED = unchecked((int)0x8000FFFF);

            static Gdip()
            {
                Initialize();
            }

            [StructLayout(LayoutKind.Sequential)]
            private struct StartupInput
            {
                private int GdiplusVersion;

                private readonly IntPtr DebugEventCallback;

                private bool SuppressBackgroundThread;

                private bool SuppressExternalCodecs;

                public static StartupInput GetDefault()
                {
                    var result = new StartupInput
                    {
                        GdiplusVersion = 1,
                        SuppressBackgroundThread = false,
                        SuppressExternalCodecs = false
                    };
                    return result;
                }
            }

            [StructLayout(LayoutKind.Sequential)]
            private struct StartupOutput
            {
                private readonly IntPtr hook;

                private readonly IntPtr unhook;
            }

            [ResourceExposure(ResourceScope.None)]
            [ResourceConsumption(ResourceScope.AppDomain, ResourceScope.AppDomain)]
            [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals")]
            private static void Initialize()
            {
                var input = StartupInput.GetDefault();

                var status = GdiplusStartup(out InitToken, ref input, out _);

                if (status != Ok)
                {
                    throw StatusException(status);
                }

                var currentDomain = AppDomain.CurrentDomain;
                currentDomain.ProcessExit += OnProcessExit;

                if (!currentDomain.IsDefaultAppDomain())
                {
                    currentDomain.DomainUnload += OnProcessExit;
                }
            }

            [PrePrepareMethod]
            [ResourceExposure(ResourceScope.AppDomain)]
            [ResourceConsumption(ResourceScope.AppDomain)]
            private static void OnProcessExit(object sender, EventArgs e) => Shutdown();

            [SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods")]
            [ResourceExposure(ResourceScope.AppDomain)]
            [ResourceConsumption(ResourceScope.AppDomain)]
            private static void Shutdown()
            {
                if (Initialized)
                {
                    ClearThreadData();
                    // unhook our shutdown handlers as we do not need to shut down more than once
                    var currentDomain = AppDomain.CurrentDomain;
                    currentDomain.ProcessExit -= OnProcessExit;
                    if (!currentDomain.IsDefaultAppDomain())
                    {
                        currentDomain.DomainUnload -= OnProcessExit;
                    }
                }
            }

            [MethodImpl(MethodImplOptions.NoInlining)]
            private static void ClearThreadData()
            {
                var slot = Thread.GetNamedDataSlot(ThreadDataSlotName);
                Thread.SetData(slot, null);
            }

            [DllImport(InteropValues.ExternDll.GdiPlus, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
            [ResourceExposure(ResourceScope.None)]
            internal static extern int GdipImageGetFrameDimensionsCount(HandleRef image, out int count);

            internal static Exception StatusException(int status)
            {
                switch (status)
                {
                    case GenericError: return new ExternalException("GdiplusGenericError");
                    case InvalidParameter: return new ArgumentException("GdiplusInvalidParameter");
                    case OutOfMemory: return new OutOfMemoryException("GdiplusOutOfMemory");
                    case ObjectBusy: return new InvalidOperationException("GdiplusObjectBusy");
                    case InsufficientBuffer: return new OutOfMemoryException("GdiplusInsufficientBuffer");
                    case NotImplemented: return new NotImplementedException("GdiplusNotImplemented");
                    case Win32Error: return new ExternalException("GdiplusGenericError");
                    case WrongState: return new InvalidOperationException("GdiplusWrongState");
                    case Aborted: return new ExternalException("GdiplusAborted");
                    case FileNotFound: return new FileNotFoundException("GdiplusFileNotFound");
                    case ValueOverflow: return new OverflowException("GdiplusOverflow");
                    case AccessDenied: return new ExternalException("GdiplusAccessDenied");
                    case UnknownImageFormat: return new ArgumentException("GdiplusUnknownImageFormat");
                    case PropertyNotFound: return new ArgumentException("GdiplusPropertyNotFoundError");
                    case PropertyNotSupported: return new ArgumentException("GdiplusPropertyNotSupportedError");
                    case FontFamilyNotFound: return new ArgumentException("GdiplusFontFamilyNotFound");
                    case FontStyleNotFound: return new ArgumentException("GdiplusFontStyleNotFound");
                    case NotTrueTypeFont: return new ArgumentException("GdiplusNotTrueTypeFont_NoName");
                    case UnsupportedGdiplusVersion: return new ExternalException("GdiplusUnsupportedGdiplusVersion");
                    case GdiplusNotInitialized: return new ExternalException("GdiplusNotInitialized");
                }

                return new ExternalException("GdiplusUnknown");
            }

            [DllImport(InteropValues.ExternDll.GdiPlus, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
            [ResourceExposure(ResourceScope.None)]
            internal static extern int GdipImageGetFrameDimensionsList(HandleRef image, IntPtr buffer, int count);

            [DllImport(InteropValues.ExternDll.GdiPlus, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
            [ResourceExposure(ResourceScope.None)]
            internal static extern int GdipImageGetFrameCount(HandleRef image, ref Guid dimensionId, int[] count);

            [DllImport(InteropValues.ExternDll.GdiPlus, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
            [ResourceExposure(ResourceScope.None)]
            internal static extern int GdipGetPropertyItemSize(HandleRef image, int propid, out int size);

            [DllImport(InteropValues.ExternDll.GdiPlus, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
            [ResourceExposure(ResourceScope.None)]
            internal static extern int GdipGetPropertyItem(HandleRef image, int propid, int size, IntPtr buffer);

            [DllImport(InteropValues.ExternDll.GdiPlus, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
            [ResourceExposure(ResourceScope.Machine)]
            internal static extern int GdipCreateHBITMAPFromBitmap(HandleRef nativeBitmap, out IntPtr hbitmap, int argbBackground);

            [DllImport(InteropValues.ExternDll.GdiPlus, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
            [ResourceExposure(ResourceScope.None)]
            internal static extern int GdipImageSelectActiveFrame(HandleRef image, ref Guid dimensionId, int frameIndex);

            [DllImport(InteropValues.ExternDll.GdiPlus, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
            [ResourceExposure(ResourceScope.Machine)]
            internal static extern int GdipCreateBitmapFromFile(string filename, out IntPtr bitmap);

            [DllImport(InteropValues.ExternDll.GdiPlus, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
            [ResourceExposure(ResourceScope.None)]
            internal static extern int GdipImageForceValidation(HandleRef image);

            [DllImport(InteropValues.ExternDll.GdiPlus, SetLastError = true, ExactSpelling = true, EntryPoint = "GdipDisposeImage", CharSet = CharSet.Unicode)]
            [ResourceExposure(ResourceScope.None)]
            private static extern int IntGdipDisposeImage(HandleRef image);

            internal static int GdipDisposeImage(HandleRef image)
            {
                if (!Initialized) return Ok;
                var result = IntGdipDisposeImage(image);
                return result;
            }

            [DllImport(InteropValues.ExternDll.GdiPlus, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
            [ResourceExposure(ResourceScope.Process)]
            private static extern int GdiplusStartup(out IntPtr token, ref StartupInput input, out StartupOutput output);

            [DllImport(InteropValues.ExternDll.GdiPlus, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
            [ResourceExposure(ResourceScope.None)]
            internal static extern int GdipGetImageRawFormat(HandleRef image, ref Guid format);

            [DllImport(InteropValues.ExternDll.User32)]
            internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref InteropValues.WINCOMPATTRDATA data);

            [DllImport(InteropValues.ExternDll.GdiPlus, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
            [ResourceExposure(ResourceScope.Machine)]
            internal static extern int GdipCreateBitmapFromStream(InteropValues.IStream stream, out IntPtr bitmap);

            [DllImport(InteropValues.ExternDll.GdiPlus, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
            [ResourceExposure(ResourceScope.Machine)]
            internal static extern int GdipCreateBitmapFromHBITMAP(HandleRef hbitmap, HandleRef hpalette, out IntPtr bitmap);

            [DllImport(InteropValues.ExternDll.GdiPlus, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
            [ResourceExposure(ResourceScope.None)]
            internal static extern int GdipGetImageEncodersSize(out int numEncoders, out int size);

            [DllImport(InteropValues.ExternDll.GdiPlus, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
            [ResourceExposure(ResourceScope.None)]
            internal static extern int GdipGetImageDecodersSize(out int numDecoders, out int size);

            [DllImport(InteropValues.ExternDll.GdiPlus, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
            [ResourceExposure(ResourceScope.None)]
            internal static extern int GdipGetImageDecoders(int numDecoders, int size, IntPtr decoders);

            [DllImport(InteropValues.ExternDll.GdiPlus, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
            [ResourceExposure(ResourceScope.None)]
            internal static extern int GdipGetImageEncoders(int numEncoders, int size, IntPtr encoders);

            [DllImport(InteropValues.ExternDll.GdiPlus, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
            [ResourceExposure(ResourceScope.None)]
            internal static extern int GdipSaveImageToStream(HandleRef image, InteropValues.IStream stream, ref Guid classId, HandleRef encoderParams);
        }
    }
}