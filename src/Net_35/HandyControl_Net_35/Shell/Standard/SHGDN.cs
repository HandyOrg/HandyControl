namespace Standard
{
    using System;

    [Flags]
    internal enum SHGDN
    {
        SHGDN_FORADDRESSBAR = 0x4000,
        SHGDN_FOREDITING = 0x1000,
        SHGDN_FORPARSING = 0x8000,
        SHGDN_INFOLDER = 1,
        SHGDN_NORMAL = 0
    }
}

