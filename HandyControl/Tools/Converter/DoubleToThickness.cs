using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HandyControl.Tools.Converter
{
    public class DoubleToThickness : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (parameter != null)
                {
                    switch (parameter.ToString())
                    {
                        case "Left":
                            return new Thickness(System.Convert.ToDouble(value), 0, 0, 0);
                        case "Top":
                            return new Thickness(0, System.Convert.ToDouble(value), 0, 0);
                        case "Right":
                            return new Thickness(0, 0, System.Convert.ToDouble(value), 0);
                        case "Buttom":
                            return new Thickness(0, 0, 0, System.Convert.ToDouble(value));
                        case "LeftTop":
                            return new Thickness(System.Convert.ToDouble(value), System.Convert.ToDouble(value), 0, 0);
                        case "LeftButtom":
                            return new Thickness(System.Convert.ToDouble(value), 0, 0, System.Convert.ToDouble(value));
                        case "RightTop":
                            return new Thickness(0, System.Convert.ToDouble(value), System.Convert.ToDouble(value), 0);
                        case "RigthButtom":
                            return new Thickness(0, 0, System.Convert.ToDouble(value), System.Convert.ToDouble(value));
                        case "LeftRight":
                            return new Thickness(System.Convert.ToDouble(value), 0, System.Convert.ToDouble(value), 0);
                        case "TopButtom":
                            return new Thickness(0, System.Convert.ToDouble(value), 0, System.Convert.ToDouble(value));
                        default:
                            return new Thickness(System.Convert.ToDouble(value));
                    }
                }
                return new Thickness(System.Convert.ToDouble(value));
            }
            return new Thickness(0);
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (parameter != null)
                {
                    switch (parameter.ToString())
                    {
                        case "Left":
                            return ((Thickness)value).Left;
                        case "Top":
                            return ((Thickness)value).Top;
                        case "Right":
                            return ((Thickness)value).Right;
                        case "Buttom":
                            return ((Thickness)value).Bottom;
                        default:
                            return ((Thickness)value).Left;
                    }
                }
                return ((Thickness)value).Left;
            }
            return 0.0;
        }
    }
}
