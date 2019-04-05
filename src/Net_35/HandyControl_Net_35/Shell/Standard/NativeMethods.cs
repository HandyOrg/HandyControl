namespace Standard
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.ConstrainedExecution;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;
    using System.Text;

    internal static class NativeMethods
    {
        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("user32.dll", EntryPoint="AdjustWindowRectEx", SetLastError=true)]
        private static extern bool _AdjustWindowRectEx(ref Standard.RECT lpRect, Standard.WS dwStyle, [MarshalAs(UnmanagedType.Bool)] bool bMenu, Standard.WS_EX dwExStyle);
        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("user32.dll", EntryPoint="ChangeWindowMessageFilter", SetLastError=true)]
        private static extern bool _ChangeWindowMessageFilter(Standard.WM message, Standard.MSGFLT dwFlag);
        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("user32.dll", EntryPoint="ChangeWindowMessageFilterEx", SetLastError=true)]
        private static extern bool _ChangeWindowMessageFilterEx(IntPtr hwnd, Standard.WM message, Standard.MSGFLT action, [In, Out, Optional] ref Standard.CHANGEFILTERSTRUCT pChangeFilterStruct);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("shell32.dll", EntryPoint="CommandLineToArgvW", CharSet=CharSet.Unicode)]
        private static extern IntPtr _CommandLineToArgvW([MarshalAs(UnmanagedType.LPWStr)] string cmdLine, out int numArgs);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("gdi32.dll", EntryPoint="CreateDIBSection", SetLastError=true)]
        private static extern Standard.SafeHBITMAP _CreateDIBSection(Standard.SafeDC hdc, [In] ref Standard.BITMAPINFO bitmapInfo, int iUsage, out IntPtr ppvBits, IntPtr hSection, int dwOffset);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("gdi32.dll", EntryPoint="CreateDIBSection", SetLastError=true)]
        private static extern Standard.SafeHBITMAP _CreateDIBSectionIntPtr(IntPtr hdc, [In] ref Standard.BITMAPINFO bitmapInfo, int iUsage, out IntPtr ppvBits, IntPtr hSection, int dwOffset);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("gdi32.dll", EntryPoint="CreateRectRgn", SetLastError=true)]
        private static extern IntPtr _CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("gdi32.dll", EntryPoint="CreateRectRgnIndirect", SetLastError=true)]
        private static extern IntPtr _CreateRectRgnIndirect([In] ref Standard.RECT lprc);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("gdi32.dll", EntryPoint="CreateRoundRectRgn", SetLastError=true)]
        private static extern IntPtr _CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("user32.dll", EntryPoint="CreateWindowExW", CharSet=CharSet.Unicode, SetLastError=true)]
        private static extern IntPtr _CreateWindowEx(Standard.WS_EX dwExStyle, [MarshalAs(UnmanagedType.LPWStr)] string lpClassName, [MarshalAs(UnmanagedType.LPWStr)] string lpWindowName, Standard.WS dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);
        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("user32.dll", EntryPoint="DrawMenuBar", SetLastError=true)]
        private static extern bool _DrawMenuBar(IntPtr hWnd);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("dwmapi.dll", EntryPoint="DwmGetColorizationColor")]
        private static extern Standard.HRESULT _DwmGetColorizationColor(out uint pcrColorization, [MarshalAs(UnmanagedType.Bool)] out bool pfOpaqueBlend);
        [DllImport("dwmapi.dll", EntryPoint="DwmGetCompositionTimingInfo")]
        private static extern Standard.HRESULT _DwmGetCompositionTimingInfo(IntPtr hwnd, ref Standard.DWM_TIMING_INFO pTimingInfo);
        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("dwmapi.dll", EntryPoint="DwmIsCompositionEnabled", PreserveSig=false)]
        private static extern bool _DwmIsCompositionEnabled();
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("dwmapi.dll", EntryPoint="DwmSetWindowAttribute")]
        private static extern void _DwmSetWindowAttribute(IntPtr hwnd, Standard.DWMWA dwAttribute, ref int pvAttribute, int cbAttribute);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("user32.dll", EntryPoint="EnableMenuItem")]
        private static extern int _EnableMenuItem(IntPtr hMenu, Standard.SC uIDEnableItem, Standard.MF uEnable);
        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("user32.dll", EntryPoint="GetClientRect", SetLastError=true)]
        private static extern bool _GetClientRect(IntPtr hwnd, out Standard.RECT lpRect);
        [DllImport("uxtheme.dll", EntryPoint="GetCurrentThemeName", CharSet=CharSet.Unicode)]
        private static extern Standard.HRESULT _GetCurrentThemeName(StringBuilder pszThemeFileName, int dwMaxNameChars, StringBuilder pszColorBuff, int cchMaxColorChars, StringBuilder pszSizeBuff, int cchMaxSizeChars);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("kernel32.dll", EntryPoint="GetModuleFileName", CharSet=CharSet.Unicode, SetLastError=true)]
        private static extern int _GetModuleFileName(IntPtr hModule, StringBuilder lpFilename, int nSize);
        [DllImport("kernel32.dll", EntryPoint="GetModuleHandleW", CharSet=CharSet.Unicode, SetLastError=true)]
        private static extern IntPtr _GetModuleHandle([MarshalAs(UnmanagedType.LPWStr)] string lpModuleName);
        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("user32.dll", EntryPoint="GetMonitorInfo", SetLastError=true)]
        private static extern bool _GetMonitorInfo(IntPtr hMonitor, [In, Out] Standard.MONITORINFO lpmi);
        [DllImport("gdi32.dll", EntryPoint="GetStockObject", SetLastError=true)]
        private static extern IntPtr _GetStockObject(Standard.StockObject fnObject);
        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("user32.dll", EntryPoint="GetWindowRect", SetLastError=true)]
        private static extern bool _GetWindowRect(IntPtr hWnd, out Standard.RECT lpRect);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("kernel32.dll", EntryPoint="LocalFree", SetLastError=true)]
        private static extern IntPtr _LocalFree(IntPtr hMem);
        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("user32.dll", EntryPoint="PostMessage", SetLastError=true)]
        private static extern bool _PostMessage(IntPtr hWnd, Standard.WM Msg, IntPtr wParam, IntPtr lParam);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("user32.dll", EntryPoint="RegisterClassExW", SetLastError=true)]
        private static extern short _RegisterClassEx([In] ref Standard.WNDCLASSEX lpwcx);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("user32.dll", EntryPoint="RegisterWindowMessage", CharSet=CharSet.Unicode, SetLastError=true)]
        private static extern uint _RegisterWindowMessage([MarshalAs(UnmanagedType.LPWStr)] string lpString);
        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("user32.dll", EntryPoint="RemoveMenu", SetLastError=true)]
        private static extern bool _RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("gdi32.dll", EntryPoint="SelectObject", SetLastError=true)]
        private static extern IntPtr _SelectObject(Standard.SafeDC hdc, IntPtr hgdiobj);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("gdi32.dll", EntryPoint="SelectObject", SetLastError=true)]
        private static extern IntPtr _SelectObjectSafeHBITMAP(Standard.SafeDC hdc, Standard.SafeHBITMAP hgdiobj);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("user32.dll", EntryPoint="SetActiveWindow", SetLastError=true)]
        private static extern IntPtr _SetActiveWindow(IntPtr hWnd);
        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("kernel32.dll", EntryPoint="SetProcessWorkingSetSize", SetLastError=true)]
        private static extern bool _SetProcessWorkingSetSize(IntPtr hProcess, IntPtr dwMinimiumWorkingSetSize, IntPtr dwMaximumWorkingSetSize);
        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("user32.dll", EntryPoint="SetWindowPos", SetLastError=true)]
        private static extern bool _SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, Standard.SWP uFlags);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("user32.dll", EntryPoint="SetWindowRgn", SetLastError=true)]
        private static extern int _SetWindowRgn(IntPtr hWnd, IntPtr hRgn, [MarshalAs(UnmanagedType.Bool)] bool bRedraw);
        [DllImport("shell32.dll", EntryPoint="SHAddToRecentDocs")]
        private static extern void _SHAddToRecentDocs_ShellLink(Standard.SHARD uFlags, Standard.IShellLinkW pv);
        [DllImport("shell32.dll", EntryPoint="SHAddToRecentDocs")]
        private static extern void _SHAddToRecentDocs_String(Standard.SHARD uFlags, [MarshalAs(UnmanagedType.LPWStr)] string pv);
        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("user32.dll", EntryPoint="SystemParametersInfoW", CharSet=CharSet.Unicode, SetLastError=true)]
        private static extern bool _SystemParametersInfo_HIGHCONTRAST(Standard.SPI uiAction, int uiParam, [In, Out] ref Standard.HIGHCONTRAST pvParam, Standard.SPIF fWinIni);
        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("user32.dll", EntryPoint="SystemParametersInfoW", CharSet=CharSet.Unicode, SetLastError=true)]
        private static extern bool _SystemParametersInfo_NONCLIENTMETRICS(Standard.SPI uiAction, int uiParam, [In, Out] ref Standard.NONCLIENTMETRICS pvParam, Standard.SPIF fWinIni);
        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("user32.dll", EntryPoint="SystemParametersInfoW", SetLastError=true)]
        private static extern bool _SystemParametersInfo_String(Standard.SPI uiAction, int uiParam, [MarshalAs(UnmanagedType.LPWStr)] string pvParam, Standard.SPIF fWinIni);
        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("user32.dll", EntryPoint="UnregisterClass", SetLastError=true)]
        private static extern bool _UnregisterClassAtom(IntPtr lpClassName, IntPtr hInstance);
        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("user32.dll", EntryPoint="UnregisterClass", CharSet=CharSet.Unicode, SetLastError=true)]
        private static extern bool _UnregisterClassName(string lpClassName, IntPtr hInstance);
        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("user32.dll", EntryPoint="UpdateLayeredWindow", SetLastError=true)]
        private static extern bool _UpdateLayeredWindow(IntPtr hwnd, Standard.SafeDC hdcDst, [In] ref Standard.POINT pptDst, [In] ref Standard.SIZE psize, Standard.SafeDC hdcSrc, [In] ref Standard.POINT pptSrc, int crKey, ref Standard.BLENDFUNCTION pblend, Standard.ULW dwFlags);
        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("user32.dll", EntryPoint="UpdateLayeredWindow", SetLastError=true)]
        private static extern bool _UpdateLayeredWindowIntPtr(IntPtr hwnd, IntPtr hdcDst, IntPtr pptDst, IntPtr psize, IntPtr hdcSrc, IntPtr pptSrc, int crKey, ref Standard.BLENDFUNCTION pblend, Standard.ULW dwFlags);
        public static Standard.RECT AdjustWindowRectEx(Standard.RECT lpRect, Standard.WS dwStyle, bool bMenu, Standard.WS_EX dwExStyle)
        {
            if (!_AdjustWindowRectEx(ref lpRect, dwStyle, bMenu, dwExStyle))
            {
                Standard.HRESULT.ThrowLastError();
            }
            return lpRect;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static Standard.HRESULT ChangeWindowMessageFilterEx(IntPtr hwnd, Standard.WM message, Standard.MSGFLT action, out Standard.MSGFLTINFO filterInfo)
        {
            filterInfo = Standard.MSGFLTINFO.NONE;
            if (!Standard.Utility.IsOSVistaOrNewer)
            {
                return Standard.HRESULT.S_FALSE;
            }
            if (!Standard.Utility.IsOSWindows7OrNewer)
            {
                if (!_ChangeWindowMessageFilter(message, action))
                {
                    return (Standard.HRESULT) Standard.Win32Error.GetLastError();
                }
                return Standard.HRESULT.S_OK;
            }
            Standard.CHANGEFILTERSTRUCT pChangeFilterStruct = new Standard.CHANGEFILTERSTRUCT {
                cbSize = (uint) Marshal.SizeOf(typeof(Standard.CHANGEFILTERSTRUCT))
            };
            if (!_ChangeWindowMessageFilterEx(hwnd, message, action, ref pChangeFilterStruct))
            {
                return (Standard.HRESULT) Standard.Win32Error.GetLastError();
            }
            filterInfo = pChangeFilterStruct.ExtStatus;
            return Standard.HRESULT.S_OK;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("gdi32.dll")]
        public static extern Standard.CombineRgnResult CombineRgn(IntPtr hrgnDest, IntPtr hrgnSrc1, IntPtr hrgnSrc2, Standard.RGN fnCombineMode);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static string[] CommandLineToArgvW(string cmdLine)
        {
            string[] strArray2;
            IntPtr zero = IntPtr.Zero;
            try
            {
                int numArgs = 0;
                zero = _CommandLineToArgvW(cmdLine, out numArgs);
                if (zero == IntPtr.Zero)
                {
                    throw new Win32Exception();
                }
                string[] strArray = new string[numArgs];
                for (int i = 0; i < numArgs; i++)
                {
                    IntPtr ptr = Marshal.ReadIntPtr(zero, i * Marshal.SizeOf(typeof(IntPtr)));
                    strArray[i] = Marshal.PtrToStringUni(ptr);
                }
                strArray2 = strArray;
            }
            finally
            {
                _LocalFree(zero);
            }
            return strArray2;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static Standard.SafeHBITMAP CreateDIBSection(Standard.SafeDC hdc, ref Standard.BITMAPINFO bitmapInfo, out IntPtr ppvBits, IntPtr hSection, int dwOffset)
        {
            Standard.SafeHBITMAP ehbitmap = null;
            if (hdc == null)
            {
                ehbitmap = _CreateDIBSectionIntPtr(IntPtr.Zero, ref bitmapInfo, 0, out ppvBits, hSection, dwOffset);
            }
            else
            {
                ehbitmap = _CreateDIBSection(hdc, ref bitmapInfo, 0, out ppvBits, hSection, dwOffset);
            }
            if (ehbitmap.IsInvalid)
            {
                Standard.HRESULT.ThrowLastError();
            }
            return ehbitmap;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static IntPtr CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect)
        {
            IntPtr ptr = _CreateRectRgn(nLeftRect, nTopRect, nRightRect, nBottomRect);
            if (IntPtr.Zero == ptr)
            {
                throw new Win32Exception();
            }
            return ptr;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static IntPtr CreateRectRgnIndirect(Standard.RECT lprc)
        {
            IntPtr ptr = _CreateRectRgnIndirect(ref lprc);
            if (IntPtr.Zero == ptr)
            {
                throw new Win32Exception();
            }
            return ptr;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse)
        {
            IntPtr ptr = _CreateRoundRectRgn(nLeftRect, nTopRect, nRightRect, nBottomRect, nWidthEllipse, nHeightEllipse);
            if (IntPtr.Zero == ptr)
            {
                throw new Win32Exception();
            }
            return ptr;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("gdi32.dll")]
        public static extern IntPtr CreateSolidBrush(int crColor);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static IntPtr CreateWindowEx(Standard.WS_EX dwExStyle, string lpClassName, string lpWindowName, Standard.WS dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam)
        {
            IntPtr ptr = _CreateWindowEx(dwExStyle, lpClassName, lpWindowName, dwStyle, x, y, nWidth, nHeight, hWndParent, hMenu, hInstance, lpParam);
            if (IntPtr.Zero == ptr)
            {
                Standard.HRESULT.ThrowLastError();
            }
            return ptr;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("user32.dll", EntryPoint="DefWindowProcW", CharSet=CharSet.Unicode)]
        public static extern IntPtr DefWindowProc(IntPtr hWnd, Standard.WM Msg, IntPtr wParam, IntPtr lParam);
        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("user32.dll")]
        public static extern bool DestroyIcon(IntPtr handle);
        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("user32.dll", SetLastError=true)]
        public static extern bool DestroyWindow(IntPtr hwnd);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static void DrawMenuBar(IntPtr hWnd)
        {
            if (!_DrawMenuBar(hWnd))
            {
                throw new Win32Exception();
            }
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("dwmapi.dll")]
        public static extern bool DwmDefWindowProc(IntPtr hwnd, Standard.WM msg, IntPtr wParam, IntPtr lParam, out IntPtr plResult);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("dwmapi.dll", PreserveSig=false)]
        public static extern void DwmExtendFrameIntoClientArea(IntPtr hwnd, ref Standard.MARGINS pMarInset);
        public static bool DwmGetColorizationColor(out uint pcrColorization, out bool pfOpaqueBlend)
        {
            if ((Standard.Utility.IsOSVistaOrNewer && IsThemeActive()) && _DwmGetColorizationColor(out pcrColorization, out pfOpaqueBlend).Succeeded)
            {
                return true;
            }
            pcrColorization = 0xff000000;
            pfOpaqueBlend = true;
            return false;
        }

        public static Standard.DWM_TIMING_INFO? DwmGetCompositionTimingInfo(IntPtr hwnd)
        {
            if (!Standard.Utility.IsOSVistaOrNewer)
            {
                return null;
            }
            Standard.DWM_TIMING_INFO pTimingInfo = new Standard.DWM_TIMING_INFO {
                cbSize = Marshal.SizeOf(typeof(Standard.DWM_TIMING_INFO))
            };
            Standard.HRESULT hresult = _DwmGetCompositionTimingInfo(hwnd, ref pTimingInfo);
            if (hresult == Standard.HRESULT.E_PENDING)
            {
                return null;
            }
            hresult.ThrowIfFailed();
            return new Standard.DWM_TIMING_INFO?(pTimingInfo);
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("dwmapi.dll", PreserveSig=false)]
        public static extern void DwmInvalidateIconicBitmaps(IntPtr hwnd);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static bool DwmIsCompositionEnabled()
        {
            if (!Standard.Utility.IsOSVistaOrNewer)
            {
                return false;
            }
            return _DwmIsCompositionEnabled();
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("dwmapi.dll", PreserveSig=false)]
        public static extern void DwmSetIconicLivePreviewBitmap(IntPtr hwnd, IntPtr hbmp, Standard.RefPOINT pptClient, Standard.DWM_SIT dwSITFlags);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("dwmapi.dll", PreserveSig=false)]
        public static extern void DwmSetIconicThumbnail(IntPtr hwnd, IntPtr hbmp, Standard.DWM_SIT dwSITFlags);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static void DwmSetWindowAttributeDisallowPeek(IntPtr hwnd, bool disallowPeek)
        {
            int pvAttribute = disallowPeek ? 1 : 0;
            _DwmSetWindowAttribute(hwnd, Standard.DWMWA.DISALLOW_PEEK, ref pvAttribute, 4);
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static void DwmSetWindowAttributeFlip3DPolicy(IntPtr hwnd, Standard.DWMFLIP3D flip3dPolicy)
        {
            int pvAttribute = (int) flip3dPolicy;
            _DwmSetWindowAttribute(hwnd, Standard.DWMWA.FLIP3D_POLICY, ref pvAttribute, 4);
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static Standard.MF EnableMenuItem(IntPtr hMenu, Standard.SC uIDEnableItem, Standard.MF uEnable)
        {
            return (Standard.MF) _EnableMenuItem(hMenu, uIDEnableItem, uEnable);
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success), SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("kernel32.dll")]
        public static extern bool FindClose(IntPtr handle);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("kernel32.dll", CharSet=CharSet.Unicode, SetLastError=true)]
        public static extern Standard.SafeFindHandle FindFirstFileW(string lpFileName, [In, Out, MarshalAs(UnmanagedType.LPStruct)] Standard.WIN32_FIND_DATAW lpFindFileData);
        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("kernel32.dll", SetLastError=true)]
        public static extern bool FindNextFileW(Standard.SafeFindHandle hndFindFile, [In, Out, MarshalAs(UnmanagedType.LPStruct)] Standard.WIN32_FIND_DATAW lpFindFileData);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("gdiplus.dll")]
        public static extern Standard.Status GdipCreateBitmapFromStream(IStream stream, out IntPtr bitmap);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("gdiplus.dll")]
        public static extern Standard.Status GdipCreateHBITMAPFromBitmap(IntPtr bitmap, out IntPtr hbmReturn, int background);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("gdiplus.dll")]
        public static extern Standard.Status GdipCreateHICONFromBitmap(IntPtr bitmap, out IntPtr hbmReturn);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("gdiplus.dll")]
        public static extern Standard.Status GdipDisposeImage(IntPtr image);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("gdiplus.dll")]
        public static extern Standard.Status GdipImageForceValidation(IntPtr image);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("gdiplus.dll")]
        public static extern Standard.Status GdiplusShutdown(IntPtr token);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("gdiplus.dll")]
        public static extern Standard.Status GdiplusStartup(out IntPtr token, Standard.StartupInput input, out Standard.StartupOutput output);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static Standard.RECT GetClientRect(IntPtr hwnd)
        {
            Standard.RECT rect;
            if (!_GetClientRect(hwnd, out rect))
            {
                Standard.HRESULT.ThrowLastError();
            }
            return rect;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("shell32.dll")]
        public static extern Standard.HRESULT GetCurrentProcessExplicitAppUserModelID([MarshalAs(UnmanagedType.LPWStr)] out string AppID);
        public static void GetCurrentThemeName(out string themeFileName, out string color, out string size)
        {
            StringBuilder pszThemeFileName = new StringBuilder(260);
            StringBuilder pszColorBuff = new StringBuilder(260);
            StringBuilder pszSizeBuff = new StringBuilder(260);
            _GetCurrentThemeName(pszThemeFileName, pszThemeFileName.Capacity, pszColorBuff, pszColorBuff.Capacity, pszSizeBuff, pszSizeBuff.Capacity).ThrowIfFailed();
            themeFileName = pszThemeFileName.ToString();
            color = pszColorBuff.ToString();
            size = pszSizeBuff.ToString();
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), Obsolete("Use SafeDC.GetDC instead.", true)]
        public static void GetDC()
        {
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("gdi32.dll")]
        public static extern int GetDeviceCaps(Standard.SafeDC hdc, Standard.DeviceCap nIndex);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static string GetModuleFileName(IntPtr hModule)
        {
            int num;
            StringBuilder lpFilename = new StringBuilder(260);
        Label_000B:
            num = _GetModuleFileName(hModule, lpFilename, lpFilename.Capacity);
            if (num == 0)
            {
                Standard.HRESULT.ThrowLastError();
            }
            if (num == lpFilename.Capacity)
            {
                lpFilename.EnsureCapacity(lpFilename.Capacity * 2);
                goto Label_000B;
            }
            return lpFilename.ToString();
        }

        public static IntPtr GetModuleHandle(string lpModuleName)
        {
            IntPtr ptr = _GetModuleHandle(lpModuleName);
            if (ptr == IntPtr.Zero)
            {
                Standard.HRESULT.ThrowLastError();
            }
            return ptr;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static Standard.MONITORINFO GetMonitorInfo(IntPtr hMonitor)
        {
            Standard.MONITORINFO lpmi = new Standard.MONITORINFO();
            if (!_GetMonitorInfo(hMonitor, lpmi))
            {
                throw new Win32Exception();
            }
            return lpmi;
        }

        public static IntPtr GetStockObject(Standard.StockObject fnObject)
        {
            return _GetStockObject(fnObject);
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, [MarshalAs(UnmanagedType.Bool)] bool bRevert);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("user32.dll")]
        public static extern int GetSystemMetrics(Standard.SM nIndex);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static IntPtr GetWindowLongPtr(IntPtr hwnd, Standard.GWL nIndex)
        {
            IntPtr zero = IntPtr.Zero;
            if (8 == IntPtr.Size)
            {
                zero = GetWindowLongPtr64(hwnd, nIndex);
            }
            else
            {
                zero = new IntPtr(GetWindowLongPtr32(hwnd, nIndex));
            }
            if (IntPtr.Zero == zero)
            {
                throw new Win32Exception();
            }
            return zero;
        }

        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist"), SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("user32.dll", EntryPoint="GetWindowLong", SetLastError=true)]
        private static extern int GetWindowLongPtr32(IntPtr hWnd, Standard.GWL nIndex);
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist"), SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("user32.dll", EntryPoint="GetWindowLongPtr", SetLastError=true)]
        private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, Standard.GWL nIndex);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static Standard.WINDOWPLACEMENT GetWindowPlacement(IntPtr hwnd)
        {
            Standard.WINDOWPLACEMENT lpwndpl = new Standard.WINDOWPLACEMENT();
            if (!GetWindowPlacement(hwnd, lpwndpl))
            {
                throw new Win32Exception();
            }
            return lpwndpl;
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("user32.dll", SetLastError=true)]
        private static extern bool GetWindowPlacement(IntPtr hwnd, Standard.WINDOWPLACEMENT lpwndpl);
        public static Standard.RECT GetWindowRect(IntPtr hwnd)
        {
            Standard.RECT rect;
            if (!_GetWindowRect(hwnd, out rect))
            {
                Standard.HRESULT.ThrowLastError();
            }
            return rect;
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("uxtheme.dll")]
        public static extern bool IsThemeActive();
        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("user32.dll")]
        public static extern bool IsWindow(IntPtr hwnd);
        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hwnd);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("user32.dll")]
        public static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static void PostMessage(IntPtr hWnd, Standard.WM Msg, IntPtr wParam, IntPtr lParam)
        {
            if (!_PostMessage(hWnd, Msg, wParam, lParam))
            {
                throw new Win32Exception();
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static short RegisterClassEx(ref Standard.WNDCLASSEX lpwcx)
        {
            short num = _RegisterClassEx(ref lpwcx);
            if (num == 0)
            {
                Standard.HRESULT.ThrowLastError();
            }
            return num;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static Standard.WM RegisterWindowMessage(string lpString)
        {
            uint num = _RegisterWindowMessage(lpString);
            if (num == 0)
            {
                Standard.HRESULT.ThrowLastError();
            }
            return (Standard.WM) num;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static void RemoveMenu(IntPtr hMenu, Standard.SC uPosition, Standard.MF uFlags)
        {
            if (!_RemoveMenu(hMenu, (uint) uPosition, (uint) uFlags))
            {
                throw new Win32Exception();
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static IntPtr SelectObject(Standard.SafeDC hdc, Standard.SafeHBITMAP hgdiobj)
        {
            IntPtr ptr = _SelectObjectSafeHBITMAP(hdc, hgdiobj);
            if (ptr == IntPtr.Zero)
            {
                Standard.HRESULT.ThrowLastError();
            }
            return ptr;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static IntPtr SelectObject(Standard.SafeDC hdc, IntPtr hgdiobj)
        {
            IntPtr ptr = _SelectObject(hdc, hgdiobj);
            if (ptr == IntPtr.Zero)
            {
                Standard.HRESULT.ThrowLastError();
            }
            return ptr;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("user32.dll", SetLastError=true)]
        public static extern int SendInput(int nInputs, ref Standard.INPUT pInputs, int cbSize);
        [DllImport("user32.dll", SetLastError=true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, Standard.WM Msg, IntPtr wParam, IntPtr lParam);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static IntPtr SetActiveWindow(IntPtr hwnd)
        {
            Standard.Verify.IsNotDefault<IntPtr>(hwnd, "hwnd");
            IntPtr ptr = _SetActiveWindow(hwnd);
            if (ptr == IntPtr.Zero)
            {
                Standard.HRESULT.ThrowLastError();
            }
            return ptr;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static IntPtr SetClassLongPtr(IntPtr hwnd, Standard.GCLP nIndex, IntPtr dwNewLong)
        {
            if (8 == IntPtr.Size)
            {
                return SetClassLongPtr64(hwnd, nIndex, dwNewLong);
            }
            return new IntPtr(SetClassLongPtr32(hwnd, nIndex, dwNewLong.ToInt32()));
        }

        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist"), SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("user32.dll", EntryPoint="SetClassLong", SetLastError=true)]
        private static extern int SetClassLongPtr32(IntPtr hWnd, Standard.GCLP nIndex, int dwNewLong);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist"), DllImport("user32.dll", EntryPoint="SetClassLongPtr", SetLastError=true)]
        private static extern IntPtr SetClassLongPtr64(IntPtr hWnd, Standard.GCLP nIndex, IntPtr dwNewLong);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("shell32.dll", PreserveSig=false)]
        public static extern void SetCurrentProcessExplicitAppUserModelID([MarshalAs(UnmanagedType.LPWStr)] string AppID);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("kernel32.dll", SetLastError=true)]
        public static extern Standard.ErrorModes SetErrorMode(Standard.ErrorModes newMode);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static void SetProcessWorkingSetSize(IntPtr hProcess, int dwMinimumWorkingSetSize, int dwMaximumWorkingSetSize)
        {
            if (!_SetProcessWorkingSetSize(hProcess, new IntPtr(dwMinimumWorkingSetSize), new IntPtr(dwMaximumWorkingSetSize)))
            {
                throw new Win32Exception();
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static IntPtr SetWindowLongPtr(IntPtr hwnd, Standard.GWL nIndex, IntPtr dwNewLong)
        {
            if (8 == IntPtr.Size)
            {
                return SetWindowLongPtr64(hwnd, nIndex, dwNewLong);
            }
            return new IntPtr(SetWindowLongPtr32(hwnd, nIndex, dwNewLong.ToInt32()));
        }

        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist"), SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("user32.dll", EntryPoint="SetWindowLong", SetLastError=true)]
        private static extern int SetWindowLongPtr32(IntPtr hWnd, Standard.GWL nIndex, int dwNewLong);
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist"), SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("user32.dll", EntryPoint="SetWindowLongPtr", SetLastError=true)]
        private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, Standard.GWL nIndex, IntPtr dwNewLong);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, Standard.SWP uFlags)
        {
            if (!_SetWindowPos(hWnd, hWndInsertAfter, x, y, cx, cy, uFlags))
            {
                return false;
            }
            return true;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static void SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw)
        {
            if (_SetWindowRgn(hWnd, hRgn, bRedraw) == 0)
            {
                throw new Win32Exception();
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("uxtheme.dll", PreserveSig=false)]
        public static extern void SetWindowThemeAttribute([In] IntPtr hwnd, [In] Standard.WINDOWTHEMEATTRIBUTETYPE eAttribute, [In] ref Standard.WTA_OPTIONS pvAttribute, [In] uint cbAttribute);
        public static void SHAddToRecentDocs(Standard.IShellLinkW shellLink)
        {
            _SHAddToRecentDocs_ShellLink(Standard.SHARD.LINK, shellLink);
        }

        public static void SHAddToRecentDocs(string path)
        {
            _SHAddToRecentDocs_String(Standard.SHARD.PATHW, path);
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("shell32.dll", PreserveSig=false)]
        public static extern Standard.HRESULT SHCreateItemFromParsingName([MarshalAs(UnmanagedType.LPWStr)] string pszPath, IBindCtx pbc, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppv);
        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("shell32.dll")]
        public static extern bool Shell_NotifyIcon(Standard.NIM dwMessage, [In] Standard.NOTIFYICONDATA lpdata);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("shell32.dll")]
        public static extern Standard.Win32Error SHFileOperation(ref Standard.SHFILEOPSTRUCT lpFileOp);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("shell32.dll", PreserveSig=false)]
        public static extern void SHGetItemFromDataObject(IDataObject pdtobj, Standard.DOGIF dwFlags, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppv);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hwnd, Standard.SW nCmdShow);
        [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        public static Standard.HIGHCONTRAST SystemParameterInfo_GetHIGHCONTRAST()
        {
            Standard.HIGHCONTRAST pvParam = new Standard.HIGHCONTRAST {
                cbSize = Marshal.SizeOf(typeof(Standard.HIGHCONTRAST))
            };
            if (!_SystemParametersInfo_HIGHCONTRAST(Standard.SPI.GETHIGHCONTRAST, pvParam.cbSize, ref pvParam, Standard.SPIF.None))
            {
                Standard.HRESULT.ThrowLastError();
            }
            return pvParam;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static Standard.NONCLIENTMETRICS SystemParameterInfo_GetNONCLIENTMETRICS()
        {
            Standard.NONCLIENTMETRICS pvParam = Standard.Utility.IsOSVistaOrNewer ? Standard.NONCLIENTMETRICS.VistaMetricsStruct : Standard.NONCLIENTMETRICS.XPMetricsStruct;
            if (!_SystemParametersInfo_NONCLIENTMETRICS(Standard.SPI.GETNONCLIENTMETRICS, pvParam.cbSize, ref pvParam, Standard.SPIF.None))
            {
                Standard.HRESULT.ThrowLastError();
            }
            return pvParam;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static void SystemParametersInfo(Standard.SPI uiAction, int uiParam, string pvParam, Standard.SPIF fWinIni)
        {
            if (!_SystemParametersInfo_String(uiAction, uiParam, pvParam, fWinIni))
            {
                Standard.HRESULT.ThrowLastError();
            }
        }

        [DllImport("user32.dll")]
        public static extern uint TrackPopupMenuEx(IntPtr hmenu, uint fuFlags, int x, int y, IntPtr hwnd, IntPtr lptpm);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static void UnregisterClass(short atom, IntPtr hinstance)
        {
            if (!_UnregisterClassAtom(new IntPtr(atom), hinstance))
            {
                Standard.HRESULT.ThrowLastError();
            }
        }

        public static void UnregisterClass(string lpClassName, IntPtr hInstance)
        {
            if (!_UnregisterClassName(lpClassName, hInstance))
            {
                Standard.HRESULT.ThrowLastError();
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static void UpdateLayeredWindow(IntPtr hwnd, int crKey, ref Standard.BLENDFUNCTION pblend, Standard.ULW dwFlags)
        {
            if (!_UpdateLayeredWindowIntPtr(hwnd, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, crKey, ref pblend, dwFlags))
            {
                Standard.HRESULT.ThrowLastError();
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static void UpdateLayeredWindow(IntPtr hwnd, Standard.SafeDC hdcDst, ref Standard.POINT pptDst, ref Standard.SIZE psize, Standard.SafeDC hdcSrc, ref Standard.POINT pptSrc, int crKey, ref Standard.BLENDFUNCTION pblend, Standard.ULW dwFlags)
        {
            if (!_UpdateLayeredWindow(hwnd, hdcDst, ref pptDst, ref psize, hdcSrc, ref pptSrc, crKey, ref pblend, dwFlags))
            {
                Standard.HRESULT.ThrowLastError();
            }
        }
    }
}

