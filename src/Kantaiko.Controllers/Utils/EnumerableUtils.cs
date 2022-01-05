using System.Collections.Generic;

namespace Kantaiko.Controllers.Utils;

public static class EnumerableUtils
{
    public static IEnumerable<T> Single<T>(T item)
    {
        yield return item;
    }
}