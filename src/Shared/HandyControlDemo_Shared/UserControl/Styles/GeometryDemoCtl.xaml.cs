
using HandyControl.Data;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace HandyControlDemo.UserControl
{
    public partial class GeometryDemoCtl
    {
        public GeometryDemoCtl()
        {
            InitializeComponent();
            GenerateGeometries();
        }

        public void GenerateGeometries()
        {
            ResourceDictionary dictionary = new ResourceDictionary();
            dictionary.Source = new Uri("pack://application:,,,/HandyControl;component/Themes/Basic/Geometries.xaml", UriKind.Absolute);
            foreach (var item in dictionary.Keys)
            {
                StackPanel stack = new StackPanel { Margin = new Thickness(10) };
                TextBlock text = new TextBlock { Text = item.ToString(), HorizontalAlignment = HorizontalAlignment.Stretch };
                Path path = new Path { Width = 26, Height = 26, Data = TryFindResource(item.ToString()) as Geometry, Fill = TryFindResource(ResourceToken.PrimaryTextBrush) as Brush };
                stack.Children.Add(path);
                stack.Children.Add(text);
                panel.Children.Add(stack);
            }
        }
    }
}
