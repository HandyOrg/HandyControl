using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using HandyControl.Controls;

namespace HandyControlDemo.Data
{
    public class PropertyGridDemoModel
    {
        [Category("Category1")]
        public string String { get; set; }

        [Category("Category2")]
        public int Integer { get; set; }

        [Category("Category2")]
        public bool Boolean { get; set; }

        [Category("Category1")]
        public Gender Enum { get; set; }

        public HorizontalAlignment HorizontalAlignment { get; set; }

        public VerticalAlignment VerticalAlignment { get; set; }

        public ImageSource ImageSource { get; set; }

        //[Editor(typeof(ColorPickerEditor), typeof(PropertyEditorBase))]
        //public SolidColorBrush Color { get; set; }

        //public SolidColorBrush Brush { get; set; }
    }

    public class ColorPickerEditor : PropertyEditorBase
    {
        public override FrameworkElement CreateElement(PropertyItem propertyItem)
        {
            return new ColorPicker()
            {
                Style = HandyControl.Tools.ResourceHelper.GetResource<Style>("ColorPickerBaseStyle"),
                HorizontalAlignment = HorizontalAlignment.Left,
                IsEnabled = !propertyItem.IsReadOnly
            };
        }

        public override DependencyProperty GetDependencyProperty()
        {
            return ColorPicker.SelectedBrushProperty;
        }
    }

    public enum Gender
    {
        [Description("男性")]
        Male,
        [Description("女性")]
        Female
    }
}
