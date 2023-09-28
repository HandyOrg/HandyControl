using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using HandyControl.Collections;
using HandyControl.Data;
using HandyControl.Interactivity;

namespace HandyControl.Controls;

[TemplatePart(Name = ElementSelectedListBox, Type = typeof(ListBox))]
[DefaultEvent(nameof(TransferredItemsChanged))]
public class Transfer : ListBox
{
    private const string ElementSelectedListBox = "PART_SelectedListBox";

    public static readonly RoutedEvent TransferredItemsChangedEvent =
        EventManager.RegisterRoutedEvent("TransferredItemsChanged", RoutingStrategy.Bubble,
            typeof(SelectionChangedEventHandler), typeof(Transfer));

    [Category("Behavior")]
    public event SelectionChangedEventHandler TransferredItemsChanged
    {
        add => AddHandler(TransferredItemsChangedEvent, value);
        remove => RemoveHandler(TransferredItemsChangedEvent, value);
    }

    private ListBox _selectedListBox;

    private static readonly DependencyPropertyKey TransferredItemsPropertyKey =
        DependencyProperty.RegisterReadOnly("TransferredItems", typeof(IList),
            typeof(Transfer), new FrameworkPropertyMetadata((IList) null));

    private static readonly DependencyProperty TransferredItemsImplProperty =
        TransferredItemsPropertyKey.DependencyProperty;

    [Bindable(true), Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public IList TransferredItems => TransferredItemsImpl;

    private IList TransferredItemsImpl => (IList) GetValue(TransferredItemsImplProperty);

    public Transfer()
    {
        CommandBindings.Add(new CommandBinding(ControlCommands.Selected, SelectItems));
        CommandBindings.Add(new CommandBinding(ControlCommands.Cancel, DeselectItems));
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        SelectItems(null, null);
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _selectedListBox = GetTemplateChild(ElementSelectedListBox) as ListBox;
    }

    protected virtual void OnTransferredItemsChanged(SelectionChangedEventArgs e)
    {
        RaiseEvent(e);
    }

    protected override bool IsItemItsOwnContainerOverride(object item) => item is TransferItem;

    protected override DependencyObject GetContainerForItemOverride() => new TransferItem();

    private void SelectItems(object sender, ExecutedRoutedEventArgs e)
    {
        if (_selectedListBox == null || SelectedItems.Count == 0)
        {
            return;
        }

        foreach (var item in SelectedItems)
        {
            if (ItemContainerGenerator.ContainerFromItem(item) is not TransferItem { IsTransferred: false } selectedItem)
            {
                continue;
            }

            selectedItem.IsTransferred = true;

            var transferItem = new TransferItem
            {
                Tag = item
            };

            if (ItemsSource != null)
            {
                transferItem.SetBinding(ContentControl.ContentProperty, new Binding(DisplayMemberPath) { Source = item });
            }
            else
            {
                transferItem.Content = IsItemItsOwnContainerOverride(item) ? ((TransferItem) item).Content : item;
            }

            _selectedListBox.Items.Add(transferItem);
        }

        SetTransferredItems(_selectedListBox.Items.OfType<TransferItem>().Select(item => item.Tag));
        OnTransferredItemsChanged(new SelectionChangedEventArgs(TransferredItemsChangedEvent, new List<object>(), SelectedItems)
        {
            Source = this
        });
        UnselectAll();
    }

    private void DeselectItems(object sender, ExecutedRoutedEventArgs e)
    {
        if (_selectedListBox == null)
        {
            return;
        }

        var deselectItems = new List<object>();
        foreach (var transferItem in _selectedListBox.Items.OfType<TransferItem>().ToList())
        {
            if (!transferItem.IsSelected)
            {
                continue;
            }

            if (ItemContainerGenerator.ContainerFromItem(transferItem.Tag) is not TransferItem selectedItem)
            {
                continue;
            }

            _selectedListBox.Items.Remove(transferItem);
            deselectItems.Add(transferItem.Tag);
            selectedItem.SetCurrentValue(TransferItem.IsTransferredProperty, ValueBoxes.FalseBox);
            selectedItem.SetCurrentValue(ListBoxItem.IsSelectedProperty, ValueBoxes.FalseBox);
        }

        SetTransferredItems(_selectedListBox.Items.OfType<TransferItem>().Select(item => item.Tag));
        OnTransferredItemsChanged(new SelectionChangedEventArgs(TransferredItemsChangedEvent, deselectItems, new List<object>())
        {
            Source = this
        });
    }

    private void SetTransferredItems(IEnumerable selectedItems)
    {
        var oldSelectedItems = (ManualObservableCollection<object>) GetValue(TransferredItemsImplProperty);

        if (oldSelectedItems == null)
        {
            oldSelectedItems = new ManualObservableCollection<object>();
            SetValue(TransferredItemsPropertyKey, oldSelectedItems);
        }

        oldSelectedItems.CanNotify = false;
        oldSelectedItems.Clear();

        foreach (var selectedItem in selectedItems)
        {
            oldSelectedItems.Add(selectedItem);
        }

        oldSelectedItems.CanNotify = true;
    }
}
