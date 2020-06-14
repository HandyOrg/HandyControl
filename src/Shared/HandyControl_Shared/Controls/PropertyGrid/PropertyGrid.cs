using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using HandyControl.Tools.Extension;

namespace HandyControl.Controls
{
    [TemplatePart(Name = ElementItemsControl, Type = typeof(ItemsControl))]
    public class PropertyGrid : Control
    {
        private const string ElementItemsControl = "PART_ItemsControl";

        private ItemsControl _itemsControl;

        public virtual PropertyResolver PropertyResolver { get; } = new PropertyResolver();

        public static readonly RoutedEvent SelectedObjectChangedEvent =
            EventManager.RegisterRoutedEvent("SelectedObjectChanged", RoutingStrategy.Bubble,
                typeof(RoutedPropertyChangedEventHandler<object>), typeof(PropertyGrid));

        public event RoutedPropertyChangedEventHandler<object> SelectedObjectChanged
        {
            add => AddHandler(SelectedObjectChangedEvent, value);
            remove => RemoveHandler(SelectedObjectChangedEvent, value);
        }

        public static readonly DependencyProperty SelectedObjectProperty = DependencyProperty.Register(
            "SelectedObject", typeof(object), typeof(PropertyGrid), new PropertyMetadata(default, OnSelectedObjectChanged));

        private static void OnSelectedObjectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (PropertyGrid)d;
            ctl.OnSelectedObjectChanged(e.OldValue, e.NewValue);
        }

        public object SelectedObject
        {
            get => GetValue(SelectedObjectProperty);
            set => SetValue(SelectedObjectProperty, value);
        }

        protected virtual void OnSelectedObjectChanged(object oldValue, object newValue)
        {
            UpdateItems(newValue);
            RaiseEvent(new RoutedPropertyChangedEventArgs<object>(oldValue, newValue, SelectedObjectChangedEvent));
        }

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
            "Description", typeof(string), typeof(PropertyGrid), new PropertyMetadata(default(string)));

        public string Description
        {
            get => (string)GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _itemsControl = GetTemplateChild(ElementItemsControl) as ItemsControl;
            UpdateItems(SelectedObject);
        }

        private void UpdateItems(object obj)
        {
            if (obj == null || _itemsControl == null) return;

            var type = obj.GetType();
            var propertyInfos = type.GetProperties();
            var itemList = propertyInfos.Select(CreatePropertyItem).Do(item => item.InitElement()).ToList();
            _itemsControl.ItemsSource = itemList;

            var categorySet = new HashSet<string>();
            foreach (var propertyItem in itemList)
            {
                categorySet.Add(propertyItem.Category);
            }

            var view = CollectionViewSource.GetDefaultView(itemList);
            view.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
        }

        protected virtual PropertyItem CreatePropertyItem(PropertyInfo propertyInfo) =>
            new PropertyItem
            {
                Category = PropertyResolver.ResolveCategory(propertyInfo),
                DisplayName = PropertyResolver.ResolveDisplayName(propertyInfo),
                Description = PropertyResolver.ResolveDescription(propertyInfo),
                Browsable = PropertyResolver.ResolveBrowsable(propertyInfo),
                IsReadOnly = PropertyResolver.ResolveIsReadOnly(propertyInfo),
                DefaultValue = PropertyResolver.ResolveDefaultValue(propertyInfo),
                Editor = PropertyResolver.ResolveEditor(propertyInfo),
                Converter = PropertyResolver.ResolveConverter(propertyInfo),
                Value = SelectedObject,
                PropertyName = propertyInfo.Name,
                PropertyTypeName = propertyInfo.PropertyType.FullName
            };
    }
}