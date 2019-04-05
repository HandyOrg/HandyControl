namespace Standard
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Explicit)]
    internal struct WTA_OPTIONS
    {
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification="Used by native code."), FieldOffset(0)]
        public Standard.WTNCA dwFlags;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification="Used by native code."), FieldOffset(4)]
        public Standard.WTNCA dwMask;
        public const uint Size = 8;
    }
}

