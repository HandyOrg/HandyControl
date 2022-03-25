using System;

namespace HandyControl.Data;

public class DisposableObject : IDisposable
{
    private EventHandler _disposing;

    public bool IsDisposed { get; private set; }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~DisposableObject()
    {
        Dispose(false);
    }

    public event EventHandler Disposing
    {
        add
        {
            ThrowIfDisposed();
            _disposing += value;
        }
        remove
        {
            _disposing -= value;
        }
    }

    protected void ThrowIfDisposed()
    {
        if (IsDisposed) throw new ObjectDisposedException(GetType().Name);
    }

    protected void Dispose(bool disposing)
    {
        if (IsDisposed) return;

        try
        {
            if (disposing)
            {
                _disposing?.Invoke(this, EventArgs.Empty);
                _disposing = null;
                DisposeManagedResources();
            }

            DisposeNativeResources();
        }
        finally
        {
            IsDisposed = true;
        }
    }

    protected virtual void DisposeManagedResources()
    {
    }

    protected virtual void DisposeNativeResources()
    {
    }
}
