using System;
using System.Runtime.InteropServices;
using HandyControl.Data;
using HandyControl.Tools;
using HandyControl.Tools.Helper;
using HandyControl.Tools.Interop;

namespace HandyControl.Controls;

public class BlurWindow : Window
{
    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);

        var versionInfo = SystemHelper.GetSystemVersionInfo();
        if (versionInfo >= SystemVersionInfo.Windows10_1903)
        {
            this.GetHwndSource()?.AddHook(HwndSourceHook);
        }
    }

    private IntPtr HwndSourceHook(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
    {
        switch (msg)
        {
            case InteropValues.WM_ENTERSIZEMOVE:
                EnableBlur(this, false);
                break;
            case InteropValues.WM_EXITSIZEMOVE:
                EnableBlur(this, true);
                break;
        }

        return IntPtr.Zero;
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        EnableBlur(this, true);
    }

    private static void EnableBlur(Window window, bool isEnabled)
    {
        var versionInfo = SystemHelper.GetSystemVersionInfo();

        var accentPolicy = new InteropValues.ACCENTPOLICY();
        var accentPolicySize = Marshal.SizeOf(accentPolicy);

        accentPolicy.AccentFlags = 2;

        if (isEnabled)
        {
            if (versionInfo >= SystemVersionInfo.Windows10_1809)
            {
                accentPolicy.AccentState = InteropValues.ACCENTSTATE.ACCENT_ENABLE_ACRYLICBLURBEHIND;
            }
            else if (versionInfo >= SystemVersionInfo.Windows10)
            {
                accentPolicy.AccentState = InteropValues.ACCENTSTATE.ACCENT_ENABLE_BLURBEHIND;
            }
            else
            {
                accentPolicy.AccentState = InteropValues.ACCENTSTATE.ACCENT_ENABLE_TRANSPARENTGRADIENT;
            }
        }
        else
        {
            accentPolicy.AccentState = InteropValues.ACCENTSTATE.ACCENT_ENABLE_BLURBEHIND;
        }

        accentPolicy.GradientColor = ResourceHelper.GetResource<uint>(ResourceToken.BlurGradientValue);

        var accentPtr = Marshal.AllocHGlobal(accentPolicySize);
        Marshal.StructureToPtr(accentPolicy, accentPtr, false);

        var data = new InteropValues.WINCOMPATTRDATA
        {
            Attribute = InteropValues.WINDOWCOMPOSITIONATTRIB.WCA_ACCENT_POLICY,
            DataSize = accentPolicySize,
            Data = accentPtr
        };

        InteropMethods.Gdip.SetWindowCompositionAttribute(window.GetHandle(), ref data);

        Marshal.FreeHGlobal(accentPtr);
    }
}
