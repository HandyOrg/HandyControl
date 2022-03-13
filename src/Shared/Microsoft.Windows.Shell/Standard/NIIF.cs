using System;

namespace Standard;

internal enum NIIF
{
    NONE,
    INFO,
    WARNING,
    ERROR,
    USER,
    NOSOUND = 16,
    LARGE_ICON = 32,
    NIIF_RESPECT_QUIET_TIME = 128,
    XP_ICON_MASK = 15
}
