using System.Windows;
using System.Windows.Controls;

namespace HandyControl.Controls
{
    public class CompareSlider : Slider
    {
        public static readonly DependencyProperty TargetContentProperty = DependencyProperty.Register(
            "TargetContent", typeof(object), typeof(CompareSlider), new PropertyMetadata(default(object)));

        public static readonly DependencyProperty SourceContentProperty = DependencyProperty.Register(
            "SourceContent", typeof(object), typeof(CompareSlider), new PropertyMetadata(default(object)));

        public object TargetContent
        {
            get => GetValue(TargetContentProperty);
            set => SetValue(TargetContentProperty, value);
        }

        public object SourceContent
        {
            get => GetValue(SourceContentProperty);
            set => SetValue(SourceContentProperty, value);
        }

        private bool _isLoaded;

        private double _tempValue;

        public CompareSlider()
        {
            Loaded += delegate
            {
                if (_isLoaded) return;
                _isLoaded = true;
                Value = _tempValue;
            };
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _tempValue = Value;
            Value = Orientation == Orientation.Horizontal ? Maximum : 0;
        }
    }
}