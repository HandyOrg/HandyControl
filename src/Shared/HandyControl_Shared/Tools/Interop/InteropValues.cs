using System;
using System.Runtime.InteropServices;
using System.Security;

namespace HandyControl.Tools.Interop
{
    public class InteropValues
    {
        #region public

        public delegate IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam);

        public delegate IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

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
            NIN_BALLOONSHOW = WM_USER + 2,
            NIN_BALLOONHIDE = WM_USER + 3,
            NIN_BALLOONTIMEOUT = WM_USER + 4,
            NIM_ADD = 0x00000000,
            NIM_MODIFY = 0x00000001,
            NIM_DELETE = 0x00000002,
            NIIF_NONE = 0x00000000,
            NIIF_INFO = 0x00000001,
            NIIF_WARNING = 0x00000002,
            NIIF_ERROR = 0x00000003,
            NIIF_LARGE_ICON = 0x00000020,
            WM_KEYDOWN = 0x0100,
            WM_KEYUP = 0x0101,
            WM_SYSKEYDOWN = 0x0104,
            WM_SYSKEYUP = 0x0105,
            WM_SYSCOMMAND = 0x112,
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
            WM_CLIPBOARDUPDATE = 0x031D,
            WM_USER = 0x0400,
            MF_BYPOSITION = 0x400,
            MF_SEPARATOR = 0x800,
            TB_GETBUTTON = WM_USER + 23,
            TB_BUTTONCOUNT = WM_USER + 24,
            TB_GETITEMRECT = WM_USER + 29,
            STANDARD_RIGHTS_REQUIRED = 0x000F0000,
            SYNCHRONIZE = 0x00100000,
            PROCESS_ALL_ACCESS = STANDARD_RIGHTS_REQUIRED | SYNCHRONIZE | 0xFFF,
            MEM_COMMIT = 0x1000,
            MEM_RELEASE = 0x8000,
            PAGE_READWRITE = 0x04,
            TBSTATE_HIDDEN = 0x08,
            VERTRES = 10,
            HORZRES = 8,
            DESKTOPVERTRES = 117,
            DESKTOPHORZRES = 118,
            LOGPIXELSX = 88,
            LOGPIXELSY = 90,
            CXFRAME = 32,
            CXSIZEFRAME = CXFRAME,
            SW_HIDE = 0;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class NOTIFYICONDATA
        {
            public int cbSize = Marshal.SizeOf(typeof(NOTIFYICONDATA));
            public IntPtr hWnd;
            public int uID;
            public int uFlags;
            public int uCallbackMessage;
            public IntPtr hIcon;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szTip = string.Empty;
            public int dwState = 0x01;
            public int dwStateMask = 0x01;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string szInfo = string.Empty;
            public int uTimeoutOrVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string szInfoTitle = string.Empty;
            public int dwInfoFlags;
        }

        [Flags]
        public enum ProcessAccess
        {
            AllAccess = CreateThread | DuplicateHandle | QueryInformation | SetInformation | Terminate | VMOperation | VMRead | VMWrite | Synchronize,
            CreateThread = 0x2,
            DuplicateHandle = 0x40,
            QueryInformation = 0x400,
            SetInformation = 0x200,
            Terminate = 0x1,
            VMOperation = 0x8,
            VMRead = 0x10,
            VMWrite = 0x20,
            Synchronize = 0x100000
        }

        [Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TBBUTTON
        {
            public int iBitmap;
            public int idCommand;
            public IntPtr fsStateStylePadding;
            public IntPtr dwData;
            public IntPtr iString;
        }

        [Flags]
        public enum AllocationType
        {
            Commit = 0x1000,
            Reserve = 0x2000,
            Decommit = 0x4000,
            Release = 0x8000,
            Reset = 0x80000,
            Physical = 0x400000,
            TopDown = 0x100000,
            WriteWatch = 0x200000,
            LargePages = 0x20000000
        }

        [Flags]
        public enum MemoryProtection
        {
            Execute = 0x10,
            ExecuteRead = 0x20,
            ExecuteReadWrite = 0x40,
            ExecuteWriteCopy = 0x80,
            NoAccess = 0x01,
            ReadOnly = 0x02,
            ReadWrite = 0x04,
            WriteCopy = 0x08,
            GuardModifierflag = 0x100,
            NoCacheModifierflag = 0x200,
            WriteCombineModifierflag = 0x400
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TRAYDATA
        {
            public IntPtr hwnd;
            public uint uID;
            public uint uCallbackMessage;
            public uint bReserved0;
            public uint bReserved1;
            public IntPtr hIcon;
        }

        [Flags]
        public enum FreeType
        {
            Decommit = 0x4000,
            Release = 0x8000,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        public enum HookType
        {
            WH_KEYBOARD_LL = 13,
            WH_MOUSE_LL = 14
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEHOOKSTRUCT
        {
            public POINT pt;
            public IntPtr hwnd;
            public uint wHitTestCode;
            public IntPtr dwExtraInfo;
        }

        #endregion

        #region internal

        internal static class ExternDll
        {
            public const string
                User32 = "user32.dll",
                Gdi32 = "gdi32.dll",
                Kernel32 = "kernel32.dll",
                Shell32 = "shell32.dll";
        }

        internal enum SM
        {
            CXSCREEN = 0,
            CYSCREEN = 1,
            CXVSCROLL = 2,
            CYHSCROLL = 3,
            CYCAPTION = 4,
            CXBORDER = 5,
            CYBORDER = 6,
            CXFIXEDFRAME = 7,
            CYFIXEDFRAME = 8,
            CYVTHUMB = 9,
            CXHTHUMB = 10,
            CXICON = 11,
            CYICON = 12,
            CXCURSOR = 13,
            CYCURSOR = 14,
            CYMENU = 15,
            CXFULLSCREEN = 16,
            CYFULLSCREEN = 17,
            CYKANJIWINDOW = 18,
            MOUSEPRESENT = 19,
            CYVSCROLL = 20,
            CXHSCROLL = 21,
            DEBUG = 22,
            SWAPBUTTON = 23,
            CXMIN = 28,
            CYMIN = 29,
            CXSIZE = 30,
            CYSIZE = 31,
            CXFRAME = 32,
            CXSIZEFRAME = CXFRAME,
            CYFRAME = 33,
            CYSIZEFRAME = CYFRAME,
            CXMINTRACK = 34,
            CYMINTRACK = 35,
            CXDOUBLECLK = 36,
            CYDOUBLECLK = 37,
            CXICONSPACING = 38,
            CYICONSPACING = 39,
            MENUDROPALIGNMENT = 40,
            PENWINDOWS = 41,
            DBCSENABLED = 42,
            CMOUSEBUTTONS = 43,
            SECURE = 44,
            CXEDGE = 45,
            CYEDGE = 46,
            CXMINSPACING = 47,
            CYMINSPACING = 48,
            CXSMICON = 49,
            CYSMICON = 50,
            CYSMCAPTION = 51,
            CXSMSIZE = 52,
            CYSMSIZE = 53,
            CXMENUSIZE = 54,
            CYMENUSIZE = 55,
            ARRANGE = 56,
            CXMINIMIZED = 57,
            CYMINIMIZED = 58,
            CXMAXTRACK = 59,
            CYMAXTRACK = 60,
            CXMAXIMIZED = 61,
            CYMAXIMIZED = 62,
            NETWORK = 63,
            CLEANBOOT = 67,
            CXDRAG = 68,
            CYDRAG = 69,
            SHOWSOUNDS = 70,
            CXMENUCHECK = 71,
            CYMENUCHECK = 72,
            SLOWMACHINE = 73,
            MIDEASTENABLED = 74,
            MOUSEWHEELPRESENT = 75,
            XVIRTUALSCREEN = 76,
            YVIRTUALSCREEN = 77,
            CXVIRTUALSCREEN = 78,
            CYVIRTUALSCREEN = 79,
            CMONITORS = 80,
            SAMEDISPLAYFORMAT = 81,
            IMMENABLED = 82,
            CXFOCUSBORDER = 83,
            CYFOCUSBORDER = 84,
            TABLETPC = 86,
            MEDIACENTER = 87,
            REMOTESESSION = 0x1000,
            REMOTECONTROL = 0x2001
        }

        internal enum CacheSlot
        {
            DpiX,

            FocusBorderWidth,
            FocusBorderHeight,
            HighContrast,
            MouseVanish,

            DropShadow,
            FlatMenu,
            WorkAreaInternal,
            WorkArea,

            IconMetrics,

            KeyboardCues,
            KeyboardDelay,
            KeyboardPreference,
            KeyboardSpeed,
            SnapToDefaultButton,
            WheelScrollLines,
            MouseHoverTime,
            MouseHoverHeight,
            MouseHoverWidth,

            MenuDropAlignment,
            MenuFade,
            MenuShowDelay,

            ComboBoxAnimation,
            ClientAreaAnimation,
            CursorShadow,
            GradientCaptions,
            HotTracking,
            ListBoxSmoothScrolling,
            MenuAnimation,
            SelectionFade,
            StylusHotTracking,
            ToolTipAnimation,
            ToolTipFade,
            UIEffects,

            MinimizeAnimation,
            Border,
            CaretWidth,
            ForegroundFlashCount,
            DragFullWindows,
            NonClientMetrics,

            ThinHorizontalBorderHeight,
            ThinVerticalBorderWidth,
            CursorWidth,
            CursorHeight,
            ThickHorizontalBorderHeight,
            ThickVerticalBorderWidth,
            MinimumHorizontalDragDistance,
            MinimumVerticalDragDistance,
            FixedFrameHorizontalBorderHeight,
            FixedFrameVerticalBorderWidth,
            FocusHorizontalBorderHeight,
            FocusVerticalBorderWidth,
            FullPrimaryScreenWidth,
            FullPrimaryScreenHeight,
            HorizontalScrollBarButtonWidth,
            HorizontalScrollBarHeight,
            HorizontalScrollBarThumbWidth,
            IconWidth,
            IconHeight,
            IconGridWidth,
            IconGridHeight,
            MaximizedPrimaryScreenWidth,
            MaximizedPrimaryScreenHeight,
            MaximumWindowTrackWidth,
            MaximumWindowTrackHeight,
            MenuCheckmarkWidth,
            MenuCheckmarkHeight,
            MenuButtonWidth,
            MenuButtonHeight,
            MinimumWindowWidth,
            MinimumWindowHeight,
            MinimizedWindowWidth,
            MinimizedWindowHeight,
            MinimizedGridWidth,
            MinimizedGridHeight,
            MinimumWindowTrackWidth,
            MinimumWindowTrackHeight,
            PrimaryScreenWidth,
            PrimaryScreenHeight,
            WindowCaptionButtonWidth,
            WindowCaptionButtonHeight,
            ResizeFrameHorizontalBorderHeight,
            ResizeFrameVerticalBorderWidth,
            SmallIconWidth,
            SmallIconHeight,
            SmallWindowCaptionButtonWidth,
            SmallWindowCaptionButtonHeight,
            VirtualScreenWidth,
            VirtualScreenHeight,
            VerticalScrollBarWidth,
            VerticalScrollBarButtonHeight,
            WindowCaptionHeight,
            KanjiWindowHeight,
            MenuBarHeight,
            VerticalScrollBarThumbHeight,
            IsImmEnabled,
            IsMediaCenter,
            IsMenuDropRightAligned,
            IsMiddleEastEnabled,
            IsMousePresent,
            IsMouseWheelPresent,
            IsPenWindows,
            IsRemotelyControlled,
            IsRemoteSession,
            ShowSounds,
            IsSlowMachine,
            SwapButtons,
            IsTabletPC,
            VirtualScreenLeft,
            VirtualScreenTop,

            PowerLineStatus,

            IsGlassEnabled,
            UxThemeName,
            UxThemeColor,
            WindowCornerRadius,
            WindowGlassColor,
            WindowGlassBrush,
            WindowNonClientFrameThickness,
            WindowResizeBorderThickness,

            NumSlots
        }

        internal static class Win32Constant
        {
            internal const int MAX_PATH = 260;
            internal const int INFOTIPSIZE = 1024;
            internal const int TRUE = 1;
            internal const int FALSE = 0;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal class WNDCLASS
        {
            public int style;
            public WndProc lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;
            public string lpszMenuName;
            public string lpszClassName;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        internal struct BITMAPINFO
        {
            public int bmiHeader_biSize;

            public int bmiHeader_biWidth;

            public int bmiHeader_biHeight;

            public short bmiHeader_biPlanes;

            public short bmiHeader_biBitCount;

            public int bmiHeader_biCompression;

            public int bmiHeader_biSizeImage;

            public int bmiHeader_biXPelsPerMeter;

            public int bmiHeader_biYPelsPerMeter;

            public int bmiHeader_biClrUsed;

            public int bmiHeader_biClrImportant;

            public BITMAPINFO(int width, int height, short bpp)
            {
                bmiHeader_biSize = SizeOf();
                bmiHeader_biWidth = width;
                bmiHeader_biHeight = height;
                bmiHeader_biPlanes = 1;
                bmiHeader_biBitCount = bpp;
                bmiHeader_biCompression = 0;
                bmiHeader_biSizeImage = 0;
                bmiHeader_biXPelsPerMeter = 0;
                bmiHeader_biYPelsPerMeter = 0;
                bmiHeader_biClrUsed = 0;
                bmiHeader_biClrImportant = 0;
            }

            [SecuritySafeCritical]
            private static int SizeOf()
            {
                return Marshal.SizeOf(typeof(BITMAPINFO));
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal class ICONINFO
        {
            public bool fIcon = false;
            public int xHotspot = 0;
            public int yHotspot = 0;
            public BitmapHandle hbmMask = null;
            public BitmapHandle hbmColor = null;
        }

        #endregion
    }
}