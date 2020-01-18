using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using HandyControl.Data;
using HandyControl.Tools;

namespace HandyControl.Controls
{
    public class BlurWindow : Window
    {
        static BlurWindow()
        {
            StyleProperty.OverrideMetadata(typeof(BlurWindow), new FrameworkPropertyMetadata(Application.Current.FindResource(ResourceToken.WindowBlur)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            EnableBlur(this);
        }

        public static SystemVersionInfo SystemVersionInfo { get; set; }

        internal static void EnableBlur(Window window)
        {
            if (SystemVersionInfo < SystemVersionInfo.Windows10 ||
                SystemVersionInfo >= SystemVersionInfo.Windows10_1903)
            {
                var colorValue = ResourceHelper.GetResource<uint>(ResourceToken.BlurGradientValue);
                var color = ColorHelper.ToColor(colorValue);
                color = Color.FromRgb(color.R, color.G, color.B);
                window.Background = new SolidColorBrush(color);
                return;
            }

            var accentPolicy = new ExternDllHelper.ACCENTPOLICY();
            var accentPolicySize = Marshal.SizeOf(accentPolicy);

            accentPolicy.AccentState = SystemVersionInfo < SystemVersionInfo.Windows10_1809
                ? ExternDllHelper.ACCENTSTATE.ACCENT_ENABLE_BLURBEHIND
                : ExternDllHelper.ACCENTSTATE.ACCENT_ENABLE_ACRYLICBLURBEHIND;

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

            ExternDllHelper.SetWindowCompositionAttribute(window.GetHandle(), ref data);

            Marshal.FreeHGlobal(accentPtr);
        }
    }
}