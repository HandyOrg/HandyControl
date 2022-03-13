using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace HandyControl.Interactivity;

public abstract class TransitionEffect : ShaderEffect
{
    // Fields
    public static readonly DependencyProperty InputProperty =
        RegisterPixelShaderSamplerProperty("Input", typeof(TransitionEffect), 0, SamplingMode.NearestNeighbor);

    public static readonly DependencyProperty OldImageProperty =
        RegisterPixelShaderSamplerProperty("OldImage", typeof(TransitionEffect), 1, SamplingMode.NearestNeighbor);

    public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register("Progress",
        typeof(double), typeof(TransitionEffect), new PropertyMetadata(0.0, PixelShaderConstantCallback(0)));

    // Methods
    protected TransitionEffect()
    {
        UpdateShaderValue(InputProperty);
        UpdateShaderValue(OldImageProperty);
        UpdateShaderValue(ProgressProperty);
    }

    // Properties
    public Brush Input
    {
        get =>
            (Brush) GetValue(InputProperty);
        set => SetValue(InputProperty, value);
    }

    public Brush OldImage
    {
        get =>
            (Brush) GetValue(OldImageProperty);
        set => SetValue(OldImageProperty, value);
    }

    public double Progress
    {
        get =>
            (double) GetValue(ProgressProperty);
        set => SetValue(ProgressProperty, value);
    }

    public new TransitionEffect CloneCurrentValue()
    {
        return (TransitionEffect) base.CloneCurrentValue();
    }

    protected abstract TransitionEffect DeepCopy();
}
