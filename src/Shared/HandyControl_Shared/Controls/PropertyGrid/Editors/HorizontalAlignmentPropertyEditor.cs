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
    public class HorizontalAlignmentPropertyEditor : PropertyEditorBase
    {
        public override FrameworkElement CreateElement(PropertyItem propertyItem) => new System.Windows.Controls.ComboBox
        {
            Style = ResourceHelper.GetResource<Style>("ComboBoxCapsule"),
            ItemsSource = Enum.GetValues(propertyItem.PropertyType),
            ItemTemplateSelector = ResourceHelper.GetResource<DataTemplateSelector>("HorizontalAlignmentPathTemplateSelector"),
            HorizontalAlignment = HorizontalAlignment.Left
        };

        public override DependencyProperty GetDependencyProperty() => Selector.SelectedValueProperty;
    }

    public class HorizontalAlignmentPathTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is HorizontalAlignment horizontalAlignment)
            {
                var dataTemplate = new DataTemplate
                {
                    DataType = typeof(System.Windows.Controls.ComboBox)
                };

                var factory = new FrameworkElementFactory(typeof(Path));
                factory.SetValue(FrameworkElement.WidthProperty, 12.0);
                factory.SetValue(FrameworkElement.HeightProperty, 10.0);
                factory.SetBinding(Shape.FillProperty, new Binding(Control.ForegroundProperty.Name)
                {
                    RelativeSource = new RelativeSource
                    {
                        AncestorType = typeof(ComboBoxItem)
                    }
                });

                switch (horizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        factory.SetValue(Path.DataProperty, ResourceHelper.GetResource<Geometry>("AlignLeftGeometry"));
                        break;
                    case HorizontalAlignment.Center:
                        factory.SetValue(Path.DataProperty, ResourceHelper.GetResource<Geometry>("AlignHCenterGeometry"));
                        break;
                    case HorizontalAlignment.Right:
                        factory.SetValue(Path.DataProperty, ResourceHelper.GetResource<Geometry>("AlignRightGeometry"));
                        break;
                    case HorizontalAlignment.Stretch:
                        factory.SetValue(Path.DataProperty, ResourceHelper.GetResource<Geometry>("AlignHStretchGeometry"));
                        break;
                }

                dataTemplate.VisualTree = factory;
                return dataTemplate;
            }

            return null;
        }
    }
}