using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using HandyControl.Controls;
using HandyControl.Data;

namespace HandyControl.Tools
{
    public class ButtonGroupItemStyleSelector : StyleSelector
    {
        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (container is ButtonGroup buttonGroup && item is ButtonBase buttonBase)
            {
                var count = buttonGroup.Items.Count;

                switch (buttonBase)
                {
                    case RadioButton _:
                        {
                            if (count == 1)
                            {
                                return ResourceHelper.GetResourceInternal<Style>(ResourceToken.RadioGroupItemSingle);
                            }

                            var index = buttonGroup.Items.IndexOf(buttonBase);
                            return buttonGroup.Orientation == Orientation.Horizontal
                                ? index == 0
                                    ? ResourceHelper.GetResourceInternal<Style>(ResourceToken.RadioGroupItemHorizontalFirst)
                                    : ResourceHelper.GetResourceInternal<Style>(index == count - 1
                                        ? ResourceToken.RadioGroupItemHorizontalLast
                                        : ResourceToken.RadioGroupItemDefault)
                                : index == 0
                                    ? ResourceHelper.GetResourceInternal<Style>(ResourceToken.RadioGroupItemVerticalFirst)
                                    : ResourceHelper.GetResourceInternal<Style>(index == count - 1
                                        ? ResourceToken.RadioGroupItemVerticalLast
                                        : ResourceToken.RadioGroupItemDefault);
                        }

                    case Button _:
                        {
                            if (count == 1)
                            {
                                return ResourceHelper.GetResourceInternal<Style>(ResourceToken.ButtonGroupItemSingle);
                            }

                            var index = buttonGroup.Items.IndexOf(buttonBase);
                            return buttonGroup.Orientation == Orientation.Horizontal
                                ? index == 0
                                    ? ResourceHelper.GetResourceInternal<Style>(ResourceToken.ButtonGroupItemHorizontalFirst)
                                    : ResourceHelper.GetResourceInternal<Style>(index == count - 1
                                        ? ResourceToken.ButtonGroupItemHorizontalLast
                                        : ResourceToken.ButtonGroupItemDefault)
                                : index == 0
                                    ? ResourceHelper.GetResourceInternal<Style>(ResourceToken.ButtonGroupItemVerticalFirst)
                                    : ResourceHelper.GetResourceInternal<Style>(index == count - 1
                                        ? ResourceToken.ButtonGroupItemVerticalLast
                                        : ResourceToken.ButtonGroupItemDefault);
                        }

                    case ToggleButton _:
                        {
                            if (count == 1)
                            {
                                return ResourceHelper.GetResourceInternal<Style>(ResourceToken.ToggleButtonGroupItemSingle);
                            }

                            var index = buttonGroup.Items.IndexOf(buttonBase);
                            return buttonGroup.Orientation == Orientation.Horizontal
                                ? index == 0
                                    ? ResourceHelper.GetResourceInternal<Style>(ResourceToken.ToggleButtonGroupItemHorizontalFirst)
                                    : ResourceHelper.GetResourceInternal<Style>(index == count - 1
                                        ? ResourceToken.ToggleButtonGroupItemHorizontalLast
                                        : ResourceToken.ToggleButtonGroupItemDefault)
                                : index == 0
                                    ? ResourceHelper.GetResourceInternal<Style>(ResourceToken.ToggleButtonGroupItemVerticalFirst)
                                    : ResourceHelper.GetResourceInternal<Style>(index == count - 1
                                        ? ResourceToken.ToggleButtonGroupItemVerticalLast
                                        : ResourceToken.ToggleButtonGroupItemDefault);
                        }
                }
            }

            return null;
        }
    }
}
