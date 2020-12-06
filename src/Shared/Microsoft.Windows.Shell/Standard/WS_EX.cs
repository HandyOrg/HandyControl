﻿using System;

namespace Standard
{
    [Flags]
    internal enum WS_EX : uint
    {
        None = 0u,
        DLGMODALFRAME = 1u,
        NOPARENTNOTIFY = 4u,
        TOPMOST = 8u,
        ACCEPTFILES = 16u,
        TRANSPARENT = 32u,
        MDICHILD = 64u,
        TOOLWINDOW = 128u,
        WINDOWEDGE = 256u,
        CLIENTEDGE = 512u,
        CONTEXTHELP = 1024u,
        RIGHT = 4096u,
        LEFT = 0u,
        RTLREADING = 8192u,
        LTRREADING = 0u,
        LEFTSCROLLBAR = 16384u,
        RIGHTSCROLLBAR = 0u,
        CONTROLPARENT = 65536u,
        STATICEDGE = 131072u,
        APPWINDOW = 262144u,
        LAYERED = 524288u,
        NOINHERITLAYOUT = 1048576u,
        LAYOUTRTL = 4194304u,
        COMPOSITED = 33554432u,
        NOACTIVATE = 134217728u,
        OVERLAPPEDWINDOW = 768u,
        PALETTEWINDOW = 392u
    }
}
