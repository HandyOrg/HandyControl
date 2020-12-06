using System.Collections.Specialized;
using System.Windows;

namespace HandyControl.Controls
{
    public class TransferGroup : SimpleItemsControl
    {
        protected override void Refresh()
        {
            if (Items.Count > 0)
            {
                foreach (FrameworkElement item in Items)
                {
                    item.Style = ItemContainerStyle;
                    ItemsHost.Children.Add(item);
                }
            }
        }

        protected override void OnItemTemplateChanged(DependencyPropertyChangedEventArgs e)
        {

        }

        protected override void OnItemContainerStyleChanged(DependencyPropertyChangedEventArgs e)
        {

        }

        protected override DependencyObject GetContainerForItemOverride() => new TransferItem();

        protected override bool IsItemItsOwnContainerOverride(object item) => item is TransferItem;

        protected override void OnItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (ItemsHost == null) return;

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null)
                    {
                        if (e.NewItems[0] is FrameworkElement item)
                        {
                            item.Style = ItemContainerStyle;
                            ItemsHost.Children.Insert(e.NewStartingIndex, item);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems != null)
                    {
                        foreach (FrameworkElement item in e.OldItems)
                        {
                            ItemsHost.Children.Remove(item);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    ItemsHost.Children.Clear();
                    break;
            }
        }
    }
}
