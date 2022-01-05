using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kantaiko.Controllers.Execution;
using Kantaiko.Controllers.Introspection;
using Kantaiko.Controllers.Result;
using Kantaiko.Routing;
using Kantaiko.Routing.Context;

namespace Kantaiko.Controllers;

public static class ControllerHandlerFactory
{
    public static IHandler<TContext, Task<ControllerExecutionResult>> CreateControllerHandler<TContext>(
        IntrospectionInfo introspectionInfo,
        IEnumerable<IChainedHandler<ControllerExecutionContext<TContext>, Task<ControllerExecutionResult>>>
            pipelineHandlers)
        where TContext : IContext
    {
        ArgumentNullException.ThrowIfNull(introspectionInfo);
        ArgumentNullException.ThrowIfNull(pipelineHandlers);

        return new ControllerPipelineHandler<TContext>(introspectionInfo, pipelineHandlers);
    }
}
