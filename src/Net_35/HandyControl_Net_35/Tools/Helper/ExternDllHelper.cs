using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security.Permissions;
using System.Threading;

namespace HandyControl.Tools
{
    internal class ExternDllHelper
    {
        private const string Gdi32 = "gdi32.dll";

        private const string User32 = "user32.dll";

        // ReSharper disable once InconsistentNaming
        public const int E_FAIL = unchecked((int) 0x80004005);

        [StructLayout(LayoutKind.Sequential)]
        internal struct WINCOMPATTRDATA
        {
            public WINDOWCOMPOSITIONATTRIB Attribute;
            public IntPtr Data;
            public int DataSize;
        }

        internal enum WINDOWCOMPOSITIONATTRIB
        {
            WCA_ACCENT_POLICY = 19
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

        [DllImport(User32)]
        internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WINCOMPATTRDATA data);

        [ReflectionPermission(SecurityAction.Assert, Unrestricted = true), SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static object PtrToStructure(IntPtr lparam, Type cls) => Marshal.PtrToStructure(lparam, cls);

        [DllImport(Gdi32, CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject(IntPtr hObject);

        [ComImport, Guid("0000000C-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IStream
        {
            int Read(
                   [In]
                     IntPtr buf,
                   [In]
                     int len);


            int Write(
                   [In]
                     IntPtr buf,
                   [In]
                     int len);

            [return: MarshalAs(UnmanagedType.I8)]
            long Seek(
                    [In, MarshalAs(UnmanagedType.I8)]
                     long dlibMove,
                    [In]
                     int dwOrigin);


            void SetSize(
                   [In, MarshalAs(UnmanagedType.I8)]
                     long libNewSize);

            [return: MarshalAs(UnmanagedType.I8)]
            long CopyTo(
                    [In, MarshalAs(UnmanagedType.Interface)]
                      IStream pstm,
                    [In, MarshalAs(UnmanagedType.I8)]
                     long cb,
                    [Out, MarshalAs(UnmanagedType.LPArray)]
                     long[] pcbRead);


            void Commit(
                   [In]
                     int grfCommitFlags);


            void Revert();


            void LockRegion(
                   [In, MarshalAs(UnmanagedType.I8)]
                     long libOffset,
                   [In, MarshalAs(UnmanagedType.I8)]
                     long cb,
                   [In]
                     int dwLockType);


            void UnlockRegion(
                   [In, MarshalAs(UnmanagedType.I8)]
                     long libOffset,
                   [In, MarshalAs(UnmanagedType.I8)]
                     long cb,
                   [In]
                     int dwLockType);


            void Stat(
                   [In]
                     IntPtr pStatstg,
                   [In]
                     int grfStatFlag);

            [return: MarshalAs(UnmanagedType.Interface)]
            IStream Clone();
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public class StreamConsts
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

        internal class Gdip
        {
            private const string Gdiplus = "gdiplus.dll";

            private const string ThreadDataSlotName = "system.drawing.threaddata";

            private static IntPtr InitToken;

            private static bool Initialized => InitToken != IntPtr.Zero;

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

            //----------------------------------------------------------------------------------------                                                           
            // Status codes
            //----------------------------------------------------------------------------------------
            internal const int Ok = 0;
            internal const int GenericError = 1;
            internal const int InvalidParameter = 2;
            internal const int OutOfMemory = 3;
            internal const int ObjectBusy = 4;
            internal const int InsufficientBuffer = 5;
            internal const int NotImplemented = 6;
            internal const int Win32Error = 7;
            internal const int WrongState = 8;
            internal const int Aborted = 9;
            internal const int FileNotFound = 10;
            internal const int ValueOverflow = 11;
            internal const int AccessDenied = 12;
            internal const int UnknownImageFormat = 13;
            internal const int FontFamilyNotFound = 14;
            internal const int FontStyleNotFound = 15;
            internal const int NotTrueTypeFont = 16;
            internal const int UnsupportedGdiplusVersion = 17;
            internal const int GdiplusNotInitialized = 18;
            internal const int PropertyNotFound = 19;
            internal const int PropertyNotSupported = 20;

            [DllImport(Gdiplus, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
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

            [DllImport(Gdiplus, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
            [ResourceExposure(ResourceScope.None)]
            internal static extern int GdipImageGetFrameDimensionsList(HandleRef image, IntPtr buffer, int count);

            [DllImport(Gdiplus, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
            [ResourceExposure(ResourceScope.None)]
            internal static extern int GdipImageGetFrameCount(HandleRef image, ref Guid dimensionId, int[] count);

            [DllImport(Gdiplus, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
            [ResourceExposure(ResourceScope.None)]
            internal static extern int GdipGetPropertyItemSize(HandleRef image, int propid, out int size);

            [DllImport(Gdiplus, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
            [ResourceExposure(ResourceScope.None)]
            internal static extern int GdipGetPropertyItem(HandleRef image, int propid, int size, IntPtr buffer);

            [DllImport(Gdiplus, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
            [ResourceExposure(ResourceScope.Machine)]
            internal static extern int GdipCreateHBITMAPFromBitmap(HandleRef nativeBitmap, out IntPtr hbitmap, int argbBackground);

            [DllImport(Gdiplus, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
            [ResourceExposure(ResourceScope.None)]
            internal static extern int GdipImageSelectActiveFrame(HandleRef image, ref Guid dimensionId, int frameIndex);

            [DllImport(Gdiplus, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
            [ResourceExposure(ResourceScope.Machine)]
            internal static extern int GdipCreateBitmapFromFile(string filename, out IntPtr bitmap);

            [DllImport(Gdiplus, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
            [ResourceExposure(ResourceScope.None)]
            internal static extern int GdipImageForceValidation(HandleRef image);

            [DllImport(Gdiplus, SetLastError = true, ExactSpelling = true, EntryPoint = "GdipDisposeImage", CharSet = CharSet.Unicode)]
            [ResourceExposure(ResourceScope.None)]
            private static extern int IntGdipDisposeImage(HandleRef image);

            internal static int GdipDisposeImage(HandleRef image)
            {
                if (!Initialized) return Ok;
                var result = IntGdipDisposeImage(image);
                return result;
            }

            [DllImport(Gdiplus, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
            [ResourceExposure(ResourceScope.Process)]
            private static extern int GdiplusStartup(out IntPtr token, ref StartupInput input, out StartupOutput output);

            [DllImport(Gdiplus, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
            [ResourceExposure(ResourceScope.None)]
            internal static extern int GdipGetImageRawFormat(HandleRef image, ref Guid format);

            [DllImport(Gdiplus, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
            [ResourceExposure(ResourceScope.Machine)]
            internal static extern int GdipCreateBitmapFromStream(IStream stream, out IntPtr bitmap);
        }
    }
}