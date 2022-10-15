using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using HandyControl.Data;
using HandyControl.Tools;

namespace HandyControl.Controls;

public class TransitioningContentControl : ContentControl
{
    private FrameworkElement _contentPresenter;

    private static Storyboard StoryboardBuildInDefault;

    private Storyboard _storyboardBuildIn;

    public TransitioningContentControl()
    {
        Loaded += TransitioningContentControl_Loaded;
        Unloaded += TransitioningContentControl_Unloaded;
    }

    public static readonly DependencyProperty TransitionModeProperty = DependencyProperty.Register(
        nameof(TransitionMode), typeof(TransitionMode), typeof(TransitioningContentControl), new PropertyMetadata(default(TransitionMode), OnTransitionModeChanged));

    private static void OnTransitionModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (TransitioningContentControl) d;
        ctl.OnTransitionModeChanged((TransitionMode) e.NewValue);
    }

    private void OnTransitionModeChanged(TransitionMode newValue)
    {
        _storyboardBuildIn = ResourceHelper.GetResourceInternal<Storyboard>($"{newValue}Transition");
        StartTransition();
    }

    public TransitionMode TransitionMode
    {
        get => (TransitionMode) GetValue(TransitionModeProperty);
        set => SetValue(TransitionModeProperty, value);
    }

    public static readonly DependencyProperty TransitionStoryboardProperty = DependencyProperty.Register(
        nameof(TransitionStoryboard), typeof(Storyboard), typeof(TransitioningContentControl), new PropertyMetadata(default(Storyboard)));

    public Storyboard TransitionStoryboard
    {
        get => (Storyboard) GetValue(TransitionStoryboardProperty);
        set => SetValue(TransitionStoryboardProperty, value);
    }

    private void TransitioningContentControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) => StartTransition();

    private void TransitioningContentControl_Loaded(object sender, RoutedEventArgs e)
    {
        IsVisibleChanged += TransitioningContentControl_IsVisibleChanged;
    }

    private void TransitioningContentControl_Unloaded(object sender, RoutedEventArgs e)
    {
        IsVisibleChanged -= TransitioningContentControl_IsVisibleChanged;
    }

    private void StartTransition()
    {
        if (_contentPresenter == null || !IsVisible) return;

        if (TransitionStoryboard != null)
        {
            TransitionStoryboard.Begin(_contentPresenter);
        }
        else if (_storyboardBuildIn != null)
        {
            _storyboardBuildIn?.Begin(_contentPresenter);
        }
        else
        {
            StoryboardBuildInDefault ??= ResourceHelper.GetResourceInternal<Storyboard>($"{default(TransitionMode)}Transition");
            StoryboardBuildInDefault?.Begin(_contentPresenter);
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

        StartTransition();
    }

    protected override void OnContentChanged(object oldContent, object newContent)
    {
        base.OnContentChanged(oldContent, newContent);

        if (newContent is null)
        {
            return;
        }

        StartTransition();
    }
}
