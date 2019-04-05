namespace Standard
{
    using System;

    internal enum SICHINT : uint
    {
        ALLFIELDS = 0x80000000,
        CANONICAL = 0x10000000,
        DISPLAY = 0,
        TEST_FILESYSPATH_IF_NOT_EQUAL = 0x20000000
    }
}

