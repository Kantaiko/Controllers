using System.Threading.Tasks;
using Kantaiko.Controllers.Result;
using Kantaiko.Properties;
using Kantaiko.Routing.Handlers;

namespace Kantaiko.Controllers.Execution.Properties;

public record ControllerExecutionProperties<TContext> : ReadOnlyPropertiesBase<ControllerExecutionProperties<TContext>>
{
    public IChainedHandler<ControllerContext<TContext>, Task<ControllerResult>>? Handler { get; init; }
}
