namespace Standard
{
    using System;

    internal enum GPS
    {
        BESTEFFORT = 0x40,
        DEFAULT = 0,
        DELAYCREATION = 0x20,
        FASTPROPERTIESONLY = 8,
        HANDLERPROPERTIESONLY = 1,
        MASK_VALID = 0xff,
        NO_OPLOCK = 0x80,
        OPENSLOWITEM = 0x10,
        READWRITE = 2,
        TEMPORARY = 4
    }
}

