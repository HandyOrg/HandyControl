using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Threading;

namespace Standard;

internal static class Verify
{
    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    [DebuggerStepThrough]
    public static void IsApartmentState(ApartmentState requiredState, string message)
    {
        if (Thread.CurrentThread.GetApartmentState() != requiredState)
        {
            throw new InvalidOperationException(message);
        }
    }

    [DebuggerStepThrough]
    [SuppressMessage("Microsoft.Performance", "CA1820:TestForEmptyStringsUsingStringLength")]
    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static void IsNeitherNullNorEmpty(string value, string name)
    {
        if (value == null)
        {
            throw new ArgumentNullException(name, "The parameter can not be either null or empty.");
        }
        if ("" == value)
        {
            throw new ArgumentException("The parameter can not be either null or empty.", name);
        }
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    [SuppressMessage("Microsoft.Performance", "CA1820:TestForEmptyStringsUsingStringLength")]
    [DebuggerStepThrough]
    public static void IsNeitherNullNorWhitespace(string value, string name)
    {
        if (value == null)
        {
            throw new ArgumentNullException(name, "The parameter can not be either null or empty or consist only of white space characters.");
        }
        if ("" == value.Trim())
        {
            throw new ArgumentException("The parameter can not be either null or empty or consist only of white space characters.", name);
        }
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    [DebuggerStepThrough]
    public static void IsNotDefault<T>(T obj, string name) where T : struct
    {
        T t = default(T);
        if (t.Equals(obj))
        {
            throw new ArgumentException("The parameter must not be the default value.", name);
        }
    }

    [DebuggerStepThrough]
    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static void IsNotNull<T>(T obj, string name) where T : class
    {
        if (obj == null)
        {
            throw new ArgumentNullException(name);
        }
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    [DebuggerStepThrough]
    public static void IsNull<T>(T obj, string name) where T : class
    {
        if (obj != null)
        {
            throw new ArgumentException("The parameter must be null.", name);
        }
    }

    [DebuggerStepThrough]
    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static void PropertyIsNotNull<T>(T obj, string name) where T : class
    {
        if (obj == null)
        {
            throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The property {0} cannot be null at this time.", new object[]
            {
                name
            }));
        }
    }

    [DebuggerStepThrough]
    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static void PropertyIsNull<T>(T obj, string name) where T : class
    {
        if (obj != null)
        {
            throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The property {0} must be null at this time.", new object[]
            {
                name
            }));
        }
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    [DebuggerStepThrough]
    public static void IsTrue(bool statement, string name)
    {
        if (!statement)
        {
            throw new ArgumentException("", name);
        }
    }

    [DebuggerStepThrough]
    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static void IsTrue(bool statement, string name, string message)
    {
        if (!statement)
        {
            throw new ArgumentException(message, name);
        }
    }

    [DebuggerStepThrough]
    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static void AreEqual<T>(T expected, T actual, string parameterName, string message)
    {
        if (expected == null)
        {
            if (actual != null && !actual.Equals(expected))
            {
                throw new ArgumentException(message, parameterName);
            }
        }
        else if (!expected.Equals(actual))
        {
            throw new ArgumentException(message, parameterName);
        }
    }

    [DebuggerStepThrough]
    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static void AreNotEqual<T>(T notExpected, T actual, string parameterName, string message)
    {
        if (notExpected == null)
        {
            if (actual == null || actual.Equals(notExpected))
            {
                throw new ArgumentException(message, parameterName);
            }
        }
        else if (notExpected.Equals(actual))
        {
            throw new ArgumentException(message, parameterName);
        }
    }

    [DebuggerStepThrough]
    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static void UriIsAbsolute(Uri uri, string parameterName)
    {
        Verify.IsNotNull<Uri>(uri, parameterName);
        if (!uri.IsAbsoluteUri)
        {
            throw new ArgumentException("The URI must be absolute.", parameterName);
        }
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    [DebuggerStepThrough]
    public static void BoundedInteger(int lowerBoundInclusive, int value, int upperBoundExclusive, string parameterName)
    {
        if (value < lowerBoundInclusive || value >= upperBoundExclusive)
        {
            throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The integer value must be bounded with [{0}, {1})", new object[]
            {
                lowerBoundInclusive,
                upperBoundExclusive
            }), parameterName);
        }
    }

    [DebuggerStepThrough]
    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static void BoundedDoubleInc(double lowerBoundInclusive, double value, double upperBoundInclusive, string message, string parameter)
    {
        if (value < lowerBoundInclusive || value > upperBoundInclusive)
        {
            throw new ArgumentException(message, parameter);
        }
    }

    [DebuggerStepThrough]
    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static void TypeSupportsInterface(Type type, Type interfaceType, string parameterName)
    {
        Verify.IsNotNull<Type>(type, "type");
        Verify.IsNotNull<Type>(interfaceType, "interfaceType");
        if (type.GetInterface(interfaceType.Name) == null)
        {
            throw new ArgumentException("The type of this parameter does not support a required interface", parameterName);
        }
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    [DebuggerStepThrough]
    public static void FileExists(string filePath, string parameterName)
    {
        Verify.IsNeitherNullNorEmpty(filePath, parameterName);
        if (!File.Exists(filePath))
        {
            throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "No file exists at \"{0}\"", new object[]
            {
                filePath
            }), parameterName);
        }
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    [DebuggerStepThrough]
    internal static void ImplementsInterface(object parameter, Type interfaceType, string parameterName)
    {
        bool flag = false;
        foreach (Type left in parameter.GetType().GetInterfaces())
        {
            if (left == interfaceType)
            {
                flag = true;
                break;
            }
        }
        if (!flag)
        {
            throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The parameter must implement interface {0}.", new object[]
            {
                interfaceType.ToString()
            }), parameterName);
        }
    }
}
