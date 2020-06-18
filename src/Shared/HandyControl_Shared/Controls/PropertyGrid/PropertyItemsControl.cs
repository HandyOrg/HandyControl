using System.Windows.Controls;

namespace HandyControl.Controls
{
    public class PropertyItemsControl : ItemsControl
    {
        protected override bool IsItemItsOwnContainerOverride(object item) => item is PropertyItem;
    }
}