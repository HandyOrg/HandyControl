namespace Standard
{
    using System;

    internal enum FOF : ushort
    {
        ALLOWUNDO = 0x40,
        CONFIRMMOUSE = 2,
        FILESONLY = 0x80,
        MULTIDESTFILES = 1,
        NO_CONNECTED_ELEMENTS = 0x2000,
        NOCONFIRMATION = 0x10,
        NOCONFIRMMKDIR = 0x200,
        NOCOPYSECURITYATTRIBS = 0x800,
        NOERRORUI = 0x400,
        NORECURSEREPARSE = 0x8000,
        NORECURSION = 0x1000,
        RENAMEONCOLLISION = 8,
        SILENT = 4,
        SIMPLEPROGRESS = 0x100,
        WANTMAPPINGHANDLE = 0x20,
        WANTNUKEWARNING = 0x4000
    }
}

