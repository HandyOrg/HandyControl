using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using HandyControl.Data;
using HandyControl.Interactivity;

namespace HandyControl.Controls;

[TemplatePart(Name = ElementSelectedListBox, Type = typeof(ListBox))]
[DefaultEvent(nameof(Transferred))]
public class Transfer : ListBox
{
    private const string ElementSelectedListBox = "PART_SelectedListBox";

    public static readonly RoutedEvent TransferredEvent =
        EventManager.RegisterRoutedEvent("Transferred", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(Transfer));

    [Category("Behavior")]
    public event RoutedEventHandler Transferred
    {
        add => AddHandler(TransferredEvent, value);
        remove => RemoveHandler(TransferredEvent, value);
    }

    private ListBox _selectedListBox;

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

    protected virtual void OnTransferred(RoutedEventArgs e)
    {
        RaiseEvent(e);
    }

    protected override bool IsItemItsOwnContainerOverride(object item) => item is TransferItem;

    protected override DependencyObject GetContainerForItemOverride() => new TransferItem();

    private void SelectItems(object sender, ExecutedRoutedEventArgs e)
    {
        if (_selectedListBox == null)
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

        OnTransferred(new RoutedEventArgs(TransferredEvent, this));
    }

    private void DeselectItems(object sender, ExecutedRoutedEventArgs e)
    {
        if (_selectedListBox == null)
        {
            return;
        }

        foreach (var transferItem in _selectedListBox.Items.OfType<TransferItem>().ToList())
        {
            if (!transferItem.IsSelected)
            {
                continue;
            }

            if (ItemContainerGenerator.ContainerFromItem(transferItem.Tag) is TransferItem selectedItem)
            {
                _selectedListBox.Items.Remove(transferItem);

                selectedItem.SetCurrentValue(TransferItem.IsTransferredProperty, ValueBoxes.FalseBox);
                selectedItem.SetCurrentValue(ListBoxItem.IsSelectedProperty, ValueBoxes.FalseBox);
            }
        }

        OnTransferred(new RoutedEventArgs(TransferredEvent, this));
    }
}
