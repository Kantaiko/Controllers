using System;

namespace Kantaiko.Controllers.Internal
{
    internal class DefaultServiceProvider : IServiceProvider
    {
        public object? GetService(Type serviceType)
        {
            throw new InvalidOperationException("DI container is not configured");
        }

        public static DefaultServiceProvider Instance { get; } = new();
    }
}
