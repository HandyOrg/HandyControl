using System.Windows;
using HandyControlDemo.Data;

namespace HandyControlDemo.UserControl;

public partial class PropertyGridDemo
{
    public PropertyGridDemo()
    {
        InitializeComponent();

        DemoModel = new PropertyGridDemoModel
        {
            String = "TestString",
            Enum = Gender.Female,
            Boolean = true,
            Integer = 98,
            VerticalAlignment = VerticalAlignment.Stretch
        };
    }

    public static readonly DependencyProperty DemoModelProperty = DependencyProperty.Register(
        nameof(DemoModel), typeof(PropertyGridDemoModel), typeof(PropertyGridDemo), new PropertyMetadata(default(PropertyGridDemoModel)));

    public PropertyGridDemoModel DemoModel
    {
        get => (PropertyGridDemoModel) GetValue(DemoModelProperty);
        set => SetValue(DemoModelProperty, value);
    }
}
