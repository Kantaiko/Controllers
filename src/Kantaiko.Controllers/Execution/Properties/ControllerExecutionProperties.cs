using System.Threading.Tasks;
using Kantaiko.Controllers.Result;
using Kantaiko.Properties;
using Kantaiko.Routing;
using Kantaiko.Routing.Context;

namespace Kantaiko.Controllers.Execution.Properties;

public record ControllerExecutionProperties<TContext> : ReadOnlyPropertiesBase<ControllerExecutionProperties<TContext>>
    where TContext : IContext
{
    public IChainedHandler<ControllerExecutionContext<TContext>, Task<ControllerExecutionResult>>? Handler
    {
        get;
        init;
    }
}
