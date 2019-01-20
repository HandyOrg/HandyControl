namespace Standard
{
    using System;

    [Flags]
    internal enum NIF : uint
    {
        GUID = 0x20,
        ICON = 2,
        INFO = 0x10,
        MESSAGE = 1,
        REALTIME = 0x40,
        SHOWTIP = 0x80,
        STATE = 8,
        TIP = 4,
        VISTA_MASK = 0xfb,
        XP_MASK = 0x3b
    }
}

