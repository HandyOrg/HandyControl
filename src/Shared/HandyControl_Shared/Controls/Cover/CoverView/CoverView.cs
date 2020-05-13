using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Data;
using HandyControl.Data;
using HandyControl.Tools;

namespace HandyControl.Controls
{
    public class CoverView : RegularItemsControl
    {
        private readonly CoverViewContent _viewContent;

        private CoverViewItem _selectedItem;

        private IEnumerable _itemsSourceInternal;

        private readonly Dictionary<object, CoverViewItem> _entryDic = new Dictionary<object, CoverViewItem>();

        private bool _isRefresh;

        public CoverView()
        {
            _viewContent = new CoverViewContent();

            AddHandler(SelectableItem.SelectedEvent, new RoutedEventHandler(CoverViewItem_OnSelected));
            _viewContent.SetBinding(CoverViewContent.ContentHeightProperty, new Binding(ItemContentHeightProperty.Name) { Source = this });
            _viewContent.SetBinding(WidthProperty, new Binding(ActualWidthProperty.Name) { Source = this });
        }

        private void CoverViewItem_OnSelected(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is CoverViewItem item)
            {
                if (_selectedItem == null)
                {
                    item.IsSelected = true;
                    _selectedItem = item;
                    if (_viewContent != null)
                    {
                        _viewContent.Content = item.Content;
                        _viewContent.ContentTemplate = ItemTemplate;
                        UpdateCoverViewContent(true);
                    }

                    return;
                }

                if (!Equals(_selectedItem, item))
                {
                    _selectedItem.IsSelected = false;
                    item.IsSelected = true;
                    _selectedItem = item;
                    if (_viewContent != null)
                    {
                        _viewContent.Content = item.Content;
                        UpdateCoverViewContent(true);
                    }

                    return;
                }

                if (_viewContent != null)
                {
                    _viewContent.Content = null;
                    _viewContent.ContentTemplate = null;
                    UpdateCoverViewContent(false);
                }
                _selectedItem.IsSelected = false;
                _selectedItem = null;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _viewContent.Style ??= CoverViewContentStyle;

            if (_selectedItem != null)
            {
                UpdateCoverViewContent(_selectedItem != null);
            }
        }

        protected override DependencyObject GetContainerForItemOverride() => new CoverViewItem();

        private void SetBindingForItem(FrameworkElement item)
        {
            item.SetBinding(MarginProperty, new Binding(ItemMarginProperty.Name) { Source = this });
            item.SetBinding(WidthProperty, new Binding(ItemWidthProperty.Name) { Source = this });
            item.SetBinding(HeightProperty, new Binding(ItemHeightProperty.Name) { Source = this });
            item.SetBinding(HeaderedSelectableItem.HeaderTemplateProperty, new Binding(ItemHeaderTemplateProperty.Name) { Source = this });
        }

        protected override bool IsItemItsOwnContainerOverride(object item) => item is CoverViewItem;

        public static readonly DependencyProperty CoverViewContentStyleProperty = DependencyProperty.Register(
            "CoverViewContentStyle", typeof(Style), typeof(CoverView), new PropertyMetadata(default(Style)));

        public Style CoverViewContentStyle
        {
            get => (Style)GetValue(CoverViewContentStyleProperty);
            set => SetValue(CoverViewContentStyleProperty, value);
        }

        internal static readonly DependencyProperty GroupsProperty = DependencyProperty.Register(
            "Groups", typeof(int), typeof(CoverView),
            new FrameworkPropertyMetadata(ValueBoxes.Int5Box, FrameworkPropertyMetadataOptions.AffectsMeasure,
                OnGroupsChanged), IsGroupsValid);

        internal static void OnGroupsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CoverView ctl)
            {
                ctl.UpdateCoverViewContent(ctl._viewContent.IsOpen);
            }
        }

        private static bool IsGroupsValid(object value)
        {
            var v = (int)value;
            return v >= 1;
        }

        public int Groups
        {
            get => (int)GetValue(GroupsProperty);
            set => SetValue(GroupsProperty, value);
        }

        public static readonly DependencyProperty ItemContentHeightProperty = DependencyProperty.Register(
            "ItemContentHeight", typeof(double), typeof(CoverView), new PropertyMetadata(ValueBoxes.Double300Box),
            ValidateHelper.IsInRangeOfPosDoubleIncludeZero);

        public double ItemContentHeight
        {
            get => (double)GetValue(ItemContentHeightProperty);
            set => SetValue(ItemContentHeightProperty, value);
        }

        public static readonly DependencyProperty ItemContentHeightFixedProperty = DependencyProperty.Register(
            "ItemContentHeightFixed", typeof(bool), typeof(CoverView), new PropertyMetadata(ValueBoxes.TrueBox));

        public bool ItemContentHeightFixed
        {
            get => (bool)GetValue(ItemContentHeightFixedProperty);
            set => SetValue(ItemContentHeightFixedProperty, value);
        }

        public static readonly DependencyProperty ItemHeaderTemplateProperty = DependencyProperty.Register(
            "ItemHeaderTemplate", typeof(DataTemplate), typeof(CoverView), new PropertyMetadata(default(DataTemplate)));

        public DataTemplate ItemHeaderTemplate
        {
            get => (DataTemplate) GetValue(ItemHeaderTemplateProperty);
            set => SetValue(ItemHeaderTemplateProperty, value);
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            Groups = (int)(sizeInfo.NewSize.Width / (ItemWidth + ItemMargin.Left + ItemMargin.Right));
        }

        protected override void Refresh()
        {
            if (ItemsHost == null) return;

            _entryDic.Clear();
            _isRefresh = true;

            foreach (var item in Items)
            {
                AddItem(item);
            }

            _isRefresh = false;
            GenerateIndex();
            UpdateCoverViewContent(_viewContent.IsOpen);
        }

        /// <summary>
        ///     更新内容视图
        /// </summary>
        private void UpdateCoverViewContent(bool isOpen)
        {
            if (_selectedItem == null) return;

            ItemsHost.Children.Remove(_viewContent);

            if (!ItemContentHeightFixed)
            {
                _viewContent.ManualHeight = 0;
                if (_viewContent.Content is FrameworkElement element)
                {
                    element.VerticalAlignment = VerticalAlignment.Top;
                    element.Arrange(new Rect(new Size(double.MaxValue, double.MaxValue)));
                    _viewContent.ManualHeight = element.ActualHeight;
                }
            }

            UpdateCoverViewContentPosition();
            if (_viewContent.CanSwitch)
            {
                _viewContent.IsOpen = isOpen;
            }
        }

        /// <summary>
        ///     更新内容视图位置
        /// </summary>
        private void UpdateCoverViewContentPosition()
        {
            if (_viewContent.Parent != null)
            {
                ItemsHost.Children.Remove(_viewContent);
            }
            var total = _entryDic.Count + 1;
            var totalRow = total / Groups + (total % Groups > 0 ? 1 : 0);
            if (total <= Groups)
            {
                ItemsHost.Children.Add(_viewContent);
            }
            else
            {
                var row = _selectedItem.Index / Groups + 1;
                if (row == totalRow)
                {
                    ItemsHost.Children.Add(_viewContent);
                }
                else
                {
                    ItemsHost.Children.Insert(row * Groups, _viewContent);
                }
            }

            _viewContent.UpdatePosition(_selectedItem.Index, Groups, _selectedItem.DesiredSize.Width);
        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            if (_itemsSourceInternal != null)
            {
                if (_itemsSourceInternal is INotifyCollectionChanged s)
                {
                    s.CollectionChanged -= InternalCollectionChanged;
                }

                Items.Clear();
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

            if (ItemsHost != null)
            {
                GenerateIndex();
            }
        }

        private void ClearItems()
        {
            _selectedItem = null;
            _viewContent.Content = null;
            _viewContent.ContentTemplate = null;

            if (ItemsHost != null)
            {
                ItemsHost.Children.Remove(_viewContent);
                ItemsHost.Children.Clear();
            }

            _entryDic.Clear();
        }

        private void RemoveItem(object item)
        {
            if (_entryDic.TryGetValue(item, out var entry))
            {
                if (ReferenceEquals(entry, _selectedItem))
                {
                    _selectedItem = null;
                    if (_viewContent != null)
                    {
                        _viewContent.Content = null;
                        _viewContent.IsOpen = false;
                        ItemsHost.Children.Remove(_viewContent);
                    }
                }

                ItemsHost.Children.Remove(entry);
                Items.Remove(item);
                _entryDic.Remove(item);
            }
        }

        private void AddItem(object item) => InsertItem(_entryDic.Count, item);

        private void InsertItem(int index, object item)
        {
            if (ItemsHost == null)
            {
                Items.Insert(index, item);
                _entryDic.Add(item, null);
            }
            else
            {
                DependencyObject container;
                if (IsItemItsOwnContainerOverride(item))
                {
                    container = item as DependencyObject;
                }
                else
                {
                    container = GetContainerForItemOverride();
                    PrepareContainerForItemOverride(container, item);
                }

                if (container is CoverViewItem element)
                {
                    SetBindingForItem(element);
                    element.Style = ItemContainerStyle;
                    element.Header ??= item;
                    _entryDic[item] = element;
                    ItemsHost.Children.Insert(index, element);

                    if (IsLoaded && !_isRefresh && _itemsSourceInternal != null)
                    {
                        Items.Insert(index, item);
                    }
                }
            }
        }

        /// <summary>
        ///     生成序号
        /// </summary>
        private void GenerateIndex()
        {
            var index = 0;
            foreach (var item in _entryDic.Values)
            {
                item.Index = index++;
            }
        }

        private void InternalCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (ItemsHost == null) return;

            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                if (_entryDic.Count == 0) return;
                ClearItems();
                Items.Clear();
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
                if (_viewContent.IsOpen)
                {
                    ItemsHost.Children.Remove(_viewContent);
                }

                var count = 0;
                foreach (var item in e.NewItems)
                {
                    var index = e.NewStartingIndex + count++;
                    InsertItem(index, item);
                }
            }

            GenerateIndex();
            if (_viewContent.IsOpen)
            {
                UpdateCoverViewContentPosition();
            }
        }

        protected override void OnItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_itemsSourceInternal != null) return;

            InternalCollectionChanged(sender, e);
        }

        protected override void OnItemTemplateChanged(DependencyPropertyChangedEventArgs e)
        {

        }

        protected override void OnItemContainerStyleChanged(DependencyPropertyChangedEventArgs e)
        {

        }
    }
}