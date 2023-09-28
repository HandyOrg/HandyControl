using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HandyControl.Data;
using HandyControl.Tools;

namespace HandyControl.Controls;

public class Gravatar : ContentControl
{
    public static readonly DependencyProperty GeneratorProperty = DependencyProperty.Register(
        nameof(Generator), typeof(IGravatarGenerator), typeof(Gravatar), new PropertyMetadata(new GithubGravatarGenerator()));

    public IGravatarGenerator Generator
    {
        get => (IGravatarGenerator) GetValue(GeneratorProperty);
        set => SetValue(GeneratorProperty, value);
    }

    public static readonly DependencyProperty IdProperty = DependencyProperty.Register(
        nameof(Id), typeof(string), typeof(Gravatar), new PropertyMetadata(default(string), OnIdChanged));

    private static void OnIdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (Gravatar) d;
        if (ctl.Source != null) return;
        ctl.Content = ctl.Generator.GetGravatar((string) e.NewValue);
    }

    public string Id
    {
        get => (string) GetValue(IdProperty);
        set => SetValue(IdProperty, value);
    }

    public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
        nameof(Source), typeof(ImageSource), typeof(Gravatar), new PropertyMetadata(default(ImageSource), OnSourceChanged));

    private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (Gravatar) d;
        var v = (ImageSource) e.NewValue;

        ctl.Background = v != null
            ? new ImageBrush(v)
            {
                Stretch = Stretch.UniformToFill
            }
            : ResourceHelper.GetResourceInternal<Brush>(ResourceToken.SecondaryRegionBrush);
    }

    public ImageSource Source
    {
        get => (ImageSource) GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }
}
