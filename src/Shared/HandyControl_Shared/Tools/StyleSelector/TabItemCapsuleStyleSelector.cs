using System.Windows;
using System.Windows.Controls;
using HandyControl.Data;

namespace HandyControl.Tools;

public class TabItemCapsuleStyleSelector : StyleSelector
{
    public override Style SelectStyle(object item, DependencyObject container)
    {
        if (container is not TabItem tabItem || VisualHelper.GetParent<TabControl>(tabItem) is not { } tabControl)
        {
            return null;
        }

        var count = tabControl.Items.Count;
        if (count == 1)
        {
            return ResourceHelper.GetResourceInternal<Style>(ResourceToken.TabItemCapsuleSingle);
        }

        var index = tabControl.ItemContainerGenerator.IndexFromContainer(tabItem);
        return index == 0
            ? ResourceHelper.GetResourceInternal<Style>(
                tabControl.TabStripPlacement is Dock.Top or Dock.Bottom
                    ? ResourceToken.TabItemCapsuleHorizontalFirst
                    : ResourceToken.TabItemCapsuleVerticalFirst)
            : ResourceHelper.GetResourceInternal<Style>(index == count - 1
                ? tabControl.TabStripPlacement is Dock.Top or Dock.Bottom
                    ? ResourceToken.TabItemCapsuleHorizontalLast
                    : ResourceToken.TabItemCapsuleVerticalLast
                : ResourceToken.TabItemCapsuleDefault);

    }
}
