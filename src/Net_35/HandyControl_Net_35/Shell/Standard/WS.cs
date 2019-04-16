namespace Standard
{
    using System;

    [Flags]
    internal enum WS : uint
    {
        BORDER = 0x800000,
        CAPTION = 0xc00000,
        CHILD = 0x40000000,
        CHILDWINDOW = 0x40000000,
        CLIPCHILDREN = 0x2000000,
        CLIPSIBLINGS = 0x4000000,
        DISABLED = 0x8000000,
        DLGFRAME = 0x400000,
        GROUP = 0x20000,
        HSCROLL = 0x100000,
        ICONIC = 0x20000000,
        MAXIMIZE = 0x1000000,
        MAXIMIZEBOX = 0x10000,
        MINIMIZE = 0x20000000,
        MINIMIZEBOX = 0x20000,
        OVERLAPPED = 0,
        OVERLAPPEDWINDOW = 0xcf0000,
        POPUP = 0x80000000,
        POPUPWINDOW = 0x80880000,
        SIZEBOX = 0x40000,
        SYSMENU = 0x80000,
        TABSTOP = 0x10000,
        THICKFRAME = 0x40000,
        TILED = 0,
        TILEDWINDOW = 0xcf0000,
        VISIBLE = 0x10000000,
        VSCROLL = 0x200000
    }
}

