using System.Windows;
using System.Windows.Controls;
using HandyControl.Tools;

namespace HandyControl.Controls
{
    public class RadioGroup : ItemsControl
    {
        protected override DependencyObject GetContainerForItemOverride() => new RadioButton();

        protected override bool IsItemItsOwnContainerOverride(object item) => item is RadioButton;

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            "Orientation", typeof(Orientation), typeof(RadioGroup), new PropertyMetadata(default(Orientation)));

        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        public static readonly DependencyProperty ItemStyleSelectorProperty = DependencyProperty.Register(
            "ItemStyleSelector", typeof(StyleSelector), typeof(RadioGroup), new PropertyMetadata(new RadioGroupItemStyleSelector()));

        public StyleSelector ItemStyleSelector
        {
            get => (StyleSelector)GetValue(ItemStyleSelectorProperty);
            set => SetValue(ItemStyleSelectorProperty, value);
        }

        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);

            var count = Items.Count;
            for (int i = 0; i < count; i++)
            {
                var item = (RadioButton)Items[i];
                item.Style = ItemStyleSelector?.SelectStyle(item, this);
            }
        }
    }
}