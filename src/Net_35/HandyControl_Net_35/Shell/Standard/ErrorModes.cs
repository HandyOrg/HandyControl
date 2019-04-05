namespace Standard
{
    using System;

    [Flags]
    internal enum ErrorModes
    {
        Default = 0,
        FailCriticalErrors = 1,
        NoAlignmentFaultExcept = 4,
        NoGpFaultErrorBox = 2,
        NoOpenFileErrorBox = 0x8000
    }
}

