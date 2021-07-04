using System;
using System.Runtime.InteropServices;
using HandyControl.Data;
using HandyControl.Tools;
using HandyControl.Tools.Helper;
using HandyControl.Tools.Interop;
using Microsoft.Win32;

namespace HandyControl.Controls
{
    public class BlurWindow : Window
    {
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            EnableBlur(this);
        }

        internal static bool IsTransparencyEnabled()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize"))
            {
                if (key != null)
                {
                    var value = key.GetValue("EnableTransparency");
                    if (value != null)
                    {
                        return Convert.ToBoolean(value);
                    }
                }
            }
            return true;
        }

        internal static void EnableBlur(Window window)
        {
            var versionInfo = SystemHelper.GetSystemVersionInfo();

            var accentPolicy = new InteropValues.ACCENTPOLICY();
            var accentPolicySize = Marshal.SizeOf(accentPolicy);

            accentPolicy.AccentFlags = 2;

            if (versionInfo >= SystemVersionInfo.Windows10_1903 && !IsTransparencyEnabled())
            {
                accentPolicy.AccentState = InteropValues.ACCENTSTATE.ACCENT_ENABLE_ACRYLICBLURBEHIND;
            }
            else if (versionInfo >= SystemVersionInfo.Windows10_1903 && IsTransparencyEnabled())
            {
                accentPolicy.AccentState = InteropValues.ACCENTSTATE.ACCENT_ENABLE_BLURBEHIND;
            }
            else if (versionInfo >= SystemVersionInfo.Windows10_1809)
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
