using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace HandyControl.Controls
{
    [TemplatePart(Name = TabHeaderItemsControl, Type = typeof(ItemsControl))]
    public class Ribbon : Selector
    {
        private const string TabHeaderItemsControl = "PART_TabHeaderItemsControl";

        private ItemsControl _tabHeaderItemsControl;

        private readonly ObservableCollection<object> _tabHeaderItemsSource = new();

        internal ItemsControl RibbonTabHeaderItemsControl => _tabHeaderItemsControl;

        public Ribbon()
        {
            SetRibbon(this, this);
            ItemContainerGenerator.StatusChanged += OnItemContainerGeneratorStatusChanged;
        }

        internal static readonly DependencyProperty RibbonProperty = DependencyProperty.RegisterAttached(
            "Ribbon", typeof(Ribbon), typeof(Ribbon),
            new FrameworkPropertyMetadata(default(Ribbon), FrameworkPropertyMetadataOptions.Inherits));

        internal static void SetRibbon(DependencyObject element, Ribbon value)
            => element.SetValue(RibbonProperty, value);

        internal static Ribbon GetRibbon(DependencyObject element)
            => (Ribbon) element.GetValue(RibbonProperty);

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _tabHeaderItemsControl = GetTemplateChild(TabHeaderItemsControl) as ItemsControl;
            if (_tabHeaderItemsControl is {ItemsSource: null})
            {
                _tabHeaderItemsControl.ItemsSource = _tabHeaderItemsSource;
            }
        }

        internal void ResetSelection()
        {
            SelectedIndex = -1;
            InitializeSelection();
        }

        internal void NotifyMouseClickedOnTabHeader(RibbonTabHeader tabHeader)
        {
            if (_tabHeaderItemsControl == null)
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

        internal void NotifyTabHeaderChanged()
        {
            if (ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
            {
                return;
            }

            RefreshHeaderCollection();
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

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);

            if (e.AddedItems is not {Count: > 0})
            {
                return;
            }

            if (e.RemovedItems.Count > 0 && ItemContainerGenerator.ContainerFromItem(e.RemovedItems[0]) is RibbonTab oldRibbonTab)
            {
                oldRibbonTab.IsSelected = false;
            }

            SelectedItem = e.AddedItems[0];
        }

        private void OnItemContainerGeneratorStatusChanged(object sender, EventArgs e)
        {
            if (ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
            {
                return;
            }

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
