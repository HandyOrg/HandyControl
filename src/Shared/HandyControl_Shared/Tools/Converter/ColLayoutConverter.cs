using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Security;
using System.Text;
using HandyControl.Tools.Extension;

namespace HandyControl.Tools.Converter
{
    public class ColLayoutConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext typeDescriptorContext, Type sourceType)
        {
            switch (Type.GetTypeCode(sourceType))
            {
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                case TypeCode.String:
                    return true;
                default:
                    return false;
            }
        }

        public override bool CanConvertTo(ITypeDescriptorContext typeDescriptorContext, Type destinationType) =>
            destinationType == typeof(InstanceDescriptor) || destinationType == typeof(string);

        public override object ConvertFrom(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object source)
        {
            if (source == null) throw GetConvertFromException(null);

            return source switch
            {
                string s => FromString(s, cultureInfo),
                double d => new ColLayout((int) d),
                _ => new ColLayout(Convert.ToInt32(source, cultureInfo))
            };
        }

        [SecurityCritical]
        public override object ConvertTo(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object value, Type destinationType)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            if (destinationType == null) throw new ArgumentNullException(nameof(destinationType));

            if (!(value is ColLayout th)) throw new ArgumentException("UnexpectedParameterType");

            if (destinationType == typeof(string)) return ToString(th, cultureInfo);
            if (destinationType == typeof(InstanceDescriptor))
            {
                var ci = typeof(ColLayout).GetConstructor(new[] { typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int) });
                return new InstanceDescriptor(ci, new object[] { th.Xs, th.Sm, th.Md, th.Lg, th.Xl, th.Xxl });
            }

            throw new ArgumentException("CannotConvertType");
        }

        private static string ToString(ColLayout th, CultureInfo cultureInfo)
        {
            var listSeparator = TokenizerHelper.GetNumericListSeparator(cultureInfo);

            // Initial capacity [128] is an estimate based on a sum of:
            // 72 = 6x double (twelve digits is generous for the range of values likely)
            //  4 = 4x separator characters
            var sb = new StringBuilder(128);

            sb.Append(th.Xs.ToString(cultureInfo));
            sb.Append(listSeparator);
            sb.Append(th.Sm.ToString(cultureInfo));
            sb.Append(listSeparator);
            sb.Append(th.Md.ToString(cultureInfo));
            sb.Append(listSeparator);
            sb.Append(th.Lg.ToString(cultureInfo));
            sb.Append(listSeparator);
            sb.Append(th.Xl.ToString(cultureInfo));
            sb.Append(listSeparator);
            sb.Append(th.Xxl.ToString(cultureInfo));
            return th.ToString();
        }

        private static ColLayout FromString(string s, CultureInfo cultureInfo)
        {
            var th = new TokenizerHelper(s, cultureInfo);
            var lengths = new int[6];
            var i = 0;

            while (th.NextToken())
            {
                if (i >= 6)
                {
                    i = 7;    // Set i to a bad value. 
                    break;
                }

                lengths[i] = th.GetCurrentToken().Value<int>();
                i++;
            }

            return i switch
            {
                1 => new ColLayout(lengths[0]),
                2 => new ColLayout {Xs = lengths[0], Sm = lengths[1]},
                3 => new ColLayout {Xs = lengths[0], Sm = lengths[1], Md = lengths[2]},
                4 => new ColLayout {Xs = lengths[0], Sm = lengths[1], Md = lengths[2], Lg = lengths[3]},
                5 => new ColLayout
                {
                    Xs = lengths[0],
                    Sm = lengths[1],
                    Md = lengths[2],
                    Lg = lengths[3],
                    Xl = lengths[4]
                },
                6 => new ColLayout
                {
                    Xs = lengths[0],
                    Sm = lengths[1],
                    Md = lengths[2],
                    Lg = lengths[3],
                    Xl = lengths[4],
                    Xxl = lengths[5]
                },
                _ => throw new FormatException("InvalidStringColLayout")
            };
        }
    }
}