using Avalonia;
using Avalonia.Controls;
using Avalonia.Styling;
using HandyControl.Data;

namespace HandyControl.Tools;

public class TabItemCapsuleThemeSelector : ThemeSelector
{
    public override ControlTheme? SelectTheme(object? item, AvaloniaObject? container)
    {
        if (container is not TabItem tabItem || VisualHelper.GetParent<TabControl>(tabItem) is not { } tabControl)
        {
            return null;
        }

        int count = tabControl.Items.Count;
        if (count == 1)
        {
            return ResourceHelper.GetResource<ControlTheme>(ResourceToken.TabItemCapsuleSingle);
        }

        int index = tabControl.IndexFromContainer(tabItem);
        return index == 0
            ? ResourceHelper.GetResource<ControlTheme>(
                tabControl.TabStripPlacement is Dock.Top or Dock.Bottom
                    ? ResourceToken.TabItemCapsuleHorizontalFirst
                    : ResourceToken.TabItemCapsuleVerticalFirst)
            : ResourceHelper.GetResource<ControlTheme>(index == count - 1
                ? tabControl.TabStripPlacement is Dock.Top or Dock.Bottom
                    ? ResourceToken.TabItemCapsuleHorizontalLast
                    : ResourceToken.TabItemCapsuleVerticalLast
                : ResourceToken.TabItemCapsuleDefault);
    }
}
