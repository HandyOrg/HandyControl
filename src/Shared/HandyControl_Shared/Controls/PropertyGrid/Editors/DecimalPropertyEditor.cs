using System;
using System.Windows;

namespace HandyControl.Controls
{
    public class DecimalPropertyEditor : PropertyEditorBase
    {
        public DecimalPropertyEditor()
        {

        }

        public DecimalPropertyEditor(decimal minimum, decimal maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        public decimal Minimum { get; set; }

        public decimal Maximum { get; set; }

        public override FrameworkElement CreateElement(PropertyItem propertyItem) => new DecimalBox
        {
            IsReadOnly = propertyItem.IsReadOnly,
            Minimum = Minimum,
            Maximum = Maximum
        };

        public override DependencyProperty GetDependencyProperty() => DecimalBox.ValueProperty;
    }
}
