using System.Threading.Tasks;
using Kantaiko.Controllers.Execution;
using Kantaiko.Controllers.Introspection.Factory.Context;
using Kantaiko.Controllers.Result;
using Kantaiko.Routing;
using Kantaiko.Routing.Context;

namespace Kantaiko.Controllers.Introspection.Factory.Attributes;

public interface IEndpointExecutionHandlerFactory<TContext> where TContext : IContext
{
    IChainedHandler<ControllerExecutionContext<TContext>, Task<ControllerExecutionResult>> CreateHandler(
        EndpointFactoryContext context);
}
