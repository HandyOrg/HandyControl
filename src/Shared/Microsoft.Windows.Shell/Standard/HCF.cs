using System;

namespace Standard;

[Flags]
internal enum HCF
{
    HIGHCONTRASTON = 1,
    AVAILABLE = 2,
    HOTKEYACTIVE = 4,
    CONFIRMHOTKEY = 8,
    HOTKEYSOUND = 16,
    INDICATOR = 32,
    HOTKEYAVAILABLE = 64
}
