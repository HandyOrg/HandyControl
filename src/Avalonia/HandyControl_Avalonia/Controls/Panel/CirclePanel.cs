using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

namespace HandyControl.Controls;

public class CirclePanel : Panel
{
    public static readonly StyledProperty<double> DiameterProperty =
        AvaloniaProperty.Register<CirclePanel, double>(nameof(Diameter), defaultValue: 170.0);

    public double Diameter
    {
        get => GetValue(DiameterProperty);
        set => SetValue(DiameterProperty, value);
    }

    public static readonly StyledProperty<bool> KeepVerticalProperty =
        AvaloniaProperty.Register<CirclePanel, bool>(nameof(KeepVertical));

    public bool KeepVertical
    {
        get => GetValue(KeepVerticalProperty);
        set => SetValue(KeepVerticalProperty, value);
    }

    public static readonly StyledProperty<double> OffsetAngleProperty =
        AvaloniaProperty.Register<CirclePanel, double>(nameof(OffsetAngle), defaultValue: 0d);

    public double OffsetAngle
    {
        get => GetValue(OffsetAngleProperty);
        set => SetValue(OffsetAngleProperty, value);
    }

    static CirclePanel()
    {
        AffectsMeasure<CirclePanel>(DiameterProperty, KeepVerticalProperty, OffsetAngleProperty);
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        var diameter = Diameter;
        var layoutChildren = Children.Where(element => element.IsVisible).ToList();

        if (layoutChildren.Count == 0) return new Size(diameter, diameter);

        var newSize = new Size(diameter, diameter);

        foreach (var element in layoutChildren)
        {
            element.Measure(newSize);
        }

        return newSize;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        var keepVertical = KeepVertical;
        var offsetAngle = OffsetAngle;
        var layoutChildren = Children.Where(element => element.IsVisible).ToList();

        if (layoutChildren.Count == 0)
        {
            return finalSize;
        }

        var i = 0;
        var perDeg = 360.0 / layoutChildren.Count;
        var radius = Diameter / 2;

        foreach (var element in layoutChildren)
        {
            var centerX = element.DesiredSize.Width / 2.0;
            var centerY = element.DesiredSize.Height / 2.0;
            var angle = perDeg * i++ + offsetAngle;

            element.RenderTransformOrigin = new RelativePoint(centerX, centerY, RelativeUnit.Absolute);
            element.RenderTransform = new RotateTransform
            {
                Angle = keepVertical ? 0 : angle
            };

            var r = Math.PI * angle / 180.0;
            var x = radius * Math.Cos(r);
            var y = radius * Math.Sin(r);

            var rectX = x + finalSize.Width / 2 - centerX;
            var rectY = y + finalSize.Height / 2 - centerY;

            element.Arrange(new Rect(rectX, rectY, element.DesiredSize.Width, element.DesiredSize.Height));
        }

        return finalSize;
    }
}
