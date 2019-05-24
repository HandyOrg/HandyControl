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

        private static int VirtualKey;

        private static readonly IntPtr KeyDownIntPtr = (IntPtr)NativeMethods.WM_KEYDOWN;

        private static readonly IntPtr KeyUpIntPtr = (IntPtr)NativeMethods.WM_KEYUP;

        private static readonly IntPtr SyskeyDownIntPtr = (IntPtr)NativeMethods.WM_SYSKEYDOWN;

        private static readonly IntPtr SyskeyUpIntPtr = (IntPtr)NativeMethods.WM_SYSKEYUP;

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
                if (wParam == KeyDownIntPtr)
                {
                    var virtualKey = Marshal.ReadInt32(lParam);
                    if (VirtualKey != virtualKey)
                    {
                        VirtualKey = virtualKey;
                        KeyDown?.Invoke(null, new KeyboardHookEventArgs(virtualKey, false));
                    }
                }
                else if (wParam == SyskeyDownIntPtr)
                {
                    var virtualKey = Marshal.ReadInt32(lParam);
                    if (VirtualKey != virtualKey)
                    {
                        VirtualKey = virtualKey;
                        KeyDown?.Invoke(null, new KeyboardHookEventArgs(virtualKey, true));
                    }
                }
                else if (wParam == KeyUpIntPtr)
                {
                    var virtualKey = Marshal.ReadInt32(lParam);
                    VirtualKey = -1;
                    KeyUp?.Invoke(null, new KeyboardHookEventArgs(virtualKey, false));
                }
                else if (wParam == SyskeyUpIntPtr)
                {
                    var virtualKey = Marshal.ReadInt32(lParam);
                    VirtualKey = -1;
                    KeyUp?.Invoke(null, new KeyboardHookEventArgs(virtualKey, true));
                }
            }
            return UnsafeNativeMethods.CallNextHookEx(HookId, nCode, wParam, lParam);
        }
    }
}