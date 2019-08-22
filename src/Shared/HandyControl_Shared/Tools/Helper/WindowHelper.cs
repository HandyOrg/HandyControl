﻿using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Interop;
using HandyControl.Tools.Extension;
using HandyControl.Tools.Interop;

namespace HandyControl.Tools
{
    public static class WindowHelper
    {
        /// <summary>
        ///     获取当前应用中处于激活的一个窗口
        /// </summary>
        /// <returns></returns>
        public static Window GetActiveWindow() => Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);

        private static readonly BitArray _cacheValid = new BitArray((int)CacheSlot.NumSlots);

        private static bool _setDpiX = true;

        private static bool _dpiInitialized;

        private static readonly object _dpiLock = new object();

        private static int _dpi;

        internal static int Dpi
        {
            [SecurityCritical, SecuritySafeCritical]
            get
            {
                if (!_dpiInitialized)
                {
                    lock (_dpiLock)
                    {
                        if (!_dpiInitialized)
                        {
                            var desktopWnd = new HandleRef(null, IntPtr.Zero);

                            // Win32Exception will get the Win32 error code so we don't have to
                            var dc = UnsafeNativeMethods.GetDC(desktopWnd);

                            // Detecting error case from unmanaged call, required by PREsharp to throw a Win32Exception
                            if (dc == IntPtr.Zero)
                            {
                                throw new Win32Exception();
                            }

                            try
                            {
                                _dpi = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(null, dc), NativeMethods.LOGPIXELSY);
                                _dpiInitialized = true;
                            }
                            finally
                            {
                                UnsafeNativeMethods.ReleaseDC(desktopWnd, new HandleRef(null, dc));
                            }
                        }
                    }
                }
                return _dpi;
            }
        }

        private static int _dpiX;

        internal static int DpiX
        {
            [SecurityCritical, SecuritySafeCritical]
            get
            {
                if (_setDpiX)
                {
                    lock (_cacheValid)
                    {
                        if (_setDpiX)
                        {
                            _setDpiX = false;
                            var desktopWnd = new HandleRef(null, IntPtr.Zero);
                            var dc = UnsafeNativeMethods.GetDC(desktopWnd);
                            if (dc == IntPtr.Zero)
                            {
                                throw new Win32Exception();
                            }
                            try
                            {
                                _dpiX = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(null, dc), NativeMethods.LOGPIXELSX);
                                _cacheValid[(int)CacheSlot.DpiX] = true;
                            }
                            finally
                            {
                                UnsafeNativeMethods.ReleaseDC(desktopWnd, new HandleRef(null, dc));
                            }
                        }
                    }
                }

                return _dpiX;
            }
        }

        private static Thickness _windowResizeBorderThickness;

        internal static Thickness WindowResizeBorderThickness
        {
            [SecurityCritical]
            get
            {
                lock (_cacheValid)
                {
                    while (!_cacheValid[(int)CacheSlot.WindowResizeBorderThickness])
                    {
                        _cacheValid[(int)CacheSlot.WindowResizeBorderThickness] = true;

                        var frameSize = new Size(NativeMethods.GetSystemMetrics(SM.CXSIZEFRAME), NativeMethods.GetSystemMetrics(SM.CYSIZEFRAME));
                        var frameSizeInDips = DpiHelper.DeviceSizeToLogical(frameSize, DpiX / 96.0, Dpi / 96.0);

                        _windowResizeBorderThickness = new Thickness(frameSizeInDips.Width, frameSizeInDips.Height, frameSizeInDips.Width, frameSizeInDips.Height);
                    }
                }

                return _windowResizeBorderThickness;
            }
        }

        public static Thickness WindowMaximizedPadding
        {
            get
            {
#if netle40
                return WindowResizeBorderThickness;
#elif Core
                var hdc = UnsafeNativeMethods.GetDC(IntPtr.Zero);
                var scale = UnsafeNativeMethods.GetDeviceCaps(hdc, NativeMethods.DESKTOPVERTRES) / (float)UnsafeNativeMethods.GetDeviceCaps(hdc, NativeMethods.VERTRES);
                UnsafeNativeMethods.ReleaseDC(IntPtr.Zero, hdc);
                return WindowResizeBorderThickness.Add(new Thickness(4 * scale));
#else
                return WindowResizeBorderThickness.Add(new Thickness(4));
#endif
            }
        }

        public static IntPtr CreateHandle() => new WindowInteropHelper(new Window()).EnsureHandle();

        public static IntPtr GetHandle(this Window window) => new WindowInteropHelper(window).EnsureHandle();
    }
}
