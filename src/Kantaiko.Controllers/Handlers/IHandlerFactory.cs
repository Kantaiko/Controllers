using System;

namespace Kantaiko.Controllers.Handlers;

public interface IHandlerFactory
{
    object CreateHandler(Type handlerType, IServiceProvider serviceProvider);
}
