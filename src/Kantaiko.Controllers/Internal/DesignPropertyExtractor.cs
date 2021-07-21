using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Kantaiko.Controllers.Internal
{
    internal static class DesignPropertyExtractor
    {
        public static IReadOnlyDictionary<string, object> GetProperties<TDesignPropertyProvider>(
            ICustomAttributeProvider attributeProvider,
            Func<TDesignPropertyProvider, IReadOnlyDictionary<string, object>> propertyExtractor)
        {
            return attributeProvider.GetCustomAttributes(true)
                .Where(x => x is TDesignPropertyProvider)
                .Cast<TDesignPropertyProvider>()
                .SelectMany(propertyExtractor)
                .GroupBy(x => x.Key)
                .ToDictionary(k => k.Key, v => v.First().Value);
        }
    }
}
