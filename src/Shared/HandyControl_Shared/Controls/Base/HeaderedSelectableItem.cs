using System.Windows;

namespace HandyControl.Controls
{
    public class HeaderedSelectableItem : SelectableItem
    {
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            "Header", typeof(object), typeof(HeaderedSelectableItem), new PropertyMetadata(default(object)));

        public object Header
        {
            get => GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(
            "HeaderTemplate", typeof(DataTemplate), typeof(HeaderedSelectableItem), new PropertyMetadata(default(DataTemplate)));

        public DataTemplate HeaderTemplate
        {
            get => (DataTemplate) GetValue(HeaderTemplateProperty);
            set => SetValue(HeaderTemplateProperty, value);
        }
    }
}