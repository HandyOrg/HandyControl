using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using HandyControl.Tools;

namespace HandyControl.Controls;

[TemplatePart(Name = ElementSliding, Type = typeof(FrameworkElement))]
public class SlidingTabContainer : ContentControl
{
    private const string ElementSliding = "PART_Sliding";

    private FrameworkElement _sliding = new();
    private System.Windows.Controls.TabControl _tabControl;

    public SlidingTabContainer()
    {
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        Loaded -= OnLoaded;
        OnSelectionChanged(null, null);
    }

    public override void OnApplyTemplate()
    {
        if (_tabControl is not null)
        {
            _tabControl.SelectionChanged -= OnSelectionChanged;
        }

        base.OnApplyTemplate();

        _tabControl = VisualHelper.GetParent<System.Windows.Controls.TabControl>(this);
        _sliding = GetTemplateChild(ElementSliding) as FrameworkElement;

        if (_tabControl is null)
        {
            return;
        }

        _tabControl.SelectionChanged += OnSelectionChanged;

        if (IsLoaded)
        {
            OnSelectionChanged(null, null);
        }
    }

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        base.OnRenderSizeChanged(sizeInfo);

        if (_tabControl is null)
        {
            return;
        }

        OnSelectionChanged(null, null);
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_sliding is null || !IsLoaded)
        {
            return;
        }

        if (_tabControl.ItemContainerGenerator.ContainerFromItem(_tabControl.SelectedItem) is not System.Windows.Controls.TabItem tabItem)
        {
            return;
        }

        var offset = VisualTreeHelper.GetOffset(tabItem);
        var tabWidth = tabItem.ActualWidth;
        var tabHeight = tabItem.ActualHeight;
        var useAnimation = _sliding.Width > 0 && _sliding.Height > 0;

        if (useAnimation)
        {
            var storyboard = new Storyboard();
            var easingFunction = new PowerEase
            {
                EasingMode = EasingMode.EaseOut,
            };

            var widthAnimation = AnimationHelper.CreateAnimation(tabWidth);
            widthAnimation.EasingFunction = easingFunction;
            Storyboard.SetTarget(widthAnimation, _sliding);
            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(WidthProperty.Name));
            storyboard.Children.Add(widthAnimation);

            var heightAnimation = AnimationHelper.CreateAnimation(tabHeight);
            heightAnimation.EasingFunction = easingFunction;
            Storyboard.SetTarget(heightAnimation, _sliding);
            Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(HeightProperty.Name));
            storyboard.Children.Add(heightAnimation);

            var xAnimation = AnimationHelper.CreateAnimation(offset.X);
            xAnimation.EasingFunction = easingFunction;
            Storyboard.SetTarget(xAnimation, _sliding);
            Storyboard.SetTargetProperty(xAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
            storyboard.Children.Add(xAnimation);

            var yAnimation = AnimationHelper.CreateAnimation(offset.Y);
            yAnimation.EasingFunction = easingFunction;
            Storyboard.SetTarget(yAnimation, _sliding);
            Storyboard.SetTargetProperty(yAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));
            storyboard.Children.Add(yAnimation);

            storyboard.Begin();
        }
        else
        {
            _sliding.Width = tabItem.ActualWidth;
            _sliding.Height = tabItem.ActualHeight;
            _sliding.RenderTransform = new TranslateTransform(offset.X, offset.Y);
        }
    }
}
