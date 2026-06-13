using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using HandyControl.Data;
using HandyControl.Tools.Converter;

namespace HandyControl.Tools.Extension;

[TypeConverter(typeof(ColLayoutConverter))]
public class ColLayout
{
    public const int ColMaxCellCount = 24;

    public const int HalfColMaxCellCount = 12;

    public const int XsMaxWidth = 768;

    public const int SmMaxWidth = 992;

    public const int MdMaxWidth = 1200;

    public const int LgMaxWidth = 1920;

    public const int XlMaxWidth = 2560;

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

    public object ProvideValue(IServiceProvider serviceProvider)
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

    public static ColLayout Parse(string s)
    {
        if (string.IsNullOrWhiteSpace(s)) throw new FormatException("InvalidStringColLayout");

        var separators = new[] { ',', ' ' };
        var tokens = s.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        var lengths = new int[6];
        var i = 0;
        foreach (var token in tokens)
        {
            if (i >= 6) throw new FormatException("InvalidStringColLayout");
            lengths[i++] = int.Parse(token, CultureInfo.InvariantCulture);
        }

        return i switch
        {
            1 => new ColLayout(lengths[0]),
            2 => new ColLayout { Xs = lengths[0], Sm = lengths[1] },
            3 => new ColLayout { Xs = lengths[0], Sm = lengths[1], Md = lengths[2] },
            4 => new ColLayout { Xs = lengths[0], Sm = lengths[1], Md = lengths[2], Lg = lengths[3] },
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

    public override string ToString()
    {
        var sb = new StringBuilder(128);
        sb.Append(Xs.ToString(CultureInfo.CurrentCulture));
        sb.Append(',');
        sb.Append(Sm.ToString(CultureInfo.CurrentCulture));
        sb.Append(',');
        sb.Append(Md.ToString(CultureInfo.CurrentCulture));
        sb.Append(',');
        sb.Append(Lg.ToString(CultureInfo.CurrentCulture));
        sb.Append(',');
        sb.Append(Xl.ToString(CultureInfo.CurrentCulture));
        sb.Append(',');
        sb.Append(Xxl.ToString(CultureInfo.CurrentCulture));
        return sb.ToString();
    }
}
