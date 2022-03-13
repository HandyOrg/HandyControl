using System;
using System.Collections.Generic;
using HandyControl.Collections;
using HandyControl.Data;

namespace HandyControl.Tools;

public class ComparerGenerator
{
    private static readonly Dictionary<Type, ComparerTypeCode> TypeCodeDic = new()
    {
        [typeof(DateTimeRange)] = ComparerTypeCode.DateTimeRange,
    };

    public static IComparer<T> GetComparer<T>()
    {
        if (TypeCodeDic.TryGetValue(typeof(T), out var comparerType))
        {
            if (comparerType == ComparerTypeCode.DateTimeRange)
            {
                return (IComparer<T>) new DateTimeRangeComparer();
            }

            return null;
        }

        return null;
    }

    private enum ComparerTypeCode
    {
        DateTimeRange
    }
}
