using System.Runtime.InteropServices;
using System.Windows.Media;
using HandyControl.Data;
using HandyControl.Tools;
using HandyControl.Tools.Interop;

namespace HandyControl.Controls
{
    public class BlurWindow : Window
    {
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            EnableBlur(this);
        }

        internal static void EnableBlur(Window window)
        {
            var versionInfo = ConfigHelper.Instance.SystemVersionInfo;

            if (versionInfo < SystemVersionInfo.Windows10 ||
                versionInfo >= SystemVersionInfo.Windows10_1903)
            {
                var colorValue = ResourceHelper.GetResource<uint>(ResourceToken.BlurGradientValue);
                var color = ColorHelper.ToColor(colorValue);
                color = Color.FromRgb(color.R, color.G, color.B);
                window.Background = new SolidColorBrush(color);
                return;
            }

            var accentPolicy = new InteropValues.ACCENTPOLICY();
            var accentPolicySize = Marshal.SizeOf(accentPolicy);

            accentPolicy.AccentState = versionInfo < SystemVersionInfo.Windows10_1809
                ? InteropValues.ACCENTSTATE.ACCENT_ENABLE_BLURBEHIND
                : InteropValues.ACCENTSTATE.ACCENT_ENABLE_ACRYLICBLURBEHIND;

            accentPolicy.AccentFlags = 2;
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