using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kantaiko.Controllers.Design.Properties;

namespace Kantaiko.Controllers.Internal
{
    internal static class DesignPropertyExtractor
    {
        public static IDesignPropertyCollection GetProperties<TDesignPropertyProvider>(
            ICustomAttributeProvider attributeProvider,
            Func<TDesignPropertyProvider, DesignPropertyCollection> propertyExtractor)
        {
            var groups = attributeProvider.GetCustomAttributes(true)
                .Where(x => x is TDesignPropertyProvider)
                .Cast<TDesignPropertyProvider>()
                .SelectMany(propertyExtractor)
                .GroupBy(x => x.Key);

            var designPropertyCollection = new DesignPropertyCollection();

            foreach (var group in groups)
            {
                designPropertyCollection[group.Key] = group.First().Value;
            }

            return designPropertyCollection;
        }
    }
}
