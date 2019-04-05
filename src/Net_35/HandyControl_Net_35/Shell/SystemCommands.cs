using System;
using System.Security;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using Standard;

namespace Microsoft.Windows.Shell
{
    public static class SystemCommands
    {
        static SystemCommands()
        {
            CloseWindowCommand = new RoutedCommand("CloseWindow", typeof(SystemCommands));
            MaximizeWindowCommand = new RoutedCommand("MaximizeWindow", typeof(SystemCommands));
            MinimizeWindowCommand = new RoutedCommand("MinimizeWindow", typeof(SystemCommands));
            RestoreWindowCommand = new RoutedCommand("RestoreWindow", typeof(SystemCommands));
            ShowSystemMenuCommand = new RoutedCommand("ShowSystemMenu", typeof(SystemCommands));
        }

        public static RoutedCommand CloseWindowCommand { get; }
        public static RoutedCommand MaximizeWindowCommand { get; }
        public static RoutedCommand MinimizeWindowCommand { get; }
        public static RoutedCommand RestoreWindowCommand { get; }
        public static RoutedCommand ShowSystemMenuCommand { get; }

        /// <SecurityNote>
        ///     Critical : Calls critical methods
        ///     <SecurityNote>
        [SecurityCritical]
        private static void _PostSystemCommand(Window window, SC command)
        {
            var hwnd = new WindowInteropHelper(window).Handle;
            if (hwnd == IntPtr.Zero || !NativeMethods.IsWindow(hwnd))
                return;

            NativeMethods.PostMessage(hwnd, WM.SYSCOMMAND, new IntPtr((int) command), IntPtr.Zero);
        }

        /// <SecurityNote>
        ///     Critical : Calls critical methods
        ///     Safe     : Demands full trust permissions
        ///     <SecurityNote>
        [SecuritySafeCritical]
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static void CloseWindow(Window window)
        {
            Verify.IsNotNull(window, "window");
            _PostSystemCommand(window, SC.CLOSE);
        }

        /// <SecurityNote>
        ///     Critical : Calls critical methods
        ///     Safe     : Demands full trust permissions
        ///     <SecurityNote>
        [SecuritySafeCritical]
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static void MaximizeWindow(Window window)
        {
            Verify.IsNotNull(window, "window");
            _PostSystemCommand(window, SC.MAXIMIZE);
        }

        /// <SecurityNote>
        ///     Critical : Calls critical methods
        ///     Safe     : Demands full trust permissions
        ///     <SecurityNote>
        [SecuritySafeCritical]
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static void MinimizeWindow(Window window)
        {
            Verify.IsNotNull(window, "window");
            _PostSystemCommand(window, SC.MINIMIZE);
        }

        /// <SecurityNote>
        ///     Critical : Calls critical methods
        ///     Safe     : Demands full trust permissions
        ///     <SecurityNote>
        [SecuritySafeCritical]
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static void RestoreWindow(Window window)
        {
            Verify.IsNotNull(window, "window");
            _PostSystemCommand(window, SC.RESTORE);
        }

        /// <summary>Display the system menu at a specified location.</summary>
        /// <param name="screenLocation">The location to display the system menu, in logical screen coordinates.</param>
        /// <SecurityNote>
        ///     Critical : Calls critical methods
        ///     Safe     : Demands full trust permissions
        ///     <SecurityNote>
        [SecuritySafeCritical]
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static void ShowSystemMenu(Window window, Point screenLocation)
        {
            Verify.IsNotNull(window, "window");
            ShowSystemMenuPhysicalCoordinates(window, DpiHelper.LogicalPixelsToDevice(screenLocation));
        }

        /// <SecurityNote>
        ///     Critical : Calls critical methods
        ///     <SecurityNote>
        [SecurityCritical]
        internal static void ShowSystemMenuPhysicalCoordinates(Window window, Point physicalScreenLocation)
        {
            const uint TPM_RETURNCMD = 0x0100;
            const uint TPM_LEFTBUTTON = 0x0;

            Verify.IsNotNull(window, "window");
            var hwnd = new WindowInteropHelper(window).Handle;
            if (hwnd == IntPtr.Zero || !NativeMethods.IsWindow(hwnd))
                return;

            var hmenu = NativeMethods.GetSystemMenu(hwnd, false);

            var cmd = NativeMethods.TrackPopupMenuEx(hmenu, TPM_LEFTBUTTON | TPM_RETURNCMD,
                (int) physicalScreenLocation.X, (int) physicalScreenLocation.Y, hwnd, IntPtr.Zero);
            if (0 != cmd)
                NativeMethods.PostMessage(hwnd, WM.SYSCOMMAND, new IntPtr(cmd), IntPtr.Zero);
        }
    }
}