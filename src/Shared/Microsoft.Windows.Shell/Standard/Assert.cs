using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace Standard;

internal static class Assert
{
    private static void _Break()
    {
        Debugger.Break();
    }

    [Conditional("DEBUG")]
    public static void Evaluate(Assert.EvaluateFunction argument)
    {
        argument();
    }

    [Obsolete("Use Assert.AreEqual instead of Assert.Equals", false)]
    [Conditional("DEBUG")]
    public static void Equals<T>(T expected, T actual)
    {
    }

    [Conditional("DEBUG")]
    public static void AreEqual<T>(T expected, T actual)
    {
        if (expected == null)
        {
            if (actual != null && !actual.Equals(expected))
            {
                Assert._Break();
                return;
            }
        }
        else if (!expected.Equals(actual))
        {
            Assert._Break();
        }
    }

    [Conditional("DEBUG")]
    public static void AreNotEqual<T>(T notExpected, T actual)
    {
        if (notExpected == null)
        {
            if (actual == null || actual.Equals(notExpected))
            {
                Assert._Break();
                return;
            }
        }
        else if (notExpected.Equals(actual))
        {
            Assert._Break();
        }
    }

    [Conditional("DEBUG")]
    public static void Implies(bool condition, bool result)
    {
        if (condition && !result)
        {
            Assert._Break();
        }
    }

    [Conditional("DEBUG")]
    public static void Implies(bool condition, Assert.ImplicationFunction result)
    {
        if (condition && !result())
        {
            Assert._Break();
        }
    }

    [Conditional("DEBUG")]
    public static void IsNeitherNullNorEmpty(string value)
    {
    }

    [Conditional("DEBUG")]
    public static void IsNeitherNullNorWhitespace(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            Assert._Break();
        }
        if (value.Trim().Length == 0)
        {
            Assert._Break();
        }
    }

    [Conditional("DEBUG")]
    public static void IsNotNull<T>(T value) where T : class
    {
        if (value == null)
        {
            Assert._Break();
        }
    }

    [Conditional("DEBUG")]
    public static void IsDefault<T>(T value) where T : struct
    {
        value.Equals(default(T));
    }

    [Conditional("DEBUG")]
    public static void IsNotDefault<T>(T value) where T : struct
    {
        value.Equals(default(T));
    }

    [Conditional("DEBUG")]
    public static void IsFalse(bool condition)
    {
        if (condition)
        {
            Assert._Break();
        }
    }

    [Conditional("DEBUG")]
    public static void IsFalse(bool condition, string message)
    {
        if (condition)
        {
            Assert._Break();
        }
    }

    [Conditional("DEBUG")]
    public static void IsTrue(bool condition)
    {
        if (!condition)
        {
            Assert._Break();
        }
    }

    [Conditional("DEBUG")]
    public static void IsTrue(bool condition, string message)
    {
        if (!condition)
        {
            Assert._Break();
        }
    }

    [Conditional("DEBUG")]
    public static void Fail()
    {
        Assert._Break();
    }

    [Conditional("DEBUG")]
    public static void Fail(string message)
    {
        Assert._Break();
    }

    [Conditional("DEBUG")]
    public static void IsNull<T>(T item) where T : class
    {
        if (item != null)
        {
            Assert._Break();
        }
    }

    [Conditional("DEBUG")]
    public static void BoundedDoubleInc(double lowerBoundInclusive, double value, double upperBoundInclusive)
    {
        if (value < lowerBoundInclusive || value > upperBoundInclusive)
        {
            Assert._Break();
        }
    }

    [Conditional("DEBUG")]
    public static void BoundedInteger(int lowerBoundInclusive, int value, int upperBoundExclusive)
    {
        if (value < lowerBoundInclusive || value >= upperBoundExclusive)
        {
            Assert._Break();
        }
    }

    [Conditional("DEBUG")]
    public static void IsApartmentState(ApartmentState expectedState)
    {
        if (Thread.CurrentThread.GetApartmentState() != expectedState)
        {
            Assert._Break();
        }
    }

    [Conditional("DEBUG")]
    public static void NullableIsNotNull<T>(T? value) where T : struct
    {
        if (value == null)
        {
            Assert._Break();
        }
    }

    [Conditional("DEBUG")]
    public static void NullableIsNull<T>(T? value) where T : struct
    {
        if (value != null)
        {
            Assert._Break();
        }
    }

    [Conditional("DEBUG")]
    public static void IsNotOnMainThread()
    {
        if (Application.Current.Dispatcher.CheckAccess())
        {
            Assert._Break();
        }
    }

    public delegate void EvaluateFunction();

    public delegate bool ImplicationFunction();
}
