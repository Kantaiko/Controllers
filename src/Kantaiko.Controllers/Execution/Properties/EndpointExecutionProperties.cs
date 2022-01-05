using System.Threading.Tasks;
using Kantaiko.Controllers.Result;
using Kantaiko.Properties;
using Kantaiko.Routing;
using Kantaiko.Routing.Context;

namespace Kantaiko.Controllers.Execution.Properties;

public record EndpointExecutionProperties<TContext> : ReadOnlyPropertiesBase<EndpointExecutionProperties<TContext>>
    where TContext : IContext
{
    public IChainedHandler<ControllerExecutionContext<TContext>, Task<ControllerExecutionResult>>? Handler
    {
        get;
        init;
    }
}
