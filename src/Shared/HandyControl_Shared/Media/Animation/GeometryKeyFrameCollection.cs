using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;

namespace HandyControl.Media.Animation;

public class GeometryKeyFrameCollection : Freezable, IList
{
    private List<GeometryKeyFrame> _keyFrames;

    private static GeometryKeyFrameCollection s_emptyCollection;

    public GeometryKeyFrameCollection()
    {
        _keyFrames = new List<GeometryKeyFrame>(2);
    }

    public static GeometryKeyFrameCollection Empty
    {
        get
        {
            if (s_emptyCollection == null)
            {
                var emptyCollection = new GeometryKeyFrameCollection
                {
                    _keyFrames = new List<GeometryKeyFrame>(0)
                };

                emptyCollection.Freeze();

                s_emptyCollection = emptyCollection;
            }

            return s_emptyCollection;
        }
    }

    public new GeometryKeyFrameCollection Clone() => (GeometryKeyFrameCollection) base.Clone();

    protected override Freezable CreateInstanceCore() => new GeometryKeyFrameCollection();

    protected override void CloneCore(Freezable sourceFreezable)
    {
        var sourceCollection = (GeometryKeyFrameCollection) sourceFreezable;
        base.CloneCore(sourceFreezable);

        var count = sourceCollection._keyFrames.Count;

        _keyFrames = new List<GeometryKeyFrame>(count);

        for (var i = 0; i < count; i++)
        {
            var keyFrame = (GeometryKeyFrame) sourceCollection._keyFrames[i].Clone();
            _keyFrames.Add(keyFrame);
            OnFreezablePropertyChanged(null, keyFrame);
        }
    }

    protected override void CloneCurrentValueCore(Freezable sourceFreezable)
    {
        var sourceCollection = (GeometryKeyFrameCollection) sourceFreezable;
        base.CloneCurrentValueCore(sourceFreezable);

        var count = sourceCollection._keyFrames.Count;

        _keyFrames = new List<GeometryKeyFrame>(count);

        for (var i = 0; i < count; i++)
        {
            var keyFrame = (GeometryKeyFrame) sourceCollection._keyFrames[i].CloneCurrentValue();
            _keyFrames.Add(keyFrame);
            OnFreezablePropertyChanged(null, keyFrame);
        }
    }

    protected override void GetAsFrozenCore(Freezable sourceFreezable)
    {
        var sourceCollection = (GeometryKeyFrameCollection) sourceFreezable;
        base.GetAsFrozenCore(sourceFreezable);

        var count = sourceCollection._keyFrames.Count;

        _keyFrames = new List<GeometryKeyFrame>(count);

        for (var i = 0; i < count; i++)
        {
            var keyFrame = (GeometryKeyFrame) sourceCollection._keyFrames[i].GetAsFrozen();
            _keyFrames.Add(keyFrame);
            OnFreezablePropertyChanged(null, keyFrame);
        }
    }

    protected override void GetCurrentValueAsFrozenCore(Freezable sourceFreezable)
    {
        var sourceCollection = (GeometryKeyFrameCollection) sourceFreezable;
        base.GetCurrentValueAsFrozenCore(sourceFreezable);

        var count = sourceCollection._keyFrames.Count;

        _keyFrames = new List<GeometryKeyFrame>(count);

        for (var i = 0; i < count; i++)
        {
            var keyFrame = (GeometryKeyFrame) sourceCollection._keyFrames[i].GetCurrentValueAsFrozen();
            _keyFrames.Add(keyFrame);
            OnFreezablePropertyChanged(null, keyFrame);
        }
    }

    protected override bool FreezeCore(bool isChecking)
    {
        var canFreeze = base.FreezeCore(isChecking);

        for (var i = 0; i < _keyFrames.Count && canFreeze; i++)
        {
            canFreeze &= Freeze(_keyFrames[i], isChecking);
        }

        return canFreeze;
    }

    public IEnumerator GetEnumerator()
    {
        ReadPreamble();

        return _keyFrames.GetEnumerator();
    }

    void ICollection.CopyTo(Array array, int index)
    {
        ReadPreamble();

        ((ICollection) _keyFrames).CopyTo(array, index);
    }

    public void CopyTo(GeometryKeyFrame[] array, int index)
    {
        ReadPreamble();

        _keyFrames.CopyTo(array, index);
    }

    public int Count
    {
        get
        {
            ReadPreamble();

            return _keyFrames.Count;
        }
    }

    public object SyncRoot
    {
        get
        {
            ReadPreamble();

            return ((ICollection) _keyFrames).SyncRoot;
        }
    }

    public bool IsSynchronized
    {
        get
        {
            ReadPreamble();

            return IsFrozen || Dispatcher != null;
        }
    }

    int IList.Add(object keyFrame) => Add((GeometryKeyFrame) keyFrame);

    public int Add(GeometryKeyFrame keyFrame)
    {
        if (keyFrame == null)
        {
            throw new ArgumentNullException(nameof(keyFrame));
        }

        WritePreamble();

        OnFreezablePropertyChanged(null, keyFrame);
        _keyFrames.Add(keyFrame);

        WritePostscript();

        return _keyFrames.Count - 1;
    }

    public void Clear()
    {
        WritePreamble();

        if (_keyFrames.Count <= 0) return;

        foreach (var frame in _keyFrames)
        {
            OnFreezablePropertyChanged(frame, null);
        }

        _keyFrames.Clear();

        WritePostscript();
    }

    bool IList.Contains(object keyFrame) => Contains((GeometryKeyFrame) keyFrame);

    public bool Contains(GeometryKeyFrame keyFrame)
    {
        ReadPreamble();

        return _keyFrames.Contains(keyFrame);
    }

    int IList.IndexOf(object keyFrame) => IndexOf((GeometryKeyFrame) keyFrame);

    public int IndexOf(GeometryKeyFrame keyFrame)
    {
        ReadPreamble();

        return _keyFrames.IndexOf(keyFrame);
    }

    void IList.Insert(int index, object keyFrame) => Insert(index, (GeometryKeyFrame) keyFrame);

    public void Insert(int index, GeometryKeyFrame keyFrame)
    {
        if (keyFrame == null)
        {
            throw new ArgumentNullException(nameof(keyFrame));
        }

        WritePreamble();

        OnFreezablePropertyChanged(null, keyFrame);
        _keyFrames.Insert(index, keyFrame);

        WritePostscript();
    }

    void IList.Remove(object keyFrame) => Remove((GeometryKeyFrame) keyFrame);

    public void Remove(GeometryKeyFrame keyFrame)
    {
        WritePreamble();

        if (_keyFrames.Contains(keyFrame))
        {
            OnFreezablePropertyChanged(keyFrame, null);
            _keyFrames.Remove(keyFrame);

            WritePostscript();
        }
    }

    public void RemoveAt(int index)
    {
        WritePreamble();

        OnFreezablePropertyChanged(_keyFrames[index], null);
        _keyFrames.RemoveAt(index);

        WritePostscript();
    }

    object IList.this[int index]
    {
        get => this[index];
        set => this[index] = (GeometryKeyFrame) value;
    }

    public GeometryKeyFrame this[int index]
    {
        get
        {
            ReadPreamble();

            return _keyFrames[index];
        }
        set
        {
            if (value == null)
            {
                throw new ArgumentNullException(string.Format(CultureInfo.InvariantCulture, "DoubleKeyFrameCollection[{0}]", index));
            }

            WritePreamble();

            if (value != _keyFrames[index])
            {
                OnFreezablePropertyChanged(_keyFrames[index], value);
                _keyFrames[index] = value;

                WritePostscript();
            }
        }
    }

    public bool IsReadOnly
    {
        get
        {
            ReadPreamble();

            return IsFrozen;
        }
    }

    public bool IsFixedSize
    {
        get
        {
            ReadPreamble();

            return IsFrozen;
        }
    }
}
