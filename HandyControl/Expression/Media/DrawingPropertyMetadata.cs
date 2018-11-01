using System;
using System.Windows;

namespace HandyControl.Expression.Media
{
    internal class DrawingPropertyMetadata : FrameworkPropertyMetadata
    {
        private DrawingPropertyMetadataOptions options;

        private PropertyChangedCallback propertyChangedCallback;

        public static event EventHandler<DrawingPropertyChangedEventArgs> DrawingPropertyChanged;

        static DrawingPropertyMetadata()
        {
            DrawingPropertyChanged += delegate (object sender, DrawingPropertyChangedEventArgs args) {
                IShape shape = sender as IShape;
                if ((shape != null) && args.Metadata.AffectsRender)
                {
                    InvalidateGeometryReasons reasons = InvalidateGeometryReasons.PropertyChanged;
                    if (args.IsAnimated)
                    {
                        reasons |= InvalidateGeometryReasons.IsAnimated;
                    }
                    shape.InvalidateGeometry(reasons);
                }
            };
        }

        public DrawingPropertyMetadata(object defaultValue) : this(defaultValue, DrawingPropertyMetadataOptions.None, null)
        {
        }

        public DrawingPropertyMetadata(PropertyChangedCallback propertyChangedCallback) : this(DependencyProperty.UnsetValue, DrawingPropertyMetadataOptions.None, propertyChangedCallback)
        {
        }

        private DrawingPropertyMetadata(DrawingPropertyMetadataOptions options, object defaultValue) : base(defaultValue, (FrameworkPropertyMetadataOptions)options)
        {
        }

        public DrawingPropertyMetadata(object defaultValue, DrawingPropertyMetadataOptions options) : this(defaultValue, options, null)
        {
        }

        public DrawingPropertyMetadata(object defaultValue, DrawingPropertyMetadataOptions options, PropertyChangedCallback propertyChangedCallback) : base(defaultValue, (FrameworkPropertyMetadataOptions)options, AttachCallback(defaultValue, options, propertyChangedCallback))
        {
        }

        private static PropertyChangedCallback AttachCallback(object defaultValue, DrawingPropertyMetadataOptions options, PropertyChangedCallback propertyChangedCallback)
        {
            DrawingPropertyMetadata metadata = new DrawingPropertyMetadata(options, defaultValue)
            {
                options = options,
                propertyChangedCallback = propertyChangedCallback
            };
            return new PropertyChangedCallback(metadata.InternalCallback);
        }

        private void InternalCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (DrawingPropertyChanged != null)
            {
                DrawingPropertyChangedEventArgs args = new DrawingPropertyChangedEventArgs
                {
                    Metadata = this,
                    IsAnimated = DependencyPropertyHelper.GetValueSource(sender, e.Property).IsAnimated
                };
                DrawingPropertyChanged(sender, args);
            }
            if (this.propertyChangedCallback != null)
            {
                this.propertyChangedCallback(sender, e);
            }
        }
    }
}