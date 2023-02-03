namespace Kantaiko.Controllers.Utils;

internal static class EnumerableUtils
{
    public static IEnumerable<T> Single<T>(T item)
    {
        yield return item;
    }
}
