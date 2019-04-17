namespace Standard
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct TITLEBARINFOEX
    {
        public int cbSize;
        public Standard.RECT rcTitleBar;
        public Standard.STATE_SYSTEM rgstate_TitleBar;
        public Standard.STATE_SYSTEM rgstate_Reserved;
        public Standard.STATE_SYSTEM rgstate_MinimizeButton;
        public Standard.STATE_SYSTEM rgstate_MaximizeButton;
        public Standard.STATE_SYSTEM rgstate_HelpButton;
        public Standard.STATE_SYSTEM rgstate_CloseButton;
        public Standard.RECT rgrect_TitleBar;
        public Standard.RECT rgrect_Reserved;
        public Standard.RECT rgrect_MinimizeButton;
        public Standard.RECT rgrect_MaximizeButton;
        public Standard.RECT rgrect_HelpButton;
        public Standard.RECT rgrect_CloseButton;
    }
}

