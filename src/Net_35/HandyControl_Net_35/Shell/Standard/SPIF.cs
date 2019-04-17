namespace Standard
{
    using System;

    [Flags]
    internal enum SPIF
    {
        None = 0,
        SENDCHANGE = 2,
        SENDWININICHANGE = 2,
        UPDATEINIFILE = 1
    }
}

