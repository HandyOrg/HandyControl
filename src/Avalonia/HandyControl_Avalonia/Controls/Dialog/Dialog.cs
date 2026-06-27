using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.VisualTree;
using HandyControl.Tools;

namespace HandyControl.Controls;

[TemplatePart("PART_BackElement", typeof(Border))]
public class Dialog : ContentControl
{
    private const string BackElement = "PART_BackElement";

    private string? _token;
    private Border? _backElement;
    private Panel? _hostPanel;

    private static readonly ControlTokenManager<Control> TokenManager = new();
    private static readonly Dictionary<string, Dialog> DialogDict = new();

    public static readonly StyledProperty<bool> IsClosedProperty =
        AvaloniaProperty.Register<Dialog, bool>(nameof(IsClosed));

    public bool IsClosed
    {
        get => GetValue(IsClosedProperty);
        internal set => SetValue(IsClosedProperty, value);
    }

    public static readonly AttachedProperty<bool> MaskCanCloseProperty =
        AvaloniaProperty.RegisterAttached<Dialog, AvaloniaObject, bool>("MaskCanClose", false);

    public static void SetMaskCanClose(AvaloniaObject element, bool value) =>
        element.SetValue(MaskCanCloseProperty, value);

    public static bool GetMaskCanClose(AvaloniaObject element) =>
        element.GetValue(MaskCanCloseProperty);

    public static readonly StyledProperty<IBrush?> MaskBrushProperty =
        AvaloniaProperty.Register<Dialog, IBrush?>(nameof(MaskBrush));

    public IBrush? MaskBrush
    {
        get => GetValue(MaskBrushProperty);
        set => SetValue(MaskBrushProperty, value);
    }

    public static readonly AttachedProperty<string?> TokenProperty =
        AvaloniaProperty.RegisterAttached<Dialog, AvaloniaObject, string?>("Token");

    static Dialog()
    {
        TokenProperty.Changed.AddClassHandler<AvaloniaObject>(TokenManager.OnTokenChanged);
    }

    public static void SetToken(AvaloniaObject element, string? value) =>
        element.SetValue(TokenProperty, value);

    public static string? GetToken(AvaloniaObject element) =>
        element.GetValue(TokenProperty);

    public static Dialog Show<T>(string? token = null) where T : new() => Show(new T(), token);

    public static Dialog Show(object content, string? token = null)
    {
        var dialog = new Dialog
        {
            _token = token,
            Content = content
        };

        if (string.IsNullOrEmpty(token))
        {
            var window = GetActiveWindow();
            if (window != null)
            {
                ShowOnWindow(dialog, window);
            }
        }
        else
        {
            Close(token);
            DialogDict[token] = dialog;
            if (TokenManager.TryGetControl(token, out var ctrl) && ctrl != null)
            {
                if (ctrl is Window window)
                {
                    ShowOnWindow(dialog, window);
                }
                else
                {
                    var panel = FindDialogContainer(ctrl);
                    if (panel != null)
                    {
                        dialog._hostPanel = panel;
                        dialog.IsClosed = false;
                        dialog.Width = panel.Bounds.Width;
                        dialog.Height = panel.Bounds.Height;
                        panel.Children.Add(dialog);
                        panel.InvalidateMeasure();
                    }
                    else
                    {
                        var win = GetActiveWindow();
                        if (win != null)
                            ShowOnWindow(dialog, win);
                    }
                }
            }
        }

        return dialog;
    }

    private static void ShowOnWindow(Dialog dialog, Window window)
    {
        var overlay = OverlayLayer.GetOverlayLayer(window);
        if (overlay == null) return;

        var size = overlay.Bounds.Size;
        if (size.Width <= 0 || size.Height <= 0)
        {
            var topLevel = TopLevel.GetTopLevel(overlay);
            if (topLevel != null)
                size = topLevel.ClientSize;
        }

        dialog._hostPanel = overlay;
        dialog.IsClosed = false;
        dialog.Width = size.Width;
        dialog.Height = size.Height;
        overlay.Children.Add(dialog);

        void OnOverlayResized(object? sender, SizeChangedEventArgs e)
        {
            if (dialog._hostPanel == null) return;
            dialog.Width = e.NewSize.Width;
            dialog.Height = e.NewSize.Height;
        }
        overlay.SizeChanged += OnOverlayResized;
    }

    public static void Close(string? token)
    {
        if (!string.IsNullOrEmpty(token) && DialogDict.TryGetValue(token, out var dialog))
        {
            dialog.Close();
        }
    }

    public void Close()
    {
        if (_hostPanel == null) return;

        _hostPanel.Children.Remove(this);
        _hostPanel = null;
        IsClosed = true;

        if (!string.IsNullOrEmpty(_token))
        {
            DialogDict.Remove(_token);
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _backElement = e.NameScope.Find<Border>(BackElement);
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);

        if (e.InitialPressMouseButton == MouseButton.Left &&
            GetMaskCanClose(this) && _backElement != null && _backElement.IsPointerOver)
        {
            Close();
        }
    }

    private static DialogContainer? FindDialogContainer(Visual root)
    {
        foreach (var child in root.GetVisualChildren())
        {
            if (child is DialogContainer found)
                return found;
            var nested = FindDialogContainer(child);
            if (nested != null)
                return nested;
        }
        return null;
    }

    private static Window? GetActiveWindow()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            return desktop.Windows.FirstOrDefault(w => w.IsActive) ?? desktop.MainWindow;
        }

        return null;
    }
}
