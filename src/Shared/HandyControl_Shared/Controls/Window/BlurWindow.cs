using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Media;
using HandyControl.Data;
using HandyControl.Tools;

namespace HandyControl.Controls
{
    public class BlurWindow : Window
    {
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            EnableBlur(this);
        }

        public static SystemVersionInfo SystemVersionInfo { get; set; }

        internal static void EnableBlur(Window window)
        {
            var accentPolicy = new ExternDllHelper.ACCENTPOLICY();
            var accentPolicySize = Marshal.SizeOf(accentPolicy);

            if (SystemVersionInfo >= SystemVersionInfo.Windows10_1809)
            {
                accentPolicy.AccentState = ExternDllHelper.ACCENTSTATE.ACCENT_ENABLE_ACRYLICBLURBEHIND;
            }
            else if (SystemVersionInfo >= SystemVersionInfo.Windows10)
            {
                accentPolicy.AccentState = ExternDllHelper.ACCENTSTATE.ACCENT_ENABLE_BLURBEHIND;
            }
            else
            {
                var colorValue = ResourceHelper.GetResource<uint>(ResourceToken.BlurGradientValue);
                var color = ColorHelper.ToColor(colorValue);
                color = Color.FromRgb(color.R, color.G, color.B);
                window.Background = new SolidColorBrush(color);
                return;
            }

            accentPolicy.AccentFlags = 2;
            accentPolicy.GradientColor = ResourceHelper.GetResource<uint>(ResourceToken.BlurGradientValue);

            var accentPtr = Marshal.AllocHGlobal(accentPolicySize);
            Marshal.StructureToPtr(accentPolicy, accentPtr, false);

            var data = new ExternDllHelper.WINCOMPATTRDATA
            {
                Attribute = ExternDllHelper.WINDOWCOMPOSITIONATTRIB.WCA_ACCENT_POLICY,
                DataSize = accentPolicySize,
                Data = accentPtr
            };

            ExternDllHelper.SetWindowCompositionAttribute(new WindowInteropHelper(window).Handle, ref data);

            Marshal.FreeHGlobal(accentPtr);
        }
    }
}