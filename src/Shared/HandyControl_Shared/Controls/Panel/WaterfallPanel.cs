using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HandyControl.Data;

namespace HandyControl.Controls
{
    public class WaterfallPanel : Panel
    {
        public static readonly DependencyProperty GroupsProperty = DependencyProperty.Register(
            "Groups", typeof(int), typeof(WaterfallPanel), new FrameworkPropertyMetadata(
                ValueBoxes.Int2Box, FrameworkPropertyMetadataOptions.AffectsMeasure), IsGroupsValid);

        public int Groups
        {
            get => (int) GetValue(GroupsProperty);
            set => SetValue(GroupsProperty, value);
        }

        private static bool IsGroupsValid(object value) => (int)value >= 1;

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            "Orientation", typeof(Orientation), typeof(WaterfallPanel), new FrameworkPropertyMetadata(
                Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public Orientation Orientation
        {
            get => (Orientation) GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }       

        protected override Size MeasureOverride(Size constraint)
        {
            var groups = Groups;

            if (groups < 1) return constraint;
            var children = InternalChildren;
            Size panelSize;

            if (Orientation == Orientation.Horizontal)
            {
                var heightArr = new double[groups].ToList();
                var itemWidth = constraint.Width / groups;
                if (double.IsNaN(itemWidth) || double.IsInfinity(itemWidth)) return constraint;

                for (int i = 0, count = children.Count; i < count; i++)
                {
                    var child = children[i];
                    if (child == null) continue;

                    child.Measure(constraint);
                    var minIndex = heightArr.IndexOf(heightArr.Min());
                    var minY = heightArr[minIndex];
                    child.Arrange(new Rect(new Point(minIndex * itemWidth, minY), new Size(itemWidth, child.DesiredSize.Height)));

                    heightArr[minIndex] = minY + child.DesiredSize.Height;
                }
                panelSize = new Size(constraint.Width, heightArr.Max());
            }
            else
            {
                var widthArr = new double[groups].ToList();
                var itemHeight = constraint.Height / groups;
                if (double.IsNaN(itemHeight) || double.IsInfinity(itemHeight)) return constraint;

                for (int i = 0, count = children.Count; i < count; i++)
                {
                    var child = children[i];
                    if (child == null) continue;

                    child.Measure(constraint);
                    var minIndex = widthArr.IndexOf(widthArr.Min());
                    var minX = widthArr[minIndex];
                    child.Arrange(new Rect(new Point(minX, minIndex * itemHeight), new Size(child.DesiredSize.Width, itemHeight)));

                    widthArr[minIndex] = minX + child.DesiredSize.Width;
                }
                panelSize = new Size(widthArr.Max(), constraint.Height);
            }

            return panelSize;
        }
    }
}