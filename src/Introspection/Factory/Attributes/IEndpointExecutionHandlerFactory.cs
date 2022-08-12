using Kantaiko.Controllers.Execution.Handlers;
using Kantaiko.Controllers.Introspection.Factory.Context;

namespace Kantaiko.Controllers.Introspection.Factory.Attributes;

public interface IEndpointExecutionHandlerFactory<TContext>
{
    IControllerExecutionHandler<TContext> CreateHandler(EndpointFactoryContext context);
}
