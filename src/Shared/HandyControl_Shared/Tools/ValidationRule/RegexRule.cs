using System.Globalization;
using System.Windows.Controls;
using HandyControl.Data;

namespace HandyControl.Tools;

public class RegexRule : ValidationRule
{
    public TextType Type { get; set; }

    public string Pattern { get; set; }

    public string ErrorContent { get; set; } = Properties.Langs.Lang.FormatError;

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (value is not string text)
        {
            return CreateErrorValidationResult();
        }

        if (!string.IsNullOrEmpty(Pattern))
        {
            if (!text.IsKindOf(Pattern))
            {
                return CreateErrorValidationResult();
            }
        }
        else if (Type != TextType.Common)
        {
            if (!text.IsKindOf(Type))
            {
                return CreateErrorValidationResult();
            }
        }

        return ValidationResult.ValidResult;
    }

    private ValidationResult CreateErrorValidationResult()
    {
        return new ValidationResult(false, ErrorContent);
    }
}
