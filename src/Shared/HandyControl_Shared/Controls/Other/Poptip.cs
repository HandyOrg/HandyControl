using System.Windows;
using System.Windows.Controls;
using HandyControl.Data;

namespace HandyControl.Controls
{
    public class Poptip : Control
    {
        public static readonly DependencyProperty PlacementProperty = DependencyProperty.Register(
            "Placement", typeof(TipPlacement), typeof(Poptip), new PropertyMetadata(TipPlacement.Top));

        public TipPlacement Placement
        {
            get => (TipPlacement) GetValue(PlacementProperty);
            set => SetValue(PlacementProperty, value);
        }
    }
}
