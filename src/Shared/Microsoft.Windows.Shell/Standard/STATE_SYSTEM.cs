﻿using System;

namespace Standard
{
    [Flags]
    internal enum STATE_SYSTEM
    {
        UNAVAILABLE = 1,
        SELECTED = 2,
        FOCUSED = 4,
        PRESSED = 8,
        CHECKED = 16,
        MIXED = 32,
        INDETERMINATE = 32,
        READONLY = 64,
        HOTTRACKED = 128,
        DEFAULT = 256,
        EXPANDED = 512,
        COLLAPSED = 1024,
        BUSY = 2048,
        FLOATING = 4096,
        MARQUEED = 8192,
        ANIMATED = 16384,
        INVISIBLE = 32768,
        OFFSCREEN = 65536,
        SIZEABLE = 131072,
        MOVEABLE = 262144,
        SELFVOICING = 524288,
        FOCUSABLE = 1048576,
        SELECTABLE = 2097152,
        LINKED = 4194304,
        TRAVERSED = 8388608,
        MULTISELECTABLE = 16777216,
        EXTSELECTABLE = 33554432,
        ALERT_LOW = 67108864,
        ALERT_MEDIUM = 134217728,
        ALERT_HIGH = 268435456,
        PROTECTED = 536870912,
        VALID = 1073741823
    }
}
