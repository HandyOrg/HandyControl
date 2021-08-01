using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using HandyControl.Data;

namespace HandyControl.Controls
{
    [TemplatePart(Name = TabHeaderItemsControl, Type = typeof(ItemsControl))]
    public class Ribbon : Selector
    {
        private const string TabHeaderItemsControl = "PART_TabHeaderItemsControl";

        private ItemsControl _tabHeaderItemsControl;

        private readonly ObservableCollection<object> _tabHeaderItemsSource = new();

        public Ribbon()
        {
            ItemContainerGenerator.StatusChanged += OnItemContainerGeneratorStatusChanged;

            AddHandler(SelectableItem.SelectedEvent, new RoutedEventHandler(OnTabHeaderSelected));
        }

        private void OnTabHeaderSelected(object sender, RoutedEventArgs e)
        {
            if (_tabHeaderItemsControl == null)
            {
                return;
            }

            if (e.OriginalSource is not RibbonTabHeader tabHeader)
            {
                return;
            }

            var selectedIndex = _tabHeaderItemsControl.ItemContainerGenerator.IndexFromContainer(tabHeader);
            var currentSelectedIndex = SelectedIndex;

            if (currentSelectedIndex < 0 || SelectedIndex != selectedIndex)
            {
                SelectedIndex = selectedIndex;
            }
        }

        private static readonly DependencyPropertyKey SelectedContentPropertyKey =
            DependencyProperty.RegisterReadOnly("SelectedContent", typeof(object), typeof(Ribbon),
                new FrameworkPropertyMetadata((object) null));

        public static readonly DependencyProperty SelectedContentProperty =
            SelectedContentPropertyKey.DependencyProperty;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object SelectedContent
        {
            get => GetValue(SelectedContentProperty);
            internal set => SetValue(SelectedContentPropertyKey, value);
        }

        public static readonly DependencyProperty IsMinimizedProperty = DependencyProperty.Register(nameof(IsMinimized),
            typeof(bool), typeof(Ribbon),
            new PropertyMetadata(ValueBoxes.FalseBox, OnIsMinimizedChanged, CoerceIsMinimized));

        private static object CoerceIsMinimized(DependencyObject d, object basevalue)
        {
            var num = (bool) basevalue ? 1 : 0;
            var ribbon = (Ribbon) d;
            return num != 0 && ribbon.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated ? ValueBoxes.FalseBox : basevalue;
        }

        private static void OnIsMinimizedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }

        public bool IsMinimized
        {
            get => (bool) GetValue(IsMinimizedProperty);
            set => SetValue(IsMinimizedProperty, ValueBoxes.BooleanBox(value));
        }

        public static readonly DependencyProperty IsDropDownOpenProperty =
            DependencyProperty.Register(nameof(IsDropDownOpen), typeof(bool), typeof(Ribbon),
                new PropertyMetadata(ValueBoxes.FalseBox, OnIsDropDownOpenChanged, CoerceIsDropDownOpen));

        private static object CoerceIsDropDownOpen(DependencyObject d, object basevalue)
        {
            return ValueBoxes.FalseBox;
        }

        private static void OnIsDropDownOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }

        public bool IsDropDownOpen
        {
            get => (bool) GetValue(IsDropDownOpenProperty);
            set => SetValue(IsDropDownOpenProperty, ValueBoxes.BooleanBox(value));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _tabHeaderItemsControl = GetTemplateChild(TabHeaderItemsControl) as ItemsControl;
            if (_tabHeaderItemsControl is {ItemsSource: null})
            {
                _tabHeaderItemsControl.ItemsSource = _tabHeaderItemsSource;
            }

            UpdateSelectedContent();
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            if (e.Action == NotifyCollectionChangedAction.Remove ||
                e.Action == NotifyCollectionChangedAction.Replace ||
                e.Action == NotifyCollectionChangedAction.Reset)
            {
                InitializeSelection();
            }

            if ((ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated || e.Action != NotifyCollectionChangedAction.Move) &&
                e.Action != NotifyCollectionChangedAction.Remove)
            {
                return;
            }

            RefreshHeaderCollection();
        }

        protected override DependencyObject GetContainerForItemOverride() => new RibbonTab();

        protected override bool IsItemItsOwnContainerOverride(object item) => item is RibbonTab;

        private void OnItemContainerGeneratorStatusChanged(object sender, EventArgs e)
        {
            if (ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
            {
                return;
            }

            CoerceValue(Ribbon.IsMinimizedProperty);
            InitializeSelection();
            RefreshHeaderCollection();
        }

        private int GetFirstVisibleTabIndex()
        {
            var count = Items.Count;
            for (var index = 0; index < count; ++index)
            {
                if (ItemContainerGenerator.ContainerFromIndex(index) is RibbonTab {IsVisible: true})
                {
                    return index;
                }
            }

            return -1;
        }

        private void InitializeSelection()
        {
            if (SelectedIndex >= 0 || Items.Count <= 0)
            {
                return;
            }

            var firstVisibleTabIndex = GetFirstVisibleTabIndex();
            if (firstVisibleTabIndex < 0)
            {
                return;
            }

            SelectedIndex = firstVisibleTabIndex;
        }

        private static bool EqualsEx(object o1, object o2)
        {
            try
            {
                return Equals(o1, o2);
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }

        private RibbonTab GetSelectedRibbonTab()
        {
            var selectedItem = SelectedItem;
            if (selectedItem != null)
            {
                if (selectedItem is not RibbonTab ribbonTab)
                {
                    ribbonTab = ItemContainerGenerator.ContainerFromIndex(SelectedIndex) as RibbonTab;

                    if (ribbonTab == null || !EqualsEx(selectedItem, ItemContainerGenerator.ItemFromContainer(ribbonTab)))
                    {
                        ribbonTab = ItemContainerGenerator.ContainerFromItem(selectedItem) as RibbonTab;
                    }
                }

                return ribbonTab;
            }

            return null;
        }

        private void UpdateSelectedContent()
        {
            if (SelectedIndex < 0)
            {
                SelectedContent = null;
                return;
            }

            var ribbonTab = GetSelectedRibbonTab();
            if (ribbonTab == null)
            {
                return;
            }

            var visualParent = VisualTreeHelper.GetParent(ribbonTab) as FrameworkElement;

            //seitem
        }

        private static object CreateDefaultHeaderObject() => new SingleSpaceObject();

        private void RefreshHeaderCollection()
        {
            var itemsCount = Items.Count;
            for (var index = 0; index < itemsCount; ++index)
            {
                object header = null;
                if (ItemContainerGenerator.ContainerFromIndex(index) is RibbonTab ribbonTab)
                {
                    header = ribbonTab.Header;
                }

                header ??= CreateDefaultHeaderObject();

                if (index >= _tabHeaderItemsSource.Count)
                {
                    _tabHeaderItemsSource.Add(header);
                }
                else
                {
                    _tabHeaderItemsSource[index] = header;
                }
            }

            var headerCount = _tabHeaderItemsSource.Count;
            for (var index = 0; index < headerCount - itemsCount; ++index)
            {
                _tabHeaderItemsSource.RemoveAt(itemsCount);
            }
        }

        private class SingleSpaceObject
        {
            public override string ToString() => " ";
        }
    }
}
