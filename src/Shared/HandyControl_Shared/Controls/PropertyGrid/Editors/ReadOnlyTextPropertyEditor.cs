using System.Windows;
using System.Windows.Data;
using HandyControl.Tools;

namespace HandyControl.Controls
{
    public class ReadOnlyTextPropertyEditor : PropertyEditorBase
    {
        public override FrameworkElement CreateElement(PropertyItem propertyItem) => new System.Windows.Controls.TextBox
        {
            IsReadOnly = true
        };

        public override DependencyProperty GetDependencyProperty() => System.Windows.Controls.TextBox.TextProperty;

        public override BindingMode GetBindingMode(PropertyItem propertyItem) => BindingMode.OneWay;

        protected override IValueConverter GetConverter(PropertyItem propertyItem) => ResourceHelper.GetResource<IValueConverter>("Object2StringConverter");
    }
}