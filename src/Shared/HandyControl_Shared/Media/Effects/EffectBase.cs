using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace HandyControl.Media.Effects
{
    public abstract class EffectBase : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty = RegisterPixelShaderSamplerProperty("Input", typeof(EffectBase), 0);

        public Brush Input
        {
            get => (Brush)GetValue(InputProperty);
            set => SetValue(InputProperty, value);
        }
    }
}
