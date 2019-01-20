namespace Standard
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential), SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class NOTIFYICONDATA
    {
        public int cbSize;
        public IntPtr hWnd;
        public int uID;
        public Standard.NIF uFlags;
        public int uCallbackMessage;
        public IntPtr hIcon;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x80)]
        public char[] szTip = new char[0x80];
        public uint dwState;
        public uint dwStateMask;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x100)]
        public char[] szInfo = new char[0x100];
        public uint uVersion;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x40)]
        public char[] szInfoTitle = new char[0x40];
        public uint dwInfoFlags;
        public Guid guidItem;
        private IntPtr hBalloonIcon;
    }
}

