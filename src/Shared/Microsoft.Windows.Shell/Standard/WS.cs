﻿using System;

namespace Standard
{
    [Flags]
    internal enum WS : uint
    {
        OVERLAPPED = 0u,
        POPUP = 2147483648u,
        CHILD = 1073741824u,
        MINIMIZE = 536870912u,
        VISIBLE = 268435456u,
        DISABLED = 134217728u,
        CLIPSIBLINGS = 67108864u,
        CLIPCHILDREN = 33554432u,
        MAXIMIZE = 16777216u,
        BORDER = 8388608u,
        DLGFRAME = 4194304u,
        VSCROLL = 2097152u,
        HSCROLL = 1048576u,
        SYSMENU = 524288u,
        THICKFRAME = 262144u,
        GROUP = 131072u,
        TABSTOP = 65536u,
        MINIMIZEBOX = 131072u,
        MAXIMIZEBOX = 65536u,
        CAPTION = 12582912u,
        TILED = 0u,
        ICONIC = 536870912u,
        SIZEBOX = 262144u,
        TILEDWINDOW = 13565952u,
        OVERLAPPEDWINDOW = 13565952u,
        POPUPWINDOW = 2156396544u,
        CHILDWINDOW = 1073741824u
    }
}
