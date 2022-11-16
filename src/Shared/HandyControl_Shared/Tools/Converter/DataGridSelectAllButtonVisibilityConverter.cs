using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace HandyControl.Tools.Converter;

public class DataGridSelectAllButtonVisibilityConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values?.Length is 2 &&
            values[0] is DataGridHeadersVisibility.All &&
            values[1] is bool showButton)
        {
            return showButton ? Visibility.Visible : Visibility.Hidden;
        }

        return Visibility.Collapsed;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
