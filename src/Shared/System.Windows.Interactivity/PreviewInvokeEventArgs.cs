using System;

namespace HandyControl.Interactivity;

public class PreviewInvokeEventArgs : EventArgs
{
    public bool Cancelling { get; set; }
}
