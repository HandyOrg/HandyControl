namespace Standard
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode, Pack=1)]
    internal struct SHFILEOPSTRUCT
    {
        public IntPtr hwnd;
        [MarshalAs(UnmanagedType.U4)]
        public Standard.FO wFunc;
        public string pFrom;
        public string pTo;
        [MarshalAs(UnmanagedType.U2)]
        public Standard.FOF fFlags;
        [MarshalAs(UnmanagedType.Bool)]
        public int fAnyOperationsAborted;
        public IntPtr hNameMappings;
        public string lpszProgressTitle;
    }
}

