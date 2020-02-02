using System;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using HandyControl.Tools.Interop;

namespace HandyControl.Data
{
    internal class GlowBitmap : DisposableObject
    {
        internal const int GlowBitmapPartCount = 16;

        private const int BytesPerPixelBgra32 = 4;

        private static readonly CachedBitmapInfo[] _transparencyMasks = new CachedBitmapInfo[GlowBitmapPartCount];

        private readonly InteropValues.BITMAPINFO _bitmapInfo;

        private readonly IntPtr _pbits;

        internal GlowBitmap(IntPtr hdcScreen, int width, int height)
        {
            _bitmapInfo.biSize = Marshal.SizeOf(typeof(InteropValues.BITMAPINFOHEADER));
            _bitmapInfo.biPlanes = 1;
            _bitmapInfo.biBitCount = 32;
            _bitmapInfo.biCompression = 0;
            _bitmapInfo.biXPelsPerMeter = 0;
            _bitmapInfo.biYPelsPerMeter = 0;
            _bitmapInfo.biWidth = width;
            _bitmapInfo.biHeight = -height;

            Handle = InteropMethods.CreateDIBSection(
                hdcScreen,
                ref _bitmapInfo,
                0u,
                out _pbits,
                IntPtr.Zero,
                0u);
        }

        internal IntPtr Handle { get; }

        internal IntPtr DIBits => _pbits;

        internal int Width => _bitmapInfo.biWidth;

        internal int Height => -_bitmapInfo.biHeight;

        protected override void DisposeNativeResources() => InteropMethods.DeleteObject(Handle);

        private static byte PremultiplyAlpha(byte channel, byte alpha) => (byte)(channel * alpha / 255.0);

        internal static GlowBitmap Create(GlowDrawingContext drawingContext, GlowBitmapPart bitmapPart, Color color)
        {
            var orCreateAlphaMask =
                GetOrCreateAlphaMask(bitmapPart);

            var glowBitmap =
                new GlowBitmap(
                    drawingContext.ScreenDC,
                    orCreateAlphaMask.Width,
                    orCreateAlphaMask.Height);

            for (var i = 0; i < orCreateAlphaMask.DIBits.Length; i += BytesPerPixelBgra32)
            {
                var b = orCreateAlphaMask.DIBits[i + 3];
                var val = PremultiplyAlpha(color.R, b);
                var val2 = PremultiplyAlpha(color.G, b);
                var val3 = PremultiplyAlpha(color.B, b);
                Marshal.WriteByte(glowBitmap.DIBits, i, val3);
                Marshal.WriteByte(glowBitmap.DIBits, i + 1, val2);
                Marshal.WriteByte(glowBitmap.DIBits, i + 2, val);
                Marshal.WriteByte(glowBitmap.DIBits, i + 3, b);
            }

            return glowBitmap;
        }

        private static CachedBitmapInfo GetOrCreateAlphaMask(GlowBitmapPart bitmapPart)
        {
            if (_transparencyMasks[(int)bitmapPart] == null)
            {
                var bitmapImage = new BitmapImage(new Uri($"pack://application:,,,/HandyControl;Component/Resources/Images/GlowWindow/{bitmapPart}.png"));

                var array = new byte[BytesPerPixelBgra32 * bitmapImage.PixelWidth * bitmapImage.PixelHeight];
                var stride = BytesPerPixelBgra32 * bitmapImage.PixelWidth;
                bitmapImage.CopyPixels(array, stride, 0);
                bitmapImage.Freeze();

                _transparencyMasks[(int)bitmapPart] =
                    new CachedBitmapInfo(
                        array,
                        bitmapImage.PixelWidth,
                        bitmapImage.PixelHeight);
            }

            return _transparencyMasks[(int)bitmapPart];
        }

        private sealed class CachedBitmapInfo
        {
            internal readonly byte[] DIBits;
            internal readonly int Height;
            internal readonly int Width;

            internal CachedBitmapInfo(byte[] diBits, int width, int height)
            {
                Width = width;
                Height = height;
                DIBits = diBits;
            }
        }
    }
}
