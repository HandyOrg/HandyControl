using System;
using System.Windows.Input;

namespace HandyControl.Data;

public class KeyboardHookEventArgs : EventArgs
{
    public bool IsSystemKey { get; }

    public Key Key { get; }

    public KeyboardHookEventArgs(int virtualKey, bool isSystemKey)
    {
        IsSystemKey = isSystemKey;
        Key = KeyInterop.KeyFromVirtualKey(virtualKey);
    }
}
