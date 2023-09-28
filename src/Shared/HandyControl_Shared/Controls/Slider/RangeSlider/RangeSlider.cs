// Adapted from https://referencesource.microsoft.com/#PresentationFramework/src/Framework/System/Windows/Controls/Slider.cs

using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using HandyControl.Data;
using HandyControl.Expression.Drawing;
using HandyControl.Tools;

namespace HandyControl.Controls;

[DefaultEvent("ValueChanged"), DefaultProperty("Value")]
[TemplatePart(Name = ElementTrack, Type = typeof(Track))]
public class RangeSlider : TwoWayRangeBase
{
    private const string ElementTrack = "PART_Track";

    private RangeTrack _track;

    private readonly ToolTip _autoToolTipStart = null;

    private readonly ToolTip _autoToolTipEnd = null;

    private RangeThumb _thumbCurrent;

    private object _thumbOriginalToolTip;

    private Point _originThumbPoint;

    private Point _previousScreenCoordPosition;

    static RangeSlider()
    {
        InitializeCommands();

        MinimumProperty.OverrideMetadata(typeof(RangeSlider), new FrameworkPropertyMetadata(ValueBoxes.Double0Box, FrameworkPropertyMetadataOptions.AffectsMeasure));
        MaximumProperty.OverrideMetadata(typeof(RangeSlider), new FrameworkPropertyMetadata(ValueBoxes.Double10Box, FrameworkPropertyMetadataOptions.AffectsMeasure));
        ValueStartProperty.OverrideMetadata(typeof(RangeSlider), new FrameworkPropertyMetadata(ValueBoxes.Double0Box, FrameworkPropertyMetadataOptions.AffectsMeasure));
        ValueEndProperty.OverrideMetadata(typeof(RangeSlider), new FrameworkPropertyMetadata(ValueBoxes.Double0Box, FrameworkPropertyMetadataOptions.AffectsMeasure));

        // Register Event Handler for the Thumb
        EventManager.RegisterClassHandler(typeof(RangeSlider), Thumb.DragStartedEvent, new DragStartedEventHandler(OnThumbDragStarted));
        EventManager.RegisterClassHandler(typeof(RangeSlider), Thumb.DragDeltaEvent, new DragDeltaEventHandler(OnThumbDragDelta));
        EventManager.RegisterClassHandler(typeof(RangeSlider), Thumb.DragCompletedEvent, new DragCompletedEventHandler(OnThumbDragCompleted));

        // Listen to MouseLeftButtonDown event to determine if slide should move focus to itself
        EventManager.RegisterClassHandler(typeof(RangeSlider), Mouse.MouseDownEvent, new MouseButtonEventHandler(OnMouseLeftButtonDown), true);
    }

    public RangeSlider()
    {
        CommandBindings.Add(new CommandBinding(IncreaseLarge, OnIncreaseLarge));
        CommandBindings.Add(new CommandBinding(IncreaseSmall, OnIncreaseSmall));

        CommandBindings.Add(new CommandBinding(DecreaseLarge, OnDecreaseLarge));
        CommandBindings.Add(new CommandBinding(DecreaseSmall, OnDecreaseSmall));

        CommandBindings.Add(new CommandBinding(CenterLarge, OnCenterLarge));
        CommandBindings.Add(new CommandBinding(CenterSmall, OnCenterSmall));
    }

    private void OnIncreaseLarge(object sender, ExecutedRoutedEventArgs e) => (sender as RangeSlider)?.OnIncreaseLarge();

    private void OnIncreaseSmall(object sender, ExecutedRoutedEventArgs e) => (sender as RangeSlider)?.OnIncreaseSmall();

    private void OnDecreaseLarge(object sender, ExecutedRoutedEventArgs e) => (sender as RangeSlider)?.OnDecreaseLarge();

    private void OnDecreaseSmall(object sender, ExecutedRoutedEventArgs e) => (sender as RangeSlider)?.OnDecreaseSmall();

    private void OnCenterLarge(object sender, ExecutedRoutedEventArgs e) => (sender as RangeSlider)?.OnCenterLarge(e.Parameter);

    private void OnCenterSmall(object sender, ExecutedRoutedEventArgs e) => (sender as RangeSlider)?.OnCenterSmall(e.Parameter);

    protected virtual void OnIncreaseLarge() => MoveToNextTick(LargeChange, false);

    protected virtual void OnIncreaseSmall() => MoveToNextTick(SmallChange, false);

    protected virtual void OnDecreaseLarge() => MoveToNextTick(-LargeChange, true);

    protected virtual void OnDecreaseSmall() => MoveToNextTick(-SmallChange, true);

    protected virtual void OnCenterLarge(object parameter) => MoveToNextTick(LargeChange, false, true);

    protected virtual void OnCenterSmall(object parameter) => MoveToNextTick(SmallChange, false, true);

    public static RoutedCommand IncreaseLarge { get; private set; }

    public static RoutedCommand IncreaseSmall { get; private set; }

    public static RoutedCommand DecreaseLarge { get; private set; }

    public static RoutedCommand DecreaseSmall { get; private set; }

    public static RoutedCommand CenterLarge { get; private set; }

    public static RoutedCommand CenterSmall { get; private set; }

    private static void InitializeCommands()
    {
        IncreaseLarge = new RoutedCommand(nameof(IncreaseLarge), typeof(RangeSlider));
        IncreaseSmall = new RoutedCommand(nameof(IncreaseSmall), typeof(RangeSlider));

        DecreaseLarge = new RoutedCommand(nameof(DecreaseLarge), typeof(RangeSlider));
        DecreaseSmall = new RoutedCommand(nameof(DecreaseSmall), typeof(RangeSlider));

        CenterLarge = new RoutedCommand(nameof(CenterLarge), typeof(RangeSlider));
        CenterSmall = new RoutedCommand(nameof(CenterSmall), typeof(RangeSlider));
    }

    public override void OnApplyTemplate()
    {
        _thumbCurrent = null;

        base.OnApplyTemplate();

        _track = GetTemplateChild(ElementTrack) as RangeTrack;

        if (_autoToolTipStart != null)
        {
            _autoToolTipStart.PlacementTarget = _track?.ThumbStart;
        }

        if (_autoToolTipEnd != null)
        {
            _autoToolTipEnd.PlacementTarget = _track?.ThumbEnd;
        }
    }

    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
        nameof(Orientation), typeof(Orientation), typeof(RangeSlider), new PropertyMetadata(default(Orientation)));

    public Orientation Orientation
    {
        get => (Orientation) GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    public static readonly DependencyProperty IsDirectionReversedProperty = DependencyProperty.Register(
        nameof(IsDirectionReversed), typeof(bool), typeof(RangeSlider), new PropertyMetadata(ValueBoxes.FalseBox));

    public bool IsDirectionReversed
    {
        get => (bool) GetValue(IsDirectionReversedProperty);
        set => SetValue(IsDirectionReversedProperty, ValueBoxes.BooleanBox(value));
    }

    public static readonly DependencyProperty DelayProperty = RepeatButton.DelayProperty.AddOwner(typeof(RangeSlider), new FrameworkPropertyMetadata(GetKeyboardDelay()));

    public int Delay
    {
        get => (int) GetValue(DelayProperty);
        set => SetValue(DelayProperty, value);
    }

    internal static int GetKeyboardDelay()
    {
        var delay = SystemParameters.KeyboardDelay;
        if (delay < 0 || delay > 3)
            delay = 0;
        return (delay + 1) * 250;
    }

    public static readonly DependencyProperty IntervalProperty = RepeatButton.IntervalProperty.AddOwner(typeof(RangeSlider), new FrameworkPropertyMetadata(GetKeyboardSpeed()));

    public int Interval
    {
        get => (int) GetValue(IntervalProperty);
        set => SetValue(IntervalProperty, value);
    }

    internal static int GetKeyboardSpeed()
    {
        var speed = SystemParameters.KeyboardSpeed;
        if (speed < 0 || speed > 31)
            speed = 31;
        return (31 - speed) * (400 - 1000 / 30) / 31 + 1000 / 30;
    }

    public static readonly DependencyProperty AutoToolTipPlacementProperty = DependencyProperty.Register(
        nameof(AutoToolTipPlacement), typeof(AutoToolTipPlacement), typeof(RangeSlider), new PropertyMetadata(default(AutoToolTipPlacement)));

    public AutoToolTipPlacement AutoToolTipPlacement
    {
        get => (AutoToolTipPlacement) GetValue(AutoToolTipPlacementProperty);
        set => SetValue(AutoToolTipPlacementProperty, value);
    }

    public static readonly DependencyProperty AutoToolTipPrecisionProperty = DependencyProperty.Register(
        nameof(AutoToolTipPrecision), typeof(int), typeof(RangeSlider), new PropertyMetadata(ValueBoxes.Int0Box),
        ValidateHelper.IsInRangeOfPosIntIncludeZero);

    public int AutoToolTipPrecision
    {
        get => (int) GetValue(AutoToolTipPrecisionProperty);
        set => SetValue(AutoToolTipPrecisionProperty, value);
    }

    public static readonly DependencyProperty IsSnapToTickEnabledProperty = DependencyProperty.Register(
        nameof(IsSnapToTickEnabled), typeof(bool), typeof(RangeSlider), new PropertyMetadata(ValueBoxes.FalseBox));

    public bool IsSnapToTickEnabled
    {
        get => (bool) GetValue(IsSnapToTickEnabledProperty);
        set => SetValue(IsSnapToTickEnabledProperty, ValueBoxes.BooleanBox(value));
    }

    public static readonly DependencyProperty TickPlacementProperty = DependencyProperty.Register(
        nameof(TickPlacement), typeof(TickPlacement), typeof(RangeSlider), new PropertyMetadata(default(TickPlacement)));

    public TickPlacement TickPlacement
    {
        get => (TickPlacement) GetValue(TickPlacementProperty);
        set => SetValue(TickPlacementProperty, value);
    }

    public static readonly DependencyProperty TickFrequencyProperty = DependencyProperty.Register(
        nameof(TickFrequency), typeof(double), typeof(RangeSlider), new PropertyMetadata(ValueBoxes.Double1Box),
        ValidateHelper.IsInRangeOfPosDoubleIncludeZero);

    public double TickFrequency
    {
        get => (double) GetValue(TickFrequencyProperty);
        set => SetValue(TickFrequencyProperty, value);
    }

    public static readonly DependencyProperty TicksProperty = DependencyProperty.Register(
        nameof(Ticks), typeof(DoubleCollection), typeof(RangeSlider), new PropertyMetadata(new DoubleCollection()));

    public DoubleCollection Ticks
    {
        get => (DoubleCollection) GetValue(TicksProperty);
        set => SetValue(TicksProperty, value);
    }

    public static readonly DependencyProperty IsMoveToPointEnabledProperty = DependencyProperty.Register(
        nameof(IsMoveToPointEnabled), typeof(bool), typeof(RangeSlider), new PropertyMetadata(ValueBoxes.FalseBox));

    public bool IsMoveToPointEnabled
    {
        get => (bool) GetValue(IsMoveToPointEnabledProperty);
        set => SetValue(IsMoveToPointEnabledProperty, ValueBoxes.BooleanBox(value));
    }

    protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        if (IsMoveToPointEnabled && _track.ThumbStart is { IsMouseOver: false } && _track.ThumbEnd is { IsMouseOver: false })
        {
            // Here we need to determine whether it's closer to the starting point or the end point. 
            var pt = e.MouseDevice.GetPosition(_track);
            UpdateValue(pt);
            e.Handled = true;
        }

        base.OnPreviewMouseLeftButtonDown(e);
    }

    private void MoveToNextTick(double direction, bool isStart, bool isCenter = false, object parameter = null)
    {
        if (MathHelper.AreClose(direction, 0)) return;

        if (isCenter)
        {
            if (parameter == null)
            {
                var pt = Mouse.GetPosition(_track);
                var newValue = _track.ValueFromPoint(pt);
                if (ValidateHelper.IsInRangeOfDouble(newValue))
                {
                    isStart = (ValueStart + ValueEnd) / 2 > newValue;
                    if (!isStart)
                    {
                        direction = -Math.Abs(direction);
                    }
                }
            }

            if (parameter is bool parameterValue)
            {
                isStart = parameterValue;
            }
        }

        var value = isStart ? ValueStart : ValueEnd;
        var next = SnapToTick(Math.Max(Minimum, Math.Min(Maximum, value + direction)));
        var greaterThan = direction > 0;

        // If the snapping brought us back to value, find the next tick point
        if (MathHelper.AreClose(next, value) &&
            !(greaterThan && MathHelper.AreClose(value, Maximum)) &&
            !(!greaterThan && MathHelper.AreClose(value, Minimum)))
        {
            // If ticks collection is available, use it.
            // Note that ticks may be unsorted.
            if (Ticks is { Count: > 0 })
            {
                foreach (var tick in Ticks)
                {
                    // Find the smallest tick greater than value or the largest tick less than value
                    if (greaterThan && MathHelper.GreaterThan(tick, value) && (MathHelper.LessThan(tick, next) || MathHelper.AreClose(next, value))
                        || !greaterThan && MathHelper.LessThan(tick, value) && (MathHelper.GreaterThan(tick, next) || MathHelper.AreClose(next, value)))
                    {
                        next = tick;
                    }
                }
            }
            else if (MathHelper.GreaterThan(TickFrequency, 0.0))
            {
                // Find the current tick we are at
                var tickNumber = Math.Round((value - Minimum) / TickFrequency);

                if (greaterThan)
                    tickNumber += 1.0;
                else
                    tickNumber -= 1.0;

                next = Minimum + tickNumber * TickFrequency;
            }
        }

        // Update if we've found a better value
        if (!MathHelper.AreClose(next, value))
        {
            SetCurrentValue(isStart ? ValueStartProperty : ValueEndProperty, next);
        }
    }

    private double SnapToTick(double value)
    {
        if (!IsSnapToTickEnabled) return value;

        var previous = Minimum;
        var next = Maximum;

        if (Ticks is { Count: > 0 })
        {
            foreach (var tick in Ticks)
            {
                if (MathHelper.AreClose(tick, value))
                {
                    return value;
                }

                if (MathHelper.LessThan(tick, value) && MathHelper.GreaterThan(tick, previous))
                {
                    previous = tick;
                }
                else if (MathHelper.GreaterThan(tick, value) && MathHelper.LessThan(tick, next))
                {
                    next = tick;
                }
            }
        }
        else if (MathHelper.GreaterThan(TickFrequency, 0.0))
        {
            previous = Minimum + Math.Round((value - Minimum) / TickFrequency) * TickFrequency;
            next = Math.Min(Maximum, previous + TickFrequency);
        }

        return MathHelper.GreaterThanOrClose(value, (previous + next) * 0.5) ? next : previous;
    }

    private void UpdateValue(Point point)
    {
        var newValue = _track.ValueFromPoint(point);
        if (ValidateHelper.IsInRangeOfDouble(newValue))
        {
            var isStart = (ValueStart + ValueEnd) / 2 > newValue;
            UpdateValue(newValue, isStart);
        }
    }

    private void UpdateValue(double value, bool isStart)
    {
        var snappedValue = SnapToTick(value);

        if (isStart)
        {
            if (!MathHelper.AreClose(snappedValue, ValueStart))
            {
                var start = Math.Max(Minimum, Math.Min(Maximum, snappedValue));
                if (start > ValueEnd)
                {
                    SetCurrentValue(ValueStartProperty, ValueEnd);
                    SetCurrentValue(ValueEndProperty, start);
                    _track.ThumbStart.CancelDrag();
                    _track.ThumbEnd.StartDrag();
                    _thumbCurrent = _track.ThumbEnd;
                }
                else
                {
                    SetCurrentValue(ValueStartProperty, start);
                }
            }
        }
        else
        {
            if (!MathHelper.AreClose(snappedValue, ValueEnd))
            {
                var end = Math.Max(Minimum, Math.Min(Maximum, snappedValue));
                if (end < ValueStart)
                {
                    SetCurrentValue(ValueEndProperty, ValueStart);
                    SetCurrentValue(ValueStartProperty, end);
                    _track.ThumbEnd.CancelDrag();
                    _track.ThumbStart.StartDrag();
                    _thumbCurrent = _track.ThumbStart;
                }
                else
                {
                    SetCurrentValue(ValueEndProperty, end);
                }
            }
        }
    }

    private static void OnThumbDragStarted(object sender, DragStartedEventArgs e) => (sender as RangeSlider)?.OnThumbDragStarted(e);

    protected virtual void OnThumbDragStarted(DragStartedEventArgs e)
    {
        // Show AutoToolTip if needed.

        if (e.OriginalSource is not RangeThumb thumb) return;
        _thumbCurrent = thumb;
        _originThumbPoint = Mouse.GetPosition(_thumbCurrent);
        _thumbCurrent.StartDrag();

        if (AutoToolTipPlacement == AutoToolTipPlacement.None)
        {
            return;
        }

        var isStart = thumb.Equals(_track.ThumbStart);
        if (!isStart)
        {
            if (!thumb.Equals(_track.ThumbEnd)) return;
        }

        // Save original tooltip
        _thumbOriginalToolTip = thumb.ToolTip;
        OnThumbDragStarted(isStart ? _autoToolTipStart : _autoToolTipEnd, isStart);
    }

    private void OnThumbDragStarted(ToolTip toolTip, bool isStart)
    {
        toolTip ??= new ToolTip
        {
            Placement = PlacementMode.Custom,
            PlacementTarget = isStart ? _track.ThumbStart : _track.ThumbEnd,
            CustomPopupPlacementCallback = AutoToolTipCustomPlacementCallback
        };

        if (isStart)
        {
            _track.ThumbStart.ToolTip = toolTip;
        }
        else
        {
            _track.ThumbEnd.ToolTip = toolTip;
        }
        toolTip.Content = GetAutoToolTipNumber(isStart);
        toolTip.IsOpen = true;
    }

    private CustomPopupPlacement[] AutoToolTipCustomPlacementCallback(Size popupSize, Size targetSize, Point offset)
    {
        switch (AutoToolTipPlacement)
        {
            case AutoToolTipPlacement.TopLeft:
                if (Orientation == Orientation.Horizontal)
                {
                    // Place popup at top of thumb
                    return new[]{new CustomPopupPlacement(
                        new Point((targetSize.Width - popupSize.Width) * 0.5, -popupSize.Height),
                        PopupPrimaryAxis.Horizontal)
                    };
                }
                else
                {
                    // Place popup at left of thumb
                    return new[] {
                        new CustomPopupPlacement(
                            new Point(-popupSize.Width, (targetSize.Height - popupSize.Height) * 0.5),
                            PopupPrimaryAxis.Vertical)
                    };
                }

            case AutoToolTipPlacement.BottomRight:
                if (Orientation == Orientation.Horizontal)
                {
                    // Place popup at bottom of thumb
                    return new[] {
                        new CustomPopupPlacement(
                            new Point((targetSize.Width - popupSize.Width) * 0.5, targetSize.Height) ,
                            PopupPrimaryAxis.Horizontal)
                    };

                }
                else
                {
                    // Place popup at right of thumb
                    return new[] {
                        new CustomPopupPlacement(
                            new Point(targetSize.Width, (targetSize.Height - popupSize.Height) * 0.5),
                            PopupPrimaryAxis.Vertical)
                    };
                }

            default:
                return new CustomPopupPlacement[] { };
        }
    }

    private string GetAutoToolTipNumber(bool isStart)
    {
        var format = (NumberFormatInfo) NumberFormatInfo.CurrentInfo.Clone();
        format.NumberDecimalDigits = AutoToolTipPrecision;
        return isStart ? ValueStart.ToString("N", format) : ValueEnd.ToString("N", format);
    }

    private static void OnThumbDragDelta(object sender, DragDeltaEventArgs e) => (sender as RangeSlider)?.OnThumbDragDelta(e);

    protected virtual void OnThumbDragDelta(DragDeltaEventArgs e)
    {
        if (e.OriginalSource is not Thumb thumb) return;
        var isStart = thumb.Equals(_track.ThumbStart);
        if (!isStart)
        {
            if (!thumb.Equals(_track.ThumbEnd)) return;
        }

        // Convert to Track's co-ordinate
        OnThumbDragDelta(_track, isStart, e);
    }

    private void OnThumbDragDelta(RangeTrack track, bool isStart, DragDeltaEventArgs e)
    {
        if (track == null || track.ThumbStart == null | _track.ThumbEnd == null) return;

        var newValue = (isStart ? ValueStart : ValueEnd) + track.ValueFromDistance(e.HorizontalChange, e.VerticalChange);
        if (ValidateHelper.IsInRangeOfDouble(newValue))
        {
            UpdateValue(newValue, isStart);
        }

        // Show AutoToolTip if needed
        if (AutoToolTipPlacement != AutoToolTipPlacement.None)
        {
            var toolTip = (isStart ? _autoToolTipStart : _autoToolTipEnd) ?? new ToolTip();

            toolTip.Content = GetAutoToolTipNumber(isStart);

            var thumb = isStart ? _track.ThumbStart : _track.ThumbEnd;

            if (!Equals(thumb.ToolTip, toolTip))
            {
                thumb.ToolTip = toolTip;
            }

            if (!toolTip.IsOpen)
            {
                toolTip.IsOpen = true;
            }
        }
    }

    private static void OnThumbDragCompleted(object sender, DragCompletedEventArgs e) => (sender as RangeSlider)?.OnThumbDragCompleted(e);

    protected virtual void OnThumbDragCompleted(DragCompletedEventArgs e)
    {
        // Show AutoToolTip if needed.

        if (e.OriginalSource is not Thumb thumb || AutoToolTipPlacement == AutoToolTipPlacement.None)
        {
            return;
        }

        var isStart = thumb.Equals(_track.ThumbStart);
        if (!isStart)
        {
            if (!thumb.Equals(_track.ThumbEnd)) return;
        }

        var toolTip = isStart ? _autoToolTipStart : _autoToolTipEnd;
        if (toolTip != null)
        {
            toolTip.IsOpen = false;
        }

        thumb.ToolTip = _thumbOriginalToolTip;
    }

    private static void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton != MouseButton.Left) return;

        var slider = (RangeSlider) sender;

        // When someone click on the Slider's part, and it's not focusable
        // Slider need to take the focus in order to process keyboard correctly
        if (!slider.IsKeyboardFocusWithin)
        {
            e.Handled = slider.Focus() || e.Handled;
        }

        if (slider._track.ThumbStart.IsMouseOver)
        {
            slider._track.ThumbStart.StartDrag();
            slider._thumbCurrent = slider._track.ThumbStart;
        }

        if (slider._track.ThumbEnd.IsMouseOver)
        {
            slider._track.ThumbEnd.StartDrag();
            slider._thumbCurrent = slider._track.ThumbEnd;
        }
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (_thumbCurrent == null) return;
        if (e.MouseDevice.LeftButton != MouseButtonState.Pressed) return;
        if (!_thumbCurrent.IsDragging) return;

        var thumbCoordPosition = e.GetPosition(_thumbCurrent);
        var screenCoordPosition = PointFromScreen(thumbCoordPosition);
        if (screenCoordPosition != _previousScreenCoordPosition)
        {
            _previousScreenCoordPosition = screenCoordPosition;
            _thumbCurrent.RaiseEvent(new DragDeltaEventArgs(thumbCoordPosition.X - _originThumbPoint.X,
                thumbCoordPosition.Y - _originThumbPoint.Y));
        }
    }

    protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
    {
        base.OnPreviewMouseLeftButtonUp(e);

        _thumbCurrent = null;
    }
}
