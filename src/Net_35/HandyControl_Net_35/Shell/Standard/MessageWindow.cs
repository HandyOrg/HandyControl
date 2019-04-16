namespace Standard
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Threading;

    internal sealed class MessageWindow : DispatcherObject, IDisposable
    {
        private string _className;
        private bool _isDisposed;
        private Standard.WndProc _wndProcCallback;
        private static readonly Dictionary<IntPtr, Standard.MessageWindow> s_windowLookup = new Dictionary<IntPtr, Standard.MessageWindow>();
        private static readonly Standard.WndProc s_WndProc = new Standard.WndProc(Standard.MessageWindow._WndProc);

        [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        public MessageWindow(Standard.CS classStyle, Standard.WS style, Standard.WS_EX exStyle, Rect location, string name, Standard.WndProc callback)
        {
            this._wndProcCallback = callback;
            this._className = "MessageWindowClass+" + Guid.NewGuid().ToString();
            Standard.WNDCLASSEX lpwcx = new Standard.WNDCLASSEX {
                cbSize = Marshal.SizeOf(typeof(Standard.WNDCLASSEX)),
                style = classStyle,
                lpfnWndProc = s_WndProc,
                hInstance = Standard.NativeMethods.GetModuleHandle(null),
                hbrBackground = Standard.NativeMethods.GetStockObject(Standard.StockObject.NULL_BRUSH),
                lpszMenuName = "",
                lpszClassName = this._className
            };
            Standard.NativeMethods.RegisterClassEx(ref lpwcx);
            GCHandle handle = new GCHandle();
            try
            {
                handle = GCHandle.Alloc(this);
                IntPtr lpParam = (IntPtr) handle;
                this.Handle = Standard.NativeMethods.CreateWindowEx(exStyle, this._className, name, style, (int) location.X, (int) location.Y, (int) location.Width, (int) location.Height, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, lpParam);
            }
            finally
            {
                handle.Free();
            }
        }

        private static object _DestroyWindow(IntPtr hwnd, string className)
        {
            Standard.Utility.SafeDestroyWindow(ref hwnd);
            Standard.NativeMethods.UnregisterClass(className, Standard.NativeMethods.GetModuleHandle(null));
            return null;
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId="disposing")]
        private void _Dispose(bool disposing, bool isHwndBeingDestroyed)
        {
            DispatcherOperationCallback method = null;
            DispatcherOperationCallback callback2 = null;
            IntPtr hwnd;
            string className;
            if (!this._isDisposed)
            {
                this._isDisposed = true;
                hwnd = this.Handle;
                className = this._className;
                if (isHwndBeingDestroyed)
                {
                    if (method == null)
                    {
                        method = arg => _DestroyWindow(IntPtr.Zero, className);
                    }
                    base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, method);
                }
                else if (this.Handle != IntPtr.Zero)
                {
                    if (base.CheckAccess())
                    {
                        _DestroyWindow(hwnd, className);
                    }
                    else
                    {
                        if (callback2 == null)
                        {
                            callback2 = arg => _DestroyWindow(hwnd, className);
                        }
                        base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, callback2);
                    }
                }
                s_windowLookup.Remove(hwnd);
                this._className = null;
                this.Handle = IntPtr.Zero;
            }
        }

        [SuppressMessage("Microsoft.Usage", "CA1816:CallGCSuppressFinalizeCorrectly")]
        private static IntPtr _WndProc(IntPtr hwnd, Standard.WM msg, IntPtr wParam, IntPtr lParam)
        {
            IntPtr zero = IntPtr.Zero;
            Standard.MessageWindow target = null;
            if (msg == Standard.WM.CREATE)
            {
                Standard.CREATESTRUCT createstruct = (Standard.CREATESTRUCT) Marshal.PtrToStructure(lParam, typeof(Standard.CREATESTRUCT));
                target = (Standard.MessageWindow) GCHandle.FromIntPtr(createstruct.lpCreateParams).Target;
                s_windowLookup.Add(hwnd, target);
            }
            else if (!s_windowLookup.TryGetValue(hwnd, out target))
            {
                return Standard.NativeMethods.DefWindowProc(hwnd, msg, wParam, lParam);
            }
            Standard.WndProc proc = target._wndProcCallback;
            if (proc != null)
            {
                zero = proc(hwnd, msg, wParam, lParam);
            }
            else
            {
                zero = Standard.NativeMethods.DefWindowProc(hwnd, msg, wParam, lParam);
            }
            if (msg == Standard.WM.NCDESTROY)
            {
                target._Dispose(true, true);
                GC.SuppressFinalize(target);
            }
            return zero;
        }

        public void Dispose()
        {
            this._Dispose(true, false);
            GC.SuppressFinalize(this);
        }

        ~MessageWindow()
        {
            this._Dispose(false, false);
        }

        public IntPtr Handle { get; private set; }
    }
}

