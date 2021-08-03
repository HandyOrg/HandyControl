using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace HandyControl.Tools.Converter
{
    public class DataGridSelectAllButtonVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null)
            {
                return Visibility.Collapsed;
            }

            if (values.Length != 2)
            {
                return Visibility.Collapsed;
            }

            if (values[0] is DataGridHeadersVisibility headersVisibility)
            {
                if (values[1] is bool showButton)
                {
                    if (headersVisibility == DataGridHeadersVisibility.All)
                    {
                        return showButton ? Visibility.Visible : Visibility.Collapsed;
                    }

                    return Visibility.Collapsed;
                }

                return Visibility.Collapsed;
            }
            else if (values[0] is bool showButton)
            {
                if (values[1] is DataGridHeadersVisibility visibility)
                {
                    if (visibility == DataGridHeadersVisibility.All)
                    {
                        return showButton ? Visibility.Visible : Visibility.Collapsed;
                    }

                    return Visibility.Collapsed;
                }

                return Visibility.Collapsed;
            }

            return Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
