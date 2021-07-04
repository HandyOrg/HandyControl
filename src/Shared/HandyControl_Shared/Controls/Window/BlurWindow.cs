using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using HandyControl.Data;
using HandyControl.Tools;
using HandyControl.Tools.Helper;
using HandyControl.Tools.Interop;

namespace HandyControl.Controls
{
    public class BlurWindow : Window
    {
        private static readonly int WM_EXITSIZEMOVE = 0x0232;
        private static readonly int WM_ENTERSIZEMOVE = 0x0231;

        public BlurWindow()
        {
            var versionInfo = SystemHelper.GetSystemVersionInfo();

            if (versionInfo >= SystemVersionInfo.Windows10_1903)
            {
                var source = HwndSource.FromHwnd(WindowHelper.GetHandle(this));
                source.AddHook(WndProc);
            }
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_ENTERSIZEMOVE)
            {
                EnableBlur(this, false);
            }
            else if (msg == WM_EXITSIZEMOVE)
            {
                EnableBlur(this);
            }

            return IntPtr.Zero;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            EnableBlur(this);
        }

        internal static void EnableBlur(Window window, bool isEnabled = true)
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
}
