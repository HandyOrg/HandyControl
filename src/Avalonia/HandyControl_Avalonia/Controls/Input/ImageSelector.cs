using System;
using System.Collections.Generic;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;

namespace HandyControl.Controls;

/// <summary>
///     A control that allows selecting an image file via a file picker dialog.
///     Displays the selected image preview inside a configurable frame.
/// </summary>
public class ImageSelector : TemplatedControl
{
    public static readonly RoutedEvent<RoutedEventArgs> ImageSelectedEvent =
        RoutedEvent.Register<ImageSelector, RoutedEventArgs>(nameof(ImageSelected), RoutingStrategies.Bubble);

    public static readonly RoutedEvent<RoutedEventArgs> ImageUnselectedEvent =
        RoutedEvent.Register<ImageSelector, RoutedEventArgs>(nameof(ImageUnselected), RoutingStrategies.Bubble);

    public event EventHandler<RoutedEventArgs>? ImageSelected
    {
        add => AddHandler(ImageSelectedEvent, value);
        remove => RemoveHandler(ImageSelectedEvent, value);
    }

    public event EventHandler<RoutedEventArgs>? ImageUnselected
    {
        add => AddHandler(ImageUnselectedEvent, value);
        remove => RemoveHandler(ImageUnselectedEvent, value);
    }

    static ImageSelector()
    {
        FocusableProperty.OverrideDefaultValue<ImageSelector>(true);
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            SwitchImage();
        }
    }

    private async void SwitchImage()
    {
        if (!HasValue)
        {
            var topLevel = TopLevel.GetTopLevel(this);
            if (topLevel == null) return;

            var patterns = ParseFilterPatterns(Filter);

            var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                AllowMultiple = false,
                FileTypeFilter =
                [
                    new FilePickerFileType("Images") { Patterns = patterns },
                ],
            });

            if (files.Count > 0)
            {
                var file = files[0];
                var localPath = file.TryGetLocalPath();
                HasValue = true;

                try
                {
                    await using var stream = await file.OpenReadAsync();
                    var bitmap = new Bitmap(stream);
                    PreviewBrush = new ImageBrush(bitmap) { Stretch = Stretch };
                }
                catch
                {
                    PreviewBrush = null;
                }

                if (localPath != null)
                    Uri = new Uri(localPath);
                ToolTip.SetTip(this, file.Name);
                RaiseEvent(new RoutedEventArgs(ImageSelectedEvent));
            }
        }
        else
        {
            HasValue = false;
            PreviewBrush = null;
            Uri = null;
            ToolTip.SetTip(this, null);
            RaiseEvent(new RoutedEventArgs(ImageUnselectedEvent));
        }
    }

    // ── Properties ──

    public static readonly StyledProperty<Stretch> StretchProperty =
        AvaloniaProperty.Register<ImageSelector, Stretch>(nameof(Stretch));

    public Stretch Stretch
    {
        get => GetValue(StretchProperty);
        set => SetValue(StretchProperty, value);
    }

    public static readonly DirectProperty<ImageSelector, Uri?> UriProperty =
        AvaloniaProperty.RegisterDirect<ImageSelector, Uri?>(nameof(Uri), o => o.Uri);

    private Uri? _uri;

    public Uri? Uri
    {
        get => _uri;
        private set => SetAndRaise(UriProperty, ref _uri, value);
    }

    public static readonly DirectProperty<ImageSelector, IBrush?> PreviewBrushProperty =
        AvaloniaProperty.RegisterDirect<ImageSelector, IBrush?>(nameof(PreviewBrush), o => o.PreviewBrush);

    private IBrush? _previewBrush;

    public IBrush? PreviewBrush
    {
        get => _previewBrush;
        private set => SetAndRaise(PreviewBrushProperty, ref _previewBrush, value);
    }
    public static readonly StyledProperty<double> StrokeThicknessProperty =
        AvaloniaProperty.Register<ImageSelector, double>(nameof(StrokeThickness), 1.0);

    public double StrokeThickness
    {
        get => GetValue(StrokeThicknessProperty);
        set => SetValue(StrokeThicknessProperty, value);
    }

    public static readonly StyledProperty<Avalonia.Collections.AvaloniaList<double>?> StrokeDashArrayProperty =
        AvaloniaProperty.Register<ImageSelector, Avalonia.Collections.AvaloniaList<double>?>(nameof(StrokeDashArray));

    public Avalonia.Collections.AvaloniaList<double>? StrokeDashArray
    {
        get => GetValue(StrokeDashArrayProperty);
        set => SetValue(StrokeDashArrayProperty, value);
    }

    public static readonly StyledProperty<string> DefaultExtProperty =
        AvaloniaProperty.Register<ImageSelector, string>(nameof(DefaultExt), ".jpg");

    public string DefaultExt
    {
        get => GetValue(DefaultExtProperty);
        set => SetValue(DefaultExtProperty, value);
    }

    public static readonly StyledProperty<string> FilterProperty =
        AvaloniaProperty.Register<ImageSelector, string>(nameof(Filter), "(.jpg)|*.jpg|(.png)|*.png");

    public string Filter
    {
        get => GetValue(FilterProperty);
        set => SetValue(FilterProperty, value);
    }

    /// <summary>
    ///     Parses a WPF-style filter string like "(.jpg)|*.jpg|(.png)|*.png"
    ///     into individual glob patterns for the cross-platform file picker.
    /// </summary>
    private static List<string> ParseFilterPatterns(string? filter)
    {
        var patterns = new List<string>();

        if (string.IsNullOrWhiteSpace(filter))
        {
            // Sensible cross-platform defaults with case variations
            patterns.AddRange(["*.jpg", "*.jpeg", "*.png", "*.bmp", "*.gif"]);
            patterns.AddRange(["*.JPG", "*.JPEG", "*.PNG", "*.BMP", "*.GIF"]);
            return patterns;
        }

        // WPF format: "Description|*.ext|Description|*.ext"
        var parts = filter.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        foreach (var part in parts)
        {
            // Skip display-name segments (e.g. "(.jpg)"); only keep pattern segments
            if (part.Contains('*') || part.Contains('?'))
            {
                // Split multi-pattern entries like "*.jpg;*.png"
                foreach (var p in part.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    var trimmed = p.Trim();
                    if (!string.IsNullOrEmpty(trimmed) && !patterns.Contains(trimmed))
                    {
                        patterns.Add(trimmed);
                        // Add uppercase variant for case-sensitive platforms (macOS)
                        var upper = trimmed.ToUpperInvariant();
                        if (upper != trimmed && !patterns.Contains(upper))
                            patterns.Add(upper);
                    }
                }
            }
        }

        // Fallback if parsing produced nothing
        if (patterns.Count == 0)
        {
            patterns.AddRange(["*.jpg", "*.jpeg", "*.png", "*.bmp", "*.gif",
                              "*.JPG", "*.JPEG", "*.PNG", "*.BMP", "*.GIF"]);
        }

        return patterns;
    }

    public static readonly DirectProperty<ImageSelector, bool> HasValueProperty =
        AvaloniaProperty.RegisterDirect<ImageSelector, bool>(nameof(HasValue), o => o.HasValue);

    private bool _hasValue;

    public bool HasValue
    {
        get => _hasValue;
        private set => SetAndRaise(HasValueProperty, ref _hasValue, value);
    }
}
