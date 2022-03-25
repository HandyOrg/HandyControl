using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Standard;

[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
[StructLayout(LayoutKind.Sequential)]
internal class RefRECT
{
    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public RefRECT(int left, int top, int right, int bottom)
    {
        this._left = left;
        this._top = top;
        this._right = right;
        this._bottom = bottom;
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public int Width
    {
        get
        {
            return this._right - this._left;
        }
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public int Height
    {
        get
        {
            return this._bottom - this._top;
        }
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
    public void Offset(int dx, int dy)
    {
        this._left += dx;
        this._top += dy;
        this._right += dx;
        this._bottom += dy;
    }

    private int _left;

    private int _top;

    private int _right;

    private int _bottom;
}
