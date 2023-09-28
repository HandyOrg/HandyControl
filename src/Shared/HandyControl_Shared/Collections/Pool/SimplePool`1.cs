//adapted from https://android.googlesource.com/platform/frameworks/base/+/master/core/java/android/util/Pools.java

using System;

namespace HandyControl.Collections;

public class SimplePool<T> : IPool<T>
{
    private readonly object[] _pool;

    private int _poolSize;

    public SimplePool(int maxPoolSize)
    {
        if (maxPoolSize <= 0)
        {
            throw new ArgumentException("The max pool size must be > 0");
        }
        _pool = new object[maxPoolSize];
    }

    public virtual T Acquire()
    {
        if (_poolSize > 0)
        {
            var lastPooledIndex = _poolSize - 1;
            var instance = (T) _pool[lastPooledIndex];
            _pool[lastPooledIndex] = null;
            _poolSize--;
            return instance;
        }
        return default;
    }

    public virtual bool Release(T instance)
    {
        if (IsInPool(instance))
        {
            throw new Exception("Already in the pool!");
        }

        if (_poolSize < _pool.Length)
        {
            _pool[_poolSize] = instance;
            _poolSize++;
            return true;
        }

        return false;
    }

    private bool IsInPool(T instance)
    {
        for (var i = 0; i < _poolSize; i++)
        {
            if (Equals(_pool[i], instance))
            {
                return true;
            }
        }
        return false;
    }
}
