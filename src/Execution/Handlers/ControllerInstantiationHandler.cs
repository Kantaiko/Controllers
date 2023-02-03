using Kantaiko.Controllers.Utils;

namespace Kantaiko.Controllers.Execution.Handlers;

/// <summary>
/// The handler that is responsible for creating controller instances by their types
/// using the specified <see cref="IControllerFactory"/>.
/// </summary>
public sealed class ControllerInstantiationHandler : IControllerExecutionHandler
{
    private readonly IControllerFactory _controllerFactory;

    /// <summary>
    /// Creates a new instance of the <see cref="ControllerInstantiationHandler"/> class.
    /// </summary>
    /// <param name="controllerFactory">The controller factory to use for creating controller instances.</param>
    public ControllerInstantiationHandler(IControllerFactory? controllerFactory = null)
    {
        _controllerFactory = controllerFactory ?? DefaultControllerFactory.Instance;
    }

    Task IControllerExecutionHandler.HandleAsync(ControllerExecutionContext context)
    {
        var endpoint = Check.ContextProperty(context.Endpoint, nameof(ControllerInstantiationHandler));

        context.ControllerInstance = _controllerFactory
            .CreateHandler(endpoint.Controller.Type, context.ServiceProvider);

        if (context.ControllerInstance is IContextAcceptor contextAcceptor)
        {
            contextAcceptor.SetContext(context.RequestContext);
        }

        return Task.CompletedTask;
    }
}
