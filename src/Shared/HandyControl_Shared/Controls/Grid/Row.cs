using HandyControl.Tools.Extension;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HandyControl.Data;
using HandyControl.Tools;

namespace HandyControl.Controls
{
    public class Row : Panel
    {
        private ColLayoutStatus _layoutStatus;

        private double _maxChildDesiredHeight;

        public static readonly DependencyProperty GutterProperty = DependencyProperty.Register(
            "Gutter", typeof(double), typeof(Row), new PropertyMetadata(ValueBoxes.Double0Box, null, OnGutterCoerce), OnGutterValidate);

        private static object OnGutterCoerce(DependencyObject d, object basevalue) => OnGutterValidate(basevalue) ? basevalue : .0;

        private static bool OnGutterValidate(object value) => ValidateHelper.IsInRangeOfPosDouble(value, true);

        public double Gutter
        {
            get => (double)GetValue(GutterProperty);
            set => SetValue(GutterProperty, value);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            var totalCellCount = 0;
            var totalRowCount = 1;
            var gutterHalf = Gutter / 2;

            foreach (var child in InternalChildren.OfType<Col>())
            {
                child.Margin = new Thickness(gutterHalf);
                child.Measure(constraint);
                var childDesiredSize = child.DesiredSize;

                if (_maxChildDesiredHeight < childDesiredSize.Height)
                {
                    _maxChildDesiredHeight = childDesiredSize.Height;
                }
            }

            var itemWidth = constraint.Width / ColLayout.ColMaxCellCount;
            var childBounds = new Rect(-gutterHalf, -gutterHalf, 0, _maxChildDesiredHeight);
            _layoutStatus = ColLayout.GetLayoutStatus(constraint.Width);

            foreach (var child in InternalChildren.OfType<Col>())
            {
                var cellCount = child.GetLayoutCellCount(_layoutStatus);
                totalCellCount += cellCount;

                if (totalCellCount > ColLayout.ColMaxCellCount)
                {
                    childBounds.X = -gutterHalf;
                    childBounds.Y += _maxChildDesiredHeight;
                    totalCellCount = cellCount;
                    totalRowCount++;
                }

                var childWidth = cellCount * itemWidth;
                childBounds.Width = childWidth;
                childBounds.X += childWidth + child.Offset * itemWidth;
            }

            return new Size(0, _maxChildDesiredHeight * totalRowCount - Gutter);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var totalCellCount = 0;
            var gutterHalf = Gutter / 2;
            var itemWidth = (finalSize.Width + Gutter) / ColLayout.ColMaxCellCount;
            var childBounds = new Rect(-gutterHalf, -gutterHalf, 0, _maxChildDesiredHeight);
            _layoutStatus = ColLayout.GetLayoutStatus(finalSize.Width);

            foreach (var child in InternalChildren.OfType<Col>())
            {
                var cellCount = child.GetLayoutCellCount(_layoutStatus);
                totalCellCount += cellCount;

                var childWidth = cellCount * itemWidth;
                childBounds.Width = childWidth;
                childBounds.X += child.Offset * itemWidth;
                if (totalCellCount > ColLayout.ColMaxCellCount)
                {
                    childBounds.X = -gutterHalf;
                    childBounds.Y += _maxChildDesiredHeight;
                    totalCellCount = cellCount;
                }

                child.Arrange(childBounds);
                childBounds.X += childWidth;
            }

            return finalSize;
        }
    }
}
