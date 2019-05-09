using HandyControl.Tools.Extension;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HandyControl.Data;

namespace HandyControl.Controls
{
    public class Row : Panel
    {
        private ColLayoutStatus _layoutStatus;

        public static readonly DependencyProperty GutterProperty = DependencyProperty.Register(
            "Gutter", typeof(double), typeof(Row), new PropertyMetadata(default(double)));

        public double Gutter
        {
            get => (double)GetValue(GutterProperty);
            set => SetValue(GutterProperty, value);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            var itemWidth = constraint.Width / ColLayout.ColMaxCellCount;
            var totalCellCount = 0;
            var totalRowCount = 1;
            var maxChildDesiredHeight = .0;

            for (int i = 0, count = InternalChildren.Count; i < count; ++i)
            {
                var child = InternalChildren[i];

                child.Measure(constraint);
                var childDesiredSize = child.DesiredSize;

                if (maxChildDesiredHeight < childDesiredSize.Height)
                {
                    maxChildDesiredHeight = childDesiredSize.Height;
                }
            }

            var childBounds = new Rect(0, 0, 0, maxChildDesiredHeight);
            _layoutStatus = ColLayout.GetLayoutStatus(constraint.Width);

            foreach (var child in InternalChildren.OfType<Col>())
            {
                var cellCount = child.GetLayoutCellCount(_layoutStatus);
                totalCellCount += cellCount;

                if (totalCellCount > ColLayout.ColMaxCellCount)
                {
                    childBounds.X = 0;
                    childBounds.Y += maxChildDesiredHeight;
                    totalCellCount = cellCount;
                    totalRowCount++;
                }

                var childWidth = cellCount * itemWidth;
                childBounds.Width = childWidth;
                child.Arrange(childBounds);
                childBounds.X += childWidth;
            }

            return new Size(constraint.Width, maxChildDesiredHeight * totalRowCount);
        }
    }
}
