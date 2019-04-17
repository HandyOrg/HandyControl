namespace Standard
{
    using System;

    [Flags]
    internal enum HCF
    {
        AVAILABLE = 2,
        CONFIRMHOTKEY = 8,
        HIGHCONTRASTON = 1,
        HOTKEYACTIVE = 4,
        HOTKEYAVAILABLE = 0x40,
        HOTKEYSOUND = 0x10,
        INDICATOR = 0x20
    }
}

