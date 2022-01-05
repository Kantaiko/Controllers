using System.Collections.Generic;
using System.Threading.Tasks;
using Kantaiko.Controllers.Result;
using Kantaiko.Routing;
using Kantaiko.Routing.Context;

namespace Kantaiko.Controllers.Execution;

using CResult = Task<ControllerExecutionResult>;

public class PipelineBuilder<TContext> where TContext : IContext
{
    private readonly List<IChainedHandler<ControllerExecutionContext<TContext>, CResult>> _handlers = new();

    public void AddHandler(IChainedHandler<ControllerExecutionContext<TContext>, CResult> handler)
    {
        _handlers.Add(handler);
    }

    public IEnumerable<IChainedHandler<ControllerExecutionContext<TContext>, CResult>> Build() => _handlers;
}
