using System;

namespace Standard;

[Flags]
internal enum Facility
{
    Null,
    Rpc,
    Dispatch,
    Storage,
    Itf,
    Win32 = 7,
    Windows,
    Control = 10,
    Ese = 3678,
    WinCodec = 2200
}
