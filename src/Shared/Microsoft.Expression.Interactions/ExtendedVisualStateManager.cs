using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using HandyControl.Data;

namespace HandyControl.Interactivity;

public class ExtendedVisualStateManager : VisualStateManager
{
    // Fields
    internal static readonly DependencyProperty CachedBackgroundProperty =
        DependencyProperty.RegisterAttached("CachedBackground", typeof(object), typeof(ExtendedVisualStateManager),
            new PropertyMetadata(null));

    internal static readonly DependencyProperty CachedEffectProperty =
        DependencyProperty.RegisterAttached("CachedEffect", typeof(Effect), typeof(ExtendedVisualStateManager),
            new PropertyMetadata(null));

    private static readonly List<DependencyProperty> ChildAffectingLayoutProperties;

    internal static readonly DependencyProperty CurrentStateProperty =
        DependencyProperty.RegisterAttached("CurrentState", typeof(VisualState), typeof(ExtendedVisualStateManager),
            new PropertyMetadata(null));

    internal static readonly DependencyProperty DidCacheBackgroundProperty =
        DependencyProperty.RegisterAttached("DidCacheBackground", typeof(bool), typeof(ExtendedVisualStateManager),
            new PropertyMetadata(ValueBoxes.FalseBox));

    private static readonly List<DependencyProperty> LayoutProperties;

    internal static readonly DependencyProperty LayoutStoryboardProperty =
        DependencyProperty.RegisterAttached("LayoutStoryboard", typeof(Storyboard),
            typeof(ExtendedVisualStateManager), new PropertyMetadata(null));

    private static Storyboard LayoutTransitionStoryboard;
    private static List<FrameworkElement> MovingElements;

    internal static readonly DependencyProperty OriginalLayoutValuesProperty =
        DependencyProperty.RegisterAttached("OriginalLayoutValues", typeof(List<OriginalLayoutValueRecord>),
            typeof(ExtendedVisualStateManager), new PropertyMetadata(null));

    public static readonly DependencyProperty RuntimeVisibilityPropertyProperty =
        DependencyProperty.RegisterAttached("RuntimeVisibilityProperty", typeof(DependencyProperty),
            typeof(ExtendedVisualStateManager), new PropertyMetadata(null));

    public static readonly DependencyProperty TransitionEffectProperty =
        DependencyProperty.RegisterAttached("TransitionEffect", typeof(TransitionEffect),
            typeof(ExtendedVisualStateManager), new PropertyMetadata(null));

    internal static readonly DependencyProperty TransitionEffectStoryboardProperty =
        DependencyProperty.RegisterAttached("TransitionEffectStoryboard", typeof(Storyboard),
            typeof(ExtendedVisualStateManager), new PropertyMetadata(null));

    public static readonly DependencyProperty UseFluidLayoutProperty =
        DependencyProperty.RegisterAttached("UseFluidLayout", typeof(bool), typeof(ExtendedVisualStateManager),
            new PropertyMetadata(ValueBoxes.FalseBox));

    private bool _changingState;

    // Methods
    static ExtendedVisualStateManager()
    {
        var list = new List<DependencyProperty>
        {
            Grid.ColumnProperty,
            Grid.ColumnSpanProperty,
            Grid.RowProperty,
            Grid.RowSpanProperty,
            Canvas.LeftProperty,
            Canvas.TopProperty,
            FrameworkElement.WidthProperty,
            FrameworkElement.HeightProperty,
            FrameworkElement.MinWidthProperty,
            FrameworkElement.MinHeightProperty,
            FrameworkElement.MaxWidthProperty,
            FrameworkElement.MaxHeightProperty,
            FrameworkElement.MarginProperty,
            FrameworkElement.HorizontalAlignmentProperty,
            FrameworkElement.VerticalAlignmentProperty,
            UIElement.VisibilityProperty,
            StackPanel.OrientationProperty
        };
        LayoutProperties = list;
        var list2 = new List<DependencyProperty>
        {
            StackPanel.OrientationProperty
        };
        ChildAffectingLayoutProperties = list2;
    }

    // Properties
    public static bool IsRunningFluidLayoutTransition =>
        LayoutTransitionStoryboard != null;

    private static void AnimateTransitionEffect(FrameworkElement stateGroupsRoot, VisualTransition transition)
    {
        var element = new DoubleAnimation
        {
            Duration = transition.GeneratedDuration,
            EasingFunction = transition.GeneratedEasingFunction,
            From = 0.0,
            To = 1.0
        };
        var sb = new Storyboard
        {
            Duration = transition.GeneratedDuration,
            Children = { element }
        };
        Storyboard.SetTarget(element, stateGroupsRoot);
        Storyboard.SetTargetProperty(element,
            new PropertyPath("(0).(1)", UIElement.EffectProperty, TransitionEffect.ProgressProperty));
        if (stateGroupsRoot is Panel panel && panel.Background == null)
        {
            SetDidCacheBackground(panel, true);
            TransferLocalValue(panel, Panel.BackgroundProperty, CachedBackgroundProperty);
            panel.Background = Brushes.Transparent;
        }
        sb.Completed += delegate
        {
            if (Equals(GetTransitionEffectStoryboard(stateGroupsRoot), sb))
                FinishTransitionEffectAnimation(stateGroupsRoot);
        };
        SetTransitionEffectStoryboard(stateGroupsRoot, sb);
        sb.Begin();
    }

    private static object CacheLocalValueHelper(DependencyObject dependencyObject, DependencyProperty property)
    {
        return dependencyObject.ReadLocalValue(property);
    }

    private static void control_LayoutUpdated(object sender, EventArgs e)
    {
        if (LayoutTransitionStoryboard != null)
            foreach (var element in MovingElements)
            {
                if (element.Parent is WrapperCanvas parent)
                {
                    var layoutRect = GetLayoutRect(parent);
                    var newRect = parent.NewRect;
                    var renderTransform = parent.RenderTransform as TranslateTransform;
                    var num = renderTransform?.X ?? 0.0;
                    var num2 = renderTransform?.Y ?? 0.0;
                    var num3 = newRect.Left - layoutRect.Left;
                    var num4 = newRect.Top - layoutRect.Top;
                    if (Math.Abs(num - num3) > 0.001 || Math.Abs(num2 - num4) > 0.001)
                    {
                        if (renderTransform == null)
                        {
                            renderTransform = new TranslateTransform();
                            parent.RenderTransform = renderTransform;
                        }
                        renderTransform.X = num3;
                        renderTransform.Y = num4;
                    }
                }
            }
    }

    private static void CopyLayoutProperties(FrameworkElement source, FrameworkElement target, bool restoring)
    {
        var canvas = restoring ? (WrapperCanvas) source : (WrapperCanvas) target;
        if (canvas.LocalValueCache == null)
            canvas.LocalValueCache = new Dictionary<DependencyProperty, object>();
        foreach (var property in LayoutProperties)
            if (!ChildAffectingLayoutProperties.Contains(property))
                if (restoring)
                {
                    ReplaceCachedLocalValueHelper(target, property, canvas.LocalValueCache[property]);
                }
                else
                {
                    var obj2 = target.GetValue(property);
                    var obj3 = CacheLocalValueHelper(source, property);
                    canvas.LocalValueCache[property] = obj3;
                    if (IsVisibilityProperty(property))
                        canvas.DestinationVisibilityCache = (Visibility) source.GetValue(property);
                    else
                        target.SetValue(property, source.GetValue(property));
                    source.SetValue(property, obj2);
                }
    }

    private static Storyboard CreateLayoutTransitionStoryboard(VisualTransition transition,
        List<FrameworkElement> movingElements, Dictionary<FrameworkElement, double> oldOpacities)
    {
        var duration = transition?.GeneratedDuration ?? new Duration(TimeSpan.Zero);
        var generatedEasingFunction = transition?.GeneratedEasingFunction;
        var storyboard = new Storyboard
        {
            Duration = duration
        };
        foreach (var element in movingElements)
        {
            if (element.Parent is WrapperCanvas parent)
            {
                var animation = new DoubleAnimation
                {
                    From = 1.0,
                    To = 0.0,
                    Duration = duration,
                    EasingFunction = generatedEasingFunction
                };
                Storyboard.SetTarget(animation, parent);
                Storyboard.SetTargetProperty(animation, new PropertyPath(WrapperCanvas.SimulationProgressProperty));
                storyboard.Children.Add(animation);
                parent.SimulationProgress = 1.0;
                var newRect = parent.NewRect;
                if (!IsClose(parent.Width, newRect.Width))
                {
                    var animation3 = new DoubleAnimation
                    {
                        From = newRect.Width,
                        To = newRect.Width,
                        Duration = duration
                    };
                    Storyboard.SetTarget(animation3, parent);
                    Storyboard.SetTargetProperty(animation3, new PropertyPath(FrameworkElement.WidthProperty));
                    storyboard.Children.Add(animation3);
                }
                if (!IsClose(parent.Height, newRect.Height))
                {
                    var animation5 = new DoubleAnimation
                    {
                        From = newRect.Height,
                        To = newRect.Height,
                        Duration = duration
                    };
                    Storyboard.SetTarget(animation5, parent);
                    Storyboard.SetTargetProperty(animation5, new PropertyPath(FrameworkElement.HeightProperty));
                    storyboard.Children.Add(animation5);
                }
                if (parent.DestinationVisibilityCache == Visibility.Collapsed)
                {
                    var margin = parent.Margin;
                    if (!IsClose(margin.Left, 0.0) || !IsClose(margin.Top, 0.0) || !IsClose(margin.Right, 0.0) ||
                        !IsClose(margin.Bottom, 0.0))
                    {
                        var frames = new ObjectAnimationUsingKeyFrames
                        {
                            Duration = duration
                        };
                        var frame2 = new DiscreteObjectKeyFrame
                        {
                            KeyTime = TimeSpan.Zero
                        };
                        var thickness2 = new Thickness();
                        frame2.Value = thickness2;
                        var keyFrame = frame2;
                        frames.KeyFrames.Add(keyFrame);
                        Storyboard.SetTarget(frames, parent);
                        Storyboard.SetTargetProperty(frames, new PropertyPath(FrameworkElement.MarginProperty));
                        storyboard.Children.Add(frames);
                    }
                    if (!IsClose(parent.MinWidth, 0.0))
                    {
                        var animation7 = new DoubleAnimation
                        {
                            From = 0.0,
                            To = 0.0,
                            Duration = duration
                        };
                        Storyboard.SetTarget(animation7, parent);
                        Storyboard.SetTargetProperty(animation7,
                            new PropertyPath(FrameworkElement.MinWidthProperty));
                        storyboard.Children.Add(animation7);
                    }
                    if (!IsClose(parent.MinHeight, 0.0))
                    {
                        var animation9 = new DoubleAnimation
                        {
                            From = 0.0,
                            To = 0.0,
                            Duration = duration
                        };
                        Storyboard.SetTarget(animation9, parent);
                        Storyboard.SetTargetProperty(animation9,
                            new PropertyPath(FrameworkElement.MinHeightProperty));
                        storyboard.Children.Add(animation9);
                    }
                }
            }
        }
        foreach (var element2 in oldOpacities.Keys)
        {
            if (element2.Parent is WrapperCanvas canvas2)
            {
                var a = oldOpacities[element2];
                var num2 = canvas2.DestinationVisibilityCache == Visibility.Visible ? 1.0 : 0.0;
                if (!IsClose(a, 1.0) || !IsClose(num2, 1.0))
                {
                    var animation11 = new DoubleAnimation
                    {
                        From = a,
                        To = num2,
                        Duration = duration,
                        EasingFunction = generatedEasingFunction
                    };
                    Storyboard.SetTarget(animation11, canvas2);
                    Storyboard.SetTargetProperty(animation11, new PropertyPath(UIElement.OpacityProperty));
                    storyboard.Children.Add(animation11);
                }
            }
        }
        return storyboard;
    }

    private static Storyboard ExtractLayoutStoryboard(VisualState state)
    {
        Storyboard layoutStoryboard = null;
        if (state.Storyboard != null)
        {
            layoutStoryboard = GetLayoutStoryboard(state.Storyboard);
            if (layoutStoryboard == null)
            {
                layoutStoryboard = new Storyboard();
                for (var i = state.Storyboard.Children.Count - 1; i >= 0; i--)
                {
                    var timeline = state.Storyboard.Children[i];
                    if (LayoutPropertyFromTimeline(timeline, false) != null)
                    {
                        state.Storyboard.Children.RemoveAt(i);
                        layoutStoryboard.Children.Add(timeline);
                    }
                }
                SetLayoutStoryboard(state.Storyboard, layoutStoryboard);
            }
        }
        if (layoutStoryboard == null)
            return new Storyboard();
        return layoutStoryboard;
    }

    private static List<FrameworkElement> FindTargetElements(FrameworkElement control,
        FrameworkElement templateRoot, Storyboard layoutStoryboard,
        List<OriginalLayoutValueRecord> originalValueRecords, List<FrameworkElement> movingElements)
    {
        var list = new List<FrameworkElement>();
        if (movingElements != null)
            list.AddRange(movingElements);
        foreach (var timeline in layoutStoryboard.Children)
        {
            var item = (FrameworkElement) GetTimelineTarget(control, templateRoot, timeline);
            if (item != null)
            {
                if (!list.Contains(item))
                    list.Add(item);
                if (ChildAffectingLayoutProperties.Contains(LayoutPropertyFromTimeline(timeline, false)))
                {
                    if (item is Panel panel)
                        foreach (FrameworkElement element2 in panel.Children)
                            if (!list.Contains(element2) && !(element2 is WrapperCanvas))
                                list.Add(element2);
                }
            }
        }
        foreach (var record in originalValueRecords)
        {
            if (!list.Contains(record.Element))
                list.Add(record.Element);
            if (ChildAffectingLayoutProperties.Contains(record.Property))
            {
                if (record.Element is Panel element)
                    foreach (FrameworkElement element3 in element.Children)
                        if (!list.Contains(element3) && !(element3 is WrapperCanvas))
                            list.Add(element3);
            }
        }
        for (var i = 0; i < list.Count; i++)
        {
            var reference = list[i];
            var parent = VisualTreeHelper.GetParent(reference) as FrameworkElement;
            if (movingElements != null && movingElements.Contains(reference) && parent is WrapperCanvas)
                parent = VisualTreeHelper.GetParent(parent) as FrameworkElement;
            if (parent != null)
            {
                if (!list.Contains(parent))
                    list.Add(parent);
                for (var j = 0; j < VisualTreeHelper.GetChildrenCount(parent); j++)
                {
                    if (VisualTreeHelper.GetChild(parent, j) is FrameworkElement child && !list.Contains(child) && !(child is WrapperCanvas))
                        list.Add(child);
                }
            }
        }
        return list;
    }

    private static VisualTransition FindTransition(VisualStateGroup group, VisualState previousState,
        VisualState state)
    {
        var str = previousState != null ? previousState.Name : string.Empty;
        var str2 = state != null ? state.Name : string.Empty;
        var num = -1;
        VisualTransition transition = null;
        foreach (VisualTransition transition2 in group.Transitions)
        {
            var num2 = 0;
            if (transition2.From == str)
                num2++;
            else if (!string.IsNullOrEmpty(transition2.From))
                continue;
            if (transition2.To == str2)
                num2 += 2;
            else if (!string.IsNullOrEmpty(transition2.To))
                continue;
            if (num2 > num)
            {
                num = num2;
                transition = transition2;
            }
        }
        return transition;
    }

    private static bool FinishesWithZeroOpacity(FrameworkElement control, FrameworkElement stateGroupsRoot,
        VisualState state, VisualState previousState)
    {
        if (state.Storyboard != null)
            foreach (var timeline in state.Storyboard.Children)
                if (TimelineIsAnimatingRootOpacity(timeline, control, stateGroupsRoot))
                {
                    var valueFromTimeline = GetValueFromTimeline(timeline, out var flag);
                    return flag && valueFromTimeline is double d && Math.Abs(d) < 0.001;
                }
        if (previousState?.Storyboard == null)
            return Math.Abs(stateGroupsRoot.Opacity) < 0.001;
        foreach (var timeline2 in previousState.Storyboard.Children)
            TimelineIsAnimatingRootOpacity(timeline2, control, stateGroupsRoot);
        var animationBaseValue = (double) stateGroupsRoot.GetAnimationBaseValue(UIElement.OpacityProperty);
        return Math.Abs(animationBaseValue) < 0.001;
    }

    private static void FinishTransitionEffectAnimation(FrameworkElement stateGroupsRoot)
    {
        SetTransitionEffectStoryboard(stateGroupsRoot, null);
        TransferLocalValue(stateGroupsRoot, CachedEffectProperty, UIElement.EffectProperty);
        if (GetDidCacheBackground(stateGroupsRoot))
        {
            TransferLocalValue(stateGroupsRoot, CachedBackgroundProperty, Panel.BackgroundProperty);
            SetDidCacheBackground(stateGroupsRoot, false);
        }
    }

    internal static object GetCachedBackground(DependencyObject obj)
    {
        return obj.GetValue(CachedBackgroundProperty);
    }

    internal static Effect GetCachedEffect(DependencyObject obj)
    {
        return (Effect) obj.GetValue(CachedEffectProperty);
    }

    internal static VisualState GetCurrentState(DependencyObject obj)
    {
        return (VisualState) obj.GetValue(CurrentStateProperty);
    }

    internal static bool GetDidCacheBackground(DependencyObject obj)
    {
        return (bool) obj.GetValue(DidCacheBackgroundProperty);
    }

    internal static Rect GetLayoutRect(FrameworkElement element)
    {
        var actualWidth = element.ActualWidth;
        var actualHeight = element.ActualHeight;
        if (element is Image || element is MediaElement)
            if (element.Parent is Canvas)
            {
                actualWidth = double.IsNaN(element.Width) ? actualWidth : element.Width;
                actualHeight = double.IsNaN(element.Height) ? actualHeight : element.Height;
            }
            else
            {
                actualWidth = element.RenderSize.Width;
                actualHeight = element.RenderSize.Height;
            }
        actualWidth = element.Visibility == Visibility.Collapsed ? 0.0 : actualWidth;
        actualHeight = element.Visibility == Visibility.Collapsed ? 0.0 : actualHeight;
        var margin = element.Margin;
        var layoutSlot = LayoutInformation.GetLayoutSlot(element);
        var x = 0.0;
        var y = 0.0;
        x = element.HorizontalAlignment switch
        {
            HorizontalAlignment.Left => layoutSlot.Left + margin.Left,
            HorizontalAlignment.Center => (layoutSlot.Left + margin.Left + layoutSlot.Right - margin.Right) / 2.0 -
                                          actualWidth / 2.0,
            HorizontalAlignment.Right => layoutSlot.Right - margin.Right - actualWidth,
            HorizontalAlignment.Stretch => Math.Max(layoutSlot.Left + margin.Left,
                (layoutSlot.Left + margin.Left + layoutSlot.Right - margin.Right) / 2.0 - actualWidth / 2.0),
            _ => x
        };
        y = element.VerticalAlignment switch
        {
            VerticalAlignment.Top => layoutSlot.Top + margin.Top,
            VerticalAlignment.Center => (layoutSlot.Top + margin.Top + layoutSlot.Bottom - margin.Bottom) / 2.0 -
                                        actualHeight / 2.0,
            VerticalAlignment.Bottom => layoutSlot.Bottom - margin.Bottom - actualHeight,
            VerticalAlignment.Stretch => Math.Max(layoutSlot.Top + margin.Top,
                (layoutSlot.Top + margin.Top + layoutSlot.Bottom - margin.Bottom) / 2.0 - actualHeight / 2.0),
            _ => y
        };
        return new Rect(x, y, actualWidth, actualHeight);
    }

    internal static Storyboard GetLayoutStoryboard(DependencyObject obj)
    {
        return (Storyboard) obj.GetValue(LayoutStoryboardProperty);
    }

    private static Dictionary<FrameworkElement, double> GetOldOpacities(FrameworkElement control,
        FrameworkElement templateRoot, Storyboard layoutStoryboard,
        List<OriginalLayoutValueRecord> originalValueRecords, List<FrameworkElement> movingElements)
    {
        var dictionary = new Dictionary<FrameworkElement, double>();
        if (movingElements != null)
            foreach (var element in movingElements)
            {
                if (element.Parent is WrapperCanvas parent)
                    dictionary.Add(element, parent.Opacity);
            }
        for (var i = originalValueRecords.Count - 1; i >= 0; i--)
        {
            var record = originalValueRecords[i];
            if (IsVisibilityProperty(record.Property) && !dictionary.TryGetValue(record.Element, out var num2))
            {
                num2 = (Visibility) record.Element.GetValue(record.Property) == Visibility.Visible ? 1.0 : 0.0;
                dictionary.Add(record.Element, num2);
            }
        }
        foreach (var timeline in layoutStoryboard.Children)
        {
            var key = (FrameworkElement) GetTimelineTarget(control, templateRoot, timeline);
            var property = LayoutPropertyFromTimeline(timeline, true);
            if (key != null && IsVisibilityProperty(property) && !dictionary.TryGetValue(key, out var num3))
            {
                num3 = (Visibility) key.GetValue(property) == Visibility.Visible ? 1.0 : 0.0;
                dictionary.Add(key, num3);
            }
        }
        return dictionary;
    }

    internal static List<OriginalLayoutValueRecord> GetOriginalLayoutValues(DependencyObject obj)
    {
        return (List<OriginalLayoutValueRecord>) obj.GetValue(OriginalLayoutValuesProperty);
    }

    private static Dictionary<FrameworkElement, Rect> GetRectsOfTargets(IEnumerable<FrameworkElement> targets,
        ICollection<FrameworkElement> movingElements)
    {
        var dictionary = new Dictionary<FrameworkElement, Rect>();
        foreach (var element in targets)
        {
            Rect layoutRect;
            if (movingElements != null && movingElements.Contains(element) && element.Parent is WrapperCanvas parent)
            {
                layoutRect = GetLayoutRect(parent);
                var renderTransform = parent.RenderTransform as TranslateTransform;
                var left = Canvas.GetLeft(element);
                var top = Canvas.GetTop(element);
                layoutRect = new Rect(
                    layoutRect.Left + (double.IsNaN(left) ? 0.0 : left) +
                    (renderTransform?.X ?? 0.0),
                    layoutRect.Top + (double.IsNaN(top) ? 0.0 : top) +
                    (renderTransform?.Y ?? 0.0), element.ActualWidth, element.ActualHeight);
            }
            else
            {
                layoutRect = GetLayoutRect(element);
            }
            dictionary.Add(element, layoutRect);
        }
        return dictionary;
    }

    public static DependencyProperty GetRuntimeVisibilityProperty(DependencyObject obj)
    {
        return (DependencyProperty) obj.GetValue(RuntimeVisibilityPropertyProperty);
    }

    private static object GetTimelineTarget(FrameworkElement control, FrameworkElement templateRoot,
        Timeline timeline)
    {
        var targetName = Storyboard.GetTargetName(timeline);
        if (string.IsNullOrEmpty(targetName))
            return null;
        if (control is UserControl)
            return control.FindName(targetName);
        return templateRoot.FindName(targetName);
    }

    public static TransitionEffect GetTransitionEffect(DependencyObject obj)
    {
        return (TransitionEffect) obj.GetValue(TransitionEffectProperty);
    }

    internal static Storyboard GetTransitionEffectStoryboard(DependencyObject obj)
    {
        return (Storyboard) obj.GetValue(TransitionEffectStoryboardProperty);
    }

    public static bool GetUseFluidLayout(DependencyObject obj)
    {
        return (bool) obj.GetValue(UseFluidLayoutProperty);
    }

    private static object GetValueFromTimeline(Timeline timeline, out bool gotValue)
    {
        if (timeline is ObjectAnimationUsingKeyFrames frames)
        {
            gotValue = true;
            return frames.KeyFrames[0].Value;
        }
        if (timeline is DoubleAnimationUsingKeyFrames frames2)
        {
            gotValue = true;
            return frames2.KeyFrames[0].Value;
        }
        if (timeline is DoubleAnimation animation)
        {
            gotValue = true;
            return animation.To;
        }
        if (timeline is ThicknessAnimationUsingKeyFrames frames3)
        {
            gotValue = true;
            return frames3.KeyFrames[0].Value;
        }
        if (timeline is ThicknessAnimation animation2)
        {
            gotValue = true;
            return animation2.To;
        }
        if (timeline is Int32AnimationUsingKeyFrames frames4)
        {
            gotValue = true;
            return frames4.KeyFrames[0].Value;
        }
        if (timeline is Int32Animation animation3)
        {
            gotValue = true;
            return animation3.To;
        }
        gotValue = false;
        return null;
    }

    protected override bool GoToStateCore(FrameworkElement control, FrameworkElement stateGroupsRoot,
        string stateName, VisualStateGroup group, VisualState state, bool useTransitions)
    {
        if (_changingState)
            return false;
        var currentState = GetCurrentState(group);
        if (!Equals(currentState, state))
        {
            var transition = FindTransition(group, currentState, state);
            var animateWithTransitionEffect =
                PrepareTransitionEffectImage(stateGroupsRoot, useTransitions, transition);
            if (!GetUseFluidLayout(group))
                return TransitionEffectAwareGoToStateCore(control, stateGroupsRoot, stateName, group, state,
                    useTransitions, transition, animateWithTransitionEffect, currentState);
            var layoutStoryboard = ExtractLayoutStoryboard(state);
            var originalLayoutValues = GetOriginalLayoutValues(group);
            if (originalLayoutValues == null)
            {
                originalLayoutValues = new List<OriginalLayoutValueRecord>();
                SetOriginalLayoutValues(group, originalLayoutValues);
            }
            if (!useTransitions)
            {
                if (LayoutTransitionStoryboard != null)
                    StopAnimations();
                var flag2 = TransitionEffectAwareGoToStateCore(control, stateGroupsRoot, stateName, group, state,
                    false, transition, animateWithTransitionEffect, currentState);
                SetLayoutStoryboardProperties(control, stateGroupsRoot, layoutStoryboard, originalLayoutValues);
                return flag2;
            }
            if (layoutStoryboard.Children.Count == 0 && originalLayoutValues.Count == 0)
                return TransitionEffectAwareGoToStateCore(control, stateGroupsRoot, stateName, group, state,
                    true, transition, animateWithTransitionEffect, currentState);
            try
            {
                _changingState = true;
                stateGroupsRoot.UpdateLayout();
                var targets = FindTargetElements(control, stateGroupsRoot, layoutStoryboard, originalLayoutValues,
                    MovingElements);
                var rectsOfTargets = GetRectsOfTargets(targets, MovingElements);
                var oldOpacities = GetOldOpacities(control, stateGroupsRoot, layoutStoryboard, originalLayoutValues,
                    MovingElements);
                if (LayoutTransitionStoryboard != null)
                {
                    stateGroupsRoot.LayoutUpdated -= control_LayoutUpdated;
                    StopAnimations();
                    stateGroupsRoot.UpdateLayout();
                }
                TransitionEffectAwareGoToStateCore(control, stateGroupsRoot, stateName, group, state,
                    true, transition, animateWithTransitionEffect, currentState);
                SetLayoutStoryboardProperties(control, stateGroupsRoot, layoutStoryboard, originalLayoutValues);
                stateGroupsRoot.UpdateLayout();
                var newRects = GetRectsOfTargets(targets, null);
                MovingElements = new List<FrameworkElement>();
                foreach (var element in targets)
                    if (rectsOfTargets[element] != newRects[element])
                        MovingElements.Add(element);
                foreach (var element2 in oldOpacities.Keys)
                    if (!MovingElements.Contains(element2))
                        MovingElements.Add(element2);
                WrapMovingElementsInCanvases(MovingElements, rectsOfTargets, newRects);
                stateGroupsRoot.LayoutUpdated += control_LayoutUpdated;
                LayoutTransitionStoryboard =
                    CreateLayoutTransitionStoryboard(transition, MovingElements, oldOpacities);

                void Handler(object sender, EventArgs e)
                {
                    stateGroupsRoot.LayoutUpdated -= control_LayoutUpdated;
                    StopAnimations();
                }

                LayoutTransitionStoryboard.Completed += Handler;
                LayoutTransitionStoryboard.Begin();
            }
            finally
            {
                _changingState = false;
            }
        }
        return true;
    }

    private static bool IsClose(double a, double b)
    {
        return Math.Abs(a - b) < 1E-07;
    }

    private static bool IsVisibilityProperty(DependencyProperty property)
    {
        if (property != UIElement.VisibilityProperty)
            return property.Name == "RuntimeVisibility";
        return true;
    }

    private static DependencyProperty LayoutPropertyFromTimeline(Timeline timeline, bool forceRuntimeProperty)
    {
        var targetProperty = Storyboard.GetTargetProperty(timeline);
        if (targetProperty != null &&
            targetProperty.PathParameters.Count != 0)
        {
            if (targetProperty.PathParameters[0] is DependencyProperty item)
            {
                if (item.Name == "RuntimeVisibility" &&
                    item.OwnerType.Name.EndsWith("DesignTimeProperties", StringComparison.Ordinal))
                {
                    if (!LayoutProperties.Contains(item))
                        LayoutProperties.Add(item);
                    if (!forceRuntimeProperty)
                        return UIElement.VisibilityProperty;
                    return item;
                }
                if (item.Name == "RuntimeWidth" &&
                    item.OwnerType.Name.EndsWith("DesignTimeProperties", StringComparison.Ordinal))
                {
                    if (!LayoutProperties.Contains(item))
                        LayoutProperties.Add(item);
                    if (!forceRuntimeProperty)
                        return FrameworkElement.WidthProperty;
                    return item;
                }
                if (item.Name == "RuntimeHeight" &&
                    item.OwnerType.Name.EndsWith("DesignTimeProperties", StringComparison.Ordinal))
                {
                    if (!LayoutProperties.Contains(item))
                        LayoutProperties.Add(item);
                    if (!forceRuntimeProperty)
                        return FrameworkElement.HeightProperty;
                    return item;
                }
                if (LayoutProperties.Contains(item))
                    return item;
            }
        }
        return null;
    }

    private static bool PrepareTransitionEffectImage(FrameworkElement stateGroupsRoot, bool useTransitions,
        VisualTransition transition)
    {
        var effect = transition == null ? null : GetTransitionEffect(transition);
        var flag = false;
        if (effect != null)
        {
            effect = effect.CloneCurrentValue();
            if (useTransitions)
            {
                flag = true;
                var pixelWidth = (int) Math.Max(1.0, stateGroupsRoot.ActualWidth);
                var pixelHeight = (int) Math.Max(1.0, stateGroupsRoot.ActualHeight);
                var bitmap = new RenderTargetBitmap(pixelWidth, pixelHeight, 96.0, 96.0, PixelFormats.Pbgra32);
                bitmap.Render(stateGroupsRoot);
                var brush = new ImageBrush
                {
                    ImageSource = bitmap
                };
                effect.OldImage = brush;
            }
            var transitionEffectStoryboard = GetTransitionEffectStoryboard(stateGroupsRoot);
            if (transitionEffectStoryboard != null)
            {
                transitionEffectStoryboard.Stop();
                FinishTransitionEffectAnimation(stateGroupsRoot);
            }
            if (useTransitions)
            {
                TransferLocalValue(stateGroupsRoot, UIElement.EffectProperty, CachedEffectProperty);
                stateGroupsRoot.Effect = effect;
            }
        }
        return flag;
    }

    private static void ReplaceCachedLocalValueHelper(FrameworkElement element, DependencyProperty property,
        object value)
    {
        if (value == DependencyProperty.UnsetValue)
        {
            element.ClearValue(property);
        }
        else
        {
            if (value is BindingExpressionBase base2)
                element.SetBinding(property, base2.ParentBindingBase);
            else
                element.SetValue(property, value);
        }
    }

    internal static void SetCachedBackground(DependencyObject obj, object value)
    {
        obj.SetValue(CachedBackgroundProperty, value);
    }

    internal static void SetCachedEffect(DependencyObject obj, Effect value)
    {
        obj.SetValue(CachedEffectProperty, value);
    }

    internal static void SetCurrentState(DependencyObject obj, VisualState value)
    {
        obj.SetValue(CurrentStateProperty, value);
    }

    internal static void SetDidCacheBackground(DependencyObject obj, bool value)
    {
        obj.SetValue(DidCacheBackgroundProperty, ValueBoxes.BooleanBox(value));
    }

    internal static void SetLayoutStoryboard(DependencyObject obj, Storyboard value)
    {
        obj.SetValue(LayoutStoryboardProperty, value);
    }

    private static void SetLayoutStoryboardProperties(FrameworkElement control, FrameworkElement templateRoot,
        Storyboard layoutStoryboard, List<OriginalLayoutValueRecord> originalValueRecords)
    {
        foreach (var record in originalValueRecords)
            ReplaceCachedLocalValueHelper(record.Element, record.Property, record.Value);
        originalValueRecords.Clear();
        foreach (var timeline in layoutStoryboard.Children)
        {
            var dependencyObject = (FrameworkElement) GetTimelineTarget(control, templateRoot, timeline);
            var property = LayoutPropertyFromTimeline(timeline, true);
            if (dependencyObject != null && property != null)
            {
                var valueFromTimeline = GetValueFromTimeline(timeline, out var flag);
                if (flag)
                {
                    var item = new OriginalLayoutValueRecord
                    {
                        Element = dependencyObject,
                        Property = property,
                        Value = CacheLocalValueHelper(dependencyObject, property)
                    };
                    originalValueRecords.Add(item);
                    dependencyObject.SetValue(property, valueFromTimeline);
                }
            }
        }
    }

    internal static void SetOriginalLayoutValues(DependencyObject obj, List<OriginalLayoutValueRecord> value)
    {
        obj.SetValue(OriginalLayoutValuesProperty, value);
    }

    public static void SetRuntimeVisibilityProperty(DependencyObject obj, DependencyProperty value)
    {
        obj.SetValue(RuntimeVisibilityPropertyProperty, value);
    }

    public static void SetTransitionEffect(DependencyObject obj, TransitionEffect value)
    {
        obj.SetValue(TransitionEffectProperty, value);
    }

    internal static void SetTransitionEffectStoryboard(DependencyObject obj, Storyboard value)
    {
        obj.SetValue(TransitionEffectStoryboardProperty, value);
    }

    public static void SetUseFluidLayout(DependencyObject obj, bool value)
    {
        obj.SetValue(UseFluidLayoutProperty, ValueBoxes.BooleanBox(value));
    }

    private static void StopAnimations()
    {
        if (LayoutTransitionStoryboard != null)
        {
            LayoutTransitionStoryboard.Stop();
            LayoutTransitionStoryboard = null;
        }
        if (MovingElements != null)
        {
            UnwrapMovingElementsFromCanvases(MovingElements);
            MovingElements = null;
        }
    }

    private static bool TimelineIsAnimatingRootOpacity(Timeline timeline, FrameworkElement control,
        FrameworkElement stateGroupsRoot)
    {
        if (!Equals(GetTimelineTarget(control, stateGroupsRoot, timeline), stateGroupsRoot))
            return false;
        var targetProperty = Storyboard.GetTargetProperty(timeline);
        return targetProperty != null &&
               targetProperty.PathParameters.Count != 0 &&
               targetProperty.PathParameters[0] == UIElement.OpacityProperty;
    }

    private static void TransferLocalValue(FrameworkElement element, DependencyProperty sourceProperty,
        DependencyProperty destProperty)
    {
        var obj2 = CacheLocalValueHelper(element, sourceProperty);
        ReplaceCachedLocalValueHelper(element, destProperty, obj2);
    }

    private bool TransitionEffectAwareGoToStateCore(FrameworkElement control, FrameworkElement stateGroupsRoot,
        string stateName, VisualStateGroup group, VisualState state, bool useTransitions,
        VisualTransition transition, bool animateWithTransitionEffect, VisualState previousState)
    {
        IEasingFunction generatedEasingFunction = null;
        if (animateWithTransitionEffect)
        {
            generatedEasingFunction = transition.GeneratedEasingFunction;
            var function2 = new DummyEasingFunction
            {
                DummyValue = FinishesWithZeroOpacity(control, stateGroupsRoot, state, previousState) ? 0.01 : 0.0
            };
            transition.GeneratedEasingFunction = function2;
        }
        var flag = base.GoToStateCore(control, stateGroupsRoot, stateName, group, state, useTransitions);
        if (animateWithTransitionEffect)
        {
            transition.GeneratedEasingFunction = generatedEasingFunction;
            if (flag)
                AnimateTransitionEffect(stateGroupsRoot, transition);
        }
        SetCurrentState(group, state);
        return flag;
    }

    private static void UnwrapMovingElementsFromCanvases(List<FrameworkElement> movingElements)
    {
        foreach (var element in movingElements)
        {
            if (element.Parent is WrapperCanvas parent)
            {
                var obj2 = CacheLocalValueHelper(element, FrameworkElement.DataContextProperty);
                element.DataContext = element.DataContext;
                var element2 = VisualTreeHelper.GetParent(parent) as FrameworkElement;
                parent.Children.Remove(element);
                if (element2 is Panel panel)
                {
                    var index = panel.Children.IndexOf(parent);
                    panel.Children.RemoveAt(index);
                    panel.Children.Insert(index, element);
                }
                else
                {
                    if (element2 is Decorator decorator)
                        decorator.Child = element;
                }
                CopyLayoutProperties(parent, element, true);
                ReplaceCachedLocalValueHelper(element, FrameworkElement.DataContextProperty, obj2);
            }
        }
    }

    private static void WrapMovingElementsInCanvases(List<FrameworkElement> movingElements,
        Dictionary<FrameworkElement, Rect> oldRects, Dictionary<FrameworkElement, Rect> newRects)
    {
        foreach (var element in movingElements)
        {
            var parent = VisualTreeHelper.GetParent(element) as FrameworkElement;
            var canvas = new WrapperCanvas
            {
                OldRect = oldRects[element],
                NewRect = newRects[element]
            };
            var obj2 = CacheLocalValueHelper(element, FrameworkElement.DataContextProperty);
            element.DataContext = element.DataContext;
            var flag = true;
            if (parent is Panel panel && !panel.IsItemsHost)
            {
                var index = panel.Children.IndexOf(element);
                panel.Children.RemoveAt(index);
                panel.Children.Insert(index, canvas);
            }
            else
            {
                if (parent is Decorator decorator)
                    decorator.Child = canvas;
                else
                    flag = false;
            }
            if (flag)
            {
                canvas.Children.Add(element);
                CopyLayoutProperties(element, canvas, false);
                ReplaceCachedLocalValueHelper(element, FrameworkElement.DataContextProperty, obj2);
            }
        }
    }

    // Nested Types
    private class DummyEasingFunction : EasingFunctionBase
    {
        // Fields
        public static readonly DependencyProperty DummyValueProperty = DependencyProperty.Register("DummyValue",
            typeof(double), typeof(DummyEasingFunction), new PropertyMetadata(0.0));

        // Properties
        public double DummyValue
        {
            private get => (double) GetValue(DummyValueProperty);
            set => SetValue(DummyValueProperty, value);
        }

        // Methods
        protected override Freezable CreateInstanceCore()
        {
            return new DummyEasingFunction();
        }

        protected override double EaseInCore(double normalizedTime)
        {
            return DummyValue;
        }
    }

    internal class OriginalLayoutValueRecord
    {
        // Properties
        public FrameworkElement Element { get; set; }

        public DependencyProperty Property { get; set; }

        public object Value { get; set; }
    }

    internal class WrapperCanvas : Canvas
    {
        // Fields
        internal static readonly DependencyProperty SimulationProgressProperty =
            DependencyProperty.Register("SimulationProgress", typeof(double), typeof(WrapperCanvas),
                new PropertyMetadata(0.0, SimulationProgressChanged));

        // Properties
        public Visibility DestinationVisibilityCache { get; set; }

        public Dictionary<DependencyProperty, object> LocalValueCache { get; set; }

        public Rect NewRect { get; set; }

        public Rect OldRect { get; set; }

        public double SimulationProgress
        {
            get =>
                (double) GetValue(SimulationProgressProperty);
            set => SetValue(SimulationProgressProperty, value);
        }

        // Methods
        private static void SimulationProgressChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var canvas = d as WrapperCanvas;
            var newValue = (double) e.NewValue;
            if (canvas != null && canvas.Children.Count > 0)
            {
                if (canvas.Children[0] is FrameworkElement element)
                {
                    element.Width = Math.Max(0.0,
                        canvas.OldRect.Width * newValue + canvas.NewRect.Width * (1.0 - newValue));
                    element.Height = Math.Max(0.0,
                        canvas.OldRect.Height * newValue + canvas.NewRect.Height * (1.0 - newValue));
                    SetLeft(element, newValue * (canvas.OldRect.Left - canvas.NewRect.Left));
                    SetTop(element, newValue * (canvas.OldRect.Top - canvas.NewRect.Top));
                }
            }
        }
    }
}
