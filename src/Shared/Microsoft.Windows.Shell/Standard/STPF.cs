using System;

namespace Standard;

[Flags]
internal enum STPF
{
    NONE = 0,
    USEAPPTHUMBNAILALWAYS = 1,
    USEAPPTHUMBNAILWHENACTIVE = 2,
    USEAPPPEEKALWAYS = 4,
    USEAPPPEEKWHENACTIVE = 8
}
