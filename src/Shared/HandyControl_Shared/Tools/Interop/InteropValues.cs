using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;

namespace HandyControl.Tools.Interop
{
    internal class InteropValues
    {
        internal static class ExternDll
        {
            public const string
                User32 = "user32.dll",
                Gdi32 = "gdi32.dll",
                GdiPlus = "gdiplus.dll",
                Kernel32 = "kernel32.dll",
                Shell32 = "shell32.dll",
                MsImg = "msimg32.dll";
        }

        internal delegate IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam);

        internal delegate IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [return: MarshalAs(UnmanagedType.Bool)]
        internal delegate bool EnumMonitorsDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData);

        internal delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        internal const int
            BITSPIXEL = 12,
            PLANES = 14,
            BI_RGB = 0,
            DIB_RGB_COLORS = 0,
            E_FAIL = unchecked((int)0x80004005),
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
            WM_ACTIVATE = 0x0006,
            WM_QUIT = 0x0012,
            WM_GETMINMAXINFO = 0x0024,
            WM_WINDOWPOSCHANGING = 0x0046,
            WM_WINDOWPOSCHANGED = 0x0047,
            WM_SETICON = 0x0080,
            WM_NCCREATE = 0x0081,
            WM_NCDESTROY = 0x0082,
            WM_NCACTIVATE = 0x0086,
            WM_NCRBUTTONDOWN = 0x00A4,
            WM_NCRBUTTONUP = 0x00A5,
            WM_NCRBUTTONDBLCLK = 0x00A6,
            WM_NCUAHDRAWCAPTION = 0x00AE,
            WM_NCUAHDRAWFRAME = 0x00AF,
            WM_KEYDOWN = 0x0100,
            WM_KEYUP = 0x0101,
            WM_SYSKEYDOWN = 0x0104,
            WM_SYSKEYUP = 0x0105,
            WM_SYSCOMMAND = 0x112,
            WM_MOUSEMOVE = 0x0200,
            WM_LBUTTONUP = 0x0202,
            WM_LBUTTONDBLCLK = 0x0203,
            WM_RBUTTONUP = 0x0205,
            WM_CLIPBOARDUPDATE = 0x031D,
            WM_USER = 0x0400,
            WS_VISIBLE = 0x10000000,
            MF_BYPOSITION = 0x400,
            MF_SEPARATOR = 0x800,
            TB_GETBUTTON = WM_USER + 23,
            TB_BUTTONCOUNT = WM_USER + 24,
            TB_GETITEMRECT = WM_USER + 29,
            VERTRES = 10,
            DESKTOPVERTRES = 117,
            LOGPIXELSX = 88,
            LOGPIXELSY = 90,
            SC_SIZE = 0xF000,
            SC_MOVE = 0xF010,
            SC_MINIMIZE = 0xF020,
            SC_MAXIMIZE = 0xF030,
            SC_RESTORE = 0xF120,
            SRCCOPY = 0x00CC0020,
            MONITOR_DEFAULTTONEAREST = 0x00000002;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal class NOTIFYICONDATA
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

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct TBBUTTON
        {
            public int iBitmap;
            public int idCommand;
            public IntPtr fsStateStylePadding;
            public IntPtr dwData;
            public IntPtr iString;
        }

        [Flags]
        internal enum AllocationType
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
        internal enum MemoryProtection
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
        internal struct TRAYDATA
        {
            public IntPtr hwnd;
            public uint uID;
            public uint uCallbackMessage;
            public uint bReserved0;
            public uint bReserved1;
            public IntPtr hIcon;
        }

        [Flags]
        internal enum FreeType
        {
            Decommit = 0x4000,
            Release = 0x8000,
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        internal enum HookType
        {
            WH_KEYBOARD_LL = 13,
            WH_MOUSE_LL = 14
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct MOUSEHOOKSTRUCT
        {
            public POINT pt;
            public IntPtr hwnd;
            public uint wHitTestCode;
            public IntPtr dwExtraInfo;
        }

        [Flags]
        internal enum ProcessAccess
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
        internal struct RECT
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

            public RECT(Rect rect)
            {
                Left = (int)rect.Left;
                Top = (int)rect.Top;
                Right = (int)rect.Right;
                Bottom = (int)rect.Bottom;
            }

            public Point Position => new Point(Left, Top);
            public Size Size => new Size(Width, Height);

            public int Height
            {
                get => Bottom - Top;
                set => Bottom = Top + value;
            }

            public int Width
            {
                get => Right - Left;
                set => Right = Left + value;
            }
        }

        internal struct BLENDFUNCTION
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }

        internal enum GWL
        {
            STYLE = -16,
            EXSTYLE = -20
        }

        internal enum GWLP
        {
            WNDPROC = -4,
            HINSTANCE = -6,
            HWNDPARENT = -8,
            USERDATA = -21,
            ID = -12
        }

        internal struct BITMAPINFOHEADER
        {
            internal uint biSize;
            internal int biWidth;
            internal int biHeight;
            internal ushort biPlanes;
            internal ushort biBitCount;
            internal uint biCompression;
            internal uint biSizeImage;
            internal int biXPelsPerMeter;
            internal int biYPelsPerMeter;
            internal uint biClrUsed;
            internal uint biClrImportant;
        }

        [Flags]
        internal enum RedrawWindowFlags : uint
        {
            Invalidate = 1u,
            InternalPaint = 2u,
            Erase = 4u,
            Validate = 8u,
            NoInternalPaint = 16u,
            NoErase = 32u,
            NoChildren = 64u,
            AllChildren = 128u,
            UpdateNow = 256u,
            EraseNow = 512u,
            Frame = 1024u,
            NoFrame = 2048u
        }

        [StructLayout(LayoutKind.Sequential)]
        internal class WINDOWPOS
        {
            public IntPtr hwnd;
            public IntPtr hwndInsertAfter;
            public int x;
            public int y;
            public int cx;
            public int cy;
            public uint flags;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal class WINDOWPLACEMENT
        {
            public int length = Marshal.SizeOf(typeof(WINDOWPLACEMENT));
            public int flags;
            public int showCmd;
            public POINT ptMinPosition;
            public POINT ptMaxPosition;
            public RECT rcNormalPosition;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        internal struct SIZE
        {
            [ComAliasName("Microsoft.VisualStudio.OLE.Interop.LONG")]
            public int cx;
            [ComAliasName("Microsoft.VisualStudio.OLE.Interop.LONG")]
            public int cy;
        }

        internal struct MONITORINFO
        {
            public uint cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public uint dwFlags;
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

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct WNDCLASS
        {
            public uint style;
            public Delegate lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpszMenuName;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpszClassName;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal class WNDCLASS4ICON
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
            public int biSize;

            public int biWidth;

            public int biHeight;

            public short biPlanes;

            public short biBitCount;

            public int biCompression;

            public int biSizeImage;

            public int biXPelsPerMeter;

            public int biYPelsPerMeter;

            public int biClrUsed;

            public int biClrImportant;

            public BITMAPINFO(int width, int height, short bpp)
            {
                biSize = SizeOf();
                biWidth = width;
                biHeight = height;
                biPlanes = 1;
                biBitCount = bpp;
                biCompression = 0;
                biSizeImage = 0;
                biXPelsPerMeter = 0;
                biYPelsPerMeter = 0;
                biClrUsed = 0;
                biClrImportant = 0;
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

        internal enum WINDOWCOMPOSITIONATTRIB
        {
            WCA_ACCENT_POLICY = 19
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct WINCOMPATTRDATA
        {
            public WINDOWCOMPOSITIONATTRIB Attribute;
            public IntPtr Data;
            public int DataSize;
        }

        internal enum ACCENTSTATE
        {
            ACCENT_DISABLED = 0,
            ACCENT_ENABLE_GRADIENT = 1,
            ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
            ACCENT_ENABLE_BLURBEHIND = 3,
            ACCENT_ENABLE_ACRYLICBLURBEHIND = 4,
            ACCENT_INVALID_STATE = 5
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct ACCENTPOLICY
        {
            public ACCENTSTATE AccentState;
            public int AccentFlags;
            public uint GradientColor;
            public int AnimationId;
        }

        [ComImport, Guid("0000000C-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface IStream
        {
            int Read([In] IntPtr buf, [In] int len);

            int Write([In] IntPtr buf, [In] int len);

            [return: MarshalAs(UnmanagedType.I8)]
            long Seek([In, MarshalAs(UnmanagedType.I8)] long dlibMove, [In] int dwOrigin);

            void SetSize([In, MarshalAs(UnmanagedType.I8)] long libNewSize);

            [return: MarshalAs(UnmanagedType.I8)]
            long CopyTo([In, MarshalAs(UnmanagedType.Interface)] IStream pstm, [In, MarshalAs(UnmanagedType.I8)] long cb, [Out, MarshalAs(UnmanagedType.LPArray)] long[] pcbRead);

            void Commit([In]int grfCommitFlags);

            void Revert();

            void LockRegion([In, MarshalAs(UnmanagedType.I8)]long libOffset, [In, MarshalAs(UnmanagedType.I8)] long cb, [In] int dwLockType);

            void UnlockRegion([In, MarshalAs(UnmanagedType.I8)] long libOffset, [In, MarshalAs(UnmanagedType.I8)] long cb, [In] int dwLockType);

            void Stat([In] IntPtr pStatstg, [In] int grfStatFlag);

            [return: MarshalAs(UnmanagedType.Interface)]
            IStream Clone();
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        internal class StreamConsts
        {
            public const int LOCK_WRITE = 0x1;
            public const int LOCK_EXCLUSIVE = 0x2;
            public const int LOCK_ONLYONCE = 0x4;
            public const int STATFLAG_DEFAULT = 0x0;
            public const int STATFLAG_NONAME = 0x1;
            public const int STATFLAG_NOOPEN = 0x2;
            public const int STGC_DEFAULT = 0x0;
            public const int STGC_OVERWRITE = 0x1;
            public const int STGC_ONLYIFCURRENT = 0x2;
            public const int STGC_DANGEROUSLYCOMMITMERELYTODISKCACHE = 0x4;
            public const int STREAM_SEEK_SET = 0x0;
            public const int STREAM_SEEK_CUR = 0x1;
            public const int STREAM_SEEK_END = 0x2;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        internal class ImageCodecInfoPrivate
        {
            [MarshalAs(UnmanagedType.Struct)]
            public Guid Clsid;
            [MarshalAs(UnmanagedType.Struct)]
            public Guid FormatID;

            public IntPtr CodecName = IntPtr.Zero;
            public IntPtr DllName = IntPtr.Zero;
            public IntPtr FormatDescription = IntPtr.Zero;
            public IntPtr FilenameExtension = IntPtr.Zero;
            public IntPtr MimeType = IntPtr.Zero;

            public int Flags;
            public int Version;
            public int SigCount;
            public int SigSize;

            public IntPtr SigPattern = IntPtr.Zero;
            public IntPtr SigMask = IntPtr.Zero;
        }

        internal class ComStreamFromDataStream : IStream
        {
            protected Stream DataStream;

            // to support seeking ahead of the stream length...
            private long _virtualPosition = -1;

            internal ComStreamFromDataStream(Stream dataStream)
            {
                this.DataStream = dataStream ?? throw new ArgumentNullException(nameof(dataStream));
            }

            private void ActualizeVirtualPosition()
            {
                if (_virtualPosition == -1) return;

                if (_virtualPosition > DataStream.Length)
                    DataStream.SetLength(_virtualPosition);

                DataStream.Position = _virtualPosition;

                _virtualPosition = -1;
            }

            public virtual IStream Clone()
            {
                NotImplemented();
                return null;
            }

            public virtual void Commit(int grfCommitFlags)
            {
                DataStream.Flush();
                ActualizeVirtualPosition();
            }

            public virtual long CopyTo(IStream pstm, long cb, long[] pcbRead)
            {
                const int bufsize = 4096; // one page
                var buffer = Marshal.AllocHGlobal(bufsize);
                if (buffer == IntPtr.Zero) throw new OutOfMemoryException();
                long written = 0;

                try
                {
                    while (written < cb)
                    {
                        var toRead = bufsize;
                        if (written + toRead > cb) toRead = (int)(cb - written);
                        var read = Read(buffer, toRead);
                        if (read == 0) break;
                        if (pstm.Write(buffer, read) != read)
                        {
                            throw EFail("Wrote an incorrect number of bytes");
                        }
                        written += read;
                    }
                }
                finally
                {
                    Marshal.FreeHGlobal(buffer);
                }
                if (pcbRead != null && pcbRead.Length > 0)
                {
                    pcbRead[0] = written;
                }

                return written;
            }

            public virtual Stream GetDataStream() => DataStream;

            public virtual void LockRegion(long libOffset, long cb, int dwLockType)
            {
            }

            protected static ExternalException EFail(string msg) => throw new ExternalException(msg, E_FAIL);

            protected static void NotImplemented() => throw new NotImplementedException();

            public virtual int Read(IntPtr buf, int length)
            {
                var buffer = new byte[length];
                var count = Read(buffer, length);
                Marshal.Copy(buffer, 0, buf, length);
                return count;
            }

            public virtual int Read(byte[] buffer, int length)
            {
                ActualizeVirtualPosition();
                return DataStream.Read(buffer, 0, length);
            }

            public virtual void Revert() => NotImplemented();

            public virtual long Seek(long offset, int origin)
            {
                var pos = _virtualPosition;
                if (_virtualPosition == -1)
                {
                    pos = DataStream.Position;
                }
                var len = DataStream.Length;

                switch (origin)
                {
                    case StreamConsts.STREAM_SEEK_SET:
                        if (offset <= len)
                        {
                            DataStream.Position = offset;
                            _virtualPosition = -1;
                        }
                        else
                        {
                            _virtualPosition = offset;
                        }
                        break;
                    case StreamConsts.STREAM_SEEK_END:
                        if (offset <= 0)
                        {
                            DataStream.Position = len + offset;
                            _virtualPosition = -1;
                        }
                        else
                        {
                            _virtualPosition = len + offset;
                        }
                        break;
                    case StreamConsts.STREAM_SEEK_CUR:
                        if (offset + pos <= len)
                        {
                            DataStream.Position = pos + offset;
                            _virtualPosition = -1;
                        }
                        else
                        {
                            _virtualPosition = offset + pos;
                        }
                        break;
                }

                return _virtualPosition != -1 ? _virtualPosition : DataStream.Position;
            }

            public virtual void SetSize(long value) => DataStream.SetLength(value);

            public virtual void Stat(IntPtr pstatstg, int grfStatFlag) => NotImplemented();

            public virtual void UnlockRegion(long libOffset, long cb, int dwLockType)
            {
            }

            public virtual int Write(IntPtr buf, int length)
            {
                var buffer = new byte[length];
                Marshal.Copy(buf, buffer, 0, length);
                return Write(buffer, length);
            }

            public virtual int Write(byte[] buffer, int length)
            {
                ActualizeVirtualPosition();
                DataStream.Write(buffer, 0, length);
                return length;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal class MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct APPBARDATA
        {
            public int cbSize;
            public IntPtr hWnd;
            public uint uCallbackMessage;
            public uint uEdge;
            public RECT rc;
            public int lParam;
        }
    }
}