using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace HandyControl.Media.Effects
{
    public class ColorComplementEffect : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty = RegisterPixelShaderSamplerProperty("Input", typeof(ColorComplementEffect), 0);

        private static readonly PixelShader Shader;

        static ColorComplementEffect()
        {
            Shader = new PixelShader
            {
                UriSource = new Uri("pack://application:,,,/HandyControl;component/Resources/Effects/ColorComplementEffect.ps")
            };
        }

        public ColorComplementEffect()
        {
            PixelShader = Shader;
            UpdateShaderValue(InputProperty);
        }

        public Brush Input
        {
            get => (Brush)GetValue(InputProperty);
            set => SetValue(InputProperty, value);
        }
    }
}
