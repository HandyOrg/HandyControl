using System;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace HandyControl.Controls
{
    public class EnumPropertyEditor : PropertyEditorBase
    {
        public override FrameworkElement CreateElement(PropertyItem propertyItem)
        {
            var picker = new System.Windows.Controls.ComboBox
            {
                IsEnabled = !propertyItem.IsReadOnly,
                ItemsSource = Enum.GetValues(propertyItem.PropertyType)
            };

            picker.SetBinding(Selector.SelectedValueProperty, CreateBinding(propertyItem));

            return picker;
        }
    }
}