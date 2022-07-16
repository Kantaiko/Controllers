using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Kantaiko.Controllers.Execution.Handlers;
using Kantaiko.Controllers.Introspection;
using Kantaiko.Controllers.Result;
using Kantaiko.Routing;
using Kantaiko.Routing.Handlers;

namespace Kantaiko.Controllers.Execution;

public class ControllerHandler<TContext> : IControllerHandler<TContext>
{
    private readonly IntrospectionInfo _introspectionInfo;
    private readonly IChainedHandler<ControllerContext<TContext>, Task<ControllerResult>> _handler;

    public ControllerHandler(
        IntrospectionInfo introspectionInfo,
        IEnumerable<IControllerExecutionHandler<TContext>> pipelineHandlers)
    {
        _introspectionInfo = introspectionInfo;
        _handler = Handler.Chain(pipelineHandlers);
    }

    public async Task<ControllerResult> HandleAsync(
        TContext input,
        IServiceProvider? serviceProvider = null,
        CancellationToken cancellationToken = default)
    {
        serviceProvider ??= DefaultServiceProvider.Instance;

        var executionContext = new ControllerContext<TContext>(
            input,
            _introspectionInfo,
            serviceProvider,
            cancellationToken
        );

        return await _handler.Handle(executionContext);
    }
}
