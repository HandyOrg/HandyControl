using System;
using System.Diagnostics.CodeAnalysis;

namespace Standard
{
	// Token: 0x02000065 RID: 101
	internal struct RECT
	{
		// Token: 0x060000CB RID: 203 RVA: 0x00004793 File Offset: 0x00002993
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public void Offset(int dx, int dy)
		{
			this._left += dx;
			this._top += dy;
			this._right += dx;
			this._bottom += dy;
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000CC RID: 204 RVA: 0x000047CD File Offset: 0x000029CD
		// (set) Token: 0x060000CD RID: 205 RVA: 0x000047D5 File Offset: 0x000029D5
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

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000CE RID: 206 RVA: 0x000047DE File Offset: 0x000029DE
		// (set) Token: 0x060000CF RID: 207 RVA: 0x000047E6 File Offset: 0x000029E6
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

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x000047EF File Offset: 0x000029EF
		// (set) Token: 0x060000D1 RID: 209 RVA: 0x000047F7 File Offset: 0x000029F7
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

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x00004800 File Offset: 0x00002A00
		// (set) Token: 0x060000D3 RID: 211 RVA: 0x00004808 File Offset: 0x00002A08
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

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00004811 File Offset: 0x00002A11
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public int Width
		{
			get
			{
				return this._right - this._left;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x00004820 File Offset: 0x00002A20
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public int Height
		{
			get
			{
				return this._bottom - this._top;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00004830 File Offset: 0x00002A30
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public POINT Position
		{
			get
			{
				return new POINT
				{
					x = this._left,
					y = this._top
				};
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x00004860 File Offset: 0x00002A60
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public SIZE Size
		{
			get
			{
				return new SIZE
				{
					cx = this.Width,
					cy = this.Height
				};
			}
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00004890 File Offset: 0x00002A90
		public static RECT Union(RECT rect1, RECT rect2)
		{
			return new RECT
			{
				Left = Math.Min(rect1.Left, rect2.Left),
				Top = Math.Min(rect1.Top, rect2.Top),
				Right = Math.Max(rect1.Right, rect2.Right),
				Bottom = Math.Max(rect1.Bottom, rect2.Bottom)
			};
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00004910 File Offset: 0x00002B10
		public override bool Equals(object obj)
		{
			bool result;
			try
			{
				RECT rect = (RECT)obj;
				result = (rect._bottom == this._bottom && rect._left == this._left && rect._right == this._right && rect._top == this._top);
			}
			catch (InvalidCastException)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x0000497C File Offset: 0x00002B7C
		public override int GetHashCode()
		{
			return (this._left << 16 | Utility.LOWORD(this._right)) ^ (this._top << 16 | Utility.LOWORD(this._bottom));
		}

		// Token: 0x040004AA RID: 1194
		private int _left;

		// Token: 0x040004AB RID: 1195
		private int _top;

		// Token: 0x040004AC RID: 1196
		private int _right;

		// Token: 0x040004AD RID: 1197
		private int _bottom;
	}
}
