// adapted from https://github.com/catdad/canvas-confetti

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using HandyControl.Tools;

namespace HandyControl.Controls;

public class ConfettiCannon : DependencyObject
{
    public static readonly DependencyProperty TokenProperty = DependencyProperty.RegisterAttached(
        "Token", typeof(string), typeof(ConfettiCannon), new PropertyMetadata(null, OnTokenChanged));

    public static void SetToken(DependencyObject element, string value)
        => element.SetValue(TokenProperty, value);

    public static string GetToken(DependencyObject element)
        => (string) element.GetValue(TokenProperty);

    private static readonly ConcurrentDictionary<string, ConfettiCannon> ConfettiCannons = new();
    private static readonly Dictionary<string, FrameworkElement> ContainerDict = new();
    private static readonly Lazy<ConfettiCannon> DefaultConfettiCannon = new(() => new ConfettiCannon(), true);
    private static readonly Random Random = new();

    private readonly DrawingVisual _offscreenVisual = new();
    private readonly ConcurrentQueue<Confetti> _confettis = [];

    private readonly EllipseGeometry _sharedCircleGeometry = new()
    {
        Transform = new RotateTransform()
    };

    private Rect _renderRect;
    private AdornerLayer _currentAdornerLayer;

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

    public static void Register(string token, FrameworkElement element)
    {
        if (string.IsNullOrEmpty(token) || element is null)
        {
            return;
        }

        ContainerDict[token] = element;
    }

    public static void Unregister(FrameworkElement element)
    {
        if (element is null)
        {
            return;
        }

        var first = ContainerDict.FirstOrDefault(item => ReferenceEquals(element, item.Value));
        Unregister(first.Key);
    }

    public static void Unregister(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return;
        }

        ContainerDict.Remove(token);
        ConfettiCannons.TryRemove(token, out _);
    }

    private static ConfettiCannon EnsureConfettiCannon(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return DefaultConfettiCannon.Value;
        }

        if (ConfettiCannons.TryGetValue(token, out ConfettiCannon confettiCannon))
        {
            return confettiCannon;
        }

        ConfettiCannons[token] = new ConfettiCannon();
        return ConfettiCannons[token];
    }

    private static void OnTokenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not FrameworkElement element)
        {
            return;
        }

        if (e.NewValue is null)
        {
            Unregister(element);
        }
        else
        {
            Register(e.NewValue.ToString(), element);
        }
    }

    private static AdornerLayer GetAdornerLayer(string token)
    {
        AdornerDecorator decorator;

        if (string.IsNullOrEmpty(token))
        {
            decorator = VisualHelper.GetChild<AdornerDecorator>(WindowHelper.GetActiveWindow());
        }
        else
        {
            ContainerDict.TryGetValue(token, out FrameworkElement element);
            decorator = element is System.Windows.Window
                ? VisualHelper.GetChild<AdornerDecorator>(element)
                : VisualHelper.GetChild<ConfettiCannonContainer>(element);
        }

        return decorator?.AdornerLayer;
    }

    private static Brush HexToBrush(string hex)
    {
        return new SolidColorBrush((Color) ColorConverter.ConvertFromString(hex));
    }

    private void UpdateConfetti(DrawingContext context, Confetti confetti)
    {
        confetti.Point = new Point(
            confetti.Point.X + Math.Sin(confetti.Angle2D) * confetti.Velocity + confetti.Drift,
            confetti.Point.Y + Math.Cos(confetti.Angle2D) * confetti.Velocity + confetti.Gravity
        );
        confetti.Velocity *= confetti.Decay;

        if (confetti.Flat)
        {
            confetti.Wobble = 0;
            confetti.WobbleX = confetti.Point.X + 10 * confetti.Scalar;
            confetti.WobbleY = confetti.Point.Y + 10 * confetti.Scalar;

            confetti.TiltSin = 0;
            confetti.TiltCos = 0;
            confetti.RandomValue = 1;
        }
        else
        {
            confetti.Wobble += confetti.WobbleSpeed;
            confetti.WobbleX = confetti.Point.X + 10 * confetti.Scalar * Math.Cos(confetti.Wobble);
            confetti.WobbleY = confetti.Point.Y + 10 * confetti.Scalar * Math.Sin(confetti.Wobble);

            confetti.TiltAngle += 0.1;
            confetti.TiltSin = Math.Sin(confetti.TiltAngle);
            confetti.TiltCos = Math.Cos(confetti.TiltAngle);
            confetti.RandomValue = Random.NextDouble() + 2;
        }

        double progress = (double) confetti.Tick++ / confetti.TotalTicks;
        confetti.Brush.Opacity = 1 - progress;

        double x1 = confetti.Point.X + confetti.RandomValue * confetti.TiltCos;
        double y1 = confetti.Point.Y + confetti.RandomValue * confetti.TiltSin;
        double x2 = confetti.WobbleX + confetti.RandomValue * confetti.TiltCos;
        double y2 = confetti.WobbleY + confetti.RandomValue * confetti.TiltSin;

        if ("circle".Equals(confetti.Shape))
        {
            double radiusX = Math.Abs(x2 - x1) * confetti.OvalScalar;
            double radiusY = Math.Abs(y2 - y1) * confetti.OvalScalar;
            double angle = 18 * confetti.Wobble;

            var geometry = new EllipseGeometry(new Rect(
                confetti.Point.X - radiusX,
                confetti.Point.Y - radiusY,
                radiusX * 2,
                radiusY * 2
            ))
            {
                Transform = new RotateTransform(angle, confetti.Point.X, confetti.Point.Y)
            };

            context.DrawGeometry(confetti.Brush, null, geometry);
        }
        else if ("star".Equals(confetti.Shape))
        {
            double rot = Math.PI / 2 * 3;
            double innerRadius = 4 * confetti.Scalar;
            double outerRadius = 8 * confetti.Scalar;
            int spikes = 5;
            double step = Math.PI / spikes;
            bool isFirst = true;

            var geometry = new StreamGeometry();
            using (StreamGeometryContext sgc = geometry.Open())
            {
                while (spikes-- > 0)
                {
                    double x = confetti.Point.X + Math.Cos(rot) * outerRadius;
                    double y = confetti.Point.Y + Math.Sin(rot) * outerRadius;

                    if (isFirst)
                    {
                        sgc.BeginFigure(new Point(x, y), true, true);
                        isFirst = false;
                    }

                    sgc.LineTo(new Point(x, y), false, false);
                    rot += step;

                    x = confetti.Point.X + Math.Cos(rot) * innerRadius;
                    y = confetti.Point.Y + Math.Sin(rot) * innerRadius;
                    sgc.LineTo(new Point(x, y), false, false);
                    rot += step;
                }
            }

            context.DrawGeometry(confetti.Brush, null, geometry);
        }
        else
        {
            var geometry = new StreamGeometry();
            using (StreamGeometryContext sgc = geometry.Open())
            {
                sgc.BeginFigure(new Point(Math.Floor(confetti.Point.X), Math.Floor(confetti.Point.Y)), true, true);
                sgc.LineTo(new Point(Math.Floor(confetti.WobbleX), Math.Floor(y1)), false, false);
                sgc.LineTo(new Point(Math.Floor(x2), Math.Floor(y2)), false, false);
                sgc.LineTo(new Point(Math.Floor(x1), Math.Floor(confetti.WobbleY)), false, false);
            }

            context.DrawGeometry(confetti.Brush, null, geometry);
        }
    }

    private void StartAnimation()
    {
        CompositionTarget.Rendering -= CompositionTargetOnRendering;
        CompositionTarget.Rendering += CompositionTargetOnRendering;
    }

    private void CompositionTargetOnRendering(object sender, EventArgs e)
    {
        RecycleConfettis();

        using (DrawingContext context = _offscreenVisual.RenderOpen())
        {
            context.PushClip(new RectangleGeometry(_renderRect));
            foreach (Confetti confetti in _confettis)
            {
                UpdateConfetti(context, confetti);
            }
        }

        if (_confettis.Count == 0)
        {
            HideContainer();
            CompositionTarget.Rendering -= CompositionTargetOnRendering;
        }
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
        List<Brush> brushes = options.GetFinalBrushes();

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

    private void ShowContainer(string token)
    {
        AdornerLayer layer = GetAdornerLayer(token);
        if (layer is null)
        {
            return;
        }

        _renderRect = new Rect(layer.RenderSize);

        if (ReferenceEquals(_currentAdornerLayer, layer))
        {
            return;
        }

        _currentAdornerLayer = layer;

        layer.Add(new VisualAdornerContainer(layer)
        {
            Child = _offscreenVisual
        });
    }

    private void HideContainer()
    {
        var adorner = VisualHelper.GetParent<VisualAdornerContainer>(_offscreenVisual);
        if (adorner != null)
        {
            adorner.Child = null;
            _currentAdornerLayer?.Remove(adorner);
            _currentAdornerLayer = null;
        }
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

        public string Token { get; set; }

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

        public List<Brush> Brushes { get; set; } = [];

        public List<string> Colors { get; set; } = DefaultColors;

        public double Scalar { get; set; } = 1;

        public bool Flat { get; set; }

        internal List<Brush> GetFinalBrushes()
        {
            if (Brushes is not null && Brushes.Count != 0)
            {
                return Brushes;
            }

            if (Colors is null || Colors.Count == 0)
            {
                return DefaultColors.Select(HexToBrush).ToList();
            }

            return Colors.Select(HexToBrush).ToList();
        }
    }

    private class Confetti
    {
        public Point Point { get; set; }

        public double Wobble { get; set; }

        public double WobbleSpeed { get; set; }

        public double Velocity { get; set; }

        public double Angle2D { get; set; }

        public double TiltAngle { get; set; }

        public Brush Brush { get; set; }

        public object Shape { get; set; }

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
