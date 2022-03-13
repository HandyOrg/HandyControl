using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Windows;

namespace HandyControl.Interactivity;

public abstract class AttachableCollection<T> : FreezableCollection<T>, IAttachedObject
    where T : DependencyObject, IAttachedObject
{
    private DependencyObject _associatedObject;
    private Collection<T> _snapshot;

    internal AttachableCollection()
    {
        INotifyCollectionChanged changed = this;
        changed.CollectionChanged += OnCollectionChanged;
        _snapshot = new Collection<T>();
    }

    public DependencyObject AssociatedObject
    {
        get
        {
            ReadPreamble();
            return _associatedObject;
        }
    }

    DependencyObject IAttachedObject.AssociatedObject => AssociatedObject;

    public void Attach(DependencyObject dependencyObject)
    {
        if (Equals(dependencyObject, AssociatedObject))
            return;
        if (AssociatedObject != null)
            throw new InvalidOperationException();
        if (Interaction.ShouldRunInDesignMode || !(bool) GetValue(DesignerProperties.IsInDesignModeProperty))
        {
            WritePreamble();
            _associatedObject = dependencyObject;
            WritePostscript();
        }
        OnAttached();
    }

    public void Detach()
    {
        OnDetaching();
        WritePreamble();
        _associatedObject = null;
        WritePostscript();
    }

    protected abstract void OnAttached();

    protected abstract void OnDetaching();

    internal abstract void ItemAdded(T item);

    internal abstract void ItemRemoved(T item);

    private void VerifyAdd(T item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        if (_snapshot.Contains(item))
            throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                ExceptionStringTable.DuplicateItemInCollectionExceptionMessage, new object[]
                {
                    typeof(T).Name,
                    GetType().Name
                }));
    }

    private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                var enumerator1 = e.NewItems.GetEnumerator();
                try
                {
                    while (enumerator1.MoveNext())
                    {
                        var current = (T) enumerator1.Current;
                        try
                        {
                            VerifyAdd(current);
                            ItemAdded(current);
                        }
                        finally
                        {
                            _snapshot.Insert(IndexOf(current), current);
                        }
                    }
                    break;
                }
                finally
                {
                    if (enumerator1 is IDisposable disposable)
                        disposable.Dispose();
                }
            case NotifyCollectionChangedAction.Remove:
                var enumerator2 = e.OldItems.GetEnumerator();
                try
                {
                    while (enumerator2.MoveNext())
                    {
                        var current = (T) enumerator2.Current;
                        ItemRemoved(current);
                        _snapshot.Remove(current);
                    }
                    break;
                }
                finally
                {
                    if (enumerator2 is IDisposable disposable)
                        disposable.Dispose();
                }
            case NotifyCollectionChangedAction.Replace:
                foreach (T oldItem in e.OldItems)
                {
                    ItemRemoved(oldItem);
                    _snapshot.Remove(oldItem);
                }
                var enumerator3 = e.NewItems.GetEnumerator();
                try
                {
                    while (enumerator3.MoveNext())
                    {
                        var current = (T) enumerator3.Current;
                        try
                        {
                            VerifyAdd(current);
                            ItemAdded(current);
                        }
                        finally
                        {
                            _snapshot.Insert(IndexOf(current), current);
                        }
                    }
                    break;
                }
                finally
                {
                    if (enumerator3 is IDisposable disposable)
                        disposable.Dispose();
                }
            case NotifyCollectionChangedAction.Reset:
                foreach (var obj in _snapshot)
                    ItemRemoved(obj);
                _snapshot = new Collection<T>();
                using (var enumerator4 = GetEnumerator())
                {
                    while (enumerator4.MoveNext())
                    {
                        var current = enumerator4.Current;
                        VerifyAdd(current);
                        ItemAdded(current);
                    }
                    break;
                }
        }
    }
}
