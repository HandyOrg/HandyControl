namespace Standard
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows;

    internal static class Assert
    {
        private static void _Break()
        {
            Debugger.Break();
        }

        [Conditional("DEBUG")]
        public static void AreEqual<T>(T expected, T actual)
        {
            if (expected == null)
            {
                if ((actual != null) && !actual.Equals(expected))
                {
                    _Break();
                }
            }
            else if (!expected.Equals(actual))
            {
                _Break();
            }
        }

        [Conditional("DEBUG")]
        public static void AreNotEqual<T>(T notExpected, T actual)
        {
            if (notExpected == null)
            {
                if ((actual == null) || actual.Equals(notExpected))
                {
                    _Break();
                }
            }
            else if (notExpected.Equals(actual))
            {
                _Break();
            }
        }

        [Conditional("DEBUG")]
        public static void BoundedDoubleInc(double lowerBoundInclusive, double value, double upperBoundInclusive)
        {
            if ((value < lowerBoundInclusive) || (value > upperBoundInclusive))
            {
                _Break();
            }
        }

        [Conditional("DEBUG")]
        public static void BoundedInteger(int lowerBoundInclusive, int value, int upperBoundExclusive)
        {
            if ((value < lowerBoundInclusive) || (value >= upperBoundExclusive))
            {
                _Break();
            }
        }

        [Obsolete("Use Assert.AreEqual instead of Assert.Equals", false), Conditional("DEBUG")]
        public static void Equals<T>(T expected, T actual)
        {
        }

        [Conditional("DEBUG")]
        public static void Evaluate(EvaluateFunction argument)
        {
            argument();
        }

        [Conditional("DEBUG")]
        public static void Fail()
        {
            _Break();
        }

        [Conditional("DEBUG")]
        public static void Fail(string message)
        {
            _Break();
        }

        [Conditional("DEBUG")]
        public static void Implies(bool condition, ImplicationFunction result)
        {
            if (condition && !result())
            {
                _Break();
            }
        }

        [Conditional("DEBUG")]
        public static void Implies(bool condition, bool result)
        {
            if (condition && !result)
            {
                _Break();
            }
        }

        [Conditional("DEBUG")]
        public static void IsApartmentState(ApartmentState expectedState)
        {
            if (Thread.CurrentThread.GetApartmentState() != expectedState)
            {
                _Break();
            }
        }

        [Conditional("DEBUG")]
        public static void IsDefault<T>(T value) where T: struct
        {
            value.Equals(default(T));
        }

        [Conditional("DEBUG")]
        public static void IsFalse(bool condition)
        {
            if (condition)
            {
                _Break();
            }
        }

        [Conditional("DEBUG")]
        public static void IsFalse(bool condition, string message)
        {
            if (condition)
            {
                _Break();
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
                _Break();
            }
            if (value.Trim().Length == 0)
            {
                _Break();
            }
        }

        [Conditional("DEBUG")]
        public static void IsNotDefault<T>(T value) where T: struct
        {
            value.Equals(default(T));
        }

        [Conditional("DEBUG")]
        public static void IsNotNull<T>(T value) where T: class
        {
            if (value == null)
            {
                _Break();
            }
        }

        [Conditional("DEBUG")]
        public static void IsNotOnMainThread()
        {
            if (Application.Current.Dispatcher.CheckAccess())
            {
                _Break();
            }
        }

        [Conditional("DEBUG")]
        public static void IsNull<T>(T item) where T: class
        {
            if (item != null)
            {
                _Break();
            }
        }

        [Conditional("DEBUG")]
        public static void IsTrue(bool condition)
        {
            if (!condition)
            {
                _Break();
            }
        }

        [Conditional("DEBUG")]
        public static void IsTrue(bool condition, string message)
        {
            if (!condition)
            {
                _Break();
            }
        }

        [Conditional("DEBUG")]
        public static void NullableIsNotNull<T>(T? value) where T: struct
        {
            if (!value.HasValue)
            {
                _Break();
            }
        }

        [Conditional("DEBUG")]
        public static void NullableIsNull<T>(T? value) where T: struct
        {
            if (value.HasValue)
            {
                _Break();
            }
        }

        public delegate void EvaluateFunction();

        public delegate bool ImplicationFunction();
    }
}

