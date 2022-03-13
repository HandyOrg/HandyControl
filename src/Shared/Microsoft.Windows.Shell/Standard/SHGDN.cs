using System;

namespace Standard;

[Flags]
internal enum SHGDN
{
    SHGDN_NORMAL = 0,
    SHGDN_INFOLDER = 1,
    SHGDN_FOREDITING = 4096,
    SHGDN_FORADDRESSBAR = 16384,
    SHGDN_FORPARSING = 32768
}
