﻿using System;

namespace Standard
{
    [Flags]
    internal enum SFGAO : uint
    {
        CANCOPY = 1u,
        CANMOVE = 2u,
        CANLINK = 4u,
        STORAGE = 8u,
        CANRENAME = 16u,
        CANDELETE = 32u,
        HASPROPSHEET = 64u,
        DROPTARGET = 256u,
        CAPABILITYMASK = 375u,
        ENCRYPTED = 8192u,
        ISSLOW = 16384u,
        GHOSTED = 32768u,
        LINK = 65536u,
        SHARE = 131072u,
        READONLY = 262144u,
        HIDDEN = 524288u,
        DISPLAYATTRMASK = 1032192u,
        FILESYSANCESTOR = 268435456u,
        FOLDER = 536870912u,
        FILESYSTEM = 1073741824u,
        HASSUBFOLDER = 2147483648u,
        CONTENTSMASK = 2147483648u,
        VALIDATE = 16777216u,
        REMOVABLE = 33554432u,
        COMPRESSED = 67108864u,
        BROWSABLE = 134217728u,
        NONENUMERATED = 1048576u,
        NEWCONTENT = 2097152u,
        CANMONIKER = 4194304u,
        HASSTORAGE = 4194304u,
        STREAM = 4194304u,
        STORAGEANCESTOR = 8388608u,
        STORAGECAPMASK = 1891958792u,
        PKEYSFGAOMASK = 2164539392u
    }
}
