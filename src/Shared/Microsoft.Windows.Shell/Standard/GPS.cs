using System;

namespace Standard;

internal enum GPS
{
    DEFAULT,
    HANDLERPROPERTIESONLY,
    READWRITE,
    TEMPORARY = 4,
    FASTPROPERTIESONLY = 8,
    OPENSLOWITEM = 16,
    DELAYCREATION = 32,
    BESTEFFORT = 64,
    NO_OPLOCK = 128,
    MASK_VALID = 255
}
