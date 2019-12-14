using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using HandyControl.Data;

namespace HandyControl.Media.Effects
{
    public class GrayScaleEffect : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty = RegisterPixelShaderSamplerProperty("Input", typeof(GrayScaleEffect), 0);

        private static readonly PixelShader Shader;

        static GrayScaleEffect()
        {
            Shader = new PixelShader
            {
                UriSource = new Uri("pack://application:,,,/HandyControl;component/Resources/Effects/GrayScaleEffect.ps")
            };
        }

        public GrayScaleEffect()
        {
            PixelShader = Shader;

            UpdateShaderValue(InputProperty);
            UpdateShaderValue(ScaleProperty);
        }

        public Brush Input
        {
            get => (Brush)GetValue(InputProperty);
            set => SetValue(InputProperty, value);
        }

        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register(
            "Scale", typeof(double), typeof(GrayScaleEffect), new PropertyMetadata(ValueBoxes.Double1Box, PixelShaderConstantCallback(0)));

        public double Scale
        {
            get => (double) GetValue(ScaleProperty);
            set => SetValue(ScaleProperty, value);
        }
    }
}
