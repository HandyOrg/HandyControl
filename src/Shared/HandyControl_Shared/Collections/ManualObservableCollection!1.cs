using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace HandyControl.Collections
{
    [Serializable]
    public class ManualObservableCollection<T> : Collection<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        private readonly SimpleMonitor _monitor = new SimpleMonitor();

        private const string CountString = "Count";

        private const string IndexerName = "Item[]";

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
                        OnPropertyChanged(CountString);
                    }

                    OnPropertyChanged(IndexerName);
                    OnCollectionReset();
                }
                else
                {
                    _oldCount = Count;
                }
            }
        }

        private int _oldCount;

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        
        public event PropertyChangedEventHandler PropertyChanged;

        public ManualObservableCollection()
        {
            
        }

        // ReSharper disable once AssignNullToNotNullAttribute
        public ManualObservableCollection(List<T> list) : base(list != null ? new List<T>(list.Count) : list) => CopyFrom(list);

        public ManualObservableCollection(IEnumerable<T> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            CopyFrom(collection);
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

        public void Move(int oldIndex, int newIndex) => MoveItem(oldIndex, newIndex);

        protected override void ClearItems()
        {
            CheckReentrancy();
            base.ClearItems();
            OnPropertyChanged(CountString);
            OnPropertyChanged(IndexerName);
            OnCollectionReset();
        }

        protected override void RemoveItem(int index)
        {
            CheckReentrancy();
            var removedItem = this[index];

            base.RemoveItem(index);

            OnPropertyChanged(CountString);
            OnPropertyChanged(IndexerName);
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, removedItem, index);
        }

        protected override void InsertItem(int index, T item)
        {
            CheckReentrancy();
            base.InsertItem(index, item);

            OnPropertyChanged(CountString);
            OnPropertyChanged(IndexerName);
            OnCollectionChanged(NotifyCollectionChangedAction.Add, item, index);
        }

        protected override void SetItem(int index, T item)
        {
            CheckReentrancy();
            var originalItem = this[index];
            base.SetItem(index, item);

            OnPropertyChanged(IndexerName);
            OnCollectionChanged(NotifyCollectionChangedAction.Replace, originalItem, item, index);
        }

        protected virtual void MoveItem(int oldIndex, int newIndex)
        {
            CheckReentrancy();

            var removedItem = this[oldIndex];

            base.RemoveItem(oldIndex);
            base.InsertItem(newIndex, removedItem);

            OnPropertyChanged(IndexerName);
            OnCollectionChanged(NotifyCollectionChangedAction.Move, removedItem, newIndex, oldIndex);
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (!CanNotify || PropertyChanged == null) return;

            PropertyChanged.Invoke(this, e);
        }

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!CanNotify || CollectionChanged == null) return;
            
            using (BlockReentrancy())
            {
                // ReSharper disable once PossibleNullReferenceException
                CollectionChanged(this, e);
            }
        }

        protected IDisposable BlockReentrancy()
        {
            _monitor.Enter();
            return _monitor;
        }

        protected void CheckReentrancy()
        {
            if (_monitor.Busy && CollectionChanged != null && CollectionChanged.GetInvocationList().Length > 1)
            {
                throw new InvalidOperationException("ObservableCollectionReentrancyNotAllowed");
            }
        }

        private void OnPropertyChanged(string propertyName) => 
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

        private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index) => 
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index));

        private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index, int oldIndex) =>
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index, oldIndex));

        private void OnCollectionChanged(NotifyCollectionChangedAction action, object oldItem, object newItem, int index) => 
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, newItem, oldItem, index));

        private void OnCollectionReset() =>
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

        private class SimpleMonitor : IDisposable
        {
            public void Enter() => ++_busyCount;

            public void Dispose() => --_busyCount;

            public bool Busy => _busyCount > 0;

            private int _busyCount;
        }
    }
}
