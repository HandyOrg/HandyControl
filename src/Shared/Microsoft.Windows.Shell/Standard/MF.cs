using System;

namespace Standard;

[Flags]
internal enum MF : uint
{
    DOES_NOT_EXIST = 4294967295u,
    ENABLED = 0u,
    BYCOMMAND = 0u,
    GRAYED = 1u,
    DISABLED = 2u
}
