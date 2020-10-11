using System.Windows.Controls;

namespace HandyControl.Controls
{
    public class PropertyItemsControl : ListBox
    {
        protected override bool IsItemItsOwnContainerOverride(object item) => item is PropertyItem;

        public PropertyItemsControl()
        {
#if !NET40
            VirtualizingPanel.SetIsVirtualizingWhenGrouping(this, true);
            VirtualizingPanel.SetScrollUnit(this, ScrollUnit.Pixel);
#endif
        }
    }
}