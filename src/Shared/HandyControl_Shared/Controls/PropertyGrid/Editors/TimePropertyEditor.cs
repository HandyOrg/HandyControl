using System.Windows;

namespace HandyControl.Controls
{
    public class TimePropertyEditor : PropertyEditorBase
    {
        public override FrameworkElement CreateElement(PropertyItem propertyItem)
        {
            var picker = new DateTimePicker
            {
                IsEnabled = !propertyItem.IsReadOnly
            };

            picker.SetBinding(TimePicker.SelectedTimeProperty, CreateBinding(propertyItem));

            return picker;
        }
    }
}