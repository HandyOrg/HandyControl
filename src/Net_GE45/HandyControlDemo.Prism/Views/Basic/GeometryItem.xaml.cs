using HandyControlDemo.Data;
using System.Windows;

namespace HandyControlDemo.Views
{
    public partial class GeometryItem
    {
        public GeometryItem() => InitializeComponent();

        public static readonly DependencyProperty InfoProperty = DependencyProperty.Register(
            "Info", typeof(GeometryItemModel), typeof(GeometryItem), new PropertyMetadata(default(GeometryItemModel)));

        public GeometryItemModel Info
        {
            get => (GeometryItemModel)GetValue(InfoProperty);
            set => SetValue(InfoProperty, value);
        }
    }
}