using System;
using System.Collections.Generic;

namespace Kantaiko.Controllers.Utils;

public static class ListExtensions
{
    public static bool InsertBefore<T>(this IList<T> list, Func<T, bool> predicate, T item)
    {
        ArgumentNullException.ThrowIfNull(list);
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(item);

        for (var index = 0; index < list.Count; index++)
        {
            if (predicate(list[index]))
            {
                list.Insert(index, item);

                return true;
            }
        }

        return false;
    }

    public static bool InsertAfter<T>(this IList<T> list, Func<T, bool> predicate, T item)
    {
        ArgumentNullException.ThrowIfNull(list);
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(item);

        for (var index = 0; index < list.Count; index++)
        {
            if (predicate(list[index]))
            {
                list.Insert(index + 1, item);

                return true;
            }
        }

        return false;
    }
}
