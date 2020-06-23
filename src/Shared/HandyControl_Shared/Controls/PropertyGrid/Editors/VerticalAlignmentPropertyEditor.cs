using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using HandyControl.Tools;

namespace HandyControl.Controls
{
    public class VerticalAlignmentPropertyEditor : PropertyEditorBase
    {
        public override FrameworkElement CreateElement(PropertyItem propertyItem) => new System.Windows.Controls.ComboBox
        {
            Style = ResourceHelper.GetResource<Style>("ComboBoxCapsule"),
            ItemsSource = Enum.GetValues(propertyItem.PropertyType),
            ItemTemplateSelector = ResourceHelper.GetResource<DataTemplateSelector>("VerticalAlignmentPathTemplateSelector"),
            HorizontalAlignment = HorizontalAlignment.Left
        };

        public override DependencyProperty GetDependencyProperty() => Selector.SelectedValueProperty;
    }

    public class VerticalAlignmentPathTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is VerticalAlignment verticalAlignment)
            {
                var dataTemplate = new DataTemplate
                {
                    DataType = typeof(System.Windows.Controls.ComboBox)
                };

                var factory = new FrameworkElementFactory(typeof(Path));
                factory.SetValue(FrameworkElement.WidthProperty, 10.0);
                factory.SetValue(FrameworkElement.HeightProperty, 12.0);
                factory.SetBinding(Shape.FillProperty, new Binding(Control.ForegroundProperty.Name)
                {
                    RelativeSource = new RelativeSource
                    {
                        AncestorType = typeof(ComboBoxItem)
                    }
                });

                switch (verticalAlignment)
                {
                    case VerticalAlignment.Top:
                        factory.SetValue(Path.DataProperty, ResourceHelper.GetResource<Geometry>("AlignTopGeometry"));
                        break;
                    case VerticalAlignment.Center:
                        factory.SetValue(Path.DataProperty, ResourceHelper.GetResource<Geometry>("AlignVCenterGeometry"));
                        break;
                    case VerticalAlignment.Bottom:
                        factory.SetValue(Path.DataProperty, ResourceHelper.GetResource<Geometry>("AlignBottomGeometry"));
                        break;
                    case VerticalAlignment.Stretch:
                        factory.SetValue(Path.DataProperty, ResourceHelper.GetResource<Geometry>("AlignVStretchGeometry"));
                        break;
                }

                dataTemplate.VisualTree = factory;
                return dataTemplate;
            }

            return null;
        }
    }
}