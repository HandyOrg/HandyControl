using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using HandyControl.Tools;

namespace HandyControl.Controls;

public class Gravatar : ContentControl
{
    public static readonly StyledProperty<IGravatarGenerator> GeneratorProperty =
        AvaloniaProperty.Register<Gravatar, IGravatarGenerator>(
            nameof(Generator), defaultValue: new GithubGravatarGenerator());

    public IGravatarGenerator Generator
    {
        get => GetValue(GeneratorProperty);
        set => SetValue(GeneratorProperty, value);
    }

    public static readonly StyledProperty<string?> IdProperty =
        AvaloniaProperty.Register<Gravatar, string?>(nameof(Id));

    public string? Id
    {
        get => GetValue(IdProperty);
        set => SetValue(IdProperty, value);
    }

    public static readonly StyledProperty<IImage?> SourceProperty =
        AvaloniaProperty.Register<Gravatar, IImage?>(nameof(Source));

    public IImage? Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    static Gravatar()
    {
        IdProperty.Changed.AddClassHandler<Gravatar>((o, e) => o.OnIdChanged(e));
        SourceProperty.Changed.AddClassHandler<Gravatar>((o, e) => o.OnSourceChanged(e));
    }

    private void OnIdChanged(AvaloniaPropertyChangedEventArgs e)
    {
        if (Source != null) return;
        Content = Generator.GetGravatar((string?) e.NewValue ?? string.Empty);
    }

    private void OnSourceChanged(AvaloniaPropertyChangedEventArgs e)
    {
        var v = (IImage?) e.NewValue;

        Background = v is IImageBrushSource brushSource
            ? new ImageBrush(brushSource) { Stretch = Stretch.UniformToFill }
            : ResourceHelper.GetResource<IBrush>(Data.ResourceToken.SecondaryRegionBrush);
    }
}
