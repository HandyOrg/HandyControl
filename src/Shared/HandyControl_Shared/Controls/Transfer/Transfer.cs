using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HandyControl.Interactivity;
using HandyControl.Tools.Extension;

namespace HandyControl.Controls
{
    /// <summary>
    ///     穿梭框
    /// </summary>
    [TemplatePart(Name = ElementItemsOrigin, Type = typeof(SimpleItemsControl))]
    [TemplatePart(Name = ElementItemsSelected, Type = typeof(SimpleItemsControl))]
    public class Transfer : SimpleItemsControl
    {
        private const string ElementItemsOrigin = "PART_ItemsOrigin";

        private const string ElementItemsSelected = "PART_ItemsSelected";

        private readonly Dictionary<object, TransferEntry> _entryDic = new Dictionary<object, TransferEntry>();

        private readonly List<TransferEntry> _entryList = new List<TransferEntry>();

        private SimpleItemsControl _itemsOrigin;

        private SimpleItemsControl _itemsSelected;

        private IEnumerable _itemsSourceInternal;

        private IEnumerable _initEnumerable;

        private NotifyCollectionChangedEventArgs _initArgs;

        public static readonly RoutedEvent SelectionChangedEvent =
            EventManager.RegisterRoutedEvent("SelectionChanged", RoutingStrategy.Bubble,
                typeof(SelectionChangedEventHandler), typeof(Transfer));

        public event SelectionChangedEventHandler SelectionChanged
        {
            add => AddHandler(SelectionChangedEvent, value);
            remove => RemoveHandler(SelectionChangedEvent, value);
        }

        private static readonly DependencyPropertyKey SelectedItemsPropertyKey =
            DependencyProperty.RegisterReadOnly(
                "SelectedItems",
                typeof(IList),
                typeof(Transfer),
                new FrameworkPropertyMetadata(default(IList)));

        public static readonly DependencyProperty SelectedItemsProperty = SelectedItemsPropertyKey.DependencyProperty;

        public IList SelectedItems
        {
            get => (IList) GetValue(SelectedItemsProperty);
            set => SetValue(SelectedItemsProperty, value);
        }

        public Transfer()
        {
            CommandBindings.Add(new CommandBinding(ControlCommands.Selected, SelectItems));
            CommandBindings.Add(new CommandBinding(ControlCommands.Cancel, DeselectItems));

            SetValue(SelectedItemsPropertyKey, new ObservableCollection<object>());
        }

        public override void OnApplyTemplate()
        {
            _itemsOrigin?.ItemsHost.Children.Clear();
            _itemsSelected?.ItemsHost.Children.Clear();

            _itemsOrigin = GetTemplateChild(ElementItemsOrigin) as SimpleItemsControl;
            _itemsSelected = GetTemplateChild(ElementItemsSelected) as SimpleItemsControl;

            if (_itemsOrigin != null && _itemsSelected != null)
            {
                if (_initEnumerable != null)
                {
                    OnItemsSourceChanged(null, _initEnumerable);
                    _initEnumerable = null;
                }

                if (_initArgs != null)
                {
                    InternalCollectionChanged(null, _initArgs);
                    _initArgs = null;
                }

                if (_itemsOrigin.Items.Count == 0)
                {
                    Refresh();
                }
            }
        }

        private void AddItem(object item)
        {
            var origin = new TransferItem
            {
                Content = item
            };
            var selected = new TransferItem
            {
                Content = item,
                IsOrigin = false
            };
            var entry = new TransferEntry
            {
                OriginItem = origin,
                SelectedItem = selected
            };
            _entryDic.Add(item, entry);
            _entryList.Add(entry);
            _itemsOrigin.Items.Add(origin);
            _itemsSelected.Items.Add(selected);
        }

        private void RemoveItem(object item)
        {
            if (_entryDic.TryGetValue(item, out var entry))
            {
                _itemsOrigin.Items.Remove(entry.OriginItem);
                _itemsSelected.Items.Remove(entry.SelectedItem);
                _entryDic.Remove(item);
                _entryList.Remove(entry);
            }
        }

        private void ClearItems()
        {
            _entryDic.Clear();
            _entryList.Clear();
            _itemsOrigin.Items.Clear();
            _itemsSelected.Items.Clear();
        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            if (_itemsOrigin == null || _itemsSelected == null)
            {
                _initEnumerable = newValue;
                return;
            }

            if (_itemsSourceInternal != null)
            {
                if (_itemsSourceInternal is INotifyCollectionChanged s)
                {
                    s.CollectionChanged -= InternalCollectionChanged;
                }

                ClearItems();
            }
            _itemsSourceInternal = newValue;
            if (_itemsSourceInternal != null)
            {
                if (_itemsSourceInternal is INotifyCollectionChanged s)
                {
                    s.CollectionChanged += InternalCollectionChanged;
                }
                foreach (var item in _itemsSourceInternal)
                {
                    AddItem(item);
                }
            }
        }

        private void InternalCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_itemsOrigin == null || _itemsSelected == null)
            {
                _initArgs = e;
                return;
            }

            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    RemoveItem(item);
                }
            }
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    AddItem(item);
                }
            }
        }

        protected override DependencyObject GetContainerForItemOverride() => new TransferItem();

        protected override bool IsItemItsOwnContainerOverride(object item) => item is TransferItem;

        private void SelectItems(object sender, ExecutedRoutedEventArgs e)
        {
            var list = _entryDic.Where(item => item.Value.OriginItem.IsSelected).ToList();
            list.Do(item => item.Value.StatusSwitch(true)).Do(item => SelectedItems.Add(item.Key));
            RaiseEvent(new SelectionChangedEventArgs(SelectionChangedEvent, new List<object>(), list));
        }

        private void DeselectItems(object sender, ExecutedRoutedEventArgs e)
        {
            var list = _entryDic.Where(item => item.Value.SelectedItem.IsSelected).ToList();
            list.Do(item => item.Value.StatusSwitch(false)).Do(item => SelectedItems.Remove(item.Key));
            RaiseEvent(new SelectionChangedEventArgs(SelectionChangedEvent, list, new List<object>()));
        }

        protected override void OnItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

        }

        protected override void OnItemTemplateChanged(DependencyPropertyChangedEventArgs e)
        {
            
        }

        protected override void OnItemContainerStyleChanged(DependencyPropertyChangedEventArgs e)
        {
            
        }

        protected override void Refresh()
        {
            if (_entryList.Count > 0)
            {
                _entryList.Select(item => item.OriginItem).Do(item => _itemsOrigin.Items.Add(item));
                _entryList.Select(item => item.SelectedItem).Do(item => _itemsSelected.Items.Add(item));
            }
        }

        private struct TransferEntry
        {
            public TransferItem OriginItem { get; set; }

            public TransferItem SelectedItem { get; set; }

            public void StatusSwitch(bool isTransferred)
            {
                if (isTransferred)
                {
                    OriginItem.IsTransferred = true;
                    SelectedItem.IsTransferred = true;
                    OriginItem.IsSelected = false;
                }
                else
                {
                    OriginItem.IsTransferred = false;
                    SelectedItem.IsTransferred = false;
                    SelectedItem.IsSelected = false;
                }
            }
        }
    }
}