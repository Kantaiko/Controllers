using Kantaiko.Controllers.Execution.Handlers;
using Kantaiko.Controllers.Introspection.Factory.Context;

namespace Kantaiko.Controllers.Introspection.Factory.Attributes;

public interface IControllerExecutionHandlerFactory<TContext>
{
    IControllerExecutionHandler<TContext> CreateHandler(ControllerFactoryContext context);
}
