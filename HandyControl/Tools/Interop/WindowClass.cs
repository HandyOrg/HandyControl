using System;
using System.Security.Permissions;

namespace HandyControl.Tools.Interop
{
    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    internal class WindowClass
    {
        internal string WindowClassName;

        private static readonly int WmTaskbarcreated = NativeMethods.RegisterWindowMessage("TaskbarCreated");

        internal IntPtr MessageWindowHandle { get; private set; }

        private readonly object _syncObj = new object();

        private bool _added;

        public WindowClass()
        {
            RegisterClass();
        }

        private void RegisterClass()
        {
            WindowClassName = $"HandyControl.Controls.NotifyIcon{Guid.NewGuid()}";
            var wndclass = new WNDCLASS
            {
                style = 0,
                lpfnWndProc = Callback,
                cbClsExtra = 0,
                cbWndExtra = 0,
                hInstance = IntPtr.Zero,
                hIcon = IntPtr.Zero,
                hCursor = IntPtr.Zero,
                hbrBackground = IntPtr.Zero,
                lpszMenuName = string.Empty,
                lpszClassName = WindowClassName
            };

            UnsafeNativeMethods.RegisterClass(wndclass);
            MessageWindowHandle = UnsafeNativeMethods.CreateWindowEx(0, WindowClassName, "", 0, 0, 0, 1, 1, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
        }

        public IntPtr Callback(IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam)
        {
            if (msg == WmTaskbarcreated)
            {
                _added = false;
                AddIcon();
            }
            return IntPtr.Zero;
        }

        private void AddIcon()
        {
            lock (_syncObj)
            {
                if (DesignerHelper.IsInDesignMode) return;

                //NativeMethods.NOTIFYICONDATA data = new NativeMethods.NOTIFYICONDATA();
                //data.uCallbackMessage = WM_TRAYMOUSEMESSAGE;
                //data.uFlags = NativeMethods.NIF_MESSAGE;
                //if (showIconInTray)
                //{
                //    if (window.Handle == IntPtr.Zero)
                //    {
                //        window.CreateHandle(new CreateParams());
                //    }
                //}
                //data.hWnd = window.Handle;
                //data.uID = id;
                //data.hIcon = IntPtr.Zero;
                //data.szTip = null;
                //if (icon != null)
                //{
                //    data.uFlags |= NativeMethods.NIF_ICON;
                //    data.hIcon = icon.Handle;
                //}
                //data.uFlags |= NativeMethods.NIF_TIP;
                //data.szTip = text;

                //if (showIconInTray && icon != null)
                //{
                //    if (!added)
                //    {
                //        UnsafeNativeMethods.Shell_NotifyIcon(NativeMethods.NIM_ADD, data);
                //        added = true;
                //    }
                //    else
                //    {
                //        UnsafeNativeMethods.Shell_NotifyIcon(NativeMethods.NIM_MODIFY, data);
                //    }
                //}
                //else if (added)
                //{
                //    UnsafeNativeMethods.Shell_NotifyIcon(NativeMethods.NIM_DELETE, data);
                //    added = false;
                //}
            }
        }
    }
}