using System.Collections.Generic;
using System.Threading.Tasks;
using Kantaiko.Controllers.Introspection;
using Kantaiko.Controllers.Result;
using Kantaiko.Routing;
using Kantaiko.Routing.Context;

namespace Kantaiko.Controllers.Execution;

public class ControllerPipelineHandler<TContext> : IHandler<TContext, Task<ControllerExecutionResult>>
    where TContext : IContext
{
    private readonly IntrospectionInfo _introspectionInfo;
    private readonly IHandler<ControllerExecutionContext<TContext>, Task<ControllerExecutionResult>> _handler;

    public ControllerPipelineHandler(
        IntrospectionInfo introspectionInfo,
        IEnumerable<IChainedHandler<ControllerExecutionContext<TContext>, Task<ControllerExecutionResult>>>
            pipelineHandlers)
    {
        _introspectionInfo = introspectionInfo;
        _handler = Handler.Chain(pipelineHandlers);
    }

    public async Task<ControllerExecutionResult> Handle(TContext input)
    {
        var executionContext = new ControllerExecutionContext<TContext>(input, _introspectionInfo);
        return await _handler.Handle(executionContext);
    }
}
