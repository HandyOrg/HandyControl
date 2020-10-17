using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace Standard
{
	// Token: 0x0200000D RID: 13
	internal static class Assert
	{
		// Token: 0x06000050 RID: 80 RVA: 0x000034C7 File Offset: 0x000016C7
		private static void _Break()
		{
			Debugger.Break();
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000034CE File Offset: 0x000016CE
		[Conditional("DEBUG")]
		public static void Evaluate(Assert.EvaluateFunction argument)
		{
			argument();
		}

		// Token: 0x06000052 RID: 82 RVA: 0x000034D6 File Offset: 0x000016D6
		[Obsolete("Use Assert.AreEqual instead of Assert.Equals", false)]
		[Conditional("DEBUG")]
		public static void Equals<T>(T expected, T actual)
		{
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000034D8 File Offset: 0x000016D8
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

		// Token: 0x06000054 RID: 84 RVA: 0x0000352C File Offset: 0x0000172C
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

		// Token: 0x06000055 RID: 85 RVA: 0x0000357E File Offset: 0x0000177E
		[Conditional("DEBUG")]
		public static void Implies(bool condition, bool result)
		{
			if (condition && !result)
			{
				Assert._Break();
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x0000358B File Offset: 0x0000178B
		[Conditional("DEBUG")]
		public static void Implies(bool condition, Assert.ImplicationFunction result)
		{
			if (condition && !result())
			{
				Assert._Break();
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x0000359D File Offset: 0x0000179D
		[Conditional("DEBUG")]
		public static void IsNeitherNullNorEmpty(string value)
		{
		}

		// Token: 0x06000058 RID: 88 RVA: 0x0000359F File Offset: 0x0000179F
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

		// Token: 0x06000059 RID: 89 RVA: 0x000035C0 File Offset: 0x000017C0
		[Conditional("DEBUG")]
		public static void IsNotNull<T>(T value) where T : class
		{
			if (value == null)
			{
				Assert._Break();
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x000035D0 File Offset: 0x000017D0
		[Conditional("DEBUG")]
		public static void IsDefault<T>(T value) where T : struct
		{
			value.Equals(default(T));
		}

		// Token: 0x0600005B RID: 91 RVA: 0x000035FC File Offset: 0x000017FC
		[Conditional("DEBUG")]
		public static void IsNotDefault<T>(T value) where T : struct
		{
			value.Equals(default(T));
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00003625 File Offset: 0x00001825
		[Conditional("DEBUG")]
		public static void IsFalse(bool condition)
		{
			if (condition)
			{
				Assert._Break();
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x0000362F File Offset: 0x0000182F
		[Conditional("DEBUG")]
		public static void IsFalse(bool condition, string message)
		{
			if (condition)
			{
				Assert._Break();
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00003639 File Offset: 0x00001839
		[Conditional("DEBUG")]
		public static void IsTrue(bool condition)
		{
			if (!condition)
			{
				Assert._Break();
			}
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00003643 File Offset: 0x00001843
		[Conditional("DEBUG")]
		public static void IsTrue(bool condition, string message)
		{
			if (!condition)
			{
				Assert._Break();
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x0000364D File Offset: 0x0000184D
		[Conditional("DEBUG")]
		public static void Fail()
		{
			Assert._Break();
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00003654 File Offset: 0x00001854
		[Conditional("DEBUG")]
		public static void Fail(string message)
		{
			Assert._Break();
		}

		// Token: 0x06000062 RID: 98 RVA: 0x0000365B File Offset: 0x0000185B
		[Conditional("DEBUG")]
		public static void IsNull<T>(T item) where T : class
		{
			if (item != null)
			{
				Assert._Break();
			}
		}

		// Token: 0x06000063 RID: 99 RVA: 0x0000366A File Offset: 0x0000186A
		[Conditional("DEBUG")]
		public static void BoundedDoubleInc(double lowerBoundInclusive, double value, double upperBoundInclusive)
		{
			if (value < lowerBoundInclusive || value > upperBoundInclusive)
			{
				Assert._Break();
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00003679 File Offset: 0x00001879
		[Conditional("DEBUG")]
		public static void BoundedInteger(int lowerBoundInclusive, int value, int upperBoundExclusive)
		{
			if (value < lowerBoundInclusive || value >= upperBoundExclusive)
			{
				Assert._Break();
			}
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00003688 File Offset: 0x00001888
		[Conditional("DEBUG")]
		public static void IsApartmentState(ApartmentState expectedState)
		{
			if (Thread.CurrentThread.GetApartmentState() != expectedState)
			{
				Assert._Break();
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x0000369C File Offset: 0x0000189C
		[Conditional("DEBUG")]
		public static void NullableIsNotNull<T>(T? value) where T : struct
		{
			if (value == null)
			{
				Assert._Break();
			}
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000036AC File Offset: 0x000018AC
		[Conditional("DEBUG")]
		public static void NullableIsNull<T>(T? value) where T : struct
		{
			if (value != null)
			{
				Assert._Break();
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000036BC File Offset: 0x000018BC
		[Conditional("DEBUG")]
		public static void IsNotOnMainThread()
		{
			if (Application.Current.Dispatcher.CheckAccess())
			{
				Assert._Break();
			}
		}

		// Token: 0x0200000E RID: 14
		// (Invoke) Token: 0x0600006A RID: 106
		public delegate void EvaluateFunction();

		// Token: 0x0200000F RID: 15
		// (Invoke) Token: 0x0600006E RID: 110
		public delegate bool ImplicationFunction();
	}
}
