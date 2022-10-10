using System;
using System.Windows;
using System.Windows.Media.Effects;
using HandyControl.Data;

namespace HandyControl.Media.Effects;

public class ColorMatrixEffect : EffectBase
{
    private static readonly PixelShader Shader;

    static ColorMatrixEffect()
    {
        Shader = new PixelShader
        {
            UriSource = new Uri("pack://application:,,,/HandyControl;component/Resources/Effects/ColorMatrixEffect.ps")
        };
    }

    public ColorMatrixEffect()
    {
        PixelShader = Shader;

        UpdateShaderValue(InputProperty);

        UpdateShaderValue(M11Property);
        UpdateShaderValue(M21Property);
        UpdateShaderValue(M31Property);
        UpdateShaderValue(M41Property);
        UpdateShaderValue(M51Property);

        UpdateShaderValue(M12Property);
        UpdateShaderValue(M22Property);
        UpdateShaderValue(M32Property);
        UpdateShaderValue(M42Property);
        UpdateShaderValue(M52Property);

        UpdateShaderValue(M13Property);
        UpdateShaderValue(M23Property);
        UpdateShaderValue(M33Property);
        UpdateShaderValue(M43Property);
        UpdateShaderValue(M53Property);

        UpdateShaderValue(M14Property);
        UpdateShaderValue(M24Property);
        UpdateShaderValue(M34Property);
        UpdateShaderValue(M44Property);
        UpdateShaderValue(M54Property);
    }

    #region Line1

    public static readonly DependencyProperty M11Property = DependencyProperty.Register(
        nameof(M11), typeof(double), typeof(ColorMatrixEffect), new PropertyMetadata(ValueBoxes.Double1Box, PixelShaderConstantCallback(0)));

    public double M11
    {
        get => (double) GetValue(M11Property);
        set => SetValue(M11Property, value);
    }

    public static readonly DependencyProperty M21Property = DependencyProperty.Register(
        nameof(M21), typeof(double), typeof(ColorMatrixEffect), new PropertyMetadata(ValueBoxes.Double0Box, PixelShaderConstantCallback(1)));

    public double M21
    {
        get => (double) GetValue(M21Property);
        set => SetValue(M21Property, value);
    }

    public static readonly DependencyProperty M31Property = DependencyProperty.Register(
        nameof(M31), typeof(double), typeof(ColorMatrixEffect), new PropertyMetadata(ValueBoxes.Double0Box, PixelShaderConstantCallback(2)));

    public double M31
    {
        get => (double) GetValue(M31Property);
        set => SetValue(M31Property, value);
    }

    public static readonly DependencyProperty M41Property = DependencyProperty.Register(
        nameof(M41), typeof(double), typeof(ColorMatrixEffect), new PropertyMetadata(ValueBoxes.Double0Box, PixelShaderConstantCallback(3)));

    public double M41
    {
        get => (double) GetValue(M41Property);
        set => SetValue(M41Property, value);
    }

    public static readonly DependencyProperty M51Property = DependencyProperty.Register(
        nameof(M51), typeof(double), typeof(ColorMatrixEffect), new PropertyMetadata(ValueBoxes.Double0Box, PixelShaderConstantCallback(4)));

    public double M51
    {
        get => (double) GetValue(M51Property);
        set => SetValue(M51Property, value);
    }

    #endregion

    #region Line2

    public static readonly DependencyProperty M12Property = DependencyProperty.Register(
        nameof(M12), typeof(double), typeof(ColorMatrixEffect), new PropertyMetadata(ValueBoxes.Double0Box, PixelShaderConstantCallback(5)));

    public double M12
    {
        get => (double) GetValue(M12Property);
        set => SetValue(M12Property, value);
    }

    public static readonly DependencyProperty M22Property = DependencyProperty.Register(
        nameof(M22), typeof(double), typeof(ColorMatrixEffect), new PropertyMetadata(ValueBoxes.Double1Box, PixelShaderConstantCallback(6)));

    public double M22
    {
        get => (double) GetValue(M22Property);
        set => SetValue(M22Property, value);
    }

    public static readonly DependencyProperty M32Property = DependencyProperty.Register(
        nameof(M32), typeof(double), typeof(ColorMatrixEffect), new PropertyMetadata(ValueBoxes.Double0Box, PixelShaderConstantCallback(7)));

    public double M32
    {
        get => (double) GetValue(M32Property);
        set => SetValue(M32Property, value);
    }

    public static readonly DependencyProperty M42Property = DependencyProperty.Register(
        nameof(M42), typeof(double), typeof(ColorMatrixEffect), new PropertyMetadata(ValueBoxes.Double0Box, PixelShaderConstantCallback(8)));

    public double M42
    {
        get => (double) GetValue(M42Property);
        set => SetValue(M42Property, value);
    }

    public static readonly DependencyProperty M52Property = DependencyProperty.Register(
        nameof(M52), typeof(double), typeof(ColorMatrixEffect), new PropertyMetadata(ValueBoxes.Double0Box, PixelShaderConstantCallback(9)));

    public double M52
    {
        get => (double) GetValue(M52Property);
        set => SetValue(M52Property, value);
    }

    #endregion

    #region Line3

    public static readonly DependencyProperty M13Property = DependencyProperty.Register(
        nameof(M13), typeof(double), typeof(ColorMatrixEffect), new PropertyMetadata(ValueBoxes.Double0Box, PixelShaderConstantCallback(10)));

    public double M13
    {
        get => (double) GetValue(M13Property);
        set => SetValue(M13Property, value);
    }

    public static readonly DependencyProperty M23Property = DependencyProperty.Register(
        nameof(M23), typeof(double), typeof(ColorMatrixEffect), new PropertyMetadata(ValueBoxes.Double0Box, PixelShaderConstantCallback(11)));

    public double M23
    {
        get => (double) GetValue(M23Property);
        set => SetValue(M23Property, value);
    }

    public static readonly DependencyProperty M33Property = DependencyProperty.Register(
        nameof(M33), typeof(double), typeof(ColorMatrixEffect), new PropertyMetadata(ValueBoxes.Double1Box, PixelShaderConstantCallback(12)));

    public double M33
    {
        get => (double) GetValue(M33Property);
        set => SetValue(M33Property, value);
    }

    public static readonly DependencyProperty M43Property = DependencyProperty.Register(
        nameof(M43), typeof(double), typeof(ColorMatrixEffect), new PropertyMetadata(ValueBoxes.Double0Box, PixelShaderConstantCallback(13)));

    public double M43
    {
        get => (double) GetValue(M43Property);
        set => SetValue(M43Property, value);
    }

    public static readonly DependencyProperty M53Property = DependencyProperty.Register(
        nameof(M53), typeof(double), typeof(ColorMatrixEffect), new PropertyMetadata(ValueBoxes.Double0Box, PixelShaderConstantCallback(14)));

    public double M53
    {
        get => (double) GetValue(M53Property);
        set => SetValue(M53Property, value);
    }

    #endregion

    #region Line4

    public static readonly DependencyProperty M14Property = DependencyProperty.Register(
        nameof(M14), typeof(double), typeof(ColorMatrixEffect), new PropertyMetadata(ValueBoxes.Double0Box, PixelShaderConstantCallback(15)));

    public double M14
    {
        get => (double) GetValue(M14Property);
        set => SetValue(M14Property, value);
    }

    public static readonly DependencyProperty M24Property = DependencyProperty.Register(
        nameof(M24), typeof(double), typeof(ColorMatrixEffect), new PropertyMetadata(ValueBoxes.Double0Box, PixelShaderConstantCallback(16)));

    public double M24
    {
        get => (double) GetValue(M24Property);
        set => SetValue(M24Property, value);
    }

    public static readonly DependencyProperty M34Property = DependencyProperty.Register(
        nameof(M34), typeof(double), typeof(ColorMatrixEffect), new PropertyMetadata(ValueBoxes.Double0Box, PixelShaderConstantCallback(17)));

    public double M34
    {
        get => (double) GetValue(M34Property);
        set => SetValue(M34Property, value);
    }

    public static readonly DependencyProperty M44Property = DependencyProperty.Register(
        nameof(M44), typeof(double), typeof(ColorMatrixEffect), new PropertyMetadata(ValueBoxes.Double1Box, PixelShaderConstantCallback(18)));

    public double M44
    {
        get => (double) GetValue(M44Property);
        set => SetValue(M44Property, value);
    }

    public static readonly DependencyProperty M54Property = DependencyProperty.Register(
        nameof(M54), typeof(double), typeof(ColorMatrixEffect), new PropertyMetadata(ValueBoxes.Double0Box, PixelShaderConstantCallback(19)));

    public double M54
    {
        get => (double) GetValue(M54Property);
        set => SetValue(M54Property, value);
    }

    #endregion
}
