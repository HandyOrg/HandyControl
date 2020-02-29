using System;
using System.Windows;
using System.Windows.Media.Effects;
using HandyControl.Data;

namespace HandyControl.Media.Effects
{
    public class BrightnessEffect : EffectBase
    {
        private static readonly PixelShader Shader;

        static BrightnessEffect()
        {
            Shader = new PixelShader
            {
                UriSource = new Uri("pack://application:,,,/HandyControl;component/Resources/Effects/BrightnessEffect.ps")
            };
        }

        public BrightnessEffect()
        {
            PixelShader = Shader;

            UpdateShaderValue(InputProperty);
            UpdateShaderValue(BrightnessProperty);
        }

        public static readonly DependencyProperty BrightnessProperty = DependencyProperty.Register(
            "Brightness", typeof(double), typeof(BrightnessEffect), new PropertyMetadata(ValueBoxes.Double1Box, PixelShaderConstantCallback(0)));

        public double Brightness
        {
            get => (double)GetValue(BrightnessProperty);
            set => SetValue(BrightnessProperty, value);
        }
    }
}
