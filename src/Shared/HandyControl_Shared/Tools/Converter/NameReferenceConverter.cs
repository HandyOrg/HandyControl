using System;
using System.ComponentModel;
using System.Globalization;
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable HeuristicUnreachableCode

namespace HandyControl.Tools.Converter
{
    public class NameReferenceConverter : System.Windows.Markup.NameReferenceConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) 
            => context == null ? null : base.ConvertFrom(context, culture, value);

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            => context == null ? null : base.ConvertTo(context, culture, value, destinationType);
    }
}