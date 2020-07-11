using System.Windows;

namespace HandyControl.Controls
{
    public class DateTimePropertyEditor : PropertyEditorBase
    {
        public override FrameworkElement CreateElement(PropertyItem propertyItem) => new DateTimePicker
        {
            IsEnabled = !propertyItem.IsReadOnly
        };

        public override DependencyProperty GetDependencyProperty() => DateTimePicker.SelectedDateTimeProperty;
    }
}