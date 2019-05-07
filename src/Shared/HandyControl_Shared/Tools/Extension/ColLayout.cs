using System;
using System.Windows.Markup;
using HandyControl.Data;

namespace HandyControl.Tools.Extension
{
    public class ColLayout : MarkupExtension
    {
        public static readonly int ColMaxCellCount = 24;

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
    }
}