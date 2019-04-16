namespace Standard
{
    using System;

    [Flags]
    internal enum STATE_SYSTEM
    {
        ALERT_HIGH = 0x10000000,
        ALERT_LOW = 0x4000000,
        ALERT_MEDIUM = 0x8000000,
        ANIMATED = 0x4000,
        BUSY = 0x800,
        CHECKED = 0x10,
        COLLAPSED = 0x400,
        DEFAULT = 0x100,
        EXPANDED = 0x200,
        EXTSELECTABLE = 0x2000000,
        FLOATING = 0x1000,
        FOCUSABLE = 0x100000,
        FOCUSED = 4,
        HOTTRACKED = 0x80,
        INDETERMINATE = 0x20,
        INVISIBLE = 0x8000,
        LINKED = 0x400000,
        MARQUEED = 0x2000,
        MIXED = 0x20,
        MOVEABLE = 0x40000,
        MULTISELECTABLE = 0x1000000,
        OFFSCREEN = 0x10000,
        PRESSED = 8,
        PROTECTED = 0x20000000,
        READONLY = 0x40,
        SELECTABLE = 0x200000,
        SELECTED = 2,
        SELFVOICING = 0x80000,
        SIZEABLE = 0x20000,
        TRAVERSED = 0x800000,
        UNAVAILABLE = 1,
        VALID = 0x3fffffff
    }
}

