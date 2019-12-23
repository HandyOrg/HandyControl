using System;
using System.Collections.Generic;
using System.Windows;

namespace HandyControl.Expression.Drawing
{
    internal static class CommonExtensions
    {
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> newItems)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (collection is List<T> list)
            {
                list.AddRange(newItems);
            }
            else
            {
                foreach (var local in newItems)
                {
                    collection.Add(local);
                }
            }
        }

        public static bool SetIfDifferent(this DependencyObject dependencyObject, DependencyProperty dependencyProperty, object value)
        {
            if (!Equals(dependencyObject.GetValue(dependencyProperty), value))
            {
                dependencyObject.SetValue(dependencyProperty, value);
                return true;
            }
            return false;
        }

        public static bool EnsureListCount<T>(this IList<T> list, int count, Func<T> factory = null)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }
            if (!list.EnsureListCountAtLeast(count, factory))
            {
                if (list.Count <= count)
                {
                    return false;
                }

                if (list is List<T> list2)
                {
                    list2.RemoveRange(count, list.Count - count);
                }
                else
                {
                    for (var i = list.Count - 1; i >= count; i--)
                    {
                        list.RemoveAt(i);
                    }
                }
            }
            return true;
        }

        public static bool EnsureListCountAtLeast<T>(this IList<T> list, int count, Func<T> factory = null)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }
            if (list.Count >= count)
            {
                return false;
            }

            if ((list is List<T> list2) && factory == null)
            {
                list2.AddRange(new T[count - list.Count]);
            }
            else
            {
                for (var i = list.Count; i < count; i++)
                {
                    list.Add(factory == null ? default : factory());
                }
            }
            return true;
        }

        public static bool ClearIfSet(this DependencyObject dependencyObject, DependencyProperty dependencyProperty)
        {
            if (dependencyObject.ReadLocalValue(dependencyProperty) != DependencyProperty.UnsetValue)
            {
                dependencyObject.ClearValue(dependencyProperty);
                return true;
            }
            return false;
        }

        public static void RemoveLast<T>(this IList<T> list)
        {
            list.RemoveAt(list.Count - 1);
        }
    }
}