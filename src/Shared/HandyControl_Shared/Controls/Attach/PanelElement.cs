using System.Windows;
using System.Windows.Controls;
using HandyControl.Interactivity;

namespace HandyControl.Controls;

public class PanelElement
{
    public static readonly DependencyProperty FluidMoveBehaviorProperty = DependencyProperty.RegisterAttached(
        "FluidMoveBehavior", typeof(FluidMoveBehavior), typeof(PanelElement), new PropertyMetadata(default(FluidMoveBehavior), OnFluidMoveBehaviorChanged));

    private static void OnFluidMoveBehaviorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Panel panel)
        {
            var collection = Interaction.GetBehaviors(panel);
            var oldBehavior = GetTempFluidMoveBehavior(panel);
            collection.ItemRemoved(oldBehavior);
            panel.SetCurrentValue(TempFluidMoveBehaviorProperty, DependencyProperty.UnsetValue);

            if (e.NewValue is FluidMoveBehavior behavior)
            {
                collection.ItemAdded(behavior);
                SetTempFluidMoveBehavior(panel, behavior);
            }
        }
    }

    public static void SetFluidMoveBehavior(DependencyObject element, FluidMoveBehavior value)
        => element.SetValue(FluidMoveBehaviorProperty, value);

    public static FluidMoveBehavior GetFluidMoveBehavior(DependencyObject element)
        => (FluidMoveBehavior) element.GetValue(FluidMoveBehaviorProperty);

    private static readonly DependencyProperty TempFluidMoveBehaviorProperty = DependencyProperty.RegisterAttached(
        "TempFluidMoveBehavior", typeof(FluidMoveBehavior), typeof(PanelElement), new PropertyMetadata(default(FluidMoveBehavior)));

    private static void SetTempFluidMoveBehavior(DependencyObject element, FluidMoveBehavior value)
        => element.SetValue(TempFluidMoveBehaviorProperty, value);

    private static FluidMoveBehavior GetTempFluidMoveBehavior(DependencyObject element)
        => (FluidMoveBehavior) element.GetValue(TempFluidMoveBehaviorProperty);
}
