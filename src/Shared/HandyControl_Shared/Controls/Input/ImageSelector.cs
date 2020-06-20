using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HandyControl.Data;

namespace HandyControl.Controls
{
    public class ImageSelector : Control
    {
        public static readonly DependencyProperty UriProperty = DependencyProperty.Register(
            "Uri", typeof(Uri), typeof(ImageSelector), new PropertyMetadata(default(Uri)));

        public Uri Uri
        {
            get => (Uri) GetValue(UriProperty);
            set => SetValue(UriProperty, value);
        }

        internal static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            "Source", typeof(ImageSource), typeof(ImageSelector), new PropertyMetadata(default(ImageSource)));

        internal ImageSource Source
        {
            get => (ImageSource) GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public static readonly DependencyProperty IsPreviewEnabledProperty = DependencyProperty.Register(
            "IsPreviewEnabled", typeof(bool), typeof(ImageSelector), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool IsPreviewEnabled
        {
            get => (bool) GetValue(IsPreviewEnabledProperty);
            set => SetValue(IsPreviewEnabledProperty, value);
        }
    }
}