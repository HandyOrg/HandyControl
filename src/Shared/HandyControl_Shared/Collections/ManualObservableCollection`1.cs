using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace HandyControl.Collections;

[Serializable]
public class ManualObservableCollection<T> : ObservableCollection<T>
{
    private const string CountString = "Count";

    private const string IndexerName = "Item[]";

    private int _oldCount;

    private bool _canNotify = true;

    public bool CanNotify
    {
        get => _canNotify;
        set
        {
            _canNotify = value;

            if (value)
            {
                if (_oldCount != Count)
                {
                    OnPropertyChanged(new PropertyChangedEventArgs(CountString));
                }

                OnPropertyChanged(new PropertyChangedEventArgs(IndexerName));
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

            }
            else
            {
                _oldCount = Count;
            }
        }
    }

    public ManualObservableCollection()
    {

    }

    public ManualObservableCollection(List<T> list) : base(list != null ? new List<T>(list.Count) : list) => CopyFrom(list);

    public ManualObservableCollection(IEnumerable<T> collection)
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));
        CopyFrom(collection);
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        if (!CanNotify) return;

        base.OnPropertyChanged(e);
    }

    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        if (!CanNotify) return;

        base.OnCollectionChanged(e);
    }

    private void CopyFrom(IEnumerable<T> collection)
    {
        var items = Items;
        if (collection != null)
        {
            using var enumerator = collection.GetEnumerator();
            while (enumerator.MoveNext())
            {
                items.Add(enumerator.Current);
            }
        }
    }
}
