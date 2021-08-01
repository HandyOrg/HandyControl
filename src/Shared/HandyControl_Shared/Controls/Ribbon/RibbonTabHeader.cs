using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HandyControl.Data;

namespace HandyControl.Controls
{
    public class RibbonTabHeader : ContentControl
    {
        public Ribbon Ribbon => Ribbon.GetRibbon(this);

        internal RibbonTab RibbonTab
        {
            get
            {
                var itemsControl = ItemsControl.ItemsControlFromItemContainer(this);
                var ribbon = Ribbon;
                if (itemsControl == null || ribbon == null)
                {
                    return null;
                }

                var index = itemsControl.ItemContainerGenerator.IndexFromContainer(this);
                return ribbon.ItemContainerGenerator.ContainerFromIndex(index) as RibbonTab;
            }
        }

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
            "IsSelected", typeof(bool), typeof(RibbonTabHeader),
            new PropertyMetadata(ValueBoxes.FalseBox, OnIsSelectedChanged, CoerceIsSelected));

        private static object CoerceIsSelected(DependencyObject d, object basevalue)
        {
            var ribbonTab = ((RibbonTabHeader) d).RibbonTab;
            return ribbonTab != null ? ValueBoxes.BooleanBox(ribbonTab.IsSelected) : basevalue;
        }

        private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((RibbonTabHeader) d).OnIsSelectedChanged((bool) e.OldValue, (bool) e.NewValue);
        }

        public bool IsSelected
        {
            get => (bool) GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        protected virtual void OnIsSelectedChanged(bool oldIsSelected, bool newIsSelected)
        {
            
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            Ribbon.NotifyMouseClickedOnTabHeader(this);
            RibbonTab.IsSelected = true;
        }
    }
}
