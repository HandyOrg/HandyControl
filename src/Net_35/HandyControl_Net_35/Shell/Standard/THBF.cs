namespace Standard
{
    using System;

    [Flags]
    internal enum THBF : uint
    {
        DISABLED = 1,
        DISMISSONCLICK = 2,
        ENABLED = 0,
        HIDDEN = 8,
        NOBACKGROUND = 4,
        NONINTERACTIVE = 0x10
    }
}

