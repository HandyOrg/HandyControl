using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using HandyControl.Controls;
using HandyControl.Data;

namespace HandyControl.Tools;

public class ButtonGroupItemStyleSelector : StyleSelector
{
    private static readonly Dictionary<string, Style> StyleDict = new()
    {
        [ResourceToken.RadioGroupItemSingle] = ResourceHelper.GetResourceInternal<Style>(ResourceToken.RadioGroupItemSingle),
        [ResourceToken.RadioGroupItemHorizontalFirst] = ResourceHelper.GetResourceInternal<Style>(ResourceToken.RadioGroupItemHorizontalFirst),
        [ResourceToken.RadioGroupItemHorizontalLast] = ResourceHelper.GetResourceInternal<Style>(ResourceToken.RadioGroupItemHorizontalLast),
        [ResourceToken.RadioGroupItemVerticalFirst] = ResourceHelper.GetResourceInternal<Style>(ResourceToken.RadioGroupItemVerticalFirst),
        [ResourceToken.RadioGroupItemVerticalLast] = ResourceHelper.GetResourceInternal<Style>(ResourceToken.RadioGroupItemVerticalLast),
        [ResourceToken.RadioGroupItemDefault] = ResourceHelper.GetResourceInternal<Style>(ResourceToken.RadioGroupItemDefault),

        [ResourceToken.ButtonGroupItemSingle] = ResourceHelper.GetResourceInternal<Style>(ResourceToken.ButtonGroupItemSingle),
        [ResourceToken.ButtonGroupItemHorizontalFirst] = ResourceHelper.GetResourceInternal<Style>(ResourceToken.ButtonGroupItemHorizontalFirst),
        [ResourceToken.ButtonGroupItemHorizontalLast] = ResourceHelper.GetResourceInternal<Style>(ResourceToken.ButtonGroupItemHorizontalLast),
        [ResourceToken.ButtonGroupItemVerticalFirst] = ResourceHelper.GetResourceInternal<Style>(ResourceToken.ButtonGroupItemVerticalFirst),
        [ResourceToken.ButtonGroupItemVerticalLast] = ResourceHelper.GetResourceInternal<Style>(ResourceToken.ButtonGroupItemVerticalLast),
        [ResourceToken.ButtonGroupItemDefault] = ResourceHelper.GetResourceInternal<Style>(ResourceToken.ButtonGroupItemDefault),

        [ResourceToken.ToggleButtonGroupItemSingle] = ResourceHelper.GetResourceInternal<Style>(ResourceToken.ToggleButtonGroupItemSingle),
        [ResourceToken.ToggleButtonGroupItemHorizontalFirst] = ResourceHelper.GetResourceInternal<Style>(ResourceToken.ToggleButtonGroupItemHorizontalFirst),
        [ResourceToken.ToggleButtonGroupItemHorizontalLast] = ResourceHelper.GetResourceInternal<Style>(ResourceToken.ToggleButtonGroupItemHorizontalLast),
        [ResourceToken.ToggleButtonGroupItemVerticalFirst] = ResourceHelper.GetResourceInternal<Style>(ResourceToken.ToggleButtonGroupItemVerticalFirst),
        [ResourceToken.ToggleButtonGroupItemVerticalLast] = ResourceHelper.GetResourceInternal<Style>(ResourceToken.ToggleButtonGroupItemVerticalLast),
        [ResourceToken.ToggleButtonGroupItemDefault] = ResourceHelper.GetResourceInternal<Style>(ResourceToken.ToggleButtonGroupItemDefault)
    };

    public override Style SelectStyle(object item, DependencyObject container)
    {
        if (container is ButtonGroup buttonGroup && item is ButtonBase buttonBase)
        {
            var count = GetVisibleButtonsCount(buttonGroup);

            switch (buttonBase)
            {
                case RadioButton: return GetRadioButtonStyle(count, buttonGroup, buttonBase);
                case Button: return GetButtonStyle(count, buttonGroup, buttonBase);
                case ToggleButton: return GetToggleButtonStyle(count, buttonGroup, buttonBase);
            }
        }

        return null;
    }

    private static int GetVisibleButtonsCount(ButtonGroup buttonGroup)
    {
        return buttonGroup.Items.OfType<ButtonBase>().Count(button => button.IsVisible);
    }

    private static Style GetToggleButtonStyle(int count, ButtonGroup buttonGroup, ButtonBase button)
    {
        if (count == 1)
        {
            return StyleDict[ResourceToken.ToggleButtonGroupItemSingle];
        }

        var index = buttonGroup.Items.IndexOf(button);
        return buttonGroup.Orientation == Orientation.Horizontal
            ? index == 0
                ? StyleDict[ResourceToken.ToggleButtonGroupItemHorizontalFirst]
                : StyleDict[index == count - 1
                    ? ResourceToken.ToggleButtonGroupItemHorizontalLast
                    : ResourceToken.ToggleButtonGroupItemDefault]
            : index == 0
                ? StyleDict[ResourceToken.ToggleButtonGroupItemVerticalFirst]
                : StyleDict[index == count - 1
                    ? ResourceToken.ToggleButtonGroupItemVerticalLast
                    : ResourceToken.ToggleButtonGroupItemDefault];
    }

    private static Style GetButtonStyle(int count, ButtonGroup buttonGroup, ButtonBase button)
    {
        if (count == 1)
        {
            return StyleDict[ResourceToken.ButtonGroupItemSingle];
        }

        var index = buttonGroup.Items.IndexOf(button);
        return buttonGroup.Orientation == Orientation.Horizontal
            ? index == 0
                ? StyleDict[ResourceToken.ButtonGroupItemHorizontalFirst]
                : StyleDict[index == count - 1
                    ? ResourceToken.ButtonGroupItemHorizontalLast
                    : ResourceToken.ButtonGroupItemDefault]
            : index == 0
                ? StyleDict[ResourceToken.ButtonGroupItemVerticalFirst]
                : StyleDict[index == count - 1
                    ? ResourceToken.ButtonGroupItemVerticalLast
                    : ResourceToken.ButtonGroupItemDefault];
    }

    private static Style GetRadioButtonStyle(int count, ButtonGroup buttonGroup, ButtonBase button)
    {
        if (count == 1)
        {
            return StyleDict[ResourceToken.RadioGroupItemSingle];
        }

        var index = buttonGroup.Items.IndexOf(button);
        return buttonGroup.Orientation == Orientation.Horizontal
            ? index == 0
                ? StyleDict[ResourceToken.RadioGroupItemHorizontalFirst]
                : StyleDict[index == count - 1
                    ? ResourceToken.RadioGroupItemHorizontalLast
                    : ResourceToken.RadioGroupItemDefault]
            : index == 0
                ? StyleDict[ResourceToken.RadioGroupItemVerticalFirst]
                : StyleDict[index == count - 1
                    ? ResourceToken.RadioGroupItemVerticalLast
                    : ResourceToken.RadioGroupItemDefault];
    }
}
