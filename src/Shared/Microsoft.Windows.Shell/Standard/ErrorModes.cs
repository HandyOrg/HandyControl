using System;

namespace Standard;

[Flags]
internal enum ErrorModes
{
    Default = 0,
    FailCriticalErrors = 1,
    NoGpFaultErrorBox = 2,
    NoAlignmentFaultExcept = 4,
    NoOpenFileErrorBox = 32768
}
