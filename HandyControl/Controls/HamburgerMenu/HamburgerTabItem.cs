using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HandyControl.Controls
{
    public class HamburgerTabItem : TabItem
    {
        public static readonly DependencyProperty IconProperty = ElementBase.Property<HamburgerTabItem, Geometry>(nameof(IconProperty), null);
        public static readonly DependencyProperty IconMoveProperty = ElementBase.Property<HamburgerTabItem, Geometry>(nameof(IconMoveProperty), null);
        public static readonly DependencyProperty TextHorizontalAlignmentProperty = ElementBase.Property<HamburgerTabItem, HorizontalAlignment>(nameof(TextHorizontalAlignmentProperty), HorizontalAlignment.Right);

        public Geometry Icon { get { return (Geometry)GetValue(IconProperty); } set { SetValue(IconProperty, value); } }
        public Geometry IconMove { get { return (Geometry)GetValue(IconMoveProperty); } set { SetValue(IconMoveProperty, value); } }
        public HorizontalAlignment TextHorizontalAlignment { get { return (HorizontalAlignment)GetValue(TextHorizontalAlignmentProperty); } set { SetValue(TextHorizontalAlignmentProperty, value); } }

        static HamburgerTabItem()
        {
            ElementBase.DefaultStyle<HamburgerTabItem>(DefaultStyleKeyProperty);
        }
    }
}
