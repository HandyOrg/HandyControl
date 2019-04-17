using System;
using System.Collections.Generic;
using System.Linq;

namespace HandyControl.Tools.Extension
{
    public static class EnumerableExtension
    {
        public static IEnumerable<TSource> Do<TSource>(this IEnumerable<TSource> source, Action<TSource> predicate)
        {
            var enumerable = source as IList<TSource> ?? source.ToList();
            foreach (var item in enumerable)
            {
                predicate.Invoke(item);
            }

            return enumerable;
        }
    }
}