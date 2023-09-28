using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HandyControl.Data;

namespace HandyControl.Controls;

public class CirclePanel : Panel
{
    public static readonly DependencyProperty DiameterProperty = DependencyProperty.Register(
        nameof(Diameter), typeof(double), typeof(CirclePanel), new FrameworkPropertyMetadata(170.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

    public double Diameter
    {
        get => (double) GetValue(DiameterProperty);
        set => SetValue(DiameterProperty, value);
    }

    public static readonly DependencyProperty KeepVerticalProperty = DependencyProperty.Register(
        nameof(KeepVertical), typeof(bool), typeof(CirclePanel), new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsMeasure));

    public bool KeepVertical
    {
        get => (bool) GetValue(KeepVerticalProperty);
        set => SetValue(KeepVerticalProperty, ValueBoxes.BooleanBox(value));
    }

    public static readonly DependencyProperty OffsetAngleProperty = DependencyProperty.Register(
        nameof(OffsetAngle), typeof(double), typeof(CirclePanel), new FrameworkPropertyMetadata(ValueBoxes.Double0Box, FrameworkPropertyMetadataOptions.AffectsMeasure));

    public double OffsetAngle
    {
        get => (double) GetValue(OffsetAngleProperty);
        set => SetValue(OffsetAngleProperty, value);
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        var diameter = Diameter;

        if (Children.Count == 0) return new Size(diameter, diameter);

        var newSize = new Size(diameter, diameter);

        foreach (UIElement element in Children)
        {
            element.Measure(newSize);
        }

        return newSize;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        var keepVertical = KeepVertical;
        var offsetAngle = OffsetAngle;

        var i = 0;
        var perDeg = 360.0 / Children.Count;
        var radius = Diameter / 2;

        foreach (UIElement element in Children)
        {
            var centerX = element.DesiredSize.Width / 2.0;
            var centerY = element.DesiredSize.Height / 2.0;
            var angle = perDeg * i++ + offsetAngle;

            var transform = new RotateTransform
            {
                CenterX = centerX,
                CenterY = centerY,
                Angle = keepVertical ? 0 : angle
            };
            element.RenderTransform = transform;

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
