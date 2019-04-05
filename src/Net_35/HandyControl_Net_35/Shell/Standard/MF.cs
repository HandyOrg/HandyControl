namespace Standard
{
    using System;

    [Flags]
    internal enum MF : uint
    {
        BYCOMMAND = 0,
        DISABLED = 2,
        DOES_NOT_EXIST = 0xffffffff,
        ENABLED = 0,
        GRAYED = 1
    }
}

