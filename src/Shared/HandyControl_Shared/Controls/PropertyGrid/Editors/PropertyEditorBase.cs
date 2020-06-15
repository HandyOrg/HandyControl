using System.Windows;
using System.Windows.Data;

namespace HandyControl.Controls
{
    public abstract class PropertyEditorBase : FrameworkElement
    {
        public abstract FrameworkElement CreateElement(PropertyItem propertyItem);

        public virtual Binding CreateBinding(PropertyItem propertyItem) =>
            new Binding(propertyItem.PropertyName)
            {
                Source = propertyItem.Value,
                Mode = GetBindingMode(propertyItem),
                UpdateSourceTrigger = GetUpdateSourceTrigger(propertyItem)
            };

        public virtual BindingMode GetBindingMode(PropertyItem propertyItem) => propertyItem.IsReadOnly ? BindingMode.OneWay : BindingMode.TwoWay;

        public virtual UpdateSourceTrigger GetUpdateSourceTrigger(PropertyItem propertyItem) => UpdateSourceTrigger.PropertyChanged;
    }
}