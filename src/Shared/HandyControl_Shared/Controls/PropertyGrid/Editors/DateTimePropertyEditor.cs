using System.Windows;

namespace HandyControl.Controls
{
    public class DateTimePropertyEditor : PropertyEditorBase
    {
        public override FrameworkElement CreateElement(PropertyItem propertyItem)
        {
            var picker = new DateTimePicker
            {
                IsEnabled = !propertyItem.IsReadOnly
            };

            picker.SetBinding(DateTimePicker.SelectedDateTimeProperty, CreateBinding(propertyItem));

            return picker;
        }
    }
}