using System;

namespace Standard;

[Flags]
internal enum THBF : uint
{
    ENABLED = 0u,
    DISABLED = 1u,
    DISMISSONCLICK = 2u,
    NOBACKGROUND = 4u,
    HIDDEN = 8u,
    NONINTERACTIVE = 16u
}
