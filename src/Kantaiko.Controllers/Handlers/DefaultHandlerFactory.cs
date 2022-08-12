using System;

namespace Kantaiko.Controllers.Handlers;

public class DefaultHandlerFactory : IHandlerFactory
{
    public static DefaultHandlerFactory Instance { get; } = new();

    public object CreateHandler(Type handlerType, IServiceProvider serviceProvider)
    {
        return Activator.CreateInstance(handlerType)!;
    }
}
