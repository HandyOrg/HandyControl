using System.Windows;
using System.Windows.Controls.Primitives;
using HandyControl.Tools;

namespace HandyControl.Controls
{
    public class BooleanPropertyEditor : PropertyEditorBase
    {
        public override FrameworkElement CreateElement(PropertyItem propertyItem)
        {
            var button = new ToggleButton
            {
                Style = ResourceHelper.GetResource<Style>("ToggleButtonSwitch"),
                HorizontalAlignment = HorizontalAlignment.Left,
                IsEnabled = !propertyItem.IsReadOnly
            };

            button.SetBinding(ToggleButton.IsCheckedProperty, CreateBinding(propertyItem));

            return button;
        }
    }
}