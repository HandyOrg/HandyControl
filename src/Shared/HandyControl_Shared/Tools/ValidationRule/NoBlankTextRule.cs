using System.Globalization;
using System.Windows.Controls;

namespace HandyControl.Tools;

public class NoBlankTextRule : ValidationRule
{
    public string ErrorContent { get; set; } = Properties.Langs.Lang.IsNecessary;

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (value is not string text)
        {
            return new ValidationResult(false, Properties.Langs.Lang.FormatError);
        }

        if (string.IsNullOrEmpty(text))
        {
            return new ValidationResult(false, ErrorContent);
        }

        return ValidationResult.ValidResult;
    }
}
