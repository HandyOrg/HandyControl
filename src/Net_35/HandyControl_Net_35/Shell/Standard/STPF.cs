namespace Standard
{
    using System;

    [Flags]
    internal enum STPF
    {
        NONE = 0,
        USEAPPPEEKALWAYS = 4,
        USEAPPPEEKWHENACTIVE = 8,
        USEAPPTHUMBNAILALWAYS = 1,
        USEAPPTHUMBNAILWHENACTIVE = 2
    }
}

