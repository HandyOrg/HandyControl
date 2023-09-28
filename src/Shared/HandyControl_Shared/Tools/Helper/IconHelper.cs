using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using HandyControl.Tools.Interop;

namespace HandyControl.Tools;

[SuppressMessage("ReSharper", "ArrangeRedundantParentheses")]
[SuppressMessage("ReSharper", "IntVariableOverflowInUncheckedContext")]
internal static class IconHelper
{
    private static Size SmallIconSize;

    private static Size IconSize;

    private static int SystemBitDepth;

    [SecurityCritical, SecuritySafeCritical]
    public static void GetIconHandlesFromImageSource(ImageSource image, out IconHandle largeIconHandle, out IconHandle smallIconHandle)
    {
        EnsureSystemMetrics();
        largeIconHandle = CreateIconHandleFromImageSource(image, IconSize);
        smallIconHandle = CreateIconHandleFromImageSource(image, SmallIconSize);
    }

    [SecurityCritical]
    public static IconHandle CreateIconHandleFromImageSource(ImageSource image, Size size)
    {
        EnsureSystemMetrics();

        var asGoodAsItGets = false;

        var bf = image as BitmapFrame;
        if (bf?.Decoder?.Frames != null)
        {
            bf = GetBestMatch(bf.Decoder.Frames, size);

            asGoodAsItGets = bf.Decoder is IconBitmapDecoder || bf.PixelWidth == (int) size.Width && bf.PixelHeight == (int) size.Height;

            image = bf;
        }

        if (!asGoodAsItGets)
        {
            bf = BitmapFrame.Create(GenerateBitmapSource(image, size));
        }

        return CreateIconHandleFromBitmapFrame(bf);
    }

    [SecurityCritical]
    private static IconHandle CreateIconHandleFromBitmapFrame(BitmapFrame sourceBitmapFrame)
    {
        BitmapSource bitmapSource = sourceBitmapFrame;

        if (bitmapSource.Format != PixelFormats.Bgra32 && bitmapSource.Format != PixelFormats.Pbgra32)
        {
            bitmapSource = new FormatConvertedBitmap(bitmapSource, PixelFormats.Bgra32, null, 0.0);
        }

        var w = bitmapSource.PixelWidth;
        var h = bitmapSource.PixelHeight;
        var bpp = bitmapSource.Format.BitsPerPixel;
        var stride = (bpp * w + 31) / 32 * 4;
        var sizeCopyPixels = stride * h;
        var xor = new byte[sizeCopyPixels];
        bitmapSource.CopyPixels(xor, stride, 0);

        return CreateIconCursor(xor, w, h, 0, 0, true);
    }

    [SecurityCritical]
    internal static IconHandle CreateIconCursor(byte[] colorArray, int width, int height, int xHotspot, int yHotspot, bool isIcon)
    {
        BitmapHandle colorBitmap = null;
        BitmapHandle maskBitmap = null;

        try
        {
            var bi = new InteropValues.BITMAPINFO(width, -height, 32)
            {
                biCompression = InteropValues.BI_RGB
            };

            var bits = IntPtr.Zero;
            colorBitmap = InteropMethods.CreateDIBSection(new HandleRef(null, IntPtr.Zero), ref bi, InteropValues.DIB_RGB_COLORS, ref bits, null, 0);

            if (colorBitmap.IsInvalid || bits == IntPtr.Zero)
            {
                return IconHandle.GetInvalidIcon();
            }

            Marshal.Copy(colorArray, 0, bits, colorArray.Length);
            var maskArray = GenerateMaskArray(width, height, colorArray);

            maskBitmap = InteropMethods.CreateBitmap(width, height, 1, 1, maskArray);
            if (maskBitmap.IsInvalid)
            {
                return IconHandle.GetInvalidIcon();
            }

            var iconInfo = new InteropValues.ICONINFO
            {
                fIcon = isIcon,
                xHotspot = xHotspot,
                yHotspot = yHotspot,
                hbmMask = maskBitmap,
                hbmColor = colorBitmap
            };

            return InteropMethods.CreateIconIndirect(iconInfo);
        }
        finally
        {
            colorBitmap?.Dispose();
            maskBitmap?.Dispose();
        }
    }

    private static byte[] GenerateMaskArray(int width, int height, byte[] colorArray)
    {
        var nCount = width * height;
        var bytesPerScanLine = AlignToBytes(width, 2) / 8;
        var bitsMask = new byte[bytesPerScanLine * height];

        for (var i = 0; i < nCount; i++)
        {
            var hPos = i % width;
            var vPos = i / width;
            var byteIndex = hPos / 8;
            var offsetBit = (byte) (0x80 >> (hPos % 8));

            if (colorArray[i * 4 + 3] == 0x00)
            {
                bitsMask[byteIndex + bytesPerScanLine * vPos] |= offsetBit;
            }
            else
            {
                bitsMask[byteIndex + bytesPerScanLine * vPos] &= (byte) ~offsetBit;
            }

            if (hPos == width - 1 && width == 8)
            {
                bitsMask[1 + bytesPerScanLine * vPos] = 0xff;
            }
        }

        return bitsMask;
    }

    internal static int AlignToBytes(double original, int nBytesCount)
    {
        var nBitsCount = 8 << (nBytesCount - 1);
        return ((int) Math.Ceiling(original) + (nBitsCount - 1)) / nBitsCount * nBitsCount;
    }

    private static BitmapSource GenerateBitmapSource(ImageSource img, Size renderSize)
    {
        var drawingDimensions = new Rect(0, 0, renderSize.Width, renderSize.Height);

        var renderRatio = renderSize.Width / renderSize.Height;
        var aspectRatio = img.Width / img.Height;

        if (img.Width <= renderSize.Width && img.Height <= renderSize.Height)
        {
            drawingDimensions = new Rect((renderSize.Width - img.Width) / 2, (renderSize.Height - img.Height) / 2, img.Width, img.Height);
        }
        else if (renderRatio > aspectRatio)
        {
            var scaledRenderWidth = (img.Width / img.Height) * renderSize.Width;
            drawingDimensions = new Rect((renderSize.Width - scaledRenderWidth) / 2, 0, scaledRenderWidth, renderSize.Height);
        }
        else if (renderRatio < aspectRatio)
        {
            var scaledRenderHeight = img.Height / img.Width * renderSize.Height;
            drawingDimensions = new Rect(0, (renderSize.Height - scaledRenderHeight) / 2, renderSize.Width, scaledRenderHeight);
        }

        var dv = new DrawingVisual();
        var dc = dv.RenderOpen();
        dc.DrawImage(img, drawingDimensions);
        dc.Close();

        var bmp = new RenderTargetBitmap((int) renderSize.Width, (int) renderSize.Height, 96, 96, PixelFormats.Pbgra32);
        bmp.Render(dv);

        return bmp;
    }

    private static BitmapFrame GetBestMatch(ReadOnlyCollection<BitmapFrame> frames, Size size)
    {
        var bestScore = int.MaxValue;
        var bestBpp = 0;
        var bestIndex = 0;

        var isBitmapIconDecoder = frames[0].Decoder is IconBitmapDecoder;

        for (var i = 0; i < frames.Count && bestScore != 0; ++i)
        {
            var currentIconBitDepth = isBitmapIconDecoder ? frames[i].Thumbnail.Format.BitsPerPixel : frames[i].Format.BitsPerPixel;

            if (currentIconBitDepth == 0)
            {
                currentIconBitDepth = 8;
            }

            var score = MatchImage(frames[i], size, currentIconBitDepth);
            if (score < bestScore)
            {
                bestIndex = i;
                bestBpp = currentIconBitDepth;
                bestScore = score;
            }
            else if (score == bestScore)
            {
                if (bestBpp < currentIconBitDepth)
                {
                    bestIndex = i;
                    bestBpp = currentIconBitDepth;
                }
            }
        }

        return frames[bestIndex];
    }

    private static int MatchImage(BitmapFrame frame, Size size, int bpp)
    {
        var score = 2 * MyAbs(bpp, SystemBitDepth, false) +
                    MyAbs(frame.PixelWidth, (int) size.Width, true) +
                    MyAbs(frame.PixelHeight, (int) size.Height, true);

        return score;
    }

    private static int MyAbs(int valueHave, int valueWant, bool fPunish)
    {
        var diff = (valueHave - valueWant);

        if (diff < 0)
        {
            diff = (fPunish ? -2 : -1) * diff;
        }

        return diff;
    }

    [SecurityCritical, SecuritySafeCritical]
    private static void EnsureSystemMetrics()
    {
        if (SystemBitDepth == 0)
        {
            var hdcDesktop = new HandleRef(null, InteropMethods.GetDC(new HandleRef()));
            try
            {
                var sysBitDepth = InteropMethods.GetDeviceCaps(hdcDesktop, InteropValues.BITSPIXEL);
                sysBitDepth *= InteropMethods.GetDeviceCaps(hdcDesktop, InteropValues.PLANES);

                if (sysBitDepth == 8)
                {
                    sysBitDepth = 4;
                }

                var cxSmallIcon = InteropMethods.GetSystemMetrics(InteropValues.SM.CXSMICON);
                var cySmallIcon = InteropMethods.GetSystemMetrics(InteropValues.SM.CYSMICON);
                var cxIcon = InteropMethods.GetSystemMetrics(InteropValues.SM.CXICON);
                var cyIcon = InteropMethods.GetSystemMetrics(InteropValues.SM.CYICON);

                SmallIconSize = new Size(cxSmallIcon, cySmallIcon);
                IconSize = new Size(cxIcon, cyIcon);
                SystemBitDepth = sysBitDepth;
            }
            finally
            {
                InteropMethods.ReleaseDC(new HandleRef(), hdcDesktop);
            }
        }
    }

    [SecurityCritical, SecuritySafeCritical]
    public static void GetDefaultIconHandles(out IconHandle largeIconHandle, out IconHandle smallIconHandle)
    {
        largeIconHandle = null;
        smallIconHandle = null;

        SecurityHelper.DemandUIWindowPermission();

        var iconModuleFile = InteropMethods.GetModuleFileName(new HandleRef());
        InteropMethods.ExtractIconEx(iconModuleFile, 0, out largeIconHandle, out smallIconHandle, 1);
    }
}
