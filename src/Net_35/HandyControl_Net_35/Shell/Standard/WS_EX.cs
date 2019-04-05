namespace Standard
{
    using System;

    [Flags]
    internal enum WS_EX : uint
    {
        ACCEPTFILES = 0x10,
        APPWINDOW = 0x40000,
        CLIENTEDGE = 0x200,
        COMPOSITED = 0x2000000,
        CONTEXTHELP = 0x400,
        CONTROLPARENT = 0x10000,
        DLGMODALFRAME = 1,
        LAYERED = 0x80000,
        LAYOUTRTL = 0x400000,
        LEFT = 0,
        LEFTSCROLLBAR = 0x4000,
        LTRREADING = 0,
        MDICHILD = 0x40,
        NOACTIVATE = 0x8000000,
        NOINHERITLAYOUT = 0x100000,
        None = 0,
        NOPARENTNOTIFY = 4,
        OVERLAPPEDWINDOW = 0x300,
        PALETTEWINDOW = 0x188,
        RIGHT = 0x1000,
        RIGHTSCROLLBAR = 0,
        RTLREADING = 0x2000,
        STATICEDGE = 0x20000,
        TOOLWINDOW = 0x80,
        TOPMOST = 8,
        TRANSPARENT = 0x20,
        WINDOWEDGE = 0x100
    }
}

