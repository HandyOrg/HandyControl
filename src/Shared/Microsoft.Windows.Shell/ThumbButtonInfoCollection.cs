using System;
using System.Windows;

namespace Microsoft.Windows.Shell
{
	// Token: 0x020000A3 RID: 163
	public class ThumbButtonInfoCollection : FreezableCollection<ThumbButtonInfo>
	{
		// Token: 0x0600030C RID: 780 RVA: 0x00008A09 File Offset: 0x00006C09
		protected override Freezable CreateInstanceCore()
		{
			return new ThumbButtonInfoCollection();
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600030D RID: 781 RVA: 0x00008A10 File Offset: 0x00006C10
		internal static ThumbButtonInfoCollection Empty
		{
			get
			{
				if (ThumbButtonInfoCollection.s_empty == null)
				{
					ThumbButtonInfoCollection thumbButtonInfoCollection = new ThumbButtonInfoCollection();
					thumbButtonInfoCollection.Freeze();
					ThumbButtonInfoCollection.s_empty = thumbButtonInfoCollection;
				}
				return ThumbButtonInfoCollection.s_empty;
			}
		}

		// Token: 0x040005E9 RID: 1513
		private static ThumbButtonInfoCollection s_empty;
	}
}
