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

        private static readonly InteropValues.HookProc Proc = HookCallback;

        public static void Start()
        {
            if (HookId != IntPtr.Zero)
            {
                Stop();
            }
            HookId = SetHook(Proc);
        }

        public static void Stop() => InteropMethods.UnhookWindowsHookEx(HookId);

        private static IntPtr SetHook(InteropValues.HookProc proc)
        {
            using (var curProcess = Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                if (curModule != null)
                {
                    return InteropMethods.SetWindowsHookEx((int)InteropValues.HookType.WH_MOUSE_LL, proc,
                        InteropMethods.GetModuleHandle(curModule.ModuleName), 0);
                }
                return IntPtr.Zero;
            }
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0) return InteropMethods.CallNextHookEx(HookId, nCode, wParam, lParam);
            var hookStruct = (InteropValues.MOUSEHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(InteropValues.MOUSEHOOKSTRUCT));
            StatusChanged?.Invoke(null, new MouseHookEventArgs
            {
                Message = (MouseHookMessageType)wParam,
                Point = new InteropValues.POINT(hookStruct.pt.X, hookStruct.pt.Y)
            });

            return InteropMethods.CallNextHookEx(HookId, nCode, wParam, lParam);
        }
    }
}
