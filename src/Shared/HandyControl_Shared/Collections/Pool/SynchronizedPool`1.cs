namespace HandyControl.Collections;

public class SynchronizedPool<T> : SimplePool<T>
{
    private readonly object _lockObj = new();

    public SynchronizedPool(int maxPoolSize) : base(maxPoolSize)
    {

    }

    public override T Acquire()
    {
        lock (_lockObj)
        {
            return base.Acquire();
        }
    }

    public override bool Release(T element)
    {
        lock (_lockObj)
        {
            return base.Release(element);
        }
    }
}
