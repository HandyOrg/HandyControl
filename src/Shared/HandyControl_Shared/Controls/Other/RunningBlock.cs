using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using HandyControl.Data;

namespace HandyControl.Controls
{
    [TemplatePart(Name = ElementContent, Type = typeof(UIElement))]
    public class RunningBlock : ContentControl
    {
        private const string ElementContent = "PART_ContentElement";

        protected Storyboard _storyboard;

        private UIElement _elementContent;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _elementContent = GetTemplateChild(ElementContent) as UIElement;
        }

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            "Orientation", typeof(Orientation), typeof(RunningBlock), new FrameworkPropertyMetadata(default(Orientation), FrameworkPropertyMetadataOptions.AffectsRender));

        public Orientation Orientation
        {
            get => (Orientation) GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        public static readonly DependencyProperty DurationProperty = DependencyProperty.Register(
            "Duration", typeof(Duration), typeof(RunningBlock), new FrameworkPropertyMetadata(new Duration(TimeSpan.FromSeconds(5)), FrameworkPropertyMetadataOptions.AffectsRender));

        public Duration Duration
        {
            get => (Duration) GetValue(DurationProperty);
            set => SetValue(DurationProperty, value);
        }

        public static readonly DependencyProperty IsRunningProperty = DependencyProperty.Register(
            "IsRunning", typeof(bool), typeof(RunningBlock), new PropertyMetadata(ValueBoxes.TrueBox, (o, args) =>
            {
                var ctl = (RunningBlock)o;
                var v = (bool)args.NewValue;
                if (v)
                {
                    ctl._storyboard?.Resume();
                }
                else
                {
                    ctl._storyboard?.Pause();
                }
            }));

        public bool IsRunning
        {
            get => (bool) GetValue(IsRunningProperty);
            set => SetValue(IsRunningProperty, value);
        }

        private void UpdateContent()
        {
            if (_elementContent == null) return;

            _storyboard = new Storyboard();

            var animation = new DoubleAnimation(-ActualWidth, ActualWidth, Duration)
            {
                RepeatBehavior = RepeatBehavior.Forever
            };
            Storyboard.SetTarget(animation, _elementContent);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.X)"));
            _storyboard.Children.Add(animation);

            _storyboard.Begin();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            UpdateContent();
        }
    }
}