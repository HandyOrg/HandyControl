using System.Windows.Controls;
#if NET35
using System.Windows;
#endif

namespace HandyControl.Controls
{
    public class CheckComboBoxItem : ListBoxItem
    {
#if NET35
        private object _isSelected;

        private bool _canCoerce;

        static CheckComboBoxItem()
        {
            IsSelectedProperty.DefaultMetadata.CoerceValueCallback = CoerceIsSelected;
        }

        private static object CoerceIsSelected(DependencyObject d, object basevalue)
            => d is CheckComboBoxItem checkComboBoxItem ? checkComboBoxItem.CoerceIsSelected(basevalue) : basevalue;

        private object CoerceIsSelected(object basevalue) => _canCoerce ? _isSelected : basevalue;

        internal void SetIsSelected(object isSelected)
        {
            _isSelected = isSelected;
            _canCoerce = true;
            CoerceValue(IsSelectedProperty);
            _canCoerce = false;
        }
#endif
    }
}
