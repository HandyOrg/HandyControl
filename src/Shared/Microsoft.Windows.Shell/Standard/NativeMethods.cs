using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace Standard
{
	// Token: 0x02000075 RID: 117
	internal static class NativeMethods
	{
		// Token: 0x060000F6 RID: 246
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("user32.dll", EntryPoint = "AdjustWindowRectEx", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _AdjustWindowRectEx(ref RECT lpRect, WS dwStyle, [MarshalAs(UnmanagedType.Bool)] bool bMenu, WS_EX dwExStyle);

		// Token: 0x060000F7 RID: 247 RVA: 0x00004A9E File Offset: 0x00002C9E
		public static RECT AdjustWindowRectEx(RECT lpRect, WS dwStyle, bool bMenu, WS_EX dwExStyle)
		{
			if (!NativeMethods._AdjustWindowRectEx(ref lpRect, dwStyle, bMenu, dwExStyle))
			{
				HRESULT.ThrowLastError();
			}
			return lpRect;
		}

		// Token: 0x060000F8 RID: 248
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("user32.dll", EntryPoint = "ChangeWindowMessageFilter", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _ChangeWindowMessageFilter(WM message, MSGFLT dwFlag);

		// Token: 0x060000F9 RID: 249
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("user32.dll", EntryPoint = "ChangeWindowMessageFilterEx", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _ChangeWindowMessageFilterEx(IntPtr hwnd, WM message, MSGFLT action, [In] [Out] [Optional] ref CHANGEFILTERSTRUCT pChangeFilterStruct);

		// Token: 0x060000FA RID: 250 RVA: 0x00004AB4 File Offset: 0x00002CB4
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static HRESULT ChangeWindowMessageFilterEx(IntPtr hwnd, WM message, MSGFLT action, out MSGFLTINFO filterInfo)
		{
			filterInfo = MSGFLTINFO.NONE;
			if (!Utility.IsOSVistaOrNewer)
			{
				return HRESULT.S_FALSE;
			}
			if (!Utility.IsOSWindows7OrNewer)
			{
				if (!NativeMethods._ChangeWindowMessageFilter(message, action))
				{
					return (HRESULT)Win32Error.GetLastError();
				}
				return HRESULT.S_OK;
			}
			else
			{
				CHANGEFILTERSTRUCT changefilterstruct = new CHANGEFILTERSTRUCT
				{
					cbSize = (uint)Marshal.SizeOf(typeof(CHANGEFILTERSTRUCT))
				};
				if (!NativeMethods._ChangeWindowMessageFilterEx(hwnd, message, action, ref changefilterstruct))
				{
					return (HRESULT)Win32Error.GetLastError();
				}
				filterInfo = changefilterstruct.ExtStatus;
				return HRESULT.S_OK;
			}
		}

		// Token: 0x060000FB RID: 251
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("gdi32.dll")]
		public static extern CombineRgnResult CombineRgn(IntPtr hrgnDest, IntPtr hrgnSrc1, IntPtr hrgnSrc2, RGN fnCombineMode);

		// Token: 0x060000FC RID: 252
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("shell32.dll", CharSet = CharSet.Unicode, EntryPoint = "CommandLineToArgvW")]
		private static extern IntPtr _CommandLineToArgvW([MarshalAs(UnmanagedType.LPWStr)] string cmdLine, out int numArgs);

		// Token: 0x060000FD RID: 253 RVA: 0x00004B3C File Offset: 0x00002D3C
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static string[] CommandLineToArgvW(string cmdLine)
		{
			IntPtr intPtr = IntPtr.Zero;
			string[] result;
			try
			{
				int num = 0;
				intPtr = NativeMethods._CommandLineToArgvW(cmdLine, out num);
				if (intPtr == IntPtr.Zero)
				{
					throw new Win32Exception();
				}
				string[] array = new string[num];
				for (int i = 0; i < num; i++)
				{
					IntPtr ptr = Marshal.ReadIntPtr(intPtr, i * Marshal.SizeOf(typeof(IntPtr)));
					array[i] = Marshal.PtrToStringUni(ptr);
				}
				result = array;
			}
			finally
			{
				NativeMethods._LocalFree(intPtr);
			}
			return result;
		}

		// Token: 0x060000FE RID: 254
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("gdi32.dll", EntryPoint = "CreateDIBSection", SetLastError = true)]
		private static extern SafeHBITMAP _CreateDIBSection(SafeDC hdc, [In] ref BITMAPINFO bitmapInfo, int iUsage, out IntPtr ppvBits, IntPtr hSection, int dwOffset);

		// Token: 0x060000FF RID: 255
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("gdi32.dll", EntryPoint = "CreateDIBSection", SetLastError = true)]
		private static extern SafeHBITMAP _CreateDIBSectionIntPtr(IntPtr hdc, [In] ref BITMAPINFO bitmapInfo, int iUsage, out IntPtr ppvBits, IntPtr hSection, int dwOffset);

		// Token: 0x06000100 RID: 256 RVA: 0x00004BC4 File Offset: 0x00002DC4
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static SafeHBITMAP CreateDIBSection(SafeDC hdc, ref BITMAPINFO bitmapInfo, out IntPtr ppvBits, IntPtr hSection, int dwOffset)
		{
			SafeHBITMAP safeHBITMAP;
			if (hdc == null)
			{
				safeHBITMAP = NativeMethods._CreateDIBSectionIntPtr(IntPtr.Zero, ref bitmapInfo, 0, out ppvBits, hSection, dwOffset);
			}
			else
			{
				safeHBITMAP = NativeMethods._CreateDIBSection(hdc, ref bitmapInfo, 0, out ppvBits, hSection, dwOffset);
			}
			if (safeHBITMAP.IsInvalid)
			{
				HRESULT.ThrowLastError();
			}
			return safeHBITMAP;
		}

		// Token: 0x06000101 RID: 257
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("gdi32.dll", EntryPoint = "CreateRoundRectRgn", SetLastError = true)]
		private static extern IntPtr _CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

		// Token: 0x06000102 RID: 258 RVA: 0x00004C04 File Offset: 0x00002E04
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse)
		{
			IntPtr intPtr = NativeMethods._CreateRoundRectRgn(nLeftRect, nTopRect, nRightRect, nBottomRect, nWidthEllipse, nHeightEllipse);
			if (IntPtr.Zero == intPtr)
			{
				throw new Win32Exception();
			}
			return intPtr;
		}

		// Token: 0x06000103 RID: 259
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("gdi32.dll", EntryPoint = "CreateRectRgn", SetLastError = true)]
		private static extern IntPtr _CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

		// Token: 0x06000104 RID: 260 RVA: 0x00004C34 File Offset: 0x00002E34
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static IntPtr CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect)
		{
			IntPtr intPtr = NativeMethods._CreateRectRgn(nLeftRect, nTopRect, nRightRect, nBottomRect);
			if (IntPtr.Zero == intPtr)
			{
				throw new Win32Exception();
			}
			return intPtr;
		}

		// Token: 0x06000105 RID: 261
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("gdi32.dll", EntryPoint = "CreateRectRgnIndirect", SetLastError = true)]
		private static extern IntPtr _CreateRectRgnIndirect([In] ref RECT lprc);

		// Token: 0x06000106 RID: 262 RVA: 0x00004C60 File Offset: 0x00002E60
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static IntPtr CreateRectRgnIndirect(RECT lprc)
		{
			IntPtr intPtr = NativeMethods._CreateRectRgnIndirect(ref lprc);
			if (IntPtr.Zero == intPtr)
			{
				throw new Win32Exception();
			}
			return intPtr;
		}

		// Token: 0x06000107 RID: 263
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateSolidBrush(int crColor);

		// Token: 0x06000108 RID: 264
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "CreateWindowExW", SetLastError = true)]
		private static extern IntPtr _CreateWindowEx(WS_EX dwExStyle, [MarshalAs(UnmanagedType.LPWStr)] string lpClassName, [MarshalAs(UnmanagedType.LPWStr)] string lpWindowName, WS dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

		// Token: 0x06000109 RID: 265 RVA: 0x00004C8C File Offset: 0x00002E8C
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static IntPtr CreateWindowEx(WS_EX dwExStyle, string lpClassName, string lpWindowName, WS dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam)
		{
			IntPtr intPtr = NativeMethods._CreateWindowEx(dwExStyle, lpClassName, lpWindowName, dwStyle, x, y, nWidth, nHeight, hWndParent, hMenu, hInstance, lpParam);
			if (IntPtr.Zero == intPtr)
			{
				HRESULT.ThrowLastError();
			}
			return intPtr;
		}

		// Token: 0x0600010A RID: 266
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "DefWindowProcW")]
		public static extern IntPtr DefWindowProc(IntPtr hWnd, WM Msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x0600010B RID: 267
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("gdi32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DeleteObject(IntPtr hObject);

		// Token: 0x0600010C RID: 268
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DestroyIcon(IntPtr handle);

		// Token: 0x0600010D RID: 269
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DestroyWindow(IntPtr hwnd);

		// Token: 0x0600010E RID: 270
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool IsWindow(IntPtr hwnd);

		// Token: 0x0600010F RID: 271
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("dwmapi.dll", PreserveSig = false)]
		public static extern void DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS pMarInset);

		// Token: 0x06000110 RID: 272
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("dwmapi.dll", EntryPoint = "DwmIsCompositionEnabled", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _DwmIsCompositionEnabled();

		// Token: 0x06000111 RID: 273
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("dwmapi.dll", EntryPoint = "DwmGetColorizationColor")]
		private static extern HRESULT _DwmGetColorizationColor(out uint pcrColorization, [MarshalAs(UnmanagedType.Bool)] out bool pfOpaqueBlend);

		// Token: 0x06000112 RID: 274 RVA: 0x00004CC8 File Offset: 0x00002EC8
		public static bool DwmGetColorizationColor(out uint pcrColorization, out bool pfOpaqueBlend)
		{
			if (Utility.IsOSVistaOrNewer && NativeMethods.IsThemeActive() && NativeMethods._DwmGetColorizationColor(out pcrColorization, out pfOpaqueBlend).Succeeded)
			{
				return true;
			}
			pcrColorization = 4278190080U;
			pfOpaqueBlend = true;
			return false;
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00004D01 File Offset: 0x00002F01
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static bool DwmIsCompositionEnabled()
		{
			return Utility.IsOSVistaOrNewer && NativeMethods._DwmIsCompositionEnabled();
		}

		// Token: 0x06000114 RID: 276
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("dwmapi.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DwmDefWindowProc(IntPtr hwnd, WM msg, IntPtr wParam, IntPtr lParam, out IntPtr plResult);

		// Token: 0x06000115 RID: 277
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("dwmapi.dll", EntryPoint = "DwmSetWindowAttribute")]
		private static extern void _DwmSetWindowAttribute(IntPtr hwnd, DWMWA dwAttribute, ref int pvAttribute, int cbAttribute);

		// Token: 0x06000116 RID: 278 RVA: 0x00004D14 File Offset: 0x00002F14
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static void DwmSetWindowAttributeFlip3DPolicy(IntPtr hwnd, DWMFLIP3D flip3dPolicy)
		{
			int num = (int)flip3dPolicy;
			NativeMethods._DwmSetWindowAttribute(hwnd, DWMWA.FLIP3D_POLICY, ref num, 4);
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00004D30 File Offset: 0x00002F30
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static void DwmSetWindowAttributeDisallowPeek(IntPtr hwnd, bool disallowPeek)
		{
			int num = disallowPeek ? 1 : 0;
			NativeMethods._DwmSetWindowAttribute(hwnd, DWMWA.DISALLOW_PEEK, ref num, 4);
		}

		// Token: 0x06000118 RID: 280
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("user32.dll", EntryPoint = "EnableMenuItem")]
		private static extern int _EnableMenuItem(IntPtr hMenu, SC uIDEnableItem, MF uEnable);

		// Token: 0x06000119 RID: 281 RVA: 0x00004D50 File Offset: 0x00002F50
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static MF EnableMenuItem(IntPtr hMenu, SC uIDEnableItem, MF uEnable)
		{
			return (MF)NativeMethods._EnableMenuItem(hMenu, uIDEnableItem, uEnable);
		}

		// Token: 0x0600011A RID: 282
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("user32.dll", EntryPoint = "RemoveMenu", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);

		// Token: 0x0600011B RID: 283 RVA: 0x00004D67 File Offset: 0x00002F67
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static void RemoveMenu(IntPtr hMenu, SC uPosition, MF uFlags)
		{
			if (!NativeMethods._RemoveMenu(hMenu, (uint)uPosition, (uint)uFlags))
			{
				throw new Win32Exception();
			}
		}

		// Token: 0x0600011C RID: 284
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("user32.dll", EntryPoint = "DrawMenuBar", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _DrawMenuBar(IntPtr hWnd);

		// Token: 0x0600011D RID: 285 RVA: 0x00004D79 File Offset: 0x00002F79
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static void DrawMenuBar(IntPtr hWnd)
		{
			if (!NativeMethods._DrawMenuBar(hWnd))
			{
				throw new Win32Exception();
			}
		}

		// Token: 0x0600011E RID: 286
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("kernel32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool FindClose(IntPtr handle);

		// Token: 0x0600011F RID: 287
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern SafeFindHandle FindFirstFileW(string lpFileName, [MarshalAs(UnmanagedType.LPStruct)] [In] [Out] WIN32_FIND_DATAW lpFindFileData);

		// Token: 0x06000120 RID: 288
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool FindNextFileW(SafeFindHandle hndFindFile, [MarshalAs(UnmanagedType.LPStruct)] [In] [Out] WIN32_FIND_DATAW lpFindFileData);

		// Token: 0x06000121 RID: 289
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("user32.dll", EntryPoint = "GetClientRect", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _GetClientRect(IntPtr hwnd, out RECT lpRect);

		// Token: 0x06000122 RID: 290 RVA: 0x00004D8C File Offset: 0x00002F8C
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static RECT GetClientRect(IntPtr hwnd)
		{
			RECT result;
			if (!NativeMethods._GetClientRect(hwnd, out result))
			{
				HRESULT.ThrowLastError();
			}
			return result;
		}

		// Token: 0x06000123 RID: 291
		[DllImport("uxtheme.dll", CharSet = CharSet.Unicode, EntryPoint = "GetCurrentThemeName")]
		private static extern HRESULT _GetCurrentThemeName(StringBuilder pszThemeFileName, int dwMaxNameChars, StringBuilder pszColorBuff, int cchMaxColorChars, StringBuilder pszSizeBuff, int cchMaxSizeChars);

		// Token: 0x06000124 RID: 292 RVA: 0x00004DAC File Offset: 0x00002FAC
		public static void GetCurrentThemeName(out string themeFileName, out string color, out string size)
		{
			StringBuilder stringBuilder = new StringBuilder(260);
			StringBuilder stringBuilder2 = new StringBuilder(260);
			StringBuilder stringBuilder3 = new StringBuilder(260);
			NativeMethods._GetCurrentThemeName(stringBuilder, stringBuilder.Capacity, stringBuilder2, stringBuilder2.Capacity, stringBuilder3, stringBuilder3.Capacity).ThrowIfFailed();
			themeFileName = stringBuilder.ToString();
			color = stringBuilder2.ToString();
			size = stringBuilder3.ToString();
		}

		// Token: 0x06000125 RID: 293
		[DllImport("uxtheme.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool IsThemeActive();

		// Token: 0x06000126 RID: 294 RVA: 0x00004E14 File Offset: 0x00003014
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[Obsolete("Use SafeDC.GetDC instead.", true)]
		public static void GetDC()
		{
		}

		// Token: 0x06000127 RID: 295
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("gdi32.dll")]
		public static extern int GetDeviceCaps(SafeDC hdc, DeviceCap nIndex);

		// Token: 0x06000128 RID: 296
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, EntryPoint = "GetModuleFileName", SetLastError = true)]
		private static extern int _GetModuleFileName(IntPtr hModule, StringBuilder lpFilename, int nSize);

		// Token: 0x06000129 RID: 297 RVA: 0x00004E18 File Offset: 0x00003018
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static string GetModuleFileName(IntPtr hModule)
		{
			StringBuilder stringBuilder = new StringBuilder(260);
			for (;;)
			{
				int num = NativeMethods._GetModuleFileName(hModule, stringBuilder, stringBuilder.Capacity);
				if (num == 0)
				{
					HRESULT.ThrowLastError();
				}
				if (num != stringBuilder.Capacity)
				{
					break;
				}
				stringBuilder.EnsureCapacity(stringBuilder.Capacity * 2);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600012A RID: 298
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, EntryPoint = "GetModuleHandleW", SetLastError = true)]
		private static extern IntPtr _GetModuleHandle([MarshalAs(UnmanagedType.LPWStr)] string lpModuleName);

		// Token: 0x0600012B RID: 299 RVA: 0x00004E68 File Offset: 0x00003068
		public static IntPtr GetModuleHandle(string lpModuleName)
		{
			IntPtr intPtr = NativeMethods._GetModuleHandle(lpModuleName);
			if (intPtr == IntPtr.Zero)
			{
				HRESULT.ThrowLastError();
			}
			return intPtr;
		}

		// Token: 0x0600012C RID: 300
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("user32.dll", EntryPoint = "GetMonitorInfo", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _GetMonitorInfo(IntPtr hMonitor, [In] [Out] MONITORINFO lpmi);

		// Token: 0x0600012D RID: 301 RVA: 0x00004E90 File Offset: 0x00003090
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static MONITORINFO GetMonitorInfo(IntPtr hMonitor)
		{
			MONITORINFO monitorinfo = new MONITORINFO();
			if (!NativeMethods._GetMonitorInfo(hMonitor, monitorinfo))
			{
				throw new Win32Exception();
			}
			return monitorinfo;
		}

		// Token: 0x0600012E RID: 302
		[DllImport("gdi32.dll", EntryPoint = "GetStockObject", SetLastError = true)]
		private static extern IntPtr _GetStockObject(StockObject fnObject);

		// Token: 0x0600012F RID: 303 RVA: 0x00004EB4 File Offset: 0x000030B4
		public static IntPtr GetStockObject(StockObject fnObject)
		{
			return NativeMethods._GetStockObject(fnObject);
		}

		// Token: 0x06000130 RID: 304
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("user32.dll")]
		public static extern IntPtr GetSystemMenu(IntPtr hWnd, [MarshalAs(UnmanagedType.Bool)] bool bRevert);

		// Token: 0x06000131 RID: 305
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("user32.dll")]
		public static extern int GetSystemMetrics(SM nIndex);

		// Token: 0x06000132 RID: 306 RVA: 0x00004ECC File Offset: 0x000030CC
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static IntPtr GetWindowLongPtr(IntPtr hwnd, GWL nIndex)
		{
			IntPtr intPtr = IntPtr.Zero;
			if (8 == IntPtr.Size)
			{
				intPtr = NativeMethods.GetWindowLongPtr64(hwnd, nIndex);
			}
			else
			{
				intPtr = new IntPtr(NativeMethods.GetWindowLongPtr32(hwnd, nIndex));
			}
			if (IntPtr.Zero == intPtr)
			{
				throw new Win32Exception();
			}
			return intPtr;
		}

		// Token: 0x06000133 RID: 307
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("uxtheme.dll", PreserveSig = false)]
		public static extern void SetWindowThemeAttribute([In] IntPtr hwnd, [In] WINDOWTHEMEATTRIBUTETYPE eAttribute, [In] ref WTA_OPTIONS pvAttribute, [In] uint cbAttribute);

		// Token: 0x06000134 RID: 308
		[SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("user32.dll", EntryPoint = "GetWindowLong", SetLastError = true)]
		private static extern int GetWindowLongPtr32(IntPtr hWnd, GWL nIndex);

		// Token: 0x06000135 RID: 309
		[SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("user32.dll", EntryPoint = "GetWindowLongPtr", SetLastError = true)]
		private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, GWL nIndex);

		// Token: 0x06000136 RID: 310
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool GetWindowPlacement(IntPtr hwnd, WINDOWPLACEMENT lpwndpl);

		// Token: 0x06000137 RID: 311 RVA: 0x00004F14 File Offset: 0x00003114
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static WINDOWPLACEMENT GetWindowPlacement(IntPtr hwnd)
		{
			WINDOWPLACEMENT windowplacement = new WINDOWPLACEMENT();
			if (NativeMethods.GetWindowPlacement(hwnd, windowplacement))
			{
				return windowplacement;
			}
			throw new Win32Exception();
		}

		// Token: 0x06000138 RID: 312
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("user32.dll", EntryPoint = "GetWindowRect", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _GetWindowRect(IntPtr hWnd, out RECT lpRect);

		// Token: 0x06000139 RID: 313 RVA: 0x00004F38 File Offset: 0x00003138
		public static RECT GetWindowRect(IntPtr hwnd)
		{
			RECT result;
			if (!NativeMethods._GetWindowRect(hwnd, out result))
			{
				HRESULT.ThrowLastError();
			}
			return result;
		}

		// Token: 0x0600013A RID: 314
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("gdiplus.dll")]
		public static extern Status GdipCreateBitmapFromStream(IStream stream, out IntPtr bitmap);

		// Token: 0x0600013B RID: 315
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("gdiplus.dll")]
		public static extern Status GdipCreateHBITMAPFromBitmap(IntPtr bitmap, out IntPtr hbmReturn, int background);

		// Token: 0x0600013C RID: 316
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("gdiplus.dll")]
		public static extern Status GdipCreateHICONFromBitmap(IntPtr bitmap, out IntPtr hbmReturn);

		// Token: 0x0600013D RID: 317
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("gdiplus.dll")]
		public static extern Status GdipDisposeImage(IntPtr image);

		// Token: 0x0600013E RID: 318
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("gdiplus.dll")]
		public static extern Status GdipImageForceValidation(IntPtr image);

		// Token: 0x0600013F RID: 319
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("gdiplus.dll")]
		public static extern Status GdiplusStartup(out IntPtr token, StartupInput input, out StartupOutput output);

		// Token: 0x06000140 RID: 320
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("gdiplus.dll")]
		public static extern Status GdiplusShutdown(IntPtr token);

		// Token: 0x06000141 RID: 321
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool IsWindowVisible(IntPtr hwnd);

		// Token: 0x06000142 RID: 322
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("kernel32.dll", EntryPoint = "LocalFree", SetLastError = true)]
		private static extern IntPtr _LocalFree(IntPtr hMem);

		// Token: 0x06000143 RID: 323
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("user32.dll")]
		public static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

		// Token: 0x06000144 RID: 324
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("user32.dll", EntryPoint = "PostMessage", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _PostMessage(IntPtr hWnd, WM Msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x06000145 RID: 325 RVA: 0x00004F55 File Offset: 0x00003155
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static void PostMessage(IntPtr hWnd, WM Msg, IntPtr wParam, IntPtr lParam)
		{
			if (!NativeMethods._PostMessage(hWnd, Msg, wParam, lParam))
			{
				throw new Win32Exception();
			}
		}

		// Token: 0x06000146 RID: 326
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("user32.dll", EntryPoint = "RegisterClassExW", SetLastError = true)]
		private static extern short _RegisterClassEx([In] ref WNDCLASSEX lpwcx);

		// Token: 0x06000147 RID: 327 RVA: 0x00004F68 File Offset: 0x00003168
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static short RegisterClassEx(ref WNDCLASSEX lpwcx)
		{
			short num = NativeMethods._RegisterClassEx(ref lpwcx);
			if (num == 0)
			{
				HRESULT.ThrowLastError();
			}
			return num;
		}

		// Token: 0x06000148 RID: 328
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "RegisterWindowMessage", SetLastError = true)]
		private static extern uint _RegisterWindowMessage([MarshalAs(UnmanagedType.LPWStr)] string lpString);

		// Token: 0x06000149 RID: 329 RVA: 0x00004F88 File Offset: 0x00003188
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static WM RegisterWindowMessage(string lpString)
		{
			uint num = NativeMethods._RegisterWindowMessage(lpString);
			if (num == 0U)
			{
				HRESULT.ThrowLastError();
			}
			return (WM)num;
		}

		// Token: 0x0600014A RID: 330
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("user32.dll", EntryPoint = "SetActiveWindow", SetLastError = true)]
		private static extern IntPtr _SetActiveWindow(IntPtr hWnd);

		// Token: 0x0600014B RID: 331 RVA: 0x00004FA8 File Offset: 0x000031A8
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static IntPtr SetActiveWindow(IntPtr hwnd)
		{
			Verify.IsNotDefault<IntPtr>(hwnd, "hwnd");
			IntPtr intPtr = NativeMethods._SetActiveWindow(hwnd);
			if (intPtr == IntPtr.Zero)
			{
				HRESULT.ThrowLastError();
			}
			return intPtr;
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00004FDA File Offset: 0x000031DA
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static IntPtr SetClassLongPtr(IntPtr hwnd, GCLP nIndex, IntPtr dwNewLong)
		{
			if (8 == IntPtr.Size)
			{
				return NativeMethods.SetClassLongPtr64(hwnd, nIndex, dwNewLong);
			}
			return new IntPtr(NativeMethods.SetClassLongPtr32(hwnd, nIndex, dwNewLong.ToInt32()));
		}

		// Token: 0x0600014D RID: 333
		[SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("user32.dll", EntryPoint = "SetClassLong", SetLastError = true)]
		private static extern int SetClassLongPtr32(IntPtr hWnd, GCLP nIndex, int dwNewLong);

		// Token: 0x0600014E RID: 334
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
		[DllImport("user32.dll", EntryPoint = "SetClassLongPtr", SetLastError = true)]
		private static extern IntPtr SetClassLongPtr64(IntPtr hWnd, GCLP nIndex, IntPtr dwNewLong);

		// Token: 0x0600014F RID: 335
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern ErrorModes SetErrorMode(ErrorModes newMode);

		// Token: 0x06000150 RID: 336
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _SetProcessWorkingSetSize(IntPtr hProcess, IntPtr dwMinimiumWorkingSetSize, IntPtr dwMaximumWorkingSetSize);

		// Token: 0x06000151 RID: 337 RVA: 0x00005000 File Offset: 0x00003200
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static void SetProcessWorkingSetSize(IntPtr hProcess, int dwMinimumWorkingSetSize, int dwMaximumWorkingSetSize)
		{
			if (!NativeMethods._SetProcessWorkingSetSize(hProcess, new IntPtr(dwMinimumWorkingSetSize), new IntPtr(dwMaximumWorkingSetSize)))
			{
				throw new Win32Exception();
			}
		}

		// Token: 0x06000152 RID: 338 RVA: 0x0000501C File Offset: 0x0000321C
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static IntPtr SetWindowLongPtr(IntPtr hwnd, GWL nIndex, IntPtr dwNewLong)
		{
			if (8 == IntPtr.Size)
			{
				return NativeMethods.SetWindowLongPtr64(hwnd, nIndex, dwNewLong);
			}
			return new IntPtr(NativeMethods.SetWindowLongPtr32(hwnd, nIndex, dwNewLong.ToInt32()));
		}

		// Token: 0x06000153 RID: 339
		[SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
		private static extern int SetWindowLongPtr32(IntPtr hWnd, GWL nIndex, int dwNewLong);

		// Token: 0x06000154 RID: 340
		[SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
		private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, GWL nIndex, IntPtr dwNewLong);

		// Token: 0x06000155 RID: 341
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("user32.dll", EntryPoint = "SetWindowRgn", SetLastError = true)]
		private static extern int _SetWindowRgn(IntPtr hWnd, IntPtr hRgn, [MarshalAs(UnmanagedType.Bool)] bool bRedraw);

		// Token: 0x06000156 RID: 342 RVA: 0x00005044 File Offset: 0x00003244
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static void SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw)
		{
			if (NativeMethods._SetWindowRgn(hWnd, hRgn, bRedraw) == 0)
			{
				throw new Win32Exception();
			}
		}

		// Token: 0x06000157 RID: 343
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("user32.dll", EntryPoint = "SetWindowPos", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, SWP uFlags);

		// Token: 0x06000158 RID: 344 RVA: 0x00005063 File Offset: 0x00003263
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, SWP uFlags)
		{
			return NativeMethods._SetWindowPos(hWnd, hWndInsertAfter, x, y, cx, cy, uFlags);
		}

		// Token: 0x06000159 RID: 345
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("shell32.dll")]
		public static extern Win32Error SHFileOperation(ref SHFILEOPSTRUCT lpFileOp);

		// Token: 0x0600015A RID: 346
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ShowWindow(IntPtr hwnd, SW nCmdShow);

		// Token: 0x0600015B RID: 347
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("user32.dll", EntryPoint = "SystemParametersInfoW", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _SystemParametersInfo_String(SPI uiAction, int uiParam, [MarshalAs(UnmanagedType.LPWStr)] string pvParam, SPIF fWinIni);

		// Token: 0x0600015C RID: 348
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "SystemParametersInfoW", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _SystemParametersInfo_NONCLIENTMETRICS(SPI uiAction, int uiParam, [In] [Out] ref NONCLIENTMETRICS pvParam, SPIF fWinIni);

		// Token: 0x0600015D RID: 349
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "SystemParametersInfoW", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _SystemParametersInfo_HIGHCONTRAST(SPI uiAction, int uiParam, [In] [Out] ref HIGHCONTRAST pvParam, SPIF fWinIni);

		// Token: 0x0600015E RID: 350 RVA: 0x00005079 File Offset: 0x00003279
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static void SystemParametersInfo(SPI uiAction, int uiParam, string pvParam, SPIF fWinIni)
		{
			if (!NativeMethods._SystemParametersInfo_String(uiAction, uiParam, pvParam, fWinIni))
			{
				HRESULT.ThrowLastError();
			}
		}

		// Token: 0x0600015F RID: 351 RVA: 0x0000508C File Offset: 0x0000328C
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static NONCLIENTMETRICS SystemParameterInfo_GetNONCLIENTMETRICS()
		{
			NONCLIENTMETRICS result = Utility.IsOSVistaOrNewer ? NONCLIENTMETRICS.VistaMetricsStruct : NONCLIENTMETRICS.XPMetricsStruct;
			if (!NativeMethods._SystemParametersInfo_NONCLIENTMETRICS(SPI.GETNONCLIENTMETRICS, result.cbSize, ref result, SPIF.None))
			{
				HRESULT.ThrowLastError();
			}
			return result;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x000050C8 File Offset: 0x000032C8
		[SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
		public static HIGHCONTRAST SystemParameterInfo_GetHIGHCONTRAST()
		{
			HIGHCONTRAST result = new HIGHCONTRAST
			{
				cbSize = Marshal.SizeOf(typeof(HIGHCONTRAST))
			};
			if (!NativeMethods._SystemParametersInfo_HIGHCONTRAST(SPI.GETHIGHCONTRAST, result.cbSize, ref result, SPIF.None))
			{
				HRESULT.ThrowLastError();
			}
			return result;
		}

		// Token: 0x06000161 RID: 353
		[DllImport("user32.dll")]
		public static extern uint TrackPopupMenuEx(IntPtr hmenu, uint fuFlags, int x, int y, IntPtr hwnd, IntPtr lptpm);

		// Token: 0x06000162 RID: 354
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("gdi32.dll", EntryPoint = "SelectObject", SetLastError = true)]
		private static extern IntPtr _SelectObject(SafeDC hdc, IntPtr hgdiobj);

		// Token: 0x06000163 RID: 355 RVA: 0x00005110 File Offset: 0x00003310
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static IntPtr SelectObject(SafeDC hdc, IntPtr hgdiobj)
		{
			IntPtr intPtr = NativeMethods._SelectObject(hdc, hgdiobj);
			if (intPtr == IntPtr.Zero)
			{
				HRESULT.ThrowLastError();
			}
			return intPtr;
		}

		// Token: 0x06000164 RID: 356
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("gdi32.dll", EntryPoint = "SelectObject", SetLastError = true)]
		private static extern IntPtr _SelectObjectSafeHBITMAP(SafeDC hdc, SafeHBITMAP hgdiobj);

		// Token: 0x06000165 RID: 357 RVA: 0x00005138 File Offset: 0x00003338
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static IntPtr SelectObject(SafeDC hdc, SafeHBITMAP hgdiobj)
		{
			IntPtr intPtr = NativeMethods._SelectObjectSafeHBITMAP(hdc, hgdiobj);
			if (intPtr == IntPtr.Zero)
			{
				HRESULT.ThrowLastError();
			}
			return intPtr;
		}

		// Token: 0x06000166 RID: 358
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("user32.dll", SetLastError = true)]
		public static extern int SendInput(int nInputs, ref INPUT pInputs, int cbSize);

		// Token: 0x06000167 RID: 359
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr SendMessage(IntPtr hWnd, WM Msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x06000168 RID: 360
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("user32.dll", EntryPoint = "UnregisterClass", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _UnregisterClassAtom(IntPtr lpClassName, IntPtr hInstance);

		// Token: 0x06000169 RID: 361
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "UnregisterClass", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _UnregisterClassName(string lpClassName, IntPtr hInstance);

		// Token: 0x0600016A RID: 362 RVA: 0x00005160 File Offset: 0x00003360
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static void UnregisterClass(short atom, IntPtr hinstance)
		{
			if (!NativeMethods._UnregisterClassAtom(new IntPtr((int)atom), hinstance))
			{
				HRESULT.ThrowLastError();
			}
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00005175 File Offset: 0x00003375
		public static void UnregisterClass(string lpClassName, IntPtr hInstance)
		{
			if (!NativeMethods._UnregisterClassName(lpClassName, hInstance))
			{
				HRESULT.ThrowLastError();
			}
		}

		// Token: 0x0600016C RID: 364
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("user32.dll", EntryPoint = "UpdateLayeredWindow", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _UpdateLayeredWindow(IntPtr hwnd, SafeDC hdcDst, [In] ref POINT pptDst, [In] ref SIZE psize, SafeDC hdcSrc, [In] ref POINT pptSrc, int crKey, ref BLENDFUNCTION pblend, ULW dwFlags);

		// Token: 0x0600016D RID: 365
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("user32.dll", EntryPoint = "UpdateLayeredWindow", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _UpdateLayeredWindowIntPtr(IntPtr hwnd, IntPtr hdcDst, IntPtr pptDst, IntPtr psize, IntPtr hdcSrc, IntPtr pptSrc, int crKey, ref BLENDFUNCTION pblend, ULW dwFlags);

		// Token: 0x0600016E RID: 366 RVA: 0x00005188 File Offset: 0x00003388
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static void UpdateLayeredWindow(IntPtr hwnd, SafeDC hdcDst, ref POINT pptDst, ref SIZE psize, SafeDC hdcSrc, ref POINT pptSrc, int crKey, ref BLENDFUNCTION pblend, ULW dwFlags)
		{
			if (!NativeMethods._UpdateLayeredWindow(hwnd, hdcDst, ref pptDst, ref psize, hdcSrc, ref pptSrc, crKey, ref pblend, dwFlags))
			{
				HRESULT.ThrowLastError();
			}
		}

		// Token: 0x0600016F RID: 367 RVA: 0x000051B0 File Offset: 0x000033B0
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static void UpdateLayeredWindow(IntPtr hwnd, int crKey, ref BLENDFUNCTION pblend, ULW dwFlags)
		{
			if (!NativeMethods._UpdateLayeredWindowIntPtr(hwnd, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, crKey, ref pblend, dwFlags))
			{
				HRESULT.ThrowLastError();
			}
		}

		// Token: 0x06000170 RID: 368
		[DllImport("shell32.dll", EntryPoint = "SHAddToRecentDocs")]
		private static extern void _SHAddToRecentDocs_String(SHARD uFlags, [MarshalAs(UnmanagedType.LPWStr)] string pv);

		// Token: 0x06000171 RID: 369
		[DllImport("shell32.dll", EntryPoint = "SHAddToRecentDocs")]
		private static extern void _SHAddToRecentDocs_ShellLink(SHARD uFlags, IShellLinkW pv);

		// Token: 0x06000172 RID: 370 RVA: 0x000051E6 File Offset: 0x000033E6
		public static void SHAddToRecentDocs(string path)
		{
			NativeMethods._SHAddToRecentDocs_String(SHARD.PATHW, path);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x000051EF File Offset: 0x000033EF
		public static void SHAddToRecentDocs(IShellLinkW shellLink)
		{
			NativeMethods._SHAddToRecentDocs_ShellLink(SHARD.LINK, shellLink);
		}

		// Token: 0x06000174 RID: 372
		[DllImport("dwmapi.dll", EntryPoint = "DwmGetCompositionTimingInfo")]
		private static extern HRESULT _DwmGetCompositionTimingInfo(IntPtr hwnd, ref DWM_TIMING_INFO pTimingInfo);

		// Token: 0x06000175 RID: 373 RVA: 0x000051F8 File Offset: 0x000033F8
		public static DWM_TIMING_INFO? DwmGetCompositionTimingInfo(IntPtr hwnd)
		{
			if (!Utility.IsOSVistaOrNewer)
			{
				return null;
			}
			DWM_TIMING_INFO value = new DWM_TIMING_INFO
			{
				cbSize = Marshal.SizeOf(typeof(DWM_TIMING_INFO))
			};
			HRESULT hrLeft = NativeMethods._DwmGetCompositionTimingInfo(hwnd, ref value);
			if (hrLeft == HRESULT.E_PENDING)
			{
				return null;
			}
			hrLeft.ThrowIfFailed();
			return new DWM_TIMING_INFO?(value);
		}

		// Token: 0x06000176 RID: 374
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("dwmapi.dll", PreserveSig = false)]
		public static extern void DwmInvalidateIconicBitmaps(IntPtr hwnd);

		// Token: 0x06000177 RID: 375
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("dwmapi.dll", PreserveSig = false)]
		public static extern void DwmSetIconicThumbnail(IntPtr hwnd, IntPtr hbmp, DWM_SIT dwSITFlags);

		// Token: 0x06000178 RID: 376
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("dwmapi.dll", PreserveSig = false)]
		public static extern void DwmSetIconicLivePreviewBitmap(IntPtr hwnd, IntPtr hbmp, RefPOINT pptClient, DWM_SIT dwSITFlags);

		// Token: 0x06000179 RID: 377
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("shell32.dll", PreserveSig = false)]
		public static extern void SHGetItemFromDataObject(IDataObject pdtobj, DOGIF dwFlags, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppv);

		// Token: 0x0600017A RID: 378
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("shell32.dll", PreserveSig = false)]
		public static extern HRESULT SHCreateItemFromParsingName([MarshalAs(UnmanagedType.LPWStr)] string pszPath, IBindCtx pbc, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppv);

		// Token: 0x0600017B RID: 379
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("shell32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool Shell_NotifyIcon(NIM dwMessage, [In] NOTIFYICONDATA lpdata);

		// Token: 0x0600017C RID: 380
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("shell32.dll", PreserveSig = false)]
		public static extern void SetCurrentProcessExplicitAppUserModelID([MarshalAs(UnmanagedType.LPWStr)] string AppID);

		// Token: 0x0600017D RID: 381
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[DllImport("shell32.dll")]
		public static extern HRESULT GetCurrentProcessExplicitAppUserModelID([MarshalAs(UnmanagedType.LPWStr)] out string AppID);
	}
}
