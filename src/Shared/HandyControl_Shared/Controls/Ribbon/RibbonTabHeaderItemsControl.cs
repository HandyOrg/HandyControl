using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace HandyControl.Controls
{
    public class RibbonTabHeaderItemsControl : ItemsControl
    {
        protected override DependencyObject GetContainerForItemOverride() => new RibbonTabHeader();

        protected override bool IsItemItsOwnContainerOverride(object item) => item is RibbonTabHeader;

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            if (element is RibbonTabHeader ribbonTabHeader)
            {
                ribbonTabHeader.PrepareRibbonTabHeader();
            }
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            var r = this.RenderSize;
        }
    }
}
