using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Threading;
using Avalonia.VisualTree;

namespace HandyControl.Controls;

public class GotoTop : Button
{
    public static readonly StyledProperty<Control?> TargetProperty =
        AvaloniaProperty.Register<GotoTop, Control?>(nameof(Target));

    public static readonly StyledProperty<bool> AnimatedProperty =
        AvaloniaProperty.Register<GotoTop, bool>(nameof(Animated), true);

    public static readonly StyledProperty<double> AnimationTimeProperty =
        AvaloniaProperty.Register<GotoTop, double>(nameof(AnimationTime), 200d);

    public static readonly StyledProperty<double> HidingHeightProperty =
        AvaloniaProperty.Register<GotoTop, double>(nameof(HidingHeight));

    public static readonly StyledProperty<bool> AutoHidingProperty =
        AvaloniaProperty.Register<GotoTop, bool>(nameof(AutoHiding), true);

    private ScrollViewer? _scrollViewer;
    private DispatcherTimer? _animationTimer;
    private DateTime _animationStartTime;
    private double _animationStartOffset;

    static GotoTop()
    {
        TargetProperty.Changed.AddClassHandler<GotoTop>((gotoTop, e) => gotoTop.CreateGotoAction(e.GetNewValue<Control?>()));
        AutoHidingProperty.Changed.AddClassHandler<GotoTop>((gotoTop, _) => gotoTop.UpdateVisibility());
        HidingHeightProperty.Changed.AddClassHandler<GotoTop>((gotoTop, _) => gotoTop.UpdateVisibility());
    }

    public GotoTop()
    {
        Loaded += (_, _) => CreateGotoAction(Target);
    }

    public Control? Target
    {
        get => GetValue(TargetProperty);
        set => SetValue(TargetProperty, value);
    }

    public bool Animated
    {
        get => GetValue(AnimatedProperty);
        set => SetValue(AnimatedProperty, value);
    }

    public double AnimationTime
    {
        get => GetValue(AnimationTimeProperty);
        set => SetValue(AnimationTimeProperty, value);
    }

    public double HidingHeight
    {
        get => GetValue(HidingHeightProperty);
        set => SetValue(HidingHeightProperty, value);
    }

    public bool AutoHiding
    {
        get => GetValue(AutoHidingProperty);
        set => SetValue(AutoHidingProperty, value);
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);

        if (_scrollViewer != null)
        {
            _scrollViewer.ScrollChanged -= ScrollViewerOnScrollChanged;
        }

        if (_animationTimer != null)
        {
            _animationTimer.Stop();
        }
    }

    public virtual void CreateGotoAction(Control? obj)
    {
        if (_scrollViewer != null)
        {
            _scrollViewer.ScrollChanged -= ScrollViewerOnScrollChanged;
        }

        _scrollViewer = null;
        var target = obj ?? this;

        if (target is ScrollViewer directScrollViewer)
        {
            _scrollViewer = directScrollViewer;
        }
        else
        {
            _scrollViewer = target.GetVisualDescendants().OfType<ScrollViewer>().FirstOrDefault();
        }

        if (_scrollViewer != null)
        {
            _scrollViewer.ScrollChanged += ScrollViewerOnScrollChanged;
        }

        UpdateVisibility();
    }

    private void ScrollViewerOnScrollChanged(object? sender, ScrollChangedEventArgs e)
    {
        UpdateVisibility();
    }

    private void UpdateVisibility()
    {
        if (!AutoHiding)
        {
            IsVisible = true;
            return;
        }

        IsVisible = (_scrollViewer?.Offset.Y ?? 0d) >= HidingHeight;
    }

    protected override void OnClick()
    {
        base.OnClick();

        if (_scrollViewer == null)
        {
            return;
        }

        if (!Animated || AnimationTime <= 0)
        {
            _scrollViewer.Offset = new Vector(_scrollViewer.Offset.X, 0);
            return;
        }

        StartAnimatedScroll();
    }

    private void StartAnimatedScroll()
    {
        if (_scrollViewer == null)
        {
            return;
        }

        _animationStartOffset = _scrollViewer.Offset.Y;
        if (_animationStartOffset <= 0)
        {
            return;
        }

        _animationStartTime = DateTime.Now;

        _animationTimer ??= new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(16) };
        _animationTimer.Tick -= AnimationTimerOnTick;
        _animationTimer.Tick += AnimationTimerOnTick;
        _animationTimer.Start();
    }

    private void AnimationTimerOnTick(object? sender, EventArgs e)
    {
        if (_scrollViewer == null)
        {
            _animationTimer?.Stop();
            return;
        }

        var total = Math.Max(AnimationTime, 1d);
        var elapsed = (DateTime.Now - _animationStartTime).TotalMilliseconds;
        var progress = Math.Clamp(elapsed / total, 0d, 1d);

        var current = _animationStartOffset * (1d - progress);
        _scrollViewer.Offset = new Vector(_scrollViewer.Offset.X, current);

        if (progress >= 1d)
        {
            _animationTimer?.Stop();
            _scrollViewer.Offset = new Vector(_scrollViewer.Offset.X, 0d);
        }
    }
}
