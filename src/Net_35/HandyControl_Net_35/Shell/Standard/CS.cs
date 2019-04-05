namespace Standard
{
    using System;

    [Flags]
    internal enum CS : uint
    {
        BYTEALIGNCLIENT = 0x1000,
        BYTEALIGNWINDOW = 0x2000,
        CLASSDC = 0x40,
        DBLCLKS = 8,
        DROPSHADOW = 0x20000,
        GLOBALCLASS = 0x4000,
        HREDRAW = 2,
        IME = 0x10000,
        NOCLOSE = 0x200,
        OWNDC = 0x20,
        PARENTDC = 0x80,
        SAVEBITS = 0x800,
        VREDRAW = 1
    }
}

