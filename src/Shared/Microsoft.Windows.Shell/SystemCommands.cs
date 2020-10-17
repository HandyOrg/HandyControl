using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using Standard;

namespace Microsoft.Windows.Shell
{
	// Token: 0x0200009D RID: 157
	public static class SystemCommands
	{
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000267 RID: 615 RVA: 0x00006863 File Offset: 0x00004A63
		// (set) Token: 0x06000268 RID: 616 RVA: 0x0000686A File Offset: 0x00004A6A
		public static RoutedCommand CloseWindowCommand { get; private set; } = new RoutedCommand("CloseWindow", typeof(SystemCommands));

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000269 RID: 617 RVA: 0x00006872 File Offset: 0x00004A72
		// (set) Token: 0x0600026A RID: 618 RVA: 0x00006879 File Offset: 0x00004A79
		public static RoutedCommand MaximizeWindowCommand { get; private set; } = new RoutedCommand("MaximizeWindow", typeof(SystemCommands));

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600026B RID: 619 RVA: 0x00006881 File Offset: 0x00004A81
		// (set) Token: 0x0600026C RID: 620 RVA: 0x00006888 File Offset: 0x00004A88
		public static RoutedCommand MinimizeWindowCommand { get; private set; } = new RoutedCommand("MinimizeWindow", typeof(SystemCommands));

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600026D RID: 621 RVA: 0x00006890 File Offset: 0x00004A90
		// (set) Token: 0x0600026E RID: 622 RVA: 0x00006897 File Offset: 0x00004A97
		public static RoutedCommand RestoreWindowCommand { get; private set; } = new RoutedCommand("RestoreWindow", typeof(SystemCommands));

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600026F RID: 623 RVA: 0x0000689F File Offset: 0x00004A9F
		// (set) Token: 0x06000270 RID: 624 RVA: 0x000068A6 File Offset: 0x00004AA6
		public static RoutedCommand ShowSystemMenuCommand { get; private set; } = new RoutedCommand("ShowSystemMenu", typeof(SystemCommands));

		// Token: 0x06000272 RID: 626 RVA: 0x0000693C File Offset: 0x00004B3C
		private static void _PostSystemCommand(Window window, SC command)
		{
			IntPtr handle = new WindowInteropHelper(window).Handle;
			if (handle == IntPtr.Zero || !NativeMethods.IsWindow(handle))
			{
				return;
			}
			NativeMethods.PostMessage(handle, WM.SYSCOMMAND, new IntPtr((int)command), IntPtr.Zero);
		}

		// Token: 0x06000273 RID: 627 RVA: 0x00006981 File Offset: 0x00004B81
		public static void CloseWindow(Window window)
		{
			Verify.IsNotNull<Window>(window, "window");
			SystemCommands._PostSystemCommand(window, SC.CLOSE);
		}

		// Token: 0x06000274 RID: 628 RVA: 0x00006999 File Offset: 0x00004B99
		public static void MaximizeWindow(Window window)
		{
			Verify.IsNotNull<Window>(window, "window");
			SystemCommands._PostSystemCommand(window, SC.MAXIMIZE);
		}

		// Token: 0x06000275 RID: 629 RVA: 0x000069B1 File Offset: 0x00004BB1
		public static void MinimizeWindow(Window window)
		{
			Verify.IsNotNull<Window>(window, "window");
			SystemCommands._PostSystemCommand(window, SC.MINIMIZE);
		}

		// Token: 0x06000276 RID: 630 RVA: 0x000069C9 File Offset: 0x00004BC9
		public static void RestoreWindow(Window window)
		{
			Verify.IsNotNull<Window>(window, "window");
			SystemCommands._PostSystemCommand(window, SC.RESTORE);
		}

		// Token: 0x06000277 RID: 631 RVA: 0x000069E1 File Offset: 0x00004BE1
		public static void ShowSystemMenu(Window window, Point screenLocation)
		{
			Verify.IsNotNull<Window>(window, "window");
			SystemCommands.ShowSystemMenuPhysicalCoordinates(window, DpiHelper.LogicalPixelsToDevice(screenLocation));
		}

		// Token: 0x06000278 RID: 632 RVA: 0x000069FC File Offset: 0x00004BFC
		internal static void ShowSystemMenuPhysicalCoordinates(Window window, Point physicalScreenLocation)
		{
			Verify.IsNotNull<Window>(window, "window");
			IntPtr handle = new WindowInteropHelper(window).Handle;
			if (handle == IntPtr.Zero || !NativeMethods.IsWindow(handle))
			{
				return;
			}
			IntPtr systemMenu = NativeMethods.GetSystemMenu(handle, false);
			uint num = NativeMethods.TrackPopupMenuEx(systemMenu, 256U, (int)physicalScreenLocation.X, (int)physicalScreenLocation.Y, handle, IntPtr.Zero);
			if (num != 0U)
			{
				NativeMethods.PostMessage(handle, WM.SYSCOMMAND, new IntPtr((long)((ulong)num)), IntPtr.Zero);
			}
		}
	}
}
