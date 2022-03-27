using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Windows.Markup;
using HandyControl.Data;
using HandyControl.Tools.Converter;

namespace HandyControl.Tools.Extension;

[TypeConverter(typeof(ColLayoutConverter))]
public class ColLayout : MarkupExtension
{
    public static readonly int ColMaxCellCount = 24;

    public static readonly int HalfColMaxCellCount = 12;

    public static readonly int XsMaxWidth = 768;

    public static readonly int SmMaxWidth = 992;

    public static readonly int MdMaxWidth = 1200;

    public static readonly int LgMaxWidth = 1920;

    public static readonly int XlMaxWidth = 2560;

    public int Xs { get; set; } = 24;

    public int Sm { get; set; } = 12;

    public int Md { get; set; } = 8;

    public int Lg { get; set; } = 6;

    public int Xl { get; set; } = 4;

    public int Xxl { get; set; } = 2;

    public ColLayout()
    {

    }

    public ColLayout(int uniformWidth)
    {
        Xs = uniformWidth;
        Sm = uniformWidth;
        Md = uniformWidth;
        Lg = uniformWidth;
        Xl = uniformWidth;
        Xxl = uniformWidth;
    }

    public ColLayout(int xs, int sm, int md, int lg, int xl, int xxl)
    {
        Xs = xs;
        Sm = sm;
        Md = md;
        Lg = lg;
        Xl = xl;
        Xxl = xxl;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return new ColLayout
        {
            Xs = Xs,
            Sm = Sm,
            Md = Md,
            Lg = Lg,
            Xl = Xl,
            Xxl = Xxl
        };
    }

    public static ColLayoutStatus GetLayoutStatus(double width)
    {
        if (width < MdMaxWidth)
        {
            if (width < SmMaxWidth)
            {
                if (width < XsMaxWidth)
                {
                    return ColLayoutStatus.Xs;
                }
                return ColLayoutStatus.Sm;
            }
            return ColLayoutStatus.Md;
        }
        if (width < XlMaxWidth)
        {
            if (width < LgMaxWidth)
            {
                return ColLayoutStatus.Lg;
            }
            return ColLayoutStatus.Xl;
        }
        return ColLayoutStatus.Xxl;
    }

    public override string ToString()
    {
        var cultureInfo = CultureInfo.CurrentCulture;
        var listSeparator = TokenizerHelper.GetNumericListSeparator(cultureInfo);

        // Initial capacity [128] is an estimate based on a sum of:
        // 72 = 6x double (twelve digits is generous for the range of values likely)
        //  4 = 4x separator characters
        var sb = new StringBuilder(128);

        sb.Append(Xs.ToString(cultureInfo));
        sb.Append(listSeparator);
        sb.Append(Sm.ToString(cultureInfo));
        sb.Append(listSeparator);
        sb.Append(Md.ToString(cultureInfo));
        sb.Append(listSeparator);
        sb.Append(Lg.ToString(cultureInfo));
        sb.Append(listSeparator);
        sb.Append(Xl.ToString(cultureInfo));
        sb.Append(listSeparator);
        sb.Append(Xxl.ToString(cultureInfo));
        return sb.ToString();
    }
}
