using System.Windows;
using HandyControl.Data;

namespace HandyControl.Controls
{
    public class TipElement
    {
        public static readonly DependencyProperty VisibilityProperty = DependencyProperty.RegisterAttached(
            "Visibility", typeof(Visibility), typeof(TipElement), new PropertyMetadata(Visibility.Collapsed));

        public static void SetVisibility(DependencyObject element, Visibility value) => element.SetValue(VisibilityProperty, value);

        public static Visibility GetVisibility(DependencyObject element) => (Visibility) element.GetValue(VisibilityProperty);

        public static readonly DependencyProperty PlacementProperty = DependencyProperty.RegisterAttached(
            "Placement", typeof(TipPlacement), typeof(TipElement), new PropertyMetadata(default(TipPlacement)));

        public static void SetPlacement(DependencyObject element, TipPlacement value) => element.SetValue(PlacementProperty, value);

        public static TipPlacement GetPlacement(DependencyObject element) => (TipPlacement) element.GetValue(PlacementProperty);
    }
}