using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Threading;

namespace Standard
{
	// Token: 0x0200009C RID: 156
	internal static class Verify
	{
		// Token: 0x06000255 RID: 597 RVA: 0x00006503 File Offset: 0x00004703
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DebuggerStepThrough]
		public static void IsApartmentState(ApartmentState requiredState, string message)
		{
			if (Thread.CurrentThread.GetApartmentState() != requiredState)
			{
				throw new InvalidOperationException(message);
			}
		}

		// Token: 0x06000256 RID: 598 RVA: 0x00006519 File Offset: 0x00004719
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

		// Token: 0x06000257 RID: 599 RVA: 0x00006543 File Offset: 0x00004743
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

		// Token: 0x06000258 RID: 600 RVA: 0x00006574 File Offset: 0x00004774
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

		// Token: 0x06000259 RID: 601 RVA: 0x000065AC File Offset: 0x000047AC
		[DebuggerStepThrough]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static void IsNotNull<T>(T obj, string name) where T : class
		{
			if (obj == null)
			{
				throw new ArgumentNullException(name);
			}
		}

		// Token: 0x0600025A RID: 602 RVA: 0x000065BD File Offset: 0x000047BD
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DebuggerStepThrough]
		public static void IsNull<T>(T obj, string name) where T : class
		{
			if (obj != null)
			{
				throw new ArgumentException("The parameter must be null.", name);
			}
		}

		// Token: 0x0600025B RID: 603 RVA: 0x000065D4 File Offset: 0x000047D4
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

		// Token: 0x0600025C RID: 604 RVA: 0x0000660C File Offset: 0x0000480C
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

		// Token: 0x0600025D RID: 605 RVA: 0x00006642 File Offset: 0x00004842
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DebuggerStepThrough]
		public static void IsTrue(bool statement, string name)
		{
			if (!statement)
			{
				throw new ArgumentException("", name);
			}
		}

		// Token: 0x0600025E RID: 606 RVA: 0x00006653 File Offset: 0x00004853
		[DebuggerStepThrough]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static void IsTrue(bool statement, string name, string message)
		{
			if (!statement)
			{
				throw new ArgumentException(message, name);
			}
		}

		// Token: 0x0600025F RID: 607 RVA: 0x00006660 File Offset: 0x00004860
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

		// Token: 0x06000260 RID: 608 RVA: 0x000066B8 File Offset: 0x000048B8
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

		// Token: 0x06000261 RID: 609 RVA: 0x0000670F File Offset: 0x0000490F
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

		// Token: 0x06000262 RID: 610 RVA: 0x0000672C File Offset: 0x0000492C
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

		// Token: 0x06000263 RID: 611 RVA: 0x00006771 File Offset: 0x00004971
		[DebuggerStepThrough]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static void BoundedDoubleInc(double lowerBoundInclusive, double value, double upperBoundInclusive, string message, string parameter)
		{
			if (value < lowerBoundInclusive || value > upperBoundInclusive)
			{
				throw new ArgumentException(message, parameter);
			}
		}

		// Token: 0x06000264 RID: 612 RVA: 0x00006784 File Offset: 0x00004984
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

		// Token: 0x06000265 RID: 613 RVA: 0x000067BC File Offset: 0x000049BC
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

		// Token: 0x06000266 RID: 614 RVA: 0x000067FC File Offset: 0x000049FC
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
}
