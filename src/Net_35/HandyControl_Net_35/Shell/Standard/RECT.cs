namespace Standard
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct RECT
    {
        private int _left;
        private int _top;
        private int _right;
        private int _bottom;
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public void Offset(int dx, int dy)
        {
            this._left += dx;
            this._top += dy;
            this._right += dx;
            this._bottom += dy;
        }

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
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public int Width
        {
            get
            {
                return (this._right - this._left);
            }
        }
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public int Height
        {
            get
            {
                return (this._bottom - this._top);
            }
        }
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public Standard.POINT Position
        {
            get
            {
                return new Standard.POINT { x = this._left, y = this._top };
            }
        }
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public Standard.SIZE Size
        {
            get
            {
                return new Standard.SIZE { cx = this.Width, cy = this.Height };
            }
        }
        public static Standard.RECT Union(Standard.RECT rect1, Standard.RECT rect2)
        {
            return new Standard.RECT { Left = Math.Min(rect1.Left, rect2.Left), Top = Math.Min(rect1.Top, rect2.Top), Right = Math.Max(rect1.Right, rect2.Right), Bottom = Math.Max(rect1.Bottom, rect2.Bottom) };
        }

        public override bool Equals(object obj)
        {
            try
            {
                Standard.RECT rect = (Standard.RECT) obj;
                return ((((rect._bottom == this._bottom) && (rect._left == this._left)) && (rect._right == this._right)) && (rect._top == this._top));
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return (((this._left << 0x10) | Standard.Utility.LOWORD(this._right)) ^ ((this._top << 0x10) | Standard.Utility.LOWORD(this._bottom)));
        }
    }
}

