using System.Windows;
using System.Windows.Controls;
using HandyControl.Data;

namespace HandyControl.Tools
{
    public class ComboBoxItemCapsuleStyleSelector : StyleSelector
    {
        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (container is ComboBoxItem comboBoxItem && VisualHelper.GetParent<ComboBox>(comboBoxItem) is { } comboBox)
            {
                var count = comboBox.Items.Count;
                if (count == 1)
                {
                    return ResourceHelper.GetResource<Style>(ResourceToken.ComboBoxItemCapsuleSingle);
                }

                var index = comboBox.ItemContainerGenerator.IndexFromContainer(comboBoxItem);
                return index == 0
                    ? ResourceHelper.GetResource<Style>(ResourceToken.ComboBoxItemCapsuleHorizontalFirst)
                    : ResourceHelper.GetResource<Style>(index == count - 1
                        ? ResourceToken.ComboBoxItemCapsuleHorizontalLast
                        : ResourceToken.ComboBoxItemCapsuleDefault);
            }

            return null;
        }
    }
}