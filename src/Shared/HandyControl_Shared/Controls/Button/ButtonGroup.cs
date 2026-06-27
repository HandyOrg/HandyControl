using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using HandyControl.Data;

namespace HandyControl.Controls;

public class ButtonGroup : ItemsControl
{
    protected override bool IsItemItsOwnContainerOverride(object item) => item is Button or RadioButton or ToggleButton;

    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
        nameof(Orientation), typeof(Orientation), typeof(ButtonGroup), new PropertyMetadata(default(Orientation)));

    public Orientation Orientation
    {
        get => (Orientation) GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    public static readonly DependencyProperty LayoutProperty = DependencyProperty.Register(
        nameof(Layout), typeof(LinearLayout), typeof(ButtonGroup), new PropertyMetadata(LinearLayout.Uniform));

    public LinearLayout Layout
    {
        get => (LinearLayout) GetValue(LayoutProperty);
        set => SetValue(LayoutProperty, value);
    }

        protected override void OnRender(DrawingContext drawingContext)
    {
        var count = Items.Count;
        for (var i = 0; i < count; i++)
        {
            if (this.GetButtonBaseByIndex(i) is ButtonBase buttonBase)
            {
                buttonBase.Style = ItemContainerStyleSelector?.SelectStyle(Items[i], this);
            }
        }
    }

}

public static class ButtonGroupExtensions
{
    public static ButtonBase GetButtonBaseByIndex(this ButtonGroup buttonGroup, int index)
    {
        return GetButtonBaseByItem(buttonGroup, buttonGroup.Items[index]);
    }

    public static ButtonBase GetButtonBaseByItem(this ButtonGroup buttonGroup, object item)
    {
        if (item is ButtonBase buttonBase)
        {
            return buttonBase;
        }
        var container = buttonGroup.ItemContainerGenerator.ContainerFromItem(item);

        if (container is ButtonBase buttonBase2)
        {
            return buttonBase2;
        }
        else if (container != null && VisualTreeHelper.GetChildrenCount(container) > 0 && VisualTreeHelper.GetChild(container, 0) is ButtonBase buttonBase3)
        {
            return buttonBase3;
        }
        return null;
    }
}
