// adapted from https://github.com/catdad/canvas-confetti

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia.Threading;
using Avalonia.VisualTree;
using HandyControl.Tools;

namespace HandyControl.Controls;

public class ConfettiCannon
{
    private static readonly ControlTokenManager<Control> TokenManager =
        new(unregisterCallback: OnTokenUnregistered);

    public static readonly AttachedProperty<string?> TokenProperty =
        AvaloniaProperty.RegisterAttached<ConfettiCannon, AvaloniaObject, string?>("Token");

    public static void SetToken(AvaloniaObject element, string? value) => element.SetValue(TokenProperty, value);

    public static string? GetToken(AvaloniaObject element) => element.GetValue(TokenProperty);

    static ConfettiCannon()
    {
        TokenProperty.Changed.AddClassHandler<AvaloniaObject>(TokenManager.OnTokenChanged);
    }

    private static readonly ConcurrentDictionary<string, ConfettiCannon> ConfettiCannons = new();
    private static readonly Lazy<ConfettiCannon> DefaultConfettiCannon = new(() => new ConfettiCannon(), true);
    private static readonly Random Random = new();

    private readonly ConcurrentQueue<Confetti> _confettis = new();
    private ConfettiOverlay? _overlay;
    private Panel? _hostPanel;
    private Rect _renderRect;
    private bool _renderingHooked;

    private ConfettiCannon()
    {
    }

    public static void Fire()
    {
        Fire(new Options());
    }

    public static void Fire(Options options)
    {
        ConfettiCannon confettiCannon = EnsureConfettiCannon(options.Token);
        confettiCannon.ShowContainer(options.Token);
        confettiCannon.AddConfettis(options);
        confettiCannon.StartAnimation();
    }

    private static void OnTokenUnregistered(string token, Control element)
    {
        ConfettiCannons.TryRemove(token, out _);
    }

    private static ConfettiCannon EnsureConfettiCannon(string? token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return DefaultConfettiCannon.Value;
        }

        return ConfettiCannons.GetOrAdd(token!, _ => new ConfettiCannon());
    }

    private static Window? GetActiveWindow()
    {
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
        {
            return null;
        }

        return desktop.Windows.FirstOrDefault(w => w.IsActive) ?? desktop.MainWindow;
    }

    private static Panel? GetHostPanel(string? token)
    {
        if (string.IsNullOrEmpty(token))
        {
            var window = GetActiveWindow();
            return window is null ? null : GetWindowOverlay(window);
        }

        if (!TokenManager.TryGetControl(token, out Control? element) || element is null)
        {
            return null;
        }

        if (element is Window w)
        {
            return GetWindowOverlay(w);
        }

        return element.GetVisualDescendants().OfType<ConfettiCannonContainer>().FirstOrDefault()
               ?? (element as ConfettiCannonContainer);
    }

    private static Panel? GetWindowOverlay(Window window)
    {
        return OverlayLayer.GetOverlayLayer(window);
    }

    private static Brush HexToBrush(string hex)
    {
        return new SolidColorBrush(Color.Parse(hex));
    }

    private void StartAnimation()
    {
        if (_renderingHooked || _overlay is null)
        {
            return;
        }

        var topLevel = TopLevel.GetTopLevel(_overlay);
        if (topLevel is null)
        {
            return;
        }

        _renderingHooked = true;
        topLevel.RequestAnimationFrame(OnRenderingFrame);
    }

    private void OnRenderingFrame(TimeSpan time)
    {
        if (_overlay is null)
        {
            _renderingHooked = false;
            return;
        }

        SyncOverlaySize();

        RecycleConfettis();

        _overlay.UpdateConfettis(_confettis, _renderRect);
        _overlay.InvalidateVisual();

        if (_confettis.IsEmpty)
        {
            HideContainer();
            _renderingHooked = false;
            return;
        }

        var topLevel = TopLevel.GetTopLevel(_overlay);
        topLevel?.RequestAnimationFrame(OnRenderingFrame);
    }

    private void RecycleConfettis()
    {
        int initialCount = _confettis.Count;

        for (int i = 0; i < initialCount; i++)
        {
            if (_confettis.TryDequeue(out var confetti))
            {
                if (!confetti.IsCompleted)
                {
                    _confettis.Enqueue(confetti);
                }
            }
        }
    }

    private void AddConfettis(Options options)
    {
        double radAngle = (270 - options.Angle) * (Math.PI / 180);
        double radSpread = options.Spread * (Math.PI / 180);

        Point origin = options.Origin;
        double startVelocity = options.StartVelocity;
        int ticks = options.Ticks;
        double decay = options.Decay;
        double draft = options.Drift;
        double gravity = options.Gravity;
        double scalar = options.Scalar;
        bool flat = options.Flat;
        List<IBrush> brushes = options.GetFinalBrushes();

        for (int i = 0; i < options.ParticleCount; i++)
        {
            _confettis.Enqueue(new Confetti
            {
                Point = new Point(origin.X * _renderRect.Width, origin.Y * _renderRect.Height),
                Wobble = Random.NextDouble() * 10,
                WobbleSpeed = Math.Min(0.11, Random.NextDouble() * 0.1 + 0.05),
                Velocity = startVelocity * 0.5 + Random.NextDouble() * startVelocity,
                Angle2D = -radAngle + (0.5 * radSpread - Random.NextDouble() * radSpread),
                TiltAngle = (Random.NextDouble() * (0.75 - 0.25) + 0.25) * Math.PI,
                Brush = brushes[i % brushes.Count],
                Shape = options.Shapes[Random.Next(options.Shapes.Count)],
                TotalTicks = ticks,
                Decay = decay,
                Drift = draft,
                RandomValue = Random.NextDouble() + 2,
                Gravity = gravity * 3,
                OvalScalar = 0.6,
                Scalar = scalar,
                Flat = flat,
            });
        }
    }

    private void ShowContainer(string? token)
    {
        Panel? host = GetHostPanel(token);
        if (host is null)
        {
            return;
        }

        // Bounds may not be assigned yet on the first hit. Fall back to the
        // owning TopLevel's client size so particles don't spawn at (0,0).
        var size = host.Bounds.Size;
        if (size.Width <= 0 || size.Height <= 0)
        {
            var topLevel = TopLevel.GetTopLevel(host);
            if (topLevel is not null)
            {
                size = topLevel.ClientSize;
            }
        }
        _renderRect = new Rect(size);

        if (ReferenceEquals(_hostPanel, host) && _overlay is not null && _overlay.GetVisualParent() == host)
        {
            return;
        }

        HideContainer();

        _hostPanel = host;
        _overlay = new ConfettiOverlay
        {
            IsHitTestVisible = false,
            Width = size.Width,
            Height = size.Height,
        };

        host.Children.Add(_overlay);
    }

    private void HideContainer()
    {
        if (_overlay is null)
        {
            return;
        }

        if (_overlay.GetVisualParent() is Panel parent)
        {
            parent.Children.Remove(_overlay);
        }

        _overlay = null;
        _hostPanel = null;
    }

    private void SyncOverlaySize()
    {
        if (_overlay is null || _hostPanel is null)
        {
            return;
        }

        var size = _hostPanel.Bounds.Size;
        if (size.Width <= 0 || size.Height <= 0)
        {
            var topLevel = TopLevel.GetTopLevel(_hostPanel);
            if (topLevel is not null)
            {
                size = topLevel.ClientSize;
            }
        }

        if (Math.Abs(_overlay.Width - size.Width) > 0.5 ||
            Math.Abs(_overlay.Height - size.Height) > 0.5)
        {
            _overlay.Width = size.Width;
            _overlay.Height = size.Height;
        }

        _renderRect = new Rect(size);
    }

    public class Options
    {
        private static readonly List<string> DefaultColors =
        [
            "#26ccff",
            "#a25afd",
            "#ff5e7e",
            "#88ff5a",
            "#fcff42",
            "#ffa62d",
            "#ff36ff"
        ];

        public string? Token { get; set; }

        public int ParticleCount { get; set; } = 50;

        public double Angle { get; set; } = 90;

        public double Spread { get; set; } = 45;

        public double StartVelocity { get; set; } = 45;

        public double Decay { get; set; } = 0.9;

        public double Gravity { get; set; } = 1;

        public double Drift { get; set; }

        public int Ticks { get; set; } = 200;

        public Point Origin { get; set; } = new(0.5, 0.5);

        public List<string> Shapes { get; set; } = ["square", "circle"];

        public List<IBrush> Brushes { get; set; } = [];

        public List<string> Colors { get; set; } = DefaultColors;

        public double Scalar { get; set; } = 1;

        public bool Flat { get; set; }

        internal List<IBrush> GetFinalBrushes()
        {
            if (Brushes is { Count: > 0 })
            {
                return Brushes;
            }

            if (Colors is null || Colors.Count == 0)
            {
                return DefaultColors.Select(HexToBrush).Cast<IBrush>().ToList();
            }

            return Colors.Select(HexToBrush).Cast<IBrush>().ToList();
        }
    }

    internal class Confetti
    {
        public Point Point { get; set; }

        public double Wobble { get; set; }

        public double WobbleSpeed { get; set; }

        public double Velocity { get; set; }

        public double Angle2D { get; set; }

        public double TiltAngle { get; set; }

        public IBrush Brush { get; set; } = Brushes.White;

        public object Shape { get; set; } = "square";

        public int Tick { get; set; }

        public int TotalTicks { get; set; }

        public double Decay { get; set; }

        public double Drift { get; set; }

        public double RandomValue { get; set; }

        public double TiltSin { get; set; }

        public double TiltCos { get; set; }

        public double WobbleX { get; set; }

        public double WobbleY { get; set; }

        public double Gravity { get; set; }

        public double Scalar { get; set; }

        public double OvalScalar { get; set; }

        public bool Flat { get; set; }

        public bool IsCompleted => Tick >= TotalTicks;
    }
}
