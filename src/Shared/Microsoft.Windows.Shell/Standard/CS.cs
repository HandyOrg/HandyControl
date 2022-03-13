using System;

namespace Standard;

[Flags]
internal enum CS : uint
{
    VREDRAW = 1u,
    HREDRAW = 2u,
    DBLCLKS = 8u,
    OWNDC = 32u,
    CLASSDC = 64u,
    PARENTDC = 128u,
    NOCLOSE = 512u,
    SAVEBITS = 2048u,
    BYTEALIGNCLIENT = 4096u,
    BYTEALIGNWINDOW = 8192u,
    GLOBALCLASS = 16384u,
    IME = 65536u,
    DROPSHADOW = 131072u
}
