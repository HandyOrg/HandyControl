using System;

namespace Standard;

[Flags]
internal enum NIF : uint
{
    MESSAGE = 1u,
    ICON = 2u,
    TIP = 4u,
    STATE = 8u,
    INFO = 16u,
    GUID = 32u,
    REALTIME = 64u,
    SHOWTIP = 128u,
    XP_MASK = 59u,
    VISTA_MASK = 251u
}
