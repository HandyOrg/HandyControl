﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HandyControl.Controls
{
    public class CirclePanel : Panel
    {
        public static readonly DependencyProperty DiameterProperty = DependencyProperty.Register(
            "Diameter", typeof(double), typeof(CirclePanel), new PropertyMetadata(170.0));

        public double Diameter
        {
            get => (double) GetValue(DiameterProperty);
            set => SetValue(DiameterProperty, value);
        }

        public static readonly DependencyProperty KeepVerticalProperty = DependencyProperty.Register(
            "KeepVertical", typeof(bool), typeof(CirclePanel), new PropertyMetadata(default(bool)));

        public bool KeepVertical
        {
            get => (bool) GetValue(KeepVerticalProperty);
            set => SetValue(KeepVerticalProperty, value);
        }

        public static readonly DependencyProperty OffsetAngleProperty = DependencyProperty.Register(
            "OffsetAngle", typeof(double), typeof(CirclePanel), new PropertyMetadata(default(double)));

        public double OffsetAngle
        {
            get => (double) GetValue(OffsetAngleProperty);
            set => SetValue(OffsetAngleProperty, value);
        }

        // ReSharper disable once RedundantAssignment
        protected override Size MeasureOverride(Size availableSize)
        {
            if (Children.Count == 0) return new Size(Diameter, Diameter);

            availableSize = new Size(Diameter, Diameter);
            var i = 0;
            var perDeg = 360.0 / Children.Count;
            var radius = Diameter / 2;
            foreach (UIElement element in Children)
            {
                element.Measure(availableSize);
                var centerX = element.DesiredSize.Width / 2.0;
                var centerY = element.DesiredSize.Height / 2.0;
                var angle = perDeg * i++ + OffsetAngle;
                var transform = new RotateTransform
                {
                    CenterX = centerX,
                    CenterY = centerY,
                    Angle = KeepVertical ? 0 : angle
                };
                element.RenderTransform = transform;
                var r = Math.PI * angle / 180.0;
                var x = radius * Math.Cos(r);
                var y = radius * Math.Sin(r);
                var rectX = x + availableSize.Width / 2 - centerX;
                var rectY = y + availableSize.Height / 2 - centerY;
                element.Arrange(new Rect(rectX, rectY, element.DesiredSize.Width, element.DesiredSize.Height));
            }
            return availableSize;
        }
    }
}