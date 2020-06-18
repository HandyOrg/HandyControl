using System.Windows;
using HandyControlDemo.Data;

namespace HandyControlDemo.UserControl
{
    public partial class PropertyGridDemoCtl
    {
        public PropertyGridDemoCtl()
        {
            InitializeComponent();

            DemoModel = new PropertyGridDemoModel
            {
                Name = "TestName",
                Gender = Gender.Female,
                IsPassed = true,
                Score = 98
            };
        }

        public static readonly DependencyProperty DemoModelProperty = DependencyProperty.Register(
            "DemoModel", typeof(PropertyGridDemoModel), typeof(PropertyGridDemoCtl), new PropertyMetadata(default(PropertyGridDemoModel)));

        public PropertyGridDemoModel DemoModel
        {
            get => (PropertyGridDemoModel) GetValue(DemoModelProperty);
            set => SetValue(DemoModelProperty, value);
        }
    }
}
