using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using Standard;

namespace Microsoft.Windows.Shell;

public static class SystemCommands
{
    public static RoutedCommand CloseWindowCommand { get; private set; } = new RoutedCommand("CloseWindow", typeof(SystemCommands));

    public static RoutedCommand MaximizeWindowCommand { get; private set; } = new RoutedCommand("MaximizeWindow", typeof(SystemCommands));

    public static RoutedCommand MinimizeWindowCommand { get; private set; } = new RoutedCommand("MinimizeWindow", typeof(SystemCommands));

    public static RoutedCommand RestoreWindowCommand { get; private set; } = new RoutedCommand("RestoreWindow", typeof(SystemCommands));

    public static RoutedCommand ShowSystemMenuCommand { get; private set; } = new RoutedCommand("ShowSystemMenu", typeof(SystemCommands));

    private static void _PostSystemCommand(Window window, SC command)
    {
        IntPtr handle = new WindowInteropHelper(window).Handle;
        if (handle == IntPtr.Zero || !NativeMethods.IsWindow(handle))
        {
            return;
        }
        NativeMethods.PostMessage(handle, WM.SYSCOMMAND, new IntPtr((int) command), IntPtr.Zero);
    }

    public static void CloseWindow(Window window)
    {
        Verify.IsNotNull<Window>(window, "window");
        SystemCommands._PostSystemCommand(window, SC.CLOSE);
    }

    public static void MaximizeWindow(Window window)
    {
        Verify.IsNotNull<Window>(window, "window");
        SystemCommands._PostSystemCommand(window, SC.MAXIMIZE);
    }

    public static void MinimizeWindow(Window window)
    {
        Verify.IsNotNull<Window>(window, "window");
        SystemCommands._PostSystemCommand(window, SC.MINIMIZE);
    }

    public static void RestoreWindow(Window window)
    {
        Verify.IsNotNull<Window>(window, "window");
        SystemCommands._PostSystemCommand(window, SC.RESTORE);
    }

    public static void ShowSystemMenu(Window window, Point screenLocation)
    {
        Verify.IsNotNull<Window>(window, "window");
        SystemCommands.ShowSystemMenuPhysicalCoordinates(window, DpiHelper.LogicalPixelsToDevice(screenLocation));
    }

    internal static void ShowSystemMenuPhysicalCoordinates(Window window, Point physicalScreenLocation)
    {
        Verify.IsNotNull<Window>(window, "window");
        IntPtr handle = new WindowInteropHelper(window).Handle;
        if (handle == IntPtr.Zero || !NativeMethods.IsWindow(handle))
        {
            return;
        }
        IntPtr systemMenu = NativeMethods.GetSystemMenu(handle, false);
        uint num = NativeMethods.TrackPopupMenuEx(systemMenu, 256u, (int) physicalScreenLocation.X, (int) physicalScreenLocation.Y, handle, IntPtr.Zero);
        if (num != 0u)
        {
            NativeMethods.PostMessage(handle, WM.SYSCOMMAND, new IntPtr((long) ((ulong) num)), IntPtr.Zero);
        }
    }
}
