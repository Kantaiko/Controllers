using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Kantaiko.Controllers.Execution.Handlers;
using Kantaiko.Controllers.Handlers;
using Kantaiko.Controllers.Introspection;
using Kantaiko.Controllers.Result;

namespace Kantaiko.Controllers.Execution;

public class ControllerHandler<TContext> : IControllerHandler<TContext>
{
    private readonly IntrospectionInfo _introspectionInfo;
    private readonly IEnumerable<IControllerExecutionHandler<TContext>> _pipelineHandlers;

    public ControllerHandler(
        IntrospectionInfo introspectionInfo,
        IEnumerable<IControllerExecutionHandler<TContext>> pipelineHandlers)
    {
        _introspectionInfo = introspectionInfo;
        _pipelineHandlers = pipelineHandlers;
    }

    public async Task<ControllerExecutionResult> HandleAsync(
        TContext input,
        IServiceProvider? serviceProvider = null,
        CancellationToken cancellationToken = default)
    {
        serviceProvider ??= EmptyServiceProvider.Instance;

        var executionContext = new ControllerExecutionContext<TContext>(
            input,
            _introspectionInfo,
            serviceProvider,
            cancellationToken
        );

        foreach (var pipelineHandler in _pipelineHandlers)
        {
            await pipelineHandler.HandleAsync(executionContext);

            if (executionContext.ExecutionResult is not null)
            {
                return executionContext.ExecutionResult;
            }
        }

        throw new InvalidOperationException("Invalid execution pipeline: no execution result");
    }
}
