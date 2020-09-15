using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using HandyControl.Data;
using HandyControl.Tools;

namespace HandyControl.Controls
{
    public class TransitioningContentControl : ContentControl
    {
        private FrameworkElement _contentPresenter;

        public TransitioningContentControl()
        {
            Loaded += TransitioningContentControl_Loaded;
            IsVisibleChanged += TransitioningContentControl_IsVisibleChanged;
        }

        public static readonly DependencyProperty TransitionModeProperty = DependencyProperty.Register(
            "TransitionMode", typeof(TransitionMode), typeof(TransitioningContentControl), new PropertyMetadata(default(TransitionMode), OnTransitionModeChanged));

        private static void OnTransitionModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (TransitioningContentControl) d;
            ctl.StartTransition();
        }

        public TransitionMode TransitionMode
        {
            get => (TransitionMode) GetValue(TransitionModeProperty);
            set => SetValue(TransitionModeProperty, value);
        }

        public static readonly DependencyProperty TransitionStoryboardProperty = DependencyProperty.Register(
            "TransitionStoryboard", typeof(Storyboard), typeof(TransitioningContentControl), new PropertyMetadata(default(Storyboard)));

        public Storyboard TransitionStoryboard
        {
            get => (Storyboard) GetValue(TransitionStoryboardProperty);
            set => SetValue(TransitionStoryboardProperty, value);
        }

        private void TransitioningContentControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) => StartTransition();

        private void TransitioningContentControl_Loaded(object sender, RoutedEventArgs e) => StartTransition();

        private void StartTransition()
        {
            if (Visibility != Visibility.Visible || _contentPresenter == null) return;

            if (TransitionStoryboard != null)
            {
                TransitionStoryboard.Begin(_contentPresenter);
            }
            else if (Application.Current != null)
            {
                ResourceHelper.GetResource<Storyboard>($"{TransitionMode.ToString()}Transition")?.Begin(_contentPresenter);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _contentPresenter = VisualTreeHelper.GetChild(this, 0) as FrameworkElement;
            if (_contentPresenter != null)
            {
                _contentPresenter.RenderTransformOrigin = new Point(0.5, 0.5);
                _contentPresenter.RenderTransform = new TransformGroup
                {
                    Children =
                    {
                        new ScaleTransform(),
                        new SkewTransform(),
                        new RotateTransform(),
                        new TranslateTransform()
                    }
                };
            }
        }
    }
}
