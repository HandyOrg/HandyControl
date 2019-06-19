using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using HandyControl.Tools;

namespace HandyControl.Controls
{
    public class ButtonGroup : ItemsControl
    {
        protected override bool IsItemItsOwnContainerOverride(object item) => item is Button || item is RadioButton || item is ToggleButton;

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            "Orientation", typeof(Orientation), typeof(ButtonGroup), new PropertyMetadata(default(Orientation)));

        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        public static readonly DependencyProperty ItemStyleSelectorProperty = DependencyProperty.Register(
            "ItemStyleSelector", typeof(StyleSelector), typeof(ButtonGroup), new PropertyMetadata(new ButtonGroupItemStyleSelector()));

        public StyleSelector ItemStyleSelector
        {
            get => (StyleSelector)GetValue(ItemStyleSelectorProperty);
            set => SetValue(ItemStyleSelectorProperty, value);
        }

        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);

            var count = Items.Count;
            for (var i = 0; i < count; i++)
            {
                var item = (ButtonBase)Items[i];
                item.Style = ItemStyleSelector?.SelectStyle(item, this);
            }
        }
    }
}
