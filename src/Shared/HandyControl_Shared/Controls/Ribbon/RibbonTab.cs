using System.Windows;
using System.Windows.Controls;
using HandyControl.Data;

namespace HandyControl.Controls
{
    public class RibbonTab : HeaderedItemsControl
    {
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
            "IsSelected", typeof(bool), typeof(RibbonTab), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool IsSelected
        {
            get => (bool) GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        protected override DependencyObject GetContainerForItemOverride() => new RibbonGroup();

        protected override bool IsItemItsOwnContainerOverride(object item) => item is RibbonGroup;
    }
}
