using System.Windows;
using HandyControl.Expression.Drawing;
#if NET35
using System;
#endif

namespace HandyControl.Tools.Extension
{
    public static class ValueExtension
    {
        public static Thickness Add(this Thickness a, Thickness b) => new Thickness(a.Left + b.Left, a.Top + b.Top, a.Right + b.Right, a.Bottom + b.Bottom);

        public static bool IsZero(this Thickness thickness) =>
            MathHelper.IsZero(thickness.Left)
            && MathHelper.IsZero(thickness.Top)
            && MathHelper.IsZero(thickness.Right)
            && MathHelper.IsZero(thickness.Bottom);

        public static bool IsUniform(this Thickness thickness) =>
            MathHelper.AreClose(thickness.Left, thickness.Top)
            && MathHelper.AreClose(thickness.Left, thickness.Right)
            && MathHelper.AreClose(thickness.Left, thickness.Bottom);

        public static bool IsNaN(this double value) => double.IsNaN(value);

#if NET35
        public static bool HasFlag(this Enum value, Enum flag)
        {
            if (flag == null)
            {
                throw new ArgumentNullException(nameof(flag));
            }

            if (value.GetType() != flag.GetType())
            {
                throw new ArgumentException("Argument_EnumTypeDoesNotMatch");
            }

            var flagNum = Convert.ToUInt64(flag);
            var valueNum = Convert.ToUInt64(value);

            return (valueNum & flagNum) == flagNum;
        }
#endif
    }
}