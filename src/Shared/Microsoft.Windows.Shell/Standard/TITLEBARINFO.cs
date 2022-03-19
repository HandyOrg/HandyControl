using System;

namespace Standard;

internal struct TITLEBARINFO
{
    public int cbSize;

    public RECT rcTitleBar;

    public STATE_SYSTEM rgstate_TitleBar;

    public STATE_SYSTEM rgstate_Reserved;

    public STATE_SYSTEM rgstate_MinimizeButton;

    public STATE_SYSTEM rgstate_MaximizeButton;

    public STATE_SYSTEM rgstate_HelpButton;

    public STATE_SYSTEM rgstate_CloseButton;
}
