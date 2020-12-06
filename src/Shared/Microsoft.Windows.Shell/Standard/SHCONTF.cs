﻿using System;

namespace Standard
{
    internal enum SHCONTF
    {
        CHECKING_FOR_CHILDREN = 16,
        FOLDERS = 32,
        NONFOLDERS = 64,
        INCLUDEHIDDEN = 128,
        INIT_ON_FIRST_NEXT = 256,
        NETPRINTERSRCH = 512,
        SHAREABLE = 1024,
        STORAGE = 2048,
        NAVIGATION_ENUM = 4096,
        FASTITEMS = 8192,
        FLATLIST = 16384,
        ENABLE_ASYNC = 32768
    }
}
