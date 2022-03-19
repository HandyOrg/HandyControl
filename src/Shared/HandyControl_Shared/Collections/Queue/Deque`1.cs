// adapted from https://www.c-sharpcorner.com/uploadfile/b942f9/implementing-a-double-ended-queue-or-deque-in-c-sharp/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace HandyControl.Collections;

[Serializable]
public class Deque<T> : IEnumerable<T>, ICollection
{
    [NonSerialized]
    private object _syncRoot;

    private List<T> _front;

    private List<T> _back;

    private int _frontDeleted;

    private int _backDeleted;

    public int Capacity => _front.Capacity + _back.Capacity;

    public int Count => _front.Count + _back.Count - _frontDeleted - _backDeleted;

    public bool IsEmpty => Count == 0;

    public IEnumerable<T> Reversed
    {
        get
        {
            if (_back.Count - _backDeleted > 0)
            {
                for (var i = _back.Count - 1; i >= _backDeleted; i--) yield return _back[i];
            }

            if (_front.Count - _frontDeleted > 0)
            {
                for (var i = _frontDeleted; i < _front.Count; i++) yield return _front[i];
            }
        }
    }

    public Deque()
    {
        _front = new List<T>();
        _back = new List<T>();
    }

    public Deque(int capacity)
    {
        if (capacity < 0) throw new ArgumentException("Capacity cannot be negative");
        var temp = capacity / 2;
        var temp2 = capacity - temp;
        _front = new List<T>(temp);
        _back = new List<T>(temp2);
    }

    public Deque(IEnumerable<T> backCollection) : this(backCollection, null)
    {

    }

    public Deque(IEnumerable<T> backCollection, IEnumerable<T> frontCollection)
    {
        if (backCollection == null && frontCollection == null) throw new ArgumentException("Collections cannot both be null");

        _front = frontCollection != null ? new List<T>(frontCollection) : new List<T>();
        _back = backCollection != null ? new List<T>(backCollection) : new List<T>();
    }

    public void AddFirst(T item)
    {
        if (_frontDeleted > 0 && _front.Count == _front.Capacity)
        {
            _front.RemoveRange(0, _frontDeleted);
            _frontDeleted = 0;
        }

        _front.Add(item);
    }

    public void AddLast(T item)
    {
        if (_backDeleted > 0 && _back.Count == _back.Capacity)
        {
            _back.RemoveRange(0, _backDeleted);
            _backDeleted = 0;
        }

        _back.Add(item);
    }

    public void AddRangeFirst(IEnumerable<T> range)
    {
        if (range == null) return;

        foreach (var item in range) AddFirst(item);
    }

    public void AddRangeLast(IEnumerable<T> range)
    {
        if (range == null) return;

        foreach (var item in range) AddLast(item);
    }

    public void Clear()
    {
        _front.Clear();
        _back.Clear();
        _frontDeleted = 0;
        _backDeleted = 0;
    }

    public bool Contains(T item)
    {
        for (var i = _frontDeleted; i < _front.Count; i++)
        {
            if (Equals(_front[i], item)) return true;
        }

        for (var i = _backDeleted; i < _back.Count; i++)
        {
            if (Equals(_back[i], item)) return true;
        }

        return false;
    }

    public void CopyTo(T[] array, int index)
    {
        if (array == null) throw new ArgumentNullException("Array cannot be null");
        if (index < 0) throw new ArgumentOutOfRangeException("Index cannot be negative");
        if (array.Length < index + Count) throw new ArgumentException("Index is invalid");
        var i = index;

        foreach (var item in this)
        {
            array[i++] = item;
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        if (_front.Count - _frontDeleted > 0)
        {
            for (var i = _front.Count - 1; i >= _frontDeleted; i--) yield return _front[i];
        }

        if (_back.Count - _backDeleted > 0)
        {
            for (var i = _backDeleted; i < _back.Count; i++) yield return _back[i];
        }
    }

    public T PeekFirst()
    {
        if (_front.Count > _frontDeleted)
        {
            return _front[_front.Count - 1];
        }

        if (_back.Count > _backDeleted)
        {
            return _back[_backDeleted];
        }

        throw new InvalidOperationException("Can't peek at empty Deque");
    }

    public T PeekLast()
    {
        if (_back.Count > _backDeleted)
        {
            return _back[_back.Count - 1];
        }

        if (_front.Count > _frontDeleted)
        {
            return _front[_frontDeleted];
        }

        throw new InvalidOperationException("Can't peek at empty Deque");
    }

    public T PopFirst()
    {
        T result;

        if (_front.Count > _frontDeleted)
        {
            result = _front[_front.Count - 1];
            _front.RemoveAt(_front.Count - 1);
        }
        else if (_back.Count > _backDeleted)
        {
            result = _back[_backDeleted];
            _backDeleted++;
        }
        else
        {
            throw new InvalidOperationException("Can't pop empty Deque");
        }

        return result;
    }


    public T PopLast()
    {
        T result;

        if (_back.Count > _backDeleted)
        {
            result = _back[_back.Count - 1];
            _back.RemoveAt(_back.Count - 1);
        }
        else if (_front.Count > _frontDeleted)
        {
            result = _front[_frontDeleted];
            _frontDeleted++;
        }
        else
        {
            throw new InvalidOperationException("Can't pop empty Deque");
        }

        return result;
    }

    public void Reverse()
    {
        var temp = _front;
        _front = _back;
        _back = temp;
        var temp2 = _frontDeleted;
        _frontDeleted = _backDeleted;
        _backDeleted = temp2;
    }

    public T[] ToArray()
    {
        if (Count == 0) return new T[0];
        var result = new T[Count];
        CopyTo(result, 0);
        return result;
    }

    public void TrimExcess()
    {
        if (_frontDeleted > 0)
        {
            _front.RemoveRange(0, _frontDeleted);
            _frontDeleted = 0;
        }

        if (_backDeleted > 0)
        {
            _back.RemoveRange(0, _backDeleted);
            _backDeleted = 0;
        }

        _front.TrimExcess();
        _back.TrimExcess();
    }

    public bool TryPeekFirst(out T item)
    {
        if (!IsEmpty)
        {
            item = PeekFirst();
            return true;
        }

        item = default;
        return false;
    }

    public bool TryPeekLast(out T item)
    {
        if (!IsEmpty)
        {
            item = PeekLast();
            return true;
        }

        item = default;
        return false;
    }

    public bool TryPopFirst(out T item)
    {
        if (!IsEmpty)
        {
            item = PopFirst();
            return true;
        }

        item = default;
        return false;
    }

    public bool TryPopLast(out T item)
    {
        if (!IsEmpty)
        {
            item = PopLast();
            return true;
        }

        item = default;
        return false;
    }

    bool ICollection.IsSynchronized => false;

    object ICollection.SyncRoot
    {
        get
        {
            if (_syncRoot == null)
            {
                Interlocked.CompareExchange<object>(ref _syncRoot, new object(), null);
            }
            return _syncRoot;
        }
    }

    void ICollection.CopyTo(Array array, int index) => CopyTo((T[]) array, index);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
