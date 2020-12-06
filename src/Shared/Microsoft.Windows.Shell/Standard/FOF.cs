﻿using System;

namespace Standard
{
    internal enum FOF : ushort
    {
        MULTIDESTFILES = 1,
        CONFIRMMOUSE,
        SILENT = 4,
        RENAMEONCOLLISION = 8,
        NOCONFIRMATION = 16,
        WANTMAPPINGHANDLE = 32,
        ALLOWUNDO = 64,
        FILESONLY = 128,
        SIMPLEPROGRESS = 256,
        NOCONFIRMMKDIR = 512,
        NOERRORUI = 1024,
        NOCOPYSECURITYATTRIBS = 2048,
        NORECURSION = 4096,
        NO_CONNECTED_ELEMENTS = 8192,
        WANTNUKEWARNING = 16384,
        NORECURSEREPARSE = 32768
    }
}
