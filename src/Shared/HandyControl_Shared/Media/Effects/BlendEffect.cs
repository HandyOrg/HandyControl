//reference doc : https://docs.microsoft.com/en-us/windows/win32/direct2d/blend#blending-examples

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace HandyControl.Media.Effects
{
    public class BlendEffect : ShaderEffect
    {
        private static readonly Dictionary<BlendEffectMode, PixelShader> ShaderDic;

        public static readonly DependencyProperty BackgroundProperty = RegisterPixelShaderSamplerProperty("Background", typeof(BlendEffect), 0);

        public static readonly DependencyProperty ForegroundProperty = RegisterPixelShaderSamplerProperty("Foreground", typeof(BlendEffect), 1);

        static BlendEffect()
        {
            ShaderDic = new Dictionary<BlendEffectMode, PixelShader>();
        }

        public BlendEffect()
        {
            if (!ShaderDic.ContainsKey(Mode))
            {
                ShaderDic[Mode] = new PixelShader
                {
                    UriSource = new Uri($"pack://application:,,,/HandyControl;component/Resources/Effects/{Mode.ToString()}BlendEffect.ps")
                };
            }

            PixelShader = ShaderDic[Mode];

            UpdateShaderValue(BackgroundProperty);
            UpdateShaderValue(ForegroundProperty);
        }

        public Brush Background
        {
            get => (Brush)GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }

        public Brush Foreground
        {
            get => (Brush)GetValue(ForegroundProperty);
            set => SetValue(ForegroundProperty, value);
        }

        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(
            "Mode", typeof(BlendEffectMode), typeof(BlendEffect), new PropertyMetadata(default(BlendEffectMode), PixelShaderConstantCallback(0)));

        public BlendEffectMode Mode
        {
            get => (BlendEffectMode) GetValue(ModeProperty);
            set => SetValue(ModeProperty, value);
        }
    }
}
