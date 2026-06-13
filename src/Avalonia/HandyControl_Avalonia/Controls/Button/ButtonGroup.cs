using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Styling;
using HandyControl.Data;

namespace HandyControl.Controls;

public class ButtonGroup : ItemsControl
{
    public static readonly StyledProperty<Orientation> OrientationProperty =
        StackPanel.OrientationProperty.AddOwner<ButtonGroup>(
            new StyledPropertyMetadata<Orientation>(defaultValue: Orientation.Horizontal));

    public Orientation Orientation
    {
        get => GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    public static readonly StyledProperty<LinearLayout> LayoutProperty =
        AvaloniaProperty.Register<ButtonGroup, LinearLayout>(nameof(Layout), defaultValue: LinearLayout.Uniform);

    public LinearLayout Layout
    {
        get => GetValue(LayoutProperty);
        set => SetValue(LayoutProperty, value);
    }

    static ButtonGroup()
    {
        AffectsMeasure<ButtonGroup>(OrientationProperty, LayoutProperty);
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        if (item is Button or RadioButton or ToggleButton)
        {
            recycleKey = null;
            return false;
        }

        return base.NeedsContainerOverride(item, index, out recycleKey);
    }

    protected override void PrepareContainerForItemOverride(Control container, object? item, int index)
    {
        base.PrepareContainerForItemOverride(container, item, index);

        container.PropertyChanged += Container_PropertyChanged;
        UpdateItemThemes();
    }

    protected override void ClearContainerForItemOverride(Control container)
    {
        base.ClearContainerForItemOverride(container);

        container.PropertyChanged -= Container_PropertyChanged;
        UpdateItemThemes();
    }

    private void Container_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property == IsVisibleProperty)
        {
            UpdateItemThemes();
        }
    }

    private void UpdateItemThemes()
    {
        var visibleButtons = Items.OfType<Button>().Where(b => b.IsVisible).ToList();
        var count = visibleButtons.Count;
        if (count == 0)
        {
            return;
        }

        for (var i = 0; i < count; i++)
        {
            var button = visibleButtons[i];
            var theme = ResolveTheme(button, i, count);
            if (theme != null)
            {
                button.SetCurrentValue(ThemeProperty, theme);
            }
        }
    }

    private ControlTheme? ResolveTheme(Button button, int index, int count)
    {
        var key = button switch
        {
            RadioButton => GetThemeKey(index, count, "RadioGroupItem"),
            ToggleButton => GetThemeKey(index, count, "ToggleButtonGroupItem"),
            _ => GetThemeKey(index, count, "ButtonGroupItem")
        };

        if (key == null)
        {
            return null;
        }

        if (this.TryFindResource(key, out var resource) && resource is ControlTheme theme)
        {
            return theme;
        }

        return null;
    }

    private string? GetThemeKey(int index, int count, string prefix)
    {
        if (count == 1)
        {
            return prefix + "Single";
        }

        if (index == 0)
        {
            return prefix + (Orientation == Orientation.Horizontal ? "HorizontalFirst" : "VerticalFirst");
        }

        if (index == count - 1)
        {
            return prefix + (Orientation == Orientation.Horizontal ? "HorizontalLast" : "VerticalLast");
        }

        return prefix + "Default";
    }
}
