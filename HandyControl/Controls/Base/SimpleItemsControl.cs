using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace HandyControl.Controls
{
    /// <summary>
    ///     ItemsControl的轻量级版本
    /// </summary>
    [DefaultProperty("Items")]
    [ContentProperty("Items")]
    [TemplatePart(Name = ElementPanel, Type = typeof(Panel))]
    public class SimpleItemsControl : Control
    {
        private const string ElementPanel = "PART_Panel";

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(
            "ItemTemplate", typeof(DataTemplate), typeof(SimpleItemsControl),
            new FrameworkPropertyMetadata(default(DataTemplate), OnItemTemplateChanged));

        public static readonly DependencyProperty ItemContainerStyleProperty = DependencyProperty.Register(
            "ItemContainerStyle", typeof(Style), typeof(SimpleItemsControl),
            new PropertyMetadata(default(Style), OnItemContainerStyleChanged));

        public SimpleItemsControl()
        {
            var items = new ObservableCollection<object>();
            items.CollectionChanged += OnItemsCollectionChanged;
            Items = items;
        }

        [Bindable(true)]
        [Category("Content")]
        public Style ItemContainerStyle
        {
            get => (Style) GetValue(ItemContainerStyleProperty);
            set => SetValue(ItemContainerStyleProperty, value);
        }

        [Bindable(true)]
        public DataTemplate ItemTemplate
        {
            get => (DataTemplate) GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Bindable(true)]
        public Collection<object> Items { get; }

        internal Panel ItemsHost { get; set; }

        public override void OnApplyTemplate()
        {
            ItemsHost?.Children.Clear();

            base.OnApplyTemplate();

            ItemsHost = GetTemplateChild(ElementPanel) as Panel;
            Refresh();
        }

        private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Refresh();
            UpdateItems();
        }

        protected virtual DependencyObject GetContainerForItemOverride() => new ContentPresenter();

        protected virtual bool IsItemItsOwnContainerOverride(object item) => item is ContentPresenter;

        protected virtual void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            switch (element)
            {
                case ContentControl contentControl:
                    contentControl.Content = item;
                    contentControl.ContentTemplate = ItemTemplate;
                    break;
                case ContentPresenter contentPresenter:
                    contentPresenter.Content = item;
                    contentPresenter.ContentTemplate = ItemTemplate;
                    break;
            }
        }

        private static void OnItemTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = d as SimpleItemsControl;
            target?.Refresh();
        }

        private static void OnItemContainerStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = d as SimpleItemsControl;
            target?.Refresh();
        }

        protected virtual void Refresh()
        {
            if (ItemsHost == null) return;

            ItemsHost.Children.Clear();
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

                if (container is FrameworkElement element)
                {
                    element.Style = ItemContainerStyle;
                    ItemsHost.Children.Add(element);
                }
            }
        }

        protected virtual void UpdateItems()
        {

        }
    }
}