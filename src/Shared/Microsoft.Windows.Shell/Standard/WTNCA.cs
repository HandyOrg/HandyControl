using System;

namespace Standard;

[Flags]
internal enum WTNCA : uint
{
    NODRAWCAPTION = 1u,
    NODRAWICON = 2u,
    NOSYSMENU = 4u,
    NOMIRRORHELP = 8u,
    VALIDBITS = 15u
}
