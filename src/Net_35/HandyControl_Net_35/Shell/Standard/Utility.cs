namespace Standard
{
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

    internal static class Utility
    {
        private static readonly Version _osVersion = Environment.OSVersion.Version;
        private static readonly Version _presentationFrameworkVersion = Assembly.GetAssembly(typeof(Window)).GetName().Version;
        private static int s_bitDepth;

        private static BitmapFrame _GetBestMatch(IList<BitmapFrame> frames, int bitDepth, int width, int height)
        {
            int num = 0x7fffffff;
            int num2 = 0;
            int num3 = 0;
            bool flag = frames[0].Decoder is IconBitmapDecoder;
            for (int i = 0; (i < frames.Count) && (num != 0); i++)
            {
                int bpp = flag ? frames[i].Thumbnail.Format.BitsPerPixel : frames[i].Format.BitsPerPixel;
                if (bpp == 0)
                {
                    bpp = 8;
                }
                int num6 = _MatchImage(frames[i], bitDepth, width, height, bpp);
                if (num6 < num)
                {
                    num3 = i;
                    num2 = bpp;
                    num = num6;
                }
                else if ((num6 == num) && (num2 < bpp))
                {
                    num3 = i;
                    num2 = bpp;
                }
            }
            return frames[num3];
        }

        private static int _GetBitDepth()
        {
            if (s_bitDepth == 0)
            {
                using (Standard.SafeDC edc = Standard.SafeDC.GetDesktop())
                {
                    s_bitDepth = Standard.NativeMethods.GetDeviceCaps(edc, Standard.DeviceCap.BITSPIXEL) * Standard.NativeMethods.GetDeviceCaps(edc, Standard.DeviceCap.PLANES);
                }
            }
            return s_bitDepth;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        private static int _HexToInt(char h)
        {
            if ((h >= '0') && (h <= '9'))
            {
                return (h - '0');
            }
            if ((h >= 'a') && (h <= 'f'))
            {
                return ((h - 'a') + 10);
            }
            if ((h >= 'A') && (h <= 'F'))
            {
                return ((h - 'A') + 10);
            }
            return -1;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        private static byte _IntToHex(int n)
        {
            if (n <= 9)
            {
                return (byte) (n + 0x30);
            }
            return (byte) ((n - 10) + 0x41);
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        private static bool _IsAsciiAlphaNumeric(byte b)
        {
            if (((b < 0x61) || (b > 0x7a)) && ((b < 0x41) || (b > 90)))
            {
                return ((b >= 0x30) && (b <= 0x39));
            }
            return true;
        }

        private static int _MatchImage(BitmapFrame frame, int bitDepth, int width, int height, int bpp)
        {
            return (((2 * _WeightedAbs(bpp, bitDepth, false)) + _WeightedAbs(frame.PixelWidth, width, true)) + _WeightedAbs(frame.PixelHeight, height, true));
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        private static bool _MemCmp(IntPtr left, IntPtr right, long cb)
        {
            int ofs = 0;
            while (ofs < (cb - 8L))
            {
                long num2 = Marshal.ReadInt64(left, ofs);
                long num3 = Marshal.ReadInt64(right, ofs);
                if (num2 != num3)
                {
                    return false;
                }
                ofs += 8;
            }
            while (ofs < cb)
            {
                byte num4 = Marshal.ReadByte(left, ofs);
                byte num5 = Marshal.ReadByte(right, ofs);
                if (num4 != num5)
                {
                    return false;
                }
                ofs++;
            }
            return true;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        private static bool _UrlEncodeIsSafe(byte b)
        {
            if (_IsAsciiAlphaNumeric(b))
            {
                return true;
            }
            switch (((char) b))
            {
                case '\'':
                case '(':
                case ')':
                case '*':
                case '-':
                case '.':
                case '_':
                case '!':
                    return true;
            }
            return false;
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

        public static void AddDependencyPropertyChangeListener(object component, DependencyProperty property, EventHandler listener)
        {
            if (component != null)
            {
                DependencyPropertyDescriptor.FromProperty(property, component.GetType()).AddValueChanged(component, listener);
            }
        }

        [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands"), SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static bool AreStreamsEqual(Stream left, Stream right)
        {
            bool flag;
            if (left == null)
            {
                return (right == null);
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
            int length = (int) left.Length;
            left.Position = 0L;
            right.Position = 0L;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            int num5 = 0;
            byte[] buffer = new byte[0x200];
            byte[] buffer2 = new byte[0x200];
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            IntPtr ptr = handle.AddrOfPinnedObject();
            GCHandle handle2 = GCHandle.Alloc(buffer2, GCHandleType.Pinned);
            IntPtr ptr2 = handle2.AddrOfPinnedObject();
            try
            {
                while (num2 < length)
                {
                    num4 = left.Read(buffer, 0, buffer.Length);
                    num5 = right.Read(buffer2, 0, buffer2.Length);
                    if (num4 != num5)
                    {
                        return false;
                    }
                    if (!_MemCmp(ptr, ptr2, (long) num4))
                    {
                        return false;
                    }
                    num2 += num4;
                    num3 += num5;
                }
                flag = true;
            }
            finally
            {
                handle.Free();
                handle2.Free();
            }
            return flag;
        }

        public static Color ColorFromArgbDword(uint color)
        {
            return Color.FromArgb((byte) ((color & -16777216) >> 0x18), (byte) ((color & 0xff0000) >> 0x10), (byte) ((color & 0xff00) >> 8), (byte) (color & 0xff));
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static void CopyStream(Stream destination, Stream source)
        {
            int num;
            destination.Position = 0L;
            if (source.CanSeek)
            {
                source.Position = 0L;
                destination.SetLength(source.Length);
            }
            byte[] buffer = new byte[0x1000];
            do
            {
                num = source.Read(buffer, 0, buffer.Length);
                if (num != 0)
                {
                    destination.Write(buffer, 0, num);
                }
            }
            while (buffer.Length == num);
            destination.Position = 0L;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static void EnsureDirectory(string path)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public static IntPtr GenerateHICON(ImageSource image, Size dimensions)
        {
            IntPtr ptr3;
            if (image == null)
            {
                return IntPtr.Zero;
            }
            BitmapFrame item = image as BitmapFrame;
            if (item != null)
            {
                item = GetBestMatch(item.Decoder.Frames, (int) dimensions.Width, (int) dimensions.Height);
            }
            else
            {
                Rect rectangle = new Rect(0.0, 0.0, dimensions.Width, dimensions.Height);
                double num = dimensions.Width / dimensions.Height;
                double num2 = image.Width / image.Height;
                if ((image.Width <= dimensions.Width) && (image.Height <= dimensions.Height))
                {
                    rectangle = new Rect((dimensions.Width - image.Width) / 2.0, (dimensions.Height - image.Height) / 2.0, image.Width, image.Height);
                }
                else if (num > num2)
                {
                    double width = (image.Width / image.Height) * dimensions.Width;
                    rectangle = new Rect((dimensions.Width - width) / 2.0, 0.0, width, dimensions.Height);
                }
                else if (num < num2)
                {
                    double height = (image.Height / image.Width) * dimensions.Height;
                    rectangle = new Rect(0.0, (dimensions.Height - height) / 2.0, dimensions.Width, height);
                }
                DrawingVisual visual = new DrawingVisual();
                DrawingContext context = visual.RenderOpen();
                context.DrawImage(image, rectangle);
                context.Close();
                RenderTargetBitmap source = new RenderTargetBitmap((int) dimensions.Width, (int) dimensions.Height, 96.0, 96.0, PixelFormats.Pbgra32);
                source.Render(visual);
                item = BitmapFrame.Create(source);
            }
            using (MemoryStream stream = new MemoryStream())
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(item);
                encoder.Save(stream);
                using (ManagedIStream stream2 = new ManagedIStream(stream))
                {
                    IntPtr zero = IntPtr.Zero;
                    try
                    {
                        IntPtr ptr2;
                        if (Standard.NativeMethods.GdipCreateBitmapFromStream(stream2, out zero) != Standard.Status.Ok)
                        {
                            return IntPtr.Zero;
                        }
                        if (Standard.NativeMethods.GdipCreateHICONFromBitmap(zero, out ptr2) != Standard.Status.Ok)
                        {
                            return IntPtr.Zero;
                        }
                        ptr3 = ptr2;
                    }
                    finally
                    {
                        SafeDisposeImage(ref zero);
                    }
                }
            }
            return ptr3;
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
            }
            else
            {
                source.Append('"');
                source.Append(value);
                source.Append('"');
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), Obsolete]
        public static string GenerateToString<T>(T @object) where T: struct
        {
            StringBuilder builder = new StringBuilder();
            foreach (PropertyInfo info in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (builder.Length != 0)
                {
                    builder.Append(", ");
                }
                object obj2 = info.GetValue(@object, null);
                string format = (obj2 == null) ? "{0}: <null>" : "{0}: \"{1}\"";
                builder.AppendFormat(format, info.Name, obj2);
            }
            return builder.ToString();
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static int GET_X_LPARAM(IntPtr lParam)
        {
            return LOWORD(lParam.ToInt32());
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static int GET_Y_LPARAM(IntPtr lParam)
        {
            return HIWORD(lParam.ToInt32());
        }

        public static BitmapFrame GetBestMatch(IList<BitmapFrame> frames, int width, int height)
        {
            return _GetBestMatch(frames, _GetBitDepth(), width, height);
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static bool GuidTryParse(string guidString, out Guid guid)
        {
            Standard.Verify.IsNeitherNullNorEmpty(guidString, "guidString");
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
            guid = new Guid();
            return false;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static string HashStreamMD5(Stream stm)
        {
            stm.Position = 0L;
            StringBuilder builder = new StringBuilder();
            using (MD5 md = MD5.Create())
            {
                foreach (byte num in md.ComputeHash(stm))
                {
                    builder.Append(num.ToString("x2", CultureInfo.InvariantCulture));
                }
            }
            return builder.ToString();
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static int HIWORD(int i)
        {
            return (short) (i >> 0x10);
        }

        public static bool IsCornerRadiusValid(CornerRadius cornerRadius)
        {
            if (!IsDoubleFiniteAndNonNegative(cornerRadius.TopLeft))
            {
                return false;
            }
            if (!IsDoubleFiniteAndNonNegative(cornerRadius.TopRight))
            {
                return false;
            }
            if (!IsDoubleFiniteAndNonNegative(cornerRadius.BottomLeft))
            {
                return false;
            }
            if (!IsDoubleFiniteAndNonNegative(cornerRadius.BottomRight))
            {
                return false;
            }
            return true;
        }

        public static bool IsDoubleFiniteAndNonNegative(double d)
        {
            return ((!double.IsNaN(d) && !double.IsInfinity(d)) && (d >= 0.0));
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static bool IsFlagSet(int value, int mask)
        {
            return (0 != (value & mask));
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static bool IsFlagSet(long value, long mask)
        {
            return (0L != (value & mask));
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static bool IsFlagSet(uint value, uint mask)
        {
            return (0 != (value & mask));
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static bool IsFlagSet(ulong value, ulong mask)
        {
            return (0L != (value & mask));
        }

        public static bool IsThicknessNonNegative(Thickness thickness)
        {
            if (!IsDoubleFiniteAndNonNegative(thickness.Top))
            {
                return false;
            }
            if (!IsDoubleFiniteAndNonNegative(thickness.Left))
            {
                return false;
            }
            if (!IsDoubleFiniteAndNonNegative(thickness.Bottom))
            {
                return false;
            }
            if (!IsDoubleFiniteAndNonNegative(thickness.Right))
            {
                return false;
            }
            return true;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static int LOWORD(int i)
        {
            return (short) (i & 0xffff);
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static bool MemCmp(byte[] left, byte[] right, int cb)
        {
            GCHandle handle = GCHandle.Alloc(left, GCHandleType.Pinned);
            IntPtr ptr = handle.AddrOfPinnedObject();
            GCHandle handle2 = GCHandle.Alloc(right, GCHandleType.Pinned);
            IntPtr ptr2 = handle2.AddrOfPinnedObject();
            bool flag = _MemCmp(ptr, ptr2, (long) cb);
            handle.Free();
            handle2.Free();
            return flag;
        }

        public static void RemoveDependencyPropertyChangeListener(object component, DependencyProperty property, EventHandler listener)
        {
            if (component != null)
            {
                DependencyPropertyDescriptor.FromProperty(property, component.GetType()).RemoveValueChanged(component, listener);
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static int RGB(Color c)
        {
            return ((c.R | (c.G << 8)) | (c.B << 0x10));
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        public static void SafeCoTaskMemFree(ref IntPtr ptr)
        {
            IntPtr ptr2 = ptr;
            ptr = IntPtr.Zero;
            if (IntPtr.Zero != ptr2)
            {
                Marshal.FreeCoTaskMem(ptr2);
            }
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
            IntPtr hObject = gdiObject;
            gdiObject = IntPtr.Zero;
            if (IntPtr.Zero != hObject)
            {
                Standard.NativeMethods.DeleteObject(hObject);
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static void SafeDestroyIcon(ref IntPtr hicon)
        {
            IntPtr handle = hicon;
            hicon = IntPtr.Zero;
            if (IntPtr.Zero != handle)
            {
                Standard.NativeMethods.DestroyIcon(handle);
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static void SafeDestroyWindow(ref IntPtr hwnd)
        {
            IntPtr ptr = hwnd;
            hwnd = IntPtr.Zero;
            if (Standard.NativeMethods.IsWindow(ptr))
            {
                Standard.NativeMethods.DestroyWindow(ptr);
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static void SafeDispose<T>(ref T disposable) where T: IDisposable
        {
            IDisposable disposable2 = (T) disposable;
            disposable = default(T);
            if (disposable2 != null)
            {
                disposable2.Dispose();
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static void SafeDisposeImage(ref IntPtr gdipImage)
        {
            IntPtr image = gdipImage;
            gdipImage = IntPtr.Zero;
            if (IntPtr.Zero != image)
            {
                Standard.NativeMethods.GdipDisposeImage(image);
            }
        }

        [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands"), SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static void SafeFreeHGlobal(ref IntPtr hglobal)
        {
            IntPtr ptr = hglobal;
            hglobal = IntPtr.Zero;
            if (IntPtr.Zero != ptr)
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        public static void SafeRelease<T>(ref T comObject) where T: class
        {
            T o = comObject;
            comObject = default(T);
            if (o != null)
            {
                Marshal.ReleaseComObject(o);
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static string UrlDecode(string url)
        {
            if (url == null)
            {
                return null;
            }
            _UrlDecoder decoder = new _UrlDecoder(url.Length, Encoding.UTF8);
            int length = url.Length;
            for (int i = 0; i < length; i++)
            {
                char ch = url[i];
                if (ch == '+')
                {
                    decoder.AddByte(0x20);
                    continue;
                }
                if ((ch == '%') && (i < (length - 2)))
                {
                    if ((url[i + 1] == 'u') && (i < (length - 5)))
                    {
                        int num3 = _HexToInt(url[i + 2]);
                        int num4 = _HexToInt(url[i + 3]);
                        int num5 = _HexToInt(url[i + 4]);
                        int num6 = _HexToInt(url[i + 5]);
                        if (((num3 < 0) || (num4 < 0)) || ((num5 < 0) || (num6 < 0)))
                        {
                            goto Label_0113;
                        }
                        decoder.AddChar((char) ((((num3 << 12) | (num4 << 8)) | (num5 << 4)) | num6));
                        i += 5;
                        continue;
                    }
                    int num7 = _HexToInt(url[i + 1]);
                    int num8 = _HexToInt(url[i + 2]);
                    if ((num7 >= 0) && (num8 >= 0))
                    {
                        decoder.AddByte((byte) ((num7 << 4) | num8));
                        i += 2;
                        continue;
                    }
                }
            Label_0113:
                if ((ch & 0xff80) == 0)
                {
                    decoder.AddByte((byte) ch);
                }
                else
                {
                    decoder.AddChar(ch);
                }
            }
            return decoder.GetString();
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static string UrlEncode(string url)
        {
            if (url == null)
            {
                return null;
            }
            byte[] bytes = Encoding.UTF8.GetBytes(url);
            bool flag = false;
            int num = 0;
            foreach (byte num2 in bytes)
            {
                if (num2 == 0x20)
                {
                    flag = true;
                }
                else if (!_UrlEncodeIsSafe(num2))
                {
                    num++;
                    flag = true;
                }
            }
            if (flag)
            {
                byte[] buffer2 = new byte[bytes.Length + (num * 2)];
                int num3 = 0;
                foreach (byte num4 in bytes)
                {
                    if (_UrlEncodeIsSafe(num4))
                    {
                        buffer2[num3++] = num4;
                    }
                    else if (num4 == 0x20)
                    {
                        buffer2[num3++] = 0x2b;
                    }
                    else
                    {
                        buffer2[num3++] = 0x25;
                        buffer2[num3++] = _IntToHex((num4 >> 4) & 15);
                        buffer2[num3++] = _IntToHex(num4 & 15);
                    }
                }
                bytes = buffer2;
            }
            return Encoding.ASCII.GetString(bytes);
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static bool IsOSVistaOrNewer
        {
            get
            {
                return (_osVersion >= new Version(6, 0));
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static bool IsOSWindows7OrNewer
        {
            get
            {
                return (_osVersion >= new Version(6, 1));
            }
        }

        public static bool IsPresentationFrameworkVersionLessThan4
        {
            get
            {
                return (_presentationFrameworkVersion < new Version(4, 0));
            }
        }

        private class _UrlDecoder
        {
            private readonly byte[] _byteBuffer;
            private int _byteCount;
            private readonly char[] _charBuffer;
            private int _charCount;
            private readonly Encoding _encoding;

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public _UrlDecoder(int size, Encoding encoding)
            {
                this._encoding = encoding;
                this._charBuffer = new char[size];
                this._byteBuffer = new byte[size];
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
            public string GetString()
            {
                this._FlushBytes();
                if (this._charCount > 0)
                {
                    return new string(this._charBuffer, 0, this._charCount);
                }
                return "";
            }
        }
    }
}

