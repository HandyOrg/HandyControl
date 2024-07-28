using System;

namespace HandyControl.Tools;

internal static class MathHelper
{
    public const double Epsilon = 2.2204460492503131e-016;

    public static bool IsZero(double value) => Math.Abs(value) < 10.0 * Epsilon;

    public static bool AreClose(double value1, double value2) =>
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        value1 == value2 || IsVerySmall(value1 - value2);

    public static bool IsVerySmall(double value) => Math.Abs(value) < 1E-06;

    public static bool GreaterThan(double value1, double value2) => value1 > value2 && !AreClose(value1, value2);
}
