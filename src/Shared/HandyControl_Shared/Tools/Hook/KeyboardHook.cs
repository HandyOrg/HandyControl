using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using HandyControl.Data;
using HandyControl.Tools.Interop;

namespace HandyControl.Tools
{
    public class KeyboardHook
    {
        public static event EventHandler<KeyboardHookEventArgs> KeyDown;

        public static event EventHandler<KeyboardHookEventArgs> KeyUp;

        private static IntPtr HookId = IntPtr.Zero;

        private static readonly InteropValues.HookProc Proc = HookCallback;

        private static int VirtualKey;

        private static readonly IntPtr KeyDownIntPtr = (IntPtr)InteropValues.WM_KEYDOWN;

        private static readonly IntPtr KeyUpIntPtr = (IntPtr)InteropValues.WM_KEYUP;

        private static readonly IntPtr SyskeyDownIntPtr = (IntPtr)InteropValues.WM_SYSKEYDOWN;

        private static readonly IntPtr SyskeyUpIntPtr = (IntPtr)InteropValues.WM_SYSKEYUP;

        private static int Count;

        public static void Start()
        {
            if (HookId == IntPtr.Zero)
            {
                HookId = SetHook(Proc);
            }

            if (HookId != IntPtr.Zero)
            {
                Count++;
            }
        }

        public static void Stop()
        {
            Count--;
            if (Count < 1)
            {
                InteropMethods.UnhookWindowsHookEx(HookId);
                HookId = IntPtr.Zero;
            }
        }

        private static IntPtr SetHook(InteropValues.HookProc proc)
        {
            using var curProcess = Process.GetCurrentProcess();
            using var curModule = curProcess.MainModule;
            
            if (curModule != null)
            {
                return InteropMethods.SetWindowsHookEx((int)InteropValues.HookType.WH_KEYBOARD_LL, proc,
                    InteropMethods.GetModuleHandle(curModule.ModuleName), 0);
            }
            return IntPtr.Zero;
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
            return InteropMethods.CallNextHookEx(HookId, nCode, wParam, lParam);
        }
    }
}