using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using HandyControl.Data;
using HandyControl.Tools.Interop;

namespace HandyControl.Tools
{
    internal class MouseHook
    {
        public static event EventHandler<MouseHookEventArgs> StatusChanged;

        private static IntPtr HookId = IntPtr.Zero;

        private static readonly UnsafeNativeMethods.HookProc Proc = HookCallback;

        public static void Start()
        {
            if (HookId != IntPtr.Zero)
            {
                Stop();
            }
            HookId = SetHook(Proc);
        }

        public static void Stop() => UnsafeNativeMethods.UnhookWindowsHookEx(HookId);

        private static IntPtr SetHook(UnsafeNativeMethods.HookProc proc)
        {
            using (var curProcess = Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                if (curModule != null)
                {
                    return UnsafeNativeMethods.SetWindowsHookEx((int) UnsafeNativeMethods.HookType.WH_MOUSE_LL, proc,
                        UnsafeNativeMethods.GetModuleHandle(curModule.ModuleName), 0);
                }
                return IntPtr.Zero;
            }
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0) return UnsafeNativeMethods.CallNextHookEx(HookId, nCode, wParam, lParam);
            var hookStruct = (UnsafeNativeMethods.MOUSEHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(UnsafeNativeMethods.MOUSEHOOKSTRUCT));
            StatusChanged?.Invoke(null, new MouseHookEventArgs
            {
                Message = (MouseHookMessageType)wParam,
                Point = new NativeMethods.POINT(hookStruct.pt.X, hookStruct.pt.Y)
            });

            return UnsafeNativeMethods.CallNextHookEx(HookId, nCode, wParam, lParam);
        }
    }
}
