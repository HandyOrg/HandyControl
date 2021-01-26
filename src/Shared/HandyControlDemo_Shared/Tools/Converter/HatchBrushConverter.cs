using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using HandyControl.Data;
using HandyControl.Tools;

namespace HandyControlDemo.Tools.Converter
{
    public class HatchBrushConverter : IValueConverter
    {
        private readonly HatchBrushGenerator _brushGenerator;

        public HatchBrushConverter()
        {
            _brushGenerator = new HatchBrushGenerator();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is HatchStyle style)
            {
                return _brushGenerator.GetHatchBrush(style, ResourceHelper.GetResource<Color>(ResourceToken.DarkPrimaryColor), ResourceHelper.GetResource<Color>(ResourceToken.BackgroundColor));
            }
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
