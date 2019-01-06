using System.Windows;
using System.Windows.Data;
using HandyControl.Data;

namespace HandyControl.Controls
{
    public class CoverView : RegularItemsControl
    {
        private readonly CoverViewContent _viewContent;

        private CoverViewItem _selectedItem;

        public CoverView()
        {
            _viewContent = new CoverViewContent {ContentHeight = ItemContentHeight};

            AddHandler(SelectableItem.SelectedEvent, new RoutedEventHandler(CoverViewItem_OnSelected));
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
                        UpdateCoverViewContent(true);
                    }

                    return;
                }

                if(!Equals(_selectedItem, item))
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
                    UpdateCoverViewContent(false);
                }
                _selectedItem.IsSelected = false;
                _selectedItem = null;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_viewContent.Style == null)
            {
                _viewContent.Style = CoverViewContentStyle;
            }

            if (_selectedItem != null)
            {
                UpdateCoverViewContent(_selectedItem != null);
            }
        }

        protected override DependencyObject GetContainerForItemOverride() => new CoverViewItem();

        protected override bool IsItemItsOwnContainerOverride(object item) => item is CoverViewItem;

        public static readonly DependencyProperty CoverViewContentStyleProperty = DependencyProperty.Register(
            "CoverViewContentStyle", typeof(Style), typeof(CoverView), new PropertyMetadata(default(Style)));

        public Style CoverViewContentStyle
        {
            get => (Style) GetValue(CoverViewContentStyleProperty);
            set => SetValue(CoverViewContentStyleProperty, value);
        }

        internal static readonly DependencyProperty GroupsProperty = DependencyProperty.Register(
            "Groups", typeof(int), typeof(CoverView), new FrameworkPropertyMetadata(ValueBoxes.Int5Box, FrameworkPropertyMetadataOptions.AffectsMeasure, OnGroupsChanged), IsGroupsValid);

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
            "ItemContentHeight", typeof(double), typeof(CoverView), new PropertyMetadata(ValueBoxes.Double300Box));

        public double ItemContentHeight
        {
            get => (double) GetValue(ItemContentHeightProperty);
            set => SetValue(ItemContentHeightProperty, value);
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            Groups = (int)(sizeInfo.NewSize.Width / (ItemWidth + ItemMargin.Left + ItemMargin.Right));
        }

        protected override void Refresh()
        {
            if (ItemsHost == null) return;

            ItemsHost.Children.Clear();
            var index = 0;
            foreach (var item in Items)
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
                    element.Index = index++;
                    element.Style = ItemContainerStyle;
                    ItemsHost.Children.Add(element);
                }
            }
        }

        /// <summary>
        ///     更新内容视图
        /// </summary>
        private void UpdateCoverViewContent(bool isOpen)
        {
            if (_selectedItem == null) return;

            ItemsHost.Children.Remove(_viewContent);

            var total = Items.Count + 1;
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
            if (_viewContent.CanSwitch)
            {
                _viewContent.IsOpen = isOpen;
            }
        }
    }
}