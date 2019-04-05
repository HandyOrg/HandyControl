using System.Windows;
using System.Windows.Controls;
using HandyControl.Data;

namespace HandyControl.Tools
{
    public class TabItemCapsuleStyleSelector : StyleSelector
    {
        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is TabItem tabItem && tabItem.Parent is TabControl tabControl)
            {
                var count = tabControl.Items.Count;
                if (count == 1)
                {
                    return ResourceHelper.GetResource<Style>(ResourceToken.TabItemCapsuleSingle);
                }

                var index = tabControl.Items.IndexOf(tabItem);
                return index == 0
                    ? ResourceHelper.GetResource<Style>(ResourceToken.TabItemCapsuleHorizontalFirst)
                    : ResourceHelper.GetResource<Style>(index == count - 1
                        ? ResourceToken.TabItemCapsuleHorizontalLast
                        : ResourceToken.TabItemCapsuleDefault);
            }

            return null;
        }
    }
}