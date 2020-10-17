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

namespace Standard
{
	// Token: 0x0200009A RID: 154
	internal static class Utility
	{
		// Token: 0x0600021F RID: 543 RVA: 0x00005538 File Offset: 0x00003738
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		private static bool _MemCmp(IntPtr left, IntPtr right, long cb)
		{
			int num = 0;
			while ((long)num < cb - 8L)
			{
				long num2 = Marshal.ReadInt64(left, num);
				long num3 = Marshal.ReadInt64(right, num);
				if (num2 != num3)
				{
					return false;
				}
				num += 8;
			}
			while ((long)num < cb)
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

		// Token: 0x06000220 RID: 544 RVA: 0x0000558F File Offset: 0x0000378F
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static int RGB(Color c)
		{
			return (int)c.R | (int)c.G << 8 | (int)c.B << 16;
		}

		// Token: 0x06000221 RID: 545 RVA: 0x000055AD File Offset: 0x000037AD
		public static Color ColorFromArgbDword(uint color)
		{
			return Color.FromArgb((byte)((color & 4278190080U) >> 24), (byte)((color & 16711680U) >> 16), (byte)((color & 65280U) >> 8), (byte)(color & 255U));
		}

		// Token: 0x06000222 RID: 546 RVA: 0x000055DC File Offset: 0x000037DC
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static int GET_X_LPARAM(IntPtr lParam)
		{
			return Utility.LOWORD(lParam.ToInt32());
		}

		// Token: 0x06000223 RID: 547 RVA: 0x000055EA File Offset: 0x000037EA
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static int GET_Y_LPARAM(IntPtr lParam)
		{
			return Utility.HIWORD(lParam.ToInt32());
		}

		// Token: 0x06000224 RID: 548 RVA: 0x000055F8 File Offset: 0x000037F8
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static int HIWORD(int i)
		{
			return (int)((short)(i >> 16));
		}

		// Token: 0x06000225 RID: 549 RVA: 0x000055FF File Offset: 0x000037FF
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static int LOWORD(int i)
		{
			return (int)((short)(i & 65535));
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000560C File Offset: 0x0000380C
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
			int num = (int)left.Length;
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
					if (!Utility._MemCmp(left2, right2, (long)num3))
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

		// Token: 0x06000227 RID: 551 RVA: 0x00005720 File Offset: 0x00003920
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

		// Token: 0x06000228 RID: 552 RVA: 0x00005774 File Offset: 0x00003974
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static bool IsFlagSet(int value, int mask)
		{
			return 0 != (value & mask);
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000577F File Offset: 0x0000397F
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static bool IsFlagSet(uint value, uint mask)
		{
			return 0U != (value & mask);
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000578A File Offset: 0x0000398A
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static bool IsFlagSet(long value, long mask)
		{
			return 0L != (value & mask);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x00005796 File Offset: 0x00003996
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static bool IsFlagSet(ulong value, ulong mask)
		{
			return 0UL != (value & mask);
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600022C RID: 556 RVA: 0x000057A2 File Offset: 0x000039A2
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static bool IsOSVistaOrNewer
		{
			get
			{
				return Utility._osVersion >= new Version(6, 0);
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600022D RID: 557 RVA: 0x000057B5 File Offset: 0x000039B5
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static bool IsOSWindows7OrNewer
		{
			get
			{
				return Utility._osVersion >= new Version(6, 1);
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600022E RID: 558 RVA: 0x000057C8 File Offset: 0x000039C8
		public static bool IsPresentationFrameworkVersionLessThan4
		{
			get
			{
				return Utility._presentationFrameworkVersion < new Version(4, 0);
			}
		}

		// Token: 0x0600022F RID: 559 RVA: 0x000057DC File Offset: 0x000039DC
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
				bitmapFrame = Utility.GetBestMatch(bitmapFrame.Decoder.Frames, (int)dimensions.Width, (int)dimensions.Height);
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
				RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)dimensions.Width, (int)dimensions.Height, 96.0, 96.0, PixelFormats.Pbgra32);
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

		// Token: 0x06000230 RID: 560 RVA: 0x00005A74 File Offset: 0x00003C74
		public static BitmapFrame GetBestMatch(IList<BitmapFrame> frames, int width, int height)
		{
			return Utility._GetBestMatch(frames, Utility._GetBitDepth(), width, height);
		}

		// Token: 0x06000231 RID: 561 RVA: 0x00005A84 File Offset: 0x00003C84
		private static int _MatchImage(BitmapFrame frame, int bitDepth, int width, int height, int bpp)
		{
			return 2 * Utility._WeightedAbs(bpp, bitDepth, false) + Utility._WeightedAbs(frame.PixelWidth, width, true) + Utility._WeightedAbs(frame.PixelHeight, height, true);
		}

		// Token: 0x06000232 RID: 562 RVA: 0x00005ABC File Offset: 0x00003CBC
		private static int _WeightedAbs(int valueHave, int valueWant, bool fPunish)
		{
			int num = valueHave - valueWant;
			if (num < 0)
			{
				num = (fPunish ? -2 : -1) * num;
			}
			return num;
		}

		// Token: 0x06000233 RID: 563 RVA: 0x00005AE0 File Offset: 0x00003CE0
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

		// Token: 0x06000234 RID: 564 RVA: 0x00005BA4 File Offset: 0x00003DA4
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

		// Token: 0x06000235 RID: 565 RVA: 0x00005BF8 File Offset: 0x00003DF8
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static void SafeDeleteFile(string path)
		{
			if (!string.IsNullOrEmpty(path))
			{
				File.Delete(path);
			}
		}

		// Token: 0x06000236 RID: 566 RVA: 0x00005C08 File Offset: 0x00003E08
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

		// Token: 0x06000237 RID: 567 RVA: 0x00005C3C File Offset: 0x00003E3C
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

		// Token: 0x06000238 RID: 568 RVA: 0x00005C70 File Offset: 0x00003E70
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

		// Token: 0x06000239 RID: 569 RVA: 0x00005CA0 File Offset: 0x00003EA0
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

		// Token: 0x0600023A RID: 570 RVA: 0x00005CCC File Offset: 0x00003ECC
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

		// Token: 0x0600023B RID: 571 RVA: 0x00005D00 File Offset: 0x00003F00
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

		// Token: 0x0600023C RID: 572 RVA: 0x00005D34 File Offset: 0x00003F34
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

		// Token: 0x0600023D RID: 573 RVA: 0x00005D68 File Offset: 0x00003F68
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

		// Token: 0x0600023E RID: 574 RVA: 0x00005D98 File Offset: 0x00003F98
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

		// Token: 0x0600023F RID: 575 RVA: 0x00005DFC File Offset: 0x00003FFC
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

		// Token: 0x06000240 RID: 576 RVA: 0x00005E84 File Offset: 0x00004084
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

		// Token: 0x06000241 RID: 577 RVA: 0x00005EE8 File Offset: 0x000040E8
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

		// Token: 0x06000242 RID: 578 RVA: 0x00005F64 File Offset: 0x00004164
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static void EnsureDirectory(string path)
		{
			if (!Directory.Exists(Path.GetDirectoryName(path)))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(path));
			}
		}

		// Token: 0x06000243 RID: 579 RVA: 0x00005F80 File Offset: 0x00004180
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static bool MemCmp(byte[] left, byte[] right, int cb)
		{
			GCHandle gchandle = GCHandle.Alloc(left, GCHandleType.Pinned);
			IntPtr left2 = gchandle.AddrOfPinnedObject();
			GCHandle gchandle2 = GCHandle.Alloc(right, GCHandleType.Pinned);
			IntPtr right2 = gchandle2.AddrOfPinnedObject();
			bool result = Utility._MemCmp(left2, right2, (long)cb);
			gchandle.Free();
			gchandle2.Free();
			return result;
		}

		// Token: 0x06000244 RID: 580 RVA: 0x00005FC8 File Offset: 0x000041C8
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
								urlDecoder.AddChar((char)(num << 12 | num2 << 8 | num3 << 4 | num4));
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
								urlDecoder.AddByte((byte)(num5 << 4 | num6));
								i += 2;
								goto IL_12D;
							}
						}
					}
					if ((c & 'ﾀ') == '\0')
					{
						urlDecoder.AddByte((byte)c);
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

		// Token: 0x06000245 RID: 581 RVA: 0x00006114 File Offset: 0x00004314
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
						array3[num2++] = Utility._IntToHex((int)(b2 & 15));
					}
				}
				array = array3;
			}
			return Encoding.ASCII.GetString(array);
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000620C File Offset: 0x0000440C
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

		// Token: 0x06000247 RID: 583 RVA: 0x0000625B File Offset: 0x0000445B
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		private static bool _IsAsciiAlphaNumeric(byte b)
		{
			return (b >= 97 && b <= 122) || (b >= 65 && b <= 90) || (b >= 48 && b <= 57);
		}

		// Token: 0x06000248 RID: 584 RVA: 0x00006282 File Offset: 0x00004482
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		private static byte _IntToHex(int n)
		{
			if (n <= 9)
			{
				return (byte)(n + 48);
			}
			return (byte)(n - 10 + 65);
		}

		// Token: 0x06000249 RID: 585 RVA: 0x00006297 File Offset: 0x00004497
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		private static int _HexToInt(char h)
		{
			if (h >= '0' && h <= '9')
			{
				return (int)(h - '0');
			}
			if (h >= 'a' && h <= 'f')
			{
				return (int)(h - 'a' + '\n');
			}
			if (h >= 'A' && h <= 'F')
			{
				return (int)(h - 'A' + '\n');
			}
			return -1;
		}

		// Token: 0x0600024A RID: 586 RVA: 0x000062D0 File Offset: 0x000044D0
		public static void AddDependencyPropertyChangeListener(object component, DependencyProperty property, EventHandler listener)
		{
			if (component == null)
			{
				return;
			}
			DependencyPropertyDescriptor dependencyPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(property, component.GetType());
			dependencyPropertyDescriptor.AddValueChanged(component, listener);
		}

		// Token: 0x0600024B RID: 587 RVA: 0x000062F8 File Offset: 0x000044F8
		public static void RemoveDependencyPropertyChangeListener(object component, DependencyProperty property, EventHandler listener)
		{
			if (component == null)
			{
				return;
			}
			DependencyPropertyDescriptor dependencyPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(property, component.GetType());
			dependencyPropertyDescriptor.RemoveValueChanged(component, listener);
		}

		// Token: 0x0600024C RID: 588 RVA: 0x00006320 File Offset: 0x00004520
		public static bool IsThicknessNonNegative(Thickness thickness)
		{
			return Utility.IsDoubleFiniteAndNonNegative(thickness.Top) && Utility.IsDoubleFiniteAndNonNegative(thickness.Left) && Utility.IsDoubleFiniteAndNonNegative(thickness.Bottom) && Utility.IsDoubleFiniteAndNonNegative(thickness.Right);
		}

		// Token: 0x0600024D RID: 589 RVA: 0x00006370 File Offset: 0x00004570
		public static bool IsCornerRadiusValid(CornerRadius cornerRadius)
		{
			return Utility.IsDoubleFiniteAndNonNegative(cornerRadius.TopLeft) && Utility.IsDoubleFiniteAndNonNegative(cornerRadius.TopRight) && Utility.IsDoubleFiniteAndNonNegative(cornerRadius.BottomLeft) && Utility.IsDoubleFiniteAndNonNegative(cornerRadius.BottomRight);
		}

		// Token: 0x0600024E RID: 590 RVA: 0x000063BE File Offset: 0x000045BE
		public static bool IsDoubleFiniteAndNonNegative(double d)
		{
			return !double.IsNaN(d) && !double.IsInfinity(d) && d >= 0.0;
		}

		// Token: 0x0400059A RID: 1434
		private static readonly Version _osVersion = Environment.OSVersion.Version;

		// Token: 0x0400059B RID: 1435
		private static readonly Version _presentationFrameworkVersion = Assembly.GetAssembly(typeof(Window)).GetName().Version;

		// Token: 0x0400059C RID: 1436
		private static int s_bitDepth;

		// Token: 0x0200009B RID: 155
		private class _UrlDecoder
		{
			// Token: 0x06000250 RID: 592 RVA: 0x0000640E File Offset: 0x0000460E
			[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
			public _UrlDecoder(int size, Encoding encoding)
			{
				this._encoding = encoding;
				this._charBuffer = new char[size];
				this._byteBuffer = new byte[size];
			}

			// Token: 0x06000251 RID: 593 RVA: 0x00006438 File Offset: 0x00004638
			[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
			public void AddByte(byte b)
			{
				this._byteBuffer[this._byteCount++] = b;
			}

			// Token: 0x06000252 RID: 594 RVA: 0x00006460 File Offset: 0x00004660
			[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
			public void AddChar(char ch)
			{
				this._FlushBytes();
				this._charBuffer[this._charCount++] = ch;
			}

			// Token: 0x06000253 RID: 595 RVA: 0x0000648C File Offset: 0x0000468C
			[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
			private void _FlushBytes()
			{
				if (this._byteCount > 0)
				{
					this._charCount += this._encoding.GetChars(this._byteBuffer, 0, this._byteCount, this._charBuffer, this._charCount);
					this._byteCount = 0;
				}
			}

			// Token: 0x06000254 RID: 596 RVA: 0x000064DA File Offset: 0x000046DA
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

			// Token: 0x0400059D RID: 1437
			private readonly Encoding _encoding;

			// Token: 0x0400059E RID: 1438
			private readonly char[] _charBuffer;

			// Token: 0x0400059F RID: 1439
			private readonly byte[] _byteBuffer;

			// Token: 0x040005A0 RID: 1440
			private int _byteCount;

			// Token: 0x040005A1 RID: 1441
			private int _charCount;
		}
	}
}
