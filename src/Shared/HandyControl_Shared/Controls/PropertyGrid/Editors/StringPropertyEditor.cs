using System.Windows;

namespace HandyControl.Controls
{
    public class StringPropertyEditor : PropertyEditorBase
    {
        public override FrameworkElement CreateElement(PropertyItem propertyItem)
        {
            var textbox = new System.Windows.Controls.TextBox
            {
                IsReadOnly = propertyItem.IsReadOnly
            };

            textbox.SetBinding(System.Windows.Controls.TextBox.TextProperty, CreateBinding(propertyItem));

            return textbox;
        }
    }
}