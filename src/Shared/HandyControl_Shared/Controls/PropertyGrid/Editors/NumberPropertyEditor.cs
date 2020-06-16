using System.Windows;

namespace HandyControl.Controls
{
    public class NumberPropertyEditor : PropertyEditorBase
    {
        public NumberPropertyEditor()
        {
            
        }

        public NumberPropertyEditor(double minimum, double maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        public double Minimum { get; set; }

        public double Maximum { get; set; }

        public override FrameworkElement CreateElement(PropertyItem propertyItem) => new NumericUpDown
        {
            IsReadOnly = propertyItem.IsReadOnly,
            Minimum = Minimum,
            Maximum = Maximum
        };

        public override DependencyProperty GetDependencyProperty() => NumericUpDown.ValueProperty;
    }
}