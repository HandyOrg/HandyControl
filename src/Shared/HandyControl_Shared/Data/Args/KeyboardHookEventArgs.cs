using System;
using System.Windows.Input;

namespace HandyControl.Data
{
    internal class KeyboardHookEventArgs : EventArgs
    {
        public Key Key { get; }

        public KeyboardHookEventArgs(int virtualKey)
        {
            Key = KeyInterop.KeyFromVirtualKey(virtualKey);
        }
    }
}