using System;

namespace Kantaiko.Controllers.Internal
{
    internal class DefaultInstanceFactory : IInstanceFactory
    {
        public object CreateInstance(Type type, IServiceProvider serviceProvider)
        {
            return Activator.CreateInstance(type) ??
                   throw new InvalidOperationException($"Unable to construct an instance of type \"{type.Name}\"");
        }

        public static DefaultInstanceFactory Instance { get; } = new();
    }
}
