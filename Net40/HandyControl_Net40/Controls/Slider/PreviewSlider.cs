using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using HandyControl.Data;


namespace HandyControl.Controls
{
    [TemplatePart(Name = PreviewContentKey, Type = typeof(ContentControl))]
    [TemplatePart(Name = TracKKey, Type = typeof(Track))]
    public class PreviewSlider : Slider
    {
        private const string PreviewContentKey = "PART_ContentBorder";

        private const string TracKKey = "PART_Track";

        private ContentControl _previewContent;

        private TranslateTransform _transform;

        private Track _track;

        /// <summary>
        ///     预览内容
        /// </summary>
        public static readonly DependencyProperty PreviewContentProperty = DependencyProperty.Register(
            "PreviewContent", typeof(object), typeof(PreviewSlider), new PropertyMetadata(default(object)));

        /// <summary>
        ///     预览内容
        /// </summary>
        public object PreviewContent
        {
            get => GetValue(PreviewContentProperty);
            set => SetValue(PreviewContentProperty, value);
        }

        public static readonly DependencyProperty PreviewPositionProperty = DependencyProperty.Register(
            "PreviewPosition", typeof(double), typeof(PreviewSlider), new PropertyMetadata(ValueBoxes.Double0Box));

        public double PreviewPosition
        {
            get => (double) GetValue(PreviewPositionProperty);
            set => SetValue(PreviewPositionProperty, value);
        }

        /// <summary>
        ///     值改变事件
        /// </summary>
        public static readonly RoutedEvent PreviewPositionChangedEvent =
            EventManager.RegisterRoutedEvent("PreviewPositionChanged", RoutingStrategy.Bubble,
                typeof(EventHandler<FunctionEventArgs<double>>), typeof(PreviewSlider));

        /// <summary>
        ///     值改变事件
        /// </summary>
        public event EventHandler<FunctionEventArgs<double>> PreviewPositionChanged
        {
            add => AddHandler(PreviewPositionChangedEvent, value);
            remove => RemoveHandler(PreviewPositionChangedEvent, value);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            var p = e.GetPosition(_track);
            _transform.X = p.X - _previewContent.ActualWidth/2;

            PreviewPosition = p.X / _track.ActualWidth * Maximum;
            RaiseEvent(new FunctionEventArgs<double>(PreviewPositionChangedEvent, this)
            {
                Info = PreviewPosition
            });
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _previewContent = Template.FindName(PreviewContentKey, this) as ContentControl;
            _track = Template.FindName(TracKKey, this) as Track;

            if (_previewContent != null)
            {
                _transform = new TranslateTransform();
                _previewContent.RenderTransform = _transform;
            }
        }
    }
}