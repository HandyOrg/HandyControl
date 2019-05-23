using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using HandyControl.Data;
using HandyControl.Tools.Interop;

namespace HandyControl.Tools
{
    internal class KeyboardHook
    {
        public static event EventHandler<KeyboardHookEventArgs> KeyDown;

        public static event EventHandler<KeyboardHookEventArgs> KeyUp;

        private static IntPtr HookId = IntPtr.Zero;

        private static readonly UnsafeNativeMethods.HookProc Proc = HookCallback;

        public static void Start() => HookId = SetHook(Proc);

        public static void Stop() => UnsafeNativeMethods.UnhookWindowsHookEx(HookId);

        private static IntPtr SetHook(UnsafeNativeMethods.HookProc proc)
        {
            using (var curProcess = Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                if (curModule != null)
                {
                    return UnsafeNativeMethods.SetWindowsHookEx((int)UnsafeNativeMethods.HookType.WH_KEYBOARD_LL, proc,
                        UnsafeNativeMethods.GetModuleHandle(curModule.ModuleName), 0);
                }
                return IntPtr.Zero;
            }
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                if (wParam == (IntPtr)NativeMethods.WM_KEYDOWN)
                {
                    var virtualKey = Marshal.ReadInt32(lParam);

                    KeyDown?.Invoke(null, new KeyboardHookEventArgs(virtualKey));
                }
                else if (wParam == (IntPtr)NativeMethods.WM_KEYUP)
                {
                    var virtualKey = Marshal.ReadInt32(lParam);

                    KeyUp?.Invoke(null, new KeyboardHookEventArgs(virtualKey));
                }
            }
            return UnsafeNativeMethods.CallNextHookEx(HookId, nCode, wParam, lParam);
        }
    }
}