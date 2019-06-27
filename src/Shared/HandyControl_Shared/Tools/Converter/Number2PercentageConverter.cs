using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace HandyControl.Tools.Converter
{
    public class Number2PercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = "";
            if (value is ProgressBar element)
            {
                double result_double = element.Value * 100 / element.Maximum;
                int result_int = (int)result_double;
                if (result_double - result_int > 0) result = (element.Value * 100 / element.Maximum).ToString("f1");
                else result = result_int.ToString();
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
