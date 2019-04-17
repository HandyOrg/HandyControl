namespace Standard
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct TITLEBARINFO
    {
        public int cbSize;
        public Standard.RECT rcTitleBar;
        public Standard.STATE_SYSTEM rgstate_TitleBar;
        public Standard.STATE_SYSTEM rgstate_Reserved;
        public Standard.STATE_SYSTEM rgstate_MinimizeButton;
        public Standard.STATE_SYSTEM rgstate_MaximizeButton;
        public Standard.STATE_SYSTEM rgstate_HelpButton;
        public Standard.STATE_SYSTEM rgstate_CloseButton;
    }
}

