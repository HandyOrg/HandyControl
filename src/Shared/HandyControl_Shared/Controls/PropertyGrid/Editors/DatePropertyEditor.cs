using System.Windows;

namespace HandyControl.Controls
{
    public class DatePropertyEditor : PropertyEditorBase
    {
        public override FrameworkElement CreateElement(PropertyItem propertyItem)
        {
            var picker = new DateTimePicker
            {
                IsEnabled = !propertyItem.IsReadOnly
            };

            picker.SetBinding(System.Windows.Controls.DatePicker.SelectedDateProperty, CreateBinding(propertyItem));

            return picker;
        }
    }
}