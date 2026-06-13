using System;
using System.ComponentModel;
using System.Globalization;
using HandyControl.Tools.Extension;

namespace HandyControl.Tools.Converter;

public class ColLayoutConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return Type.GetTypeCode(sourceType) switch
        {
            TypeCode.Int16 or TypeCode.UInt16
                or TypeCode.Int32 or TypeCode.UInt32
                or TypeCode.Int64 or TypeCode.UInt64
                or TypeCode.Single or TypeCode.Double or TypeCode.Decimal
                or TypeCode.String => true,
            _ => false
        };
    }

    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        => destinationType == typeof(string);

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value == null) throw GetConvertFromException(null!);

        return value switch
        {
            string s => ColLayout.Parse(s),
            double d => new ColLayout((int) d),
            _ => new ColLayout(System.Convert.ToInt32(value, culture))
        };
    }

    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if (value is ColLayout layout && destinationType == typeof(string))
        {
            return layout.ToString();
        }
        throw new ArgumentException("CannotConvertType");
    }
}
