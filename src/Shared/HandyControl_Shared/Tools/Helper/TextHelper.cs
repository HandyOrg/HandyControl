using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace HandyControl.Tools.Helper;

internal class TextHelper
{
    public static FormattedText CreateFormattedText(string text, FlowDirection flowDirection, Typeface typeface, double fontSize)
    {
#if NET40 || NET45 || NET451 || NET452 || NET46 || NET461
        var formattedText = new FormattedText(
            text,
            CultureInfo.CurrentUICulture,
            flowDirection,
            typeface,
            fontSize, Brushes.Black);
#else
        var formattedText = new FormattedText(
            text,
            CultureInfo.CurrentUICulture,
            flowDirection,
            typeface,
            fontSize, Brushes.Black, DpiHelper.DeviceDpiX);
#endif

        return formattedText;
    }
}
