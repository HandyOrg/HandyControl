using System;

namespace Standard;

internal struct TITLEBARINFOEX
{
    public int cbSize;

    public RECT rcTitleBar;

    public STATE_SYSTEM rgstate_TitleBar;

    public STATE_SYSTEM rgstate_Reserved;

    public STATE_SYSTEM rgstate_MinimizeButton;

    public STATE_SYSTEM rgstate_MaximizeButton;

    public STATE_SYSTEM rgstate_HelpButton;

    public STATE_SYSTEM rgstate_CloseButton;

    public RECT rgrect_TitleBar;

    public RECT rgrect_Reserved;

    public RECT rgrect_MinimizeButton;

    public RECT rgrect_MaximizeButton;

    public RECT rgrect_HelpButton;

    public RECT rgrect_CloseButton;
}
