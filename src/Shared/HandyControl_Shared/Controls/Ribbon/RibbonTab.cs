using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using HandyControl.Data;
using HandyControl.Tools;

namespace HandyControl.Controls
{
    public class RibbonTab : HeaderedItemsControl
    {
        public Ribbon Ribbon => Ribbon.GetRibbon(this);

        internal RibbonTabHeader RibbonTabHeader
        {
            get
            {
                var ribbon = Ribbon;
                if (ribbon == null)
                {
                    return null;
                }

                var index = ribbon.ItemContainerGenerator.IndexFromContainer(this);
                if (index < 0)
                {
                    return null;
                }

                var headerItemsControl = ribbon.RibbonTabHeaderItemsControl;
                return headerItemsControl?.ItemContainerGenerator.ContainerFromIndex(index) as RibbonTabHeader;
            }
        }

        static RibbonTab()
        {
            VisibilityProperty.OverrideMetadata(typeof(RibbonTab), new PropertyMetadata(Visibility.Visible, OnVisibilityChanged));
        }

        public static readonly DependencyProperty IsSelectedProperty =
            Selector.IsSelectedProperty.AddOwner(typeof(RibbonTab),
                new FrameworkPropertyMetadata(ValueBoxes.FalseBox,
                    FrameworkPropertyMetadataOptions.AffectsParentMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault |
                    FrameworkPropertyMetadataOptions.Journal,
                    OnIsSelectedChanged));

        private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ribbonTab = (RibbonTab) d;
            if (ribbonTab.IsSelected)
            {
                ribbonTab.OnSelected(new RoutedEventArgs(Selector.SelectedEvent, ribbonTab));
            }
            else
            {
                ribbonTab.OnUnselected(new RoutedEventArgs(Selector.UnselectedEvent, ribbonTab));
            }

            ribbonTab.RibbonTabHeader?.CoerceValue(RibbonTabHeader.IsSelectedProperty);
        }

        public bool IsSelected
        {
            get => (bool) GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        protected virtual void OnSelected(RoutedEventArgs e) => RaiseEvent(e);

        protected virtual void OnUnselected(RoutedEventArgs e) => RaiseEvent(e);

        protected override void OnHeaderChanged(object oldHeader, object newHeader)
        {
            base.OnHeaderChanged(oldHeader, newHeader);
            Ribbon?.NotifyTabHeaderChanged();
        }

        private static void OnVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ribbonTab = (RibbonTab) d;
            var ribbon = ribbonTab.Ribbon;

            if (ribbon == null ||
                !ribbonTab.IsSelected ||
                (Visibility) e.OldValue != Visibility.Visible ||
                (Visibility) e.NewValue == Visibility.Visible)
            {
                return;
            }

            ribbon.ResetSelection();
        }

        protected override DependencyObject GetContainerForItemOverride() => new RibbonGroup();

        protected override bool IsItemItsOwnContainerOverride(object item) => item is RibbonGroup;
    }
}
