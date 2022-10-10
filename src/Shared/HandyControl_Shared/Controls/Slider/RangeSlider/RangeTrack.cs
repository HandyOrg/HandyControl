// Adapted from https://referencesource.microsoft.com/#PresentationFramework/src/Framework/System/Windows/Controls/Primitives/Track.cs

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using HandyControl.Data;

namespace HandyControl.Controls;

public class RangeTrack : FrameworkElement
{
    private RepeatButton _increaseButton;

    private RepeatButton _centerButton;

    private RepeatButton _decreaseButton;

    private RangeThumb _thumbStart;

    private RangeThumb _thumbEnd;

    private Visual[] _visualChildren;

    private double Density { get; set; } = double.NaN;

    public RepeatButton DecreaseRepeatButton
    {
        get => _decreaseButton;
        set
        {
            if (Equals(_increaseButton, value) || Equals(_centerButton, value))
            {
                throw new NotSupportedException("SameButtons");
            }
            UpdateComponent(_decreaseButton, value);
            _decreaseButton = value;

            if (_decreaseButton != null)
            {
                CommandManager.InvalidateRequerySuggested(); // Should post an idle queue item to update IsEnabled on button
            }
        }
    }

    public RepeatButton CenterRepeatButton
    {
        get => _centerButton;
        set
        {
            if (Equals(_increaseButton, value) || Equals(_decreaseButton, value))
            {
                throw new NotSupportedException("SameButtons");
            }
            UpdateComponent(_centerButton, value);
            _centerButton = value;

            if (_centerButton != null)
            {
                CommandManager.InvalidateRequerySuggested(); // Should post an idle queue item to update IsEnabled on button
            }
        }
    }

    public RepeatButton IncreaseRepeatButton
    {
        get => _increaseButton;
        set
        {
            if (Equals(_decreaseButton, value) || Equals(_centerButton, value))
            {
                throw new NotSupportedException("SameButtons");
            }
            UpdateComponent(_increaseButton, value);
            _increaseButton = value;

            if (_increaseButton != null)
            {
                CommandManager.InvalidateRequerySuggested(); // Should post an idle queue item to update IsEnabled on button
            }
        }
    }

    public RangeThumb ThumbStart
    {
        get => _thumbStart;
        set
        {
            UpdateComponent(_thumbStart, value);
            _thumbStart = value;
        }
    }

    public RangeThumb ThumbEnd
    {
        get => _thumbEnd;
        set
        {
            UpdateComponent(_thumbEnd, value);
            _thumbEnd = value;
        }
    }

    static RangeTrack()
    {
        IsEnabledProperty.OverrideMetadata(typeof(RangeTrack), new UIPropertyMetadata(OnIsEnabledChanged));
    }

    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
        nameof(Orientation), typeof(Orientation), typeof(RangeTrack),
        new FrameworkPropertyMetadata(default(Orientation), FrameworkPropertyMetadataOptions.AffectsMeasure));

    public Orientation Orientation
    {
        get => (Orientation) GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
        nameof(Minimum), typeof(double), typeof(RangeTrack),
        new FrameworkPropertyMetadata(ValueBoxes.Double0Box, FrameworkPropertyMetadataOptions.AffectsArrange));

    public double Minimum
    {
        get => (double) GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }

    public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
        nameof(Maximum), typeof(double), typeof(RangeTrack),
        new FrameworkPropertyMetadata(ValueBoxes.Double1Box, FrameworkPropertyMetadataOptions.AffectsArrange));

    public double Maximum
    {
        get => (double) GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }

    public static readonly DependencyProperty ValueStartProperty = DependencyProperty.Register(
        nameof(ValueStart), typeof(double), typeof(RangeTrack),
        new FrameworkPropertyMetadata(ValueBoxes.Double0Box,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault |
            FrameworkPropertyMetadataOptions.AffectsArrange));

    public double ValueStart
    {
        get => (double) GetValue(ValueStartProperty);
        set => SetValue(ValueStartProperty, value);
    }

    public static readonly DependencyProperty ValueEndProperty = DependencyProperty.Register(
        nameof(ValueEnd), typeof(double), typeof(RangeTrack),
        new FrameworkPropertyMetadata(ValueBoxes.Double0Box,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault |
            FrameworkPropertyMetadataOptions.AffectsArrange));

    public double ValueEnd
    {
        get => (double) GetValue(ValueEndProperty);
        set => SetValue(ValueEndProperty, value);
    }

    public static readonly DependencyProperty IsDirectionReversedProperty = DependencyProperty.Register(
        nameof(IsDirectionReversed), typeof(bool), typeof(RangeTrack), new PropertyMetadata(ValueBoxes.FalseBox));

    public bool IsDirectionReversed
    {
        get => (bool) GetValue(IsDirectionReversedProperty);
        set => SetValue(IsDirectionReversedProperty, ValueBoxes.BooleanBox(value));
    }

    protected override Visual GetVisualChild(int index)
    {
        if (_visualChildren?[index] == null)
        {
            // ReSharper disable once UseNameofExpression
            // ReSharper disable once LocalizableElement
            throw new ArgumentOutOfRangeException("index", index, "ArgumentOutOfRange");
        }
        return _visualChildren[index];
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        var desiredSize = new Size();

        // Only measure thumb
        // Repeat buttons will be sized based on thumb
        if (_thumbStart != null)
        {
            _thumbStart.Measure(availableSize);
            desiredSize = _thumbStart.DesiredSize;
        }

        if (_thumbEnd != null)
        {
            _thumbEnd.Measure(availableSize);
            desiredSize = new Size(Math.Max(_thumbEnd.DesiredSize.Width, desiredSize.Width),
                Math.Max(_thumbEnd.DesiredSize.Height, desiredSize.Height));
        }

        return desiredSize;
    }

    private static void CoerceLength(ref double componentLength, double trackLength)
    {
        if (componentLength < 0)
        {
            componentLength = 0;
        }
        else if (componentLength > trackLength || double.IsNaN(componentLength))
        {
            componentLength = trackLength;
        }
    }

    protected override Size ArrangeOverride(Size arrangeSize)
    {
        var isVertical = Orientation == Orientation.Vertical;

        ComputeLengths(arrangeSize, isVertical, out var decreaseButtonLength, out var centerButtonLength,
            out var increaseButtonLength, out var thumbStartLength, out var thumbEndLength);

        var offset = new Point();
        var pieceSize = arrangeSize;
        var isDirectionReversed = IsDirectionReversed;

        if (isVertical)
        {
            CoerceLength(ref decreaseButtonLength, arrangeSize.Height);
            CoerceLength(ref centerButtonLength, arrangeSize.Height);
            CoerceLength(ref increaseButtonLength, arrangeSize.Height);
            CoerceLength(ref thumbStartLength, arrangeSize.Height);
            CoerceLength(ref thumbEndLength, arrangeSize.Height);

            offset.Y = isDirectionReversed ? decreaseButtonLength + thumbEndLength + centerButtonLength + thumbStartLength : 0;
            pieceSize.Height = increaseButtonLength;

            IncreaseRepeatButton?.Arrange(new Rect(offset, pieceSize));

            offset.Y = isDirectionReversed ? decreaseButtonLength + thumbEndLength : increaseButtonLength + thumbStartLength;
            pieceSize.Height = centerButtonLength;

            CenterRepeatButton?.Arrange(new Rect(offset, pieceSize));

            offset.Y = isDirectionReversed ? 0 : increaseButtonLength + thumbStartLength + centerButtonLength + thumbEndLength;
            pieceSize.Height = decreaseButtonLength;

            DecreaseRepeatButton?.Arrange(new Rect(offset, pieceSize));

            offset.Y = isDirectionReversed
                ? decreaseButtonLength + thumbEndLength + centerButtonLength
                : increaseButtonLength + thumbStartLength + centerButtonLength;
            pieceSize.Height = thumbStartLength;

            ArrangeThumb(isDirectionReversed, false, offset, pieceSize);

            offset.Y = isDirectionReversed ? decreaseButtonLength : increaseButtonLength;
            pieceSize.Height = thumbEndLength;

            ArrangeThumb(isDirectionReversed, true, offset, pieceSize);
        }
        else
        {
            CoerceLength(ref decreaseButtonLength, arrangeSize.Width);
            CoerceLength(ref centerButtonLength, arrangeSize.Width);
            CoerceLength(ref increaseButtonLength, arrangeSize.Width);
            CoerceLength(ref thumbStartLength, arrangeSize.Width);
            CoerceLength(ref thumbEndLength, arrangeSize.Width);

            offset.X = isDirectionReversed ? 0 : decreaseButtonLength + thumbEndLength + centerButtonLength + thumbStartLength;
            pieceSize.Width = increaseButtonLength;

            IncreaseRepeatButton?.Arrange(new Rect(offset, pieceSize));

            offset.X = isDirectionReversed ? increaseButtonLength + thumbStartLength : decreaseButtonLength + thumbEndLength;
            pieceSize.Width = centerButtonLength;

            CenterRepeatButton?.Arrange(new Rect(offset, pieceSize));

            offset.X = isDirectionReversed ? increaseButtonLength + thumbStartLength + centerButtonLength + thumbEndLength : 0;
            pieceSize.Width = decreaseButtonLength;

            DecreaseRepeatButton?.Arrange(new Rect(offset, pieceSize));

            offset.X = isDirectionReversed ? increaseButtonLength : decreaseButtonLength;
            pieceSize.Width = thumbStartLength;

            ArrangeThumb(isDirectionReversed, false, offset, pieceSize);

            offset.X = isDirectionReversed
                ? increaseButtonLength + thumbStartLength + centerButtonLength
                : decreaseButtonLength + thumbEndLength + centerButtonLength;
            pieceSize.Width = thumbEndLength;

            ArrangeThumb(isDirectionReversed, true, offset, pieceSize);
        }

        return arrangeSize;
    }

    private void ArrangeThumb(bool isDirectionReversed, bool isStart, Point offset, Size pieceSize)
    {
        if (isStart)
        {
            if (isDirectionReversed)
            {
                ThumbStart?.Arrange(new Rect(offset, pieceSize));
            }
            else
            {
                ThumbEnd?.Arrange(new Rect(offset, pieceSize));
            }
        }
        else
        {
            if (isDirectionReversed)
            {
                ThumbEnd?.Arrange(new Rect(offset, pieceSize));
            }
            else
            {
                ThumbStart?.Arrange(new Rect(offset, pieceSize));
            }
        }
    }

    private void ComputeLengths(Size arrangeSize, bool isVertical, out double decreaseButtonLength,
        out double centerButtonLength, out double increaseButtonLength, out double thumbStartLength,
        out double thumbEndLength)
    {
        var min = Minimum;
        var range = Math.Max(0.0, Maximum - min);
        var offsetStart = Math.Min(range, ValueStart - min);
        var offsetEnd = Math.Min(range, ValueEnd - min);

        double trackLength;

        // Compute thumb size
        if (isVertical)
        {
            trackLength = arrangeSize.Height;
            thumbStartLength = _thumbStart?.DesiredSize.Height ?? 0;
            thumbEndLength = _thumbEnd?.DesiredSize.Height ?? 0;
        }
        else
        {
            trackLength = arrangeSize.Width;
            thumbStartLength = _thumbStart?.DesiredSize.Width ?? 0;
            thumbEndLength = _thumbEnd?.DesiredSize.Width ?? 0;
        }

        CoerceLength(ref thumbStartLength, trackLength);
        CoerceLength(ref thumbEndLength, trackLength);

        var remainingTrackLength = trackLength - thumbStartLength - thumbEndLength;

        decreaseButtonLength = remainingTrackLength * offsetStart / range;
        CoerceLength(ref decreaseButtonLength, remainingTrackLength);

        centerButtonLength = remainingTrackLength * offsetEnd / range - decreaseButtonLength;
        CoerceLength(ref centerButtonLength, remainingTrackLength);

        increaseButtonLength = remainingTrackLength - decreaseButtonLength - centerButtonLength;
        CoerceLength(ref increaseButtonLength, remainingTrackLength);

        Density = range / remainingTrackLength;
    }

    protected override int VisualChildrenCount
    {
        get
        {
            if (_visualChildren == null) return 0;

            for (var i = 0; i < _visualChildren.Length; i++)
            {
                if (_visualChildren[i] == null) return i;
            }

            return _visualChildren.Length;
        }
    }

    private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if ((bool) e.NewValue)
        {
            Mouse.Synchronize();
        }
    }

    public virtual double ValueFromPoint(Point pt)
    {
        return Orientation == Orientation.Horizontal
            ? !IsDirectionReversed
                ? pt.X / RenderSize.Width * Maximum
                : (1 - pt.X / RenderSize.Width) * Maximum
            : !IsDirectionReversed
                ? pt.Y / RenderSize.Height * Maximum
                : (1 - pt.X / RenderSize.Height) * Maximum;
    }

    public virtual double ValueFromDistance(double horizontal, double vertical)
    {
        double scale = IsDirectionReversed ? -1 : 1;
        return Orientation == Orientation.Horizontal
            ? scale * horizontal * Density
            : -1 * scale * vertical * Density;
    }

    private void UpdateComponent(Control oldValue, Control newValue)
    {
        if (oldValue != newValue)
        {
            _visualChildren ??= new Visual[5];

            if (oldValue != null)
            {
                // notify the visual layer that the old component has been removed.
                RemoveVisualChild(oldValue);
            }

            // Remove the old value from our z index list and add new value to end
            int i = 0;
            while (i < 5)
            {
                // Array isn't full, break
                if (_visualChildren[i] == null)
                    break;

                // found the old value
                if (_visualChildren[i] == oldValue)
                {
                    // Move values down until end of array or a null element
                    while (i < 4 && _visualChildren[i + 1] != null)
                    {
                        _visualChildren[i] = _visualChildren[i + 1];
                        i++;
                    }
                }
                else
                {
                    i++;
                }
            }
            // Add newValue at end of z-order
            _visualChildren[i] = newValue;

            AddVisualChild(newValue);

            InvalidateMeasure();
            InvalidateArrange();
        }
    }
}
