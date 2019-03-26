using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security;

namespace HandyControl.Tools.Interop
{
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal struct BITMAPINFO
    {
        public int bmiHeader_biSize;

        public int bmiHeader_biWidth;

        public int bmiHeader_biHeight;

        public short bmiHeader_biPlanes;

        public short bmiHeader_biBitCount;

        public int bmiHeader_biCompression;

        public int bmiHeader_biSizeImage;

        public int bmiHeader_biXPelsPerMeter;

        public int bmiHeader_biYPelsPerMeter;

        public int bmiHeader_biClrUsed;

        public int bmiHeader_biClrImportant;

        public BITMAPINFO(int width, int height, short bpp)
        {
            bmiHeader_biSize = SizeOf();
            bmiHeader_biWidth = width;
            bmiHeader_biHeight = height;
            bmiHeader_biPlanes = 1;
            bmiHeader_biBitCount = bpp;
            bmiHeader_biCompression = 0;
            bmiHeader_biSizeImage = 0;
            bmiHeader_biXPelsPerMeter = 0;
            bmiHeader_biYPelsPerMeter = 0;
            bmiHeader_biClrUsed = 0;
            bmiHeader_biClrImportant = 0;
        }

        [SecuritySafeCritical]
        private static int SizeOf()
        {
            return Marshal.SizeOf(typeof(BITMAPINFO));
        }
    }
}