using System;

namespace HandyControl.Controls;

public interface ISingleOpen : IDisposable
{
    bool CanDispose { get; }
}
