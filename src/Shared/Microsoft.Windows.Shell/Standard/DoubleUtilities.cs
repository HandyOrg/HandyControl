using System;
using System.Diagnostics.CodeAnalysis;

namespace Standard
{
	// Token: 0x02000010 RID: 16
	internal static class DoubleUtilities
	{
		// Token: 0x06000071 RID: 113 RVA: 0x000036D4 File Offset: 0x000018D4
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

		// Token: 0x06000072 RID: 114 RVA: 0x00003705 File Offset: 0x00001905
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static bool LessThan(double value1, double value2)
		{
			return value1 < value2 && !DoubleUtilities.AreClose(value1, value2);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003717 File Offset: 0x00001917
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static bool GreaterThan(double value1, double value2)
		{
			return value1 > value2 && !DoubleUtilities.AreClose(value1, value2);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003729 File Offset: 0x00001929
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static bool LessThanOrClose(double value1, double value2)
		{
			return value1 < value2 || DoubleUtilities.AreClose(value1, value2);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003738 File Offset: 0x00001938
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static bool GreaterThanOrClose(double value1, double value2)
		{
			return value1 > value2 || DoubleUtilities.AreClose(value1, value2);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003747 File Offset: 0x00001947
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static bool IsFinite(double value)
		{
			return !double.IsNaN(value) && !double.IsInfinity(value);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x0000375C File Offset: 0x0000195C
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static bool IsValidSize(double value)
		{
			return DoubleUtilities.IsFinite(value) && DoubleUtilities.GreaterThanOrClose(value, 0.0);
		}

		// Token: 0x0400003E RID: 62
		private const double Epsilon = 1.53E-06;
	}
}
