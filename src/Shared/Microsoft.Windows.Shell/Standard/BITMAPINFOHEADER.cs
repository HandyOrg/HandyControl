using System;
using System.Runtime.InteropServices;

namespace Standard;

[StructLayout(LayoutKind.Sequential, Pack = 2)]
internal struct BITMAPINFOHEADER
{
    public int biSize;

    public int biWidth;

    public int biHeight;

    public short biPlanes;

    public short biBitCount;

    public BI biCompression;

    public int biSizeImage;

    public int biXPelsPerMeter;

    public int biYPelsPerMeter;

    public int biClrUsed;

    public int biClrImportant;
}
