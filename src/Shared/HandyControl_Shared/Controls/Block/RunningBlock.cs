using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using HandyControl.Data;
using HandyControl.Expression.Drawing;

namespace HandyControl.Controls
{
    [TemplatePart(Name = ElementContent, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = ElementPanel, Type = typeof(Panel))]
    public class RunningBlock : ContentControl
    {
        private const string ElementContent = "PART_ContentElement";

        private const string ElementPanel = "PART_Panel";

        protected Storyboard _storyboard;

        private FrameworkElement _elementContent;

        private FrameworkElement _elementPanel;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _elementContent = GetTemplateChild(ElementContent) as FrameworkElement;
            _elementPanel = GetTemplateChild(ElementPanel) as Panel;
        }

        public static readonly DependencyProperty RunawayProperty = DependencyProperty.Register(
            "Runaway", typeof(bool), typeof(RunningBlock), new FrameworkPropertyMetadata(ValueBoxes.TrueBox, FrameworkPropertyMetadataOptions.AffectsRender));

        public bool Runaway
        {
            get => (bool) GetValue(RunawayProperty);
            set => SetValue(RunawayProperty, value);
        }

        public static readonly DependencyProperty AutoRunProperty = DependencyProperty.Register(
            "AutoRun", typeof(bool), typeof(RunningBlock), new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsRender));

        public bool AutoRun
        {
            get => (bool) GetValue(AutoRunProperty);
            set => SetValue(AutoRunProperty, value);
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

        public static readonly DependencyProperty SpeedProperty = DependencyProperty.Register(
            "Speed", typeof(double), typeof(RunningBlock), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsRender));

        public double Speed
        {
            get => (double) GetValue(SpeedProperty);
            set => SetValue(SpeedProperty, value);
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

        public static readonly DependencyProperty AutoReverseProperty = DependencyProperty.Register(
            "AutoReverse", typeof(bool), typeof(RunningBlock), new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsRender));

        public bool AutoReverse
        {
            get => (bool) GetValue(AutoReverseProperty);
            set => SetValue(AutoReverseProperty, value);
        }

        private void UpdateContent()
        {
            if (_elementContent == null || _elementPanel == null) return;

            _storyboard?.Stop();

            _elementPanel.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            _elementPanel.Width = _elementPanel.DesiredSize.Width;
            _elementPanel.Height = _elementPanel.DesiredSize.Height;

            double from;
            double to;
            PropertyPath propertyPath;

            if (Orientation == Orientation.Horizontal)
            {
                if (AutoRun && _elementPanel.Width < ActualWidth)
                {
                    return;
                }

                if (Runaway)
                {
                    from = -_elementPanel.Width;
                    to = ActualWidth;
                }
                else
                {
                    from = 0;
                    to = ActualWidth - _elementPanel.Width;
                    SetCurrentValue(AutoReverseProperty, ValueBoxes.TrueBox);
                }
                propertyPath = new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.X)");
            }
            else
            {
                if (AutoRun && _elementPanel.Height < ActualHeight)
                {
                    return;
                }

                if (Runaway)
                {
                    from = -_elementPanel.Height;
                    to = ActualHeight;
                }
                else
                {
                    from = 0;
                    to = ActualHeight - _elementPanel.Height;
                    SetCurrentValue(AutoReverseProperty, ValueBoxes.TrueBox);
                }
                propertyPath = new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.Y)");
            }

            var duration = double.IsNaN(Speed)
                ? Duration
                : !MathHelper.IsVerySmall(Speed)
                    ? TimeSpan.FromSeconds((to - from) / Speed)
                    : Duration;

            var animation = new DoubleAnimation(from, to, duration)
            {
                RepeatBehavior = RepeatBehavior.Forever,
                AutoReverse = AutoReverse
            };

            Storyboard.SetTargetProperty(animation, propertyPath);
            Storyboard.SetTarget(animation, _elementContent);

            _storyboard = new Storyboard();
            _storyboard.Children.Add(animation);
            _storyboard.Begin();
        }

        protected override void OnRender(DrawingContext drawingContext) => UpdateContent();
    }
}