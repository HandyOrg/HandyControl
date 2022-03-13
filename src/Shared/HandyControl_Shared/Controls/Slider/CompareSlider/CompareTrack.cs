using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace HandyControl.Controls;

public class CompareTrack : Track
{
    protected override Size ArrangeOverride(Size arrangeSize)
    {
        base.ArrangeOverride(arrangeSize);

        var isVertical = Orientation == Orientation.Vertical;
        ComputeSliderLengths(arrangeSize, isVertical, out var decreaseButtonLength, out var thumbLength,
            out var increaseButtonLength);

        var offset = new Point();
        var pieceSize = arrangeSize;
        var isDirectionReversed = IsDirectionReversed;

        if (isVertical)
        {
            CoerceLength(ref decreaseButtonLength, arrangeSize.Height);
            CoerceLength(ref increaseButtonLength, arrangeSize.Height);
            CoerceLength(ref thumbLength, arrangeSize.Height);

            offset.Y = isDirectionReversed ? decreaseButtonLength + thumbLength : 0.0;
            pieceSize.Height = increaseButtonLength;

            IncreaseRepeatButton?.Arrange(new Rect(offset, pieceSize));

            offset.Y = isDirectionReversed ? 0.0 : increaseButtonLength + thumbLength;
            pieceSize.Height = decreaseButtonLength;

            if (DecreaseRepeatButton != null)
            {
                DecreaseRepeatButton.Arrange(new Rect(offset, pieceSize));
                DecreaseRepeatButton.Height = pieceSize.Height;
            }

            offset.Y = isDirectionReversed ? decreaseButtonLength : increaseButtonLength;
            pieceSize.Height = thumbLength;

            Thumb?.Arrange(new Rect(offset, pieceSize));
        }
        else
        {
            CoerceLength(ref decreaseButtonLength, arrangeSize.Width);
            CoerceLength(ref increaseButtonLength, arrangeSize.Width);
            CoerceLength(ref thumbLength, arrangeSize.Width);

            offset.X = isDirectionReversed ? increaseButtonLength + thumbLength : 0.0;
            pieceSize.Width = decreaseButtonLength;

            DecreaseRepeatButton?.Arrange(new Rect(offset, pieceSize));


            offset.X = isDirectionReversed ? 0.0 : decreaseButtonLength + thumbLength;
            pieceSize.Width = increaseButtonLength;

            if (IncreaseRepeatButton != null)
            {
                IncreaseRepeatButton.Arrange(new Rect(offset, pieceSize));
                IncreaseRepeatButton.Width = pieceSize.Width;
            }

            offset.X = isDirectionReversed ? increaseButtonLength : decreaseButtonLength;
            pieceSize.Width = thumbLength;

            Thumb?.Arrange(new Rect(offset, pieceSize));
        }

        return arrangeSize;
    }

    private void ComputeSliderLengths(Size arrangeSize, bool isVertical, out double decreaseButtonLength,
        out double thumbLength, out double increaseButtonLength)
    {
        var min = Minimum;
        var range = Math.Max(0.0, Maximum - min);
        var offset = Math.Min(range, Value - min);

        double trackLength;

        // Compute thumb size
        if (isVertical)
        {
            trackLength = arrangeSize.Height;
            thumbLength = Thumb?.DesiredSize.Height ?? 0;
        }
        else
        {
            trackLength = arrangeSize.Width;
            thumbLength = Thumb?.DesiredSize.Width ?? 0;
        }

        CoerceLength(ref thumbLength, trackLength);

        var remainingTrackLength = trackLength - thumbLength;

        decreaseButtonLength = remainingTrackLength * offset / range;
        CoerceLength(ref decreaseButtonLength, remainingTrackLength);

        increaseButtonLength = remainingTrackLength - decreaseButtonLength;
        CoerceLength(ref increaseButtonLength, remainingTrackLength);
    }

    private static void CoerceLength(ref double componentLength, double trackLength)
    {
        if (componentLength < 0)
            componentLength = 0.0;
        else if (componentLength > trackLength || double.IsNaN(componentLength))
            componentLength = trackLength;
    }
}
