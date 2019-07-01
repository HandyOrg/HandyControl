using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace HandyControl.Tools.Converter
{

    public class Number2PercentageConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string result = "";
            if (values.Length == 2)
            {
                var result_double = int.Parse(values[1].ToString()) * 100 / int.Parse(values[0].ToString());
                return result_double.ToString(result_double - (int)result_double > 0 ? "f1" : "f0");
            }
            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
