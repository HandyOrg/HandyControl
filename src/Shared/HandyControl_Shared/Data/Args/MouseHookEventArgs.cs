using System;
using HandyControl.Tools.Interop;

namespace HandyControl.Data
{
    internal class MouseHookEventArgs : EventArgs
    {
        public MouseHookMessageType Message { get; set; }

        public InteropValues.POINT Point { get; set; }
    }
}