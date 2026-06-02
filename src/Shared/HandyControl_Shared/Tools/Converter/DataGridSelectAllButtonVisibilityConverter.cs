using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace HandyControl.Tools.Converter;

public class DataGridSelectAllButtonVisibilityConverter : IMultiValueConverter
{
    private const int DesiredParamsCount = 3;

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values?.Length is not DesiredParamsCount)
        {
            return Visibility.Collapsed;
        }

        if (values[0] is not DataGridHeadersVisibility.All)
        {
            return Visibility.Hidden;
        }

        return values[1] is not SelectionMode.Single && values[2] is true
            ? Visibility.Visible
            : Visibility.Hidden;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
