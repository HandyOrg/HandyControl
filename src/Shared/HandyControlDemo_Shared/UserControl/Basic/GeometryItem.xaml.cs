using System.Windows;
using HandyControlDemo.Data;

namespace HandyControlDemo.UserControl
{
    public partial class GeometryItem
    {
        public GeometryItem() => InitializeComponent();

        public static readonly DependencyProperty InfoProperty = DependencyProperty.Register(
            "Info", typeof(GeometryItemModel), typeof(GeometryItem), new PropertyMetadata(default(GeometryItemModel)));

        public GeometryItemModel Info
        {
            get => (GeometryItemModel) GetValue(InfoProperty);
            set => SetValue(InfoProperty, value);
        }
    }
}