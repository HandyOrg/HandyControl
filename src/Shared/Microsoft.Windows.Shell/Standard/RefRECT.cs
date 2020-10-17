using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000066 RID: 102
	[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
	[StructLayout(LayoutKind.Sequential)]
	internal class RefRECT
	{
		// Token: 0x060000DB RID: 219 RVA: 0x000049A9 File Offset: 0x00002BA9
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public RefRECT(int left, int top, int right, int bottom)
		{
			this._left = left;
			this._top = top;
			this._right = right;
			this._bottom = bottom;
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000DC RID: 220 RVA: 0x000049CE File Offset: 0x00002BCE
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public int Width
		{
			get
			{
				return this._right - this._left;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000DD RID: 221 RVA: 0x000049DD File Offset: 0x00002BDD
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public int Height
		{
			get
			{
				return this._bottom - this._top;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000DE RID: 222 RVA: 0x000049EC File Offset: 0x00002BEC
		// (set) Token: 0x060000DF RID: 223 RVA: 0x000049F4 File Offset: 0x00002BF4
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public int Left
		{
			get
			{
				return this._left;
			}
			set
			{
				this._left = value;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x000049FD File Offset: 0x00002BFD
		// (set) Token: 0x060000E1 RID: 225 RVA: 0x00004A05 File Offset: 0x00002C05
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public int Right
		{
			get
			{
				return this._right;
			}
			set
			{
				this._right = value;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x00004A0E File Offset: 0x00002C0E
		// (set) Token: 0x060000E3 RID: 227 RVA: 0x00004A16 File Offset: 0x00002C16
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public int Top
		{
			get
			{
				return this._top;
			}
			set
			{
				this._top = value;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x00004A1F File Offset: 0x00002C1F
		// (set) Token: 0x060000E5 RID: 229 RVA: 0x00004A27 File Offset: 0x00002C27
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public int Bottom
		{
			get
			{
				return this._bottom;
			}
			set
			{
				this._bottom = value;
			}
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00004A30 File Offset: 0x00002C30
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public void Offset(int dx, int dy)
		{
			this._left += dx;
			this._top += dy;
			this._right += dx;
			this._bottom += dy;
		}

		// Token: 0x040004AE RID: 1198
		private int _left;

		// Token: 0x040004AF RID: 1199
		private int _top;

		// Token: 0x040004B0 RID: 1200
		private int _right;

		// Token: 0x040004B1 RID: 1201
		private int _bottom;
	}
}
