using System;
using System.Windows.Interop;
using HandyControl.Tools.Interop;

namespace HandyControl.Tools;

public class ClipboardHook
{
    public static event Action ContentChanged;

    private static HwndSource HWndSource;

    private static IntPtr HookId = IntPtr.Zero;

    private static int Count;

    public static void Start()
    {
        if (HookId == IntPtr.Zero)
        {
            HookId = WindowHelper.CreateHandle();
            HWndSource = HwndSource.FromHwnd(HookId);
            if (HWndSource != null)
            {
                HWndSource.AddHook(WinProc);
                InteropMethods.AddClipboardFormatListener(HookId);
            }
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
            HWndSource.RemoveHook(WinProc);
            InteropMethods.RemoveClipboardFormatListener(HookId);

            HookId = IntPtr.Zero;
        }
    }

    private static IntPtr WinProc(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
    {
        if (msg == InteropValues.WM_CLIPBOARDUPDATE)
        {
            ContentChanged?.Invoke();
        }
        return IntPtr.Zero;
    }
}
