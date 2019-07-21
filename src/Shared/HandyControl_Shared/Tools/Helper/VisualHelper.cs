using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Media;
using HandyControl.Tools.Interop;
using HandyControl.Tools.Extension;

namespace HandyControl.Tools
{
    public class VisualHelper
    {
        internal static VisualStateGroup TryGetVisualStateGroup(DependencyObject d, string groupName)
        {
            var root = GetImplementationRoot(d);
            if (root == null)
            {
                return null;
            }

            return VisualStateManager
                .GetVisualStateGroups(root)?
                .OfType<VisualStateGroup>()
                .FirstOrDefault(group => string.CompareOrdinal(groupName, group.Name) == 0);
        }

        internal static FrameworkElement GetImplementationRoot(DependencyObject d)
        {
            return 1 == VisualTreeHelper.GetChildrenCount(d)
                ? VisualTreeHelper.GetChild(d, 0) as FrameworkElement
                : null;
        }

        internal static T GetChild<T>(DependencyObject d) where T : DependencyObject
        {
            if (d is T t)
            {
                return t;
            }

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(d); i++)
            {
                var child = VisualTreeHelper.GetChild(d, i);

                var result = GetChild<T>(child);
                if (result != null) return result;
            }

            return default;
        }

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
    }
}