using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Standard;

internal static class Utility
{
    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    private static bool _MemCmp(IntPtr left, IntPtr right, long cb)
    {
        int num = 0;
        while ((long) num < cb - 8L)
        {
            long num2 = Marshal.ReadInt64(left, num);
            long num3 = Marshal.ReadInt64(right, num);
            if (num2 != num3)
            {
                return false;
            }
            num += 8;
        }
        while ((long) num < cb)
        {
            byte b = Marshal.ReadByte(left, num);
            byte b2 = Marshal.ReadByte(right, num);
            if (b != b2)
            {
                return false;
            }
            num++;
        }
        return true;
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static int RGB(Color c)
    {
        return (int) c.R | (int) c.G << 8 | (int) c.B << 16;
    }

    public static Color ColorFromArgbDword(uint color)
    {
        return Color.FromArgb((byte) ((color & 4278190080u) >> 24), (byte) ((color & 16711680u) >> 16), (byte) ((color & 65280u) >> 8), (byte) (color & 255u));
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static int GET_X_LPARAM(IntPtr lParam)
    {
        return Utility.LOWORD(lParam.ToInt32());
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static int GET_Y_LPARAM(IntPtr lParam)
    {
        return Utility.HIWORD(lParam.ToInt32());
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static int HIWORD(int i)
    {
        return (int) ((short) (i >> 16));
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static int LOWORD(int i)
    {
        return (int) ((short) (i & 65535));
    }

    [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static bool AreStreamsEqual(Stream left, Stream right)
    {
        if (left == null)
        {
            return right == null;
        }
        if (right == null)
        {
            return false;
        }
        if (!left.CanRead || !right.CanRead)
        {
            throw new NotSupportedException("The streams can't be read for comparison");
        }
        if (left.Length != right.Length)
        {
            return false;
        }
        int num = (int) left.Length;
        left.Position = 0L;
        right.Position = 0L;
        int i = 0;
        int num2 = 0;
        byte[] array = new byte[512];
        byte[] array2 = new byte[512];
        GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
        IntPtr left2 = gchandle.AddrOfPinnedObject();
        GCHandle gchandle2 = GCHandle.Alloc(array2, GCHandleType.Pinned);
        IntPtr right2 = gchandle2.AddrOfPinnedObject();
        bool result;
        try
        {
            while (i < num)
            {
                int num3 = left.Read(array, 0, array.Length);
                int num4 = right.Read(array2, 0, array2.Length);
                if (num3 != num4)
                {
                    return false;
                }
                if (!Utility._MemCmp(left2, right2, (long) num3))
                {
                    return false;
                }
                i += num3;
                num2 += num4;
            }
            result = true;
        }
        finally
        {
            gchandle.Free();
            gchandle2.Free();
        }
        return result;
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static bool GuidTryParse(string guidString, out Guid guid)
    {
        Verify.IsNeitherNullNorEmpty(guidString, "guidString");
        try
        {
            guid = new Guid(guidString);
            return true;
        }
        catch (FormatException)
        {
        }
        catch (OverflowException)
        {
        }
        guid = default(Guid);
        return false;
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static bool IsFlagSet(int value, int mask)
    {
        return 0 != (value & mask);
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static bool IsFlagSet(uint value, uint mask)
    {
        return 0u != (value & mask);
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static bool IsFlagSet(long value, long mask)
    {
        return 0L != (value & mask);
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static bool IsFlagSet(ulong value, ulong mask)
    {
        return 0UL != (value & mask);
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static bool IsOSVistaOrNewer
    {
        get
        {
            return Utility._osVersion >= new Version(6, 0);
        }
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static bool IsOSWindows7OrNewer
    {
        get
        {
            return Utility._osVersion >= new Version(6, 1);
        }
    }

    public static bool IsPresentationFrameworkVersionLessThan4
    {
        get
        {
            return Utility._presentationFrameworkVersion < new Version(4, 0);
        }
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
    public static IntPtr GenerateHICON(ImageSource image, Size dimensions)
    {
        if (image == null)
        {
            return IntPtr.Zero;
        }
        BitmapFrame bitmapFrame = image as BitmapFrame;
        if (bitmapFrame != null)
        {
            bitmapFrame = Utility.GetBestMatch(bitmapFrame.Decoder.Frames, (int) dimensions.Width, (int) dimensions.Height);
        }
        else
        {
            Rect rectangle = new Rect(0.0, 0.0, dimensions.Width, dimensions.Height);
            double num = dimensions.Width / dimensions.Height;
            double num2 = image.Width / image.Height;
            if (image.Width <= dimensions.Width && image.Height <= dimensions.Height)
            {
                rectangle = new Rect((dimensions.Width - image.Width) / 2.0, (dimensions.Height - image.Height) / 2.0, image.Width, image.Height);
            }
            else if (num > num2)
            {
                double num3 = image.Width / image.Height * dimensions.Width;
                rectangle = new Rect((dimensions.Width - num3) / 2.0, 0.0, num3, dimensions.Height);
            }
            else if (num < num2)
            {
                double num4 = image.Height / image.Width * dimensions.Height;
                rectangle = new Rect(0.0, (dimensions.Height - num4) / 2.0, dimensions.Width, num4);
            }
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            drawingContext.DrawImage(image, rectangle);
            drawingContext.Close();
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int) dimensions.Width, (int) dimensions.Height, 96.0, 96.0, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(drawingVisual);
            bitmapFrame = BitmapFrame.Create(renderTargetBitmap);
        }
        IntPtr result;
        using (MemoryStream memoryStream = new MemoryStream())
        {
            new PngBitmapEncoder
            {
                Frames =
                {
                    bitmapFrame
                }
            }.Save(memoryStream);
            using (ManagedIStream managedIStream = new ManagedIStream(memoryStream))
            {
                IntPtr zero = IntPtr.Zero;
                try
                {
                    Status status = NativeMethods.GdipCreateBitmapFromStream(managedIStream, out zero);
                    if (status != Status.Ok)
                    {
                        result = IntPtr.Zero;
                    }
                    else
                    {
                        IntPtr intPtr;
                        status = NativeMethods.GdipCreateHICONFromBitmap(zero, out intPtr);
                        if (status != Status.Ok)
                        {
                            result = IntPtr.Zero;
                        }
                        else
                        {
                            result = intPtr;
                        }
                    }
                }
                finally
                {
                    Utility.SafeDisposeImage(ref zero);
                }
            }
        }
        return result;
    }

    public static BitmapFrame GetBestMatch(IList<BitmapFrame> frames, int width, int height)
    {
        return Utility._GetBestMatch(frames, Utility._GetBitDepth(), width, height);
    }

    private static int _MatchImage(BitmapFrame frame, int bitDepth, int width, int height, int bpp)
    {
        return 2 * Utility._WeightedAbs(bpp, bitDepth, false) + Utility._WeightedAbs(frame.PixelWidth, width, true) + Utility._WeightedAbs(frame.PixelHeight, height, true);
    }

    private static int _WeightedAbs(int valueHave, int valueWant, bool fPunish)
    {
        int num = valueHave - valueWant;
        if (num < 0)
        {
            num = (fPunish ? -2 : -1) * num;
        }
        return num;
    }

    private static BitmapFrame _GetBestMatch(IList<BitmapFrame> frames, int bitDepth, int width, int height)
    {
        int num = int.MaxValue;
        int num2 = 0;
        int index = 0;
        bool flag = frames[0].Decoder is IconBitmapDecoder;
        int num3 = 0;
        while (num3 < frames.Count && num != 0)
        {
            int num4 = flag ? frames[num3].Thumbnail.Format.BitsPerPixel : frames[num3].Format.BitsPerPixel;
            if (num4 == 0)
            {
                num4 = 8;
            }
            int num5 = Utility._MatchImage(frames[num3], bitDepth, width, height, num4);
            if (num5 < num)
            {
                index = num3;
                num2 = num4;
                num = num5;
            }
            else if (num5 == num && num2 < num4)
            {
                index = num3;
                num2 = num4;
            }
            num3++;
        }
        return frames[index];
    }

    private static int _GetBitDepth()
    {
        if (Utility.s_bitDepth == 0)
        {
            using (SafeDC desktop = SafeDC.GetDesktop())
            {
                Utility.s_bitDepth = NativeMethods.GetDeviceCaps(desktop, DeviceCap.BITSPIXEL) * NativeMethods.GetDeviceCaps(desktop, DeviceCap.PLANES);
            }
        }
        return Utility.s_bitDepth;
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static void SafeDeleteFile(string path)
    {
        if (!string.IsNullOrEmpty(path))
        {
            File.Delete(path);
        }
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static void SafeDeleteObject(ref IntPtr gdiObject)
    {
        IntPtr intPtr = gdiObject;
        gdiObject = IntPtr.Zero;
        if (IntPtr.Zero != intPtr)
        {
            NativeMethods.DeleteObject(intPtr);
        }
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static void SafeDestroyIcon(ref IntPtr hicon)
    {
        IntPtr intPtr = hicon;
        hicon = IntPtr.Zero;
        if (IntPtr.Zero != intPtr)
        {
            NativeMethods.DestroyIcon(intPtr);
        }
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static void SafeDestroyWindow(ref IntPtr hwnd)
    {
        IntPtr hwnd2 = hwnd;
        hwnd = IntPtr.Zero;
        if (NativeMethods.IsWindow(hwnd2))
        {
            NativeMethods.DestroyWindow(hwnd2);
        }
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static void SafeDispose<T>(ref T disposable) where T : IDisposable
    {
        IDisposable disposable2 = disposable;
        disposable = default(T);
        if (disposable2 != null)
        {
            disposable2.Dispose();
        }
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static void SafeDisposeImage(ref IntPtr gdipImage)
    {
        IntPtr intPtr = gdipImage;
        gdipImage = IntPtr.Zero;
        if (IntPtr.Zero != intPtr)
        {
            NativeMethods.GdipDisposeImage(intPtr);
        }
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
    public static void SafeCoTaskMemFree(ref IntPtr ptr)
    {
        IntPtr intPtr = ptr;
        ptr = IntPtr.Zero;
        if (IntPtr.Zero != intPtr)
        {
            Marshal.FreeCoTaskMem(intPtr);
        }
    }

    [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static void SafeFreeHGlobal(ref IntPtr hglobal)
    {
        IntPtr intPtr = hglobal;
        hglobal = IntPtr.Zero;
        if (IntPtr.Zero != intPtr)
        {
            Marshal.FreeHGlobal(intPtr);
        }
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
    public static void SafeRelease<T>(ref T comObject) where T : class
    {
        T t = comObject;
        comObject = default(T);
        if (t != null)
        {
            Marshal.ReleaseComObject(t);
        }
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static void GeneratePropertyString(StringBuilder source, string propertyName, string value)
    {
        if (source.Length != 0)
        {
            source.Append(' ');
        }
        source.Append(propertyName);
        source.Append(": ");
        if (string.IsNullOrEmpty(value))
        {
            source.Append("<null>");
            return;
        }
        source.Append('"');
        source.Append(value);
        source.Append('"');
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    [Obsolete]
    public static string GenerateToString<T>(T @object) where T : struct
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (PropertyInfo propertyInfo in typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            if (stringBuilder.Length != 0)
            {
                stringBuilder.Append(", ");
            }
            object value = propertyInfo.GetValue(@object, null);
            string format = (value == null) ? "{0}: <null>" : "{0}: \"{1}\"";
            stringBuilder.AppendFormat(format, propertyInfo.Name, value);
        }
        return stringBuilder.ToString();
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static void CopyStream(Stream destination, Stream source)
    {
        destination.Position = 0L;
        if (source.CanSeek)
        {
            source.Position = 0L;
            destination.SetLength(source.Length);
        }
        byte[] array = new byte[4096];
        int num;
        do
        {
            num = source.Read(array, 0, array.Length);
            if (num != 0)
            {
                destination.Write(array, 0, num);
            }
        }
        while (array.Length == num);
        destination.Position = 0L;
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static string HashStreamMD5(Stream stm)
    {
        stm.Position = 0L;
        StringBuilder stringBuilder = new StringBuilder();
        using (MD5 md = MD5.Create())
        {
            foreach (byte b in md.ComputeHash(stm))
            {
                stringBuilder.Append(b.ToString("x2", CultureInfo.InvariantCulture));
            }
        }
        return stringBuilder.ToString();
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static void EnsureDirectory(string path)
    {
        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static bool MemCmp(byte[] left, byte[] right, int cb)
    {
        GCHandle gchandle = GCHandle.Alloc(left, GCHandleType.Pinned);
        IntPtr left2 = gchandle.AddrOfPinnedObject();
        GCHandle gchandle2 = GCHandle.Alloc(right, GCHandleType.Pinned);
        IntPtr right2 = gchandle2.AddrOfPinnedObject();
        bool result = Utility._MemCmp(left2, right2, (long) cb);
        gchandle.Free();
        gchandle2.Free();
        return result;
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static string UrlDecode(string url)
    {
        if (url == null)
        {
            return null;
        }
        Utility._UrlDecoder urlDecoder = new Utility._UrlDecoder(url.Length, Encoding.UTF8);
        int length = url.Length;
        for (int i = 0; i < length; i++)
        {
            char c = url[i];
            if (c == '+')
            {
                urlDecoder.AddByte(32);
            }
            else
            {
                if (c == '%' && i < length - 2)
                {
                    if (url[i + 1] == 'u' && i < length - 5)
                    {
                        int num = Utility._HexToInt(url[i + 2]);
                        int num2 = Utility._HexToInt(url[i + 3]);
                        int num3 = Utility._HexToInt(url[i + 4]);
                        int num4 = Utility._HexToInt(url[i + 5]);
                        if (num >= 0 && num2 >= 0 && num3 >= 0 && num4 >= 0)
                        {
                            urlDecoder.AddChar((char) (num << 12 | num2 << 8 | num3 << 4 | num4));
                            i += 5;
                            goto IL_12D;
                        }
                    }
                    else
                    {
                        int num5 = Utility._HexToInt(url[i + 1]);
                        int num6 = Utility._HexToInt(url[i + 2]);
                        if (num5 >= 0 && num6 >= 0)
                        {
                            urlDecoder.AddByte((byte) (num5 << 4 | num6));
                            i += 2;
                            goto IL_12D;
                        }
                    }
                }
                if ((c & 'ﾀ') == '\0')
                {
                    urlDecoder.AddByte((byte) c);
                }
                else
                {
                    urlDecoder.AddChar(c);
                }
            }
IL_12D:;
        }
        return urlDecoder.GetString();
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static string UrlEncode(string url)
    {
        if (url == null)
        {
            return null;
        }
        byte[] array = Encoding.UTF8.GetBytes(url);
        bool flag = false;
        int num = 0;
        foreach (byte b in array)
        {
            if (b == 32)
            {
                flag = true;
            }
            else if (!Utility._UrlEncodeIsSafe(b))
            {
                num++;
                flag = true;
            }
        }
        if (flag)
        {
            byte[] array3 = new byte[array.Length + num * 2];
            int num2 = 0;
            foreach (byte b2 in array)
            {
                if (Utility._UrlEncodeIsSafe(b2))
                {
                    array3[num2++] = b2;
                }
                else if (b2 == 32)
                {
                    array3[num2++] = 43;
                }
                else
                {
                    array3[num2++] = 37;
                    array3[num2++] = Utility._IntToHex(b2 >> 4 & 15);
                    array3[num2++] = Utility._IntToHex((int) (b2 & 15));
                }
            }
            array = array3;
        }
        return Encoding.ASCII.GetString(array);
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    private static bool _UrlEncodeIsSafe(byte b)
    {
        if (Utility._IsAsciiAlphaNumeric(b))
        {
            return true;
        }
        if (b != 33)
        {
            switch (b)
            {
                case 39:
                case 40:
                case 41:
                case 42:
                case 45:
                case 46:
                    return true;
                case 43:
                case 44:
                    break;
                default:
                    if (b == 95)
                    {
                        return true;
                    }
                    break;
            }
            return false;
        }
        return true;
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    private static bool _IsAsciiAlphaNumeric(byte b)
    {
        return (b >= 97 && b <= 122) || (b >= 65 && b <= 90) || (b >= 48 && b <= 57);
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    private static byte _IntToHex(int n)
    {
        if (n <= 9)
        {
            return (byte) (n + 48);
        }
        return (byte) (n - 10 + 65);
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    private static int _HexToInt(char h)
    {
        if (h >= '0' && h <= '9')
        {
            return (int) (h - '0');
        }
        if (h >= 'a' && h <= 'f')
        {
            return (int) (h - 'a' + '\n');
        }
        if (h >= 'A' && h <= 'F')
        {
            return (int) (h - 'A' + '\n');
        }
        return -1;
    }

    public static void AddDependencyPropertyChangeListener(object component, DependencyProperty property, EventHandler listener)
    {
        if (component == null)
        {
            return;
        }
        DependencyPropertyDescriptor dependencyPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(property, component.GetType());
        dependencyPropertyDescriptor.AddValueChanged(component, listener);
    }

    public static void RemoveDependencyPropertyChangeListener(object component, DependencyProperty property, EventHandler listener)
    {
        if (component == null)
        {
            return;
        }
        DependencyPropertyDescriptor dependencyPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(property, component.GetType());
        dependencyPropertyDescriptor.RemoveValueChanged(component, listener);
    }

    public static bool IsThicknessNonNegative(Thickness thickness)
    {
        return Utility.IsDoubleFiniteAndNonNegative(thickness.Top) && Utility.IsDoubleFiniteAndNonNegative(thickness.Left) && Utility.IsDoubleFiniteAndNonNegative(thickness.Bottom) && Utility.IsDoubleFiniteAndNonNegative(thickness.Right);
    }

    public static bool IsCornerRadiusValid(CornerRadius cornerRadius)
    {
        return Utility.IsDoubleFiniteAndNonNegative(cornerRadius.TopLeft) && Utility.IsDoubleFiniteAndNonNegative(cornerRadius.TopRight) && Utility.IsDoubleFiniteAndNonNegative(cornerRadius.BottomLeft) && Utility.IsDoubleFiniteAndNonNegative(cornerRadius.BottomRight);
    }

    public static bool IsDoubleFiniteAndNonNegative(double d)
    {
        return !double.IsNaN(d) && !double.IsInfinity(d) && d >= 0.0;
    }

    private static readonly Version _osVersion = Environment.OSVersion.Version;

    private static readonly Version _presentationFrameworkVersion = Assembly.GetAssembly(typeof(Window)).GetName().Version;

    private static int s_bitDepth;

    private class _UrlDecoder
    {
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public _UrlDecoder(int size, Encoding encoding)
        {
            this._encoding = encoding;
            this._charBuffer = new char[size];
            this._byteBuffer = new byte[size];
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public void AddByte(byte b)
        {
            this._byteBuffer[this._byteCount++] = b;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public void AddChar(char ch)
        {
            this._FlushBytes();
            this._charBuffer[this._charCount++] = ch;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        private void _FlushBytes()
        {
            if (this._byteCount > 0)
            {
                this._charCount += this._encoding.GetChars(this._byteBuffer, 0, this._byteCount, this._charBuffer, this._charCount);
                this._byteCount = 0;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public string GetString()
        {
            this._FlushBytes();
            if (this._charCount > 0)
            {
                return new string(this._charBuffer, 0, this._charCount);
            }
            return "";
        }

        private readonly Encoding _encoding;

        private readonly char[] _charBuffer;

        private readonly byte[] _byteBuffer;

        private int _byteCount;

        private int _charCount;
    }
}
