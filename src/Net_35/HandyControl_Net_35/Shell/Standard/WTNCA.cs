namespace Standard
{
    using System;

    [Flags]
    internal enum WTNCA : uint
    {
        NODRAWCAPTION = 1,
        NODRAWICON = 2,
        NOMIRRORHELP = 8,
        NOSYSMENU = 4,
        VALIDBITS = 15
    }
}

