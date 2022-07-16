using System;
using System.Collections.Generic;
using Kantaiko.Controllers.Execution;
using Kantaiko.Controllers.Execution.Handlers;
using Kantaiko.Controllers.Introspection;

namespace Kantaiko.Controllers;

public static class ControllerHandlerFactory
{
    public static IControllerHandler<TContext> CreateControllerHandler<TContext>(
        IntrospectionInfo introspectionInfo,
        IEnumerable<IControllerExecutionHandler<TContext>> handlers)
    {
        ArgumentNullException.ThrowIfNull(introspectionInfo);
        ArgumentNullException.ThrowIfNull(handlers);

        return new ControllerHandler<TContext>(introspectionInfo, handlers);
    }
}
