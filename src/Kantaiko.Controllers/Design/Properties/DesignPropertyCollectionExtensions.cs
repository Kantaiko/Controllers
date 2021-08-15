using System.Diagnostics.CodeAnalysis;
using Kantaiko.Controllers.Exceptions;

namespace Kantaiko.Controllers.Design.Properties
{
    public static class DesignPropertyCollectionExtensions
    {
        public static T? GetOptionalProperty<T>(this IDesignPropertyCollection collection, string key)
            where T : notnull
        {
            return collection.TryGetProperty<T>(key, out var value) ? value : default;
        }

        public static bool TryGetProperty<T>(this IDesignPropertyCollection collection, string key,
            [MaybeNullWhen(false)] out T value)
            where T : notnull
        {
            if (!collection.TryGetValue(key, out var storedValue))
            {
                value = default;
                return false;
            }

            if (storedValue is not T actualValue)
            {
                throw new InvalidDesignPropertyTypeException(key, typeof(T), storedValue.GetType());
            }

            value = actualValue;
            return true;
        }
    }
}
