using System;
using System.Windows.Interop;
using HandyControl.Tools.Interop;

namespace HandyControl.Tools
{
    public class ClipboardHook
    {
        private static HwndSource HWndSource;

        private static IntPtr HookId = IntPtr.Zero;

        public static event Action ContentChanged;

        public static void Start()
        {
            if (HookId != IntPtr.Zero)
            {
                Stop();
            }

            HookId = WindowHelper.CreateHandle();
            HWndSource = HwndSource.FromHwnd(HookId);
            if (HWndSource != null)
            {
                HWndSource.AddHook(WinProc);
                NativeMethods.AddClipboardFormatListener(HookId);
            }
        }

        public static void Stop()
        {
            HWndSource.RemoveHook(WinProc);
            NativeMethods.RemoveClipboardFormatListener(HookId);
        }

        private static IntPtr WinProc(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
        {
            if (msg == NativeMethods.WM_CLIPBOARDUPDATE)
            {
                ContentChanged?.Invoke();
            }
            return IntPtr.Zero;
        }
    }
}
