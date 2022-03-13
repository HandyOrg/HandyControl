using System;
using System.Diagnostics.CodeAnalysis;

namespace Standard;

internal static class DoubleUtilities
{
    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static bool AreClose(double value1, double value2)
    {
        if (value1 == value2)
        {
            return true;
        }
        double num = value1 - value2;
        return num < 1.53E-06 && num > -1.53E-06;
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static bool LessThan(double value1, double value2)
    {
        return value1 < value2 && !DoubleUtilities.AreClose(value1, value2);
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static bool GreaterThan(double value1, double value2)
    {
        return value1 > value2 && !DoubleUtilities.AreClose(value1, value2);
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static bool LessThanOrClose(double value1, double value2)
    {
        return value1 < value2 || DoubleUtilities.AreClose(value1, value2);
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static bool GreaterThanOrClose(double value1, double value2)
    {
        return value1 > value2 || DoubleUtilities.AreClose(value1, value2);
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static bool IsFinite(double value)
    {
        return !double.IsNaN(value) && !double.IsInfinity(value);
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static bool IsValidSize(double value)
    {
        return DoubleUtilities.IsFinite(value) && DoubleUtilities.GreaterThanOrClose(value, 0.0);
    }

    private const double Epsilon = 1.53E-06;
}
