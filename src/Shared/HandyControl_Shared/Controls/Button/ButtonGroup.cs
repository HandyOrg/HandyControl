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
            var item = (ButtonBase) Items[i];
            item.Style = ItemContainerStyleSelector?.SelectStyle(item, this);
        }
    }
}
