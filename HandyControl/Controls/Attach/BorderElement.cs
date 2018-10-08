﻿using System.Windows;

// ReSharper disable once CheckNamespace
namespace HandyControl.Controls
{
    public class BorderElement : DependencyObject
    {
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached(
            "CornerRadius", typeof(CornerRadius), typeof(BorderElement), new FrameworkPropertyMetadata(default(CornerRadius), FrameworkPropertyMetadataOptions.Inherits));

        public static void SetCornerRadius(DependencyObject element, CornerRadius value) => element.SetValue(CornerRadiusProperty, value);

        public static CornerRadius GetCornerRadius(DependencyObject element) => (CornerRadius)element.GetValue(CornerRadiusProperty);
    }
}