using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace HandyControl.Controls;

public class Card : ContentControl
{
    public static readonly StyledProperty<object?> HeaderProperty =
        AvaloniaProperty.Register<Card, object?>(nameof(Header));

    public object? Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> HeaderTemplateProperty =
        AvaloniaProperty.Register<Card, IDataTemplate?>(nameof(HeaderTemplate));

    public IDataTemplate? HeaderTemplate
    {
        get => GetValue(HeaderTemplateProperty);
        set => SetValue(HeaderTemplateProperty, value);
    }

    public static readonly StyledProperty<string?> HeaderStringFormatProperty =
        AvaloniaProperty.Register<Card, string?>(nameof(HeaderStringFormat));

    public string? HeaderStringFormat
    {
        get => GetValue(HeaderStringFormatProperty);
        set => SetValue(HeaderStringFormatProperty, value);
    }

    public static readonly StyledProperty<object?> FooterProperty =
        AvaloniaProperty.Register<Card, object?>(nameof(Footer));

    public object? Footer
    {
        get => GetValue(FooterProperty);
        set => SetValue(FooterProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> FooterTemplateProperty =
        AvaloniaProperty.Register<Card, IDataTemplate?>(nameof(FooterTemplate));

    public IDataTemplate? FooterTemplate
    {
        get => GetValue(FooterTemplateProperty);
        set => SetValue(FooterTemplateProperty, value);
    }

    public static readonly StyledProperty<string?> FooterStringFormatProperty =
        AvaloniaProperty.Register<Card, string?>(nameof(FooterStringFormat));

    public string? FooterStringFormat
    {
        get => GetValue(FooterStringFormatProperty);
        set => SetValue(FooterStringFormatProperty, value);
    }
}
