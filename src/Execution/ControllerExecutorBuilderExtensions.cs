using Kantaiko.Controllers.Execution.Handlers;

namespace Kantaiko.Controllers.Execution;

/// <summary>
/// The extension methods for <see cref="ControllerExecutorBuilder"/>.
/// </summary>
public static class ControllerExecutorBuilderExtensions
{
    /// <summary>
    /// Adds the <see cref="ControllerInstantiationHandler"/> to the execution pipeline.
    /// </summary>
    /// <param name="builder">The <see cref="ControllerExecutorBuilder"/>.</param>
    /// <param name="controllerFactory">The controller factory that will be used to create controllers.</param>
    public static void AddControllerInstantiation(this ControllerExecutorBuilder builder,
        IControllerFactory? controllerFactory = null)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Handlers.Add(new ControllerInstantiationHandler(controllerFactory));
    }

    /// <summary>
    /// Adds the <see cref="EndpointInvocationHandler"/> to the execution pipeline.
    /// </summary>
    /// <param name="builder">The <see cref="ControllerExecutorBuilder"/>.</param>
    /// <param name="awaitResult">Whether the result of the endpoint should be awaited.</param>
    public static void AddEndpointInvocation(this ControllerExecutorBuilder builder, bool awaitResult = true)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Handlers.Add(new EndpointInvocationHandler(awaitResult));
    }

    /// <summary>
    /// Adds the <see cref="ControllerInstantiationHandler"/> and <see cref="EndpointInvocationHandler"/>
    /// to the execution pipeline.
    /// <br/>
    /// In most cases should be called in the end of the configuration.
    /// </summary>
    /// <param name="builder">The <see cref="ControllerExecutorBuilder"/>.</param>
    /// <param name="controllerFactory">The controller factory that will be used to create controllers.</param>
    /// <param name="awaitResult">Whether the result of the endpoint should be awaited.</param>
    public static void AddDefaultHandlers(this ControllerExecutorBuilder builder,
        IControllerFactory? controllerFactory = null, bool awaitResult = true)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.AddControllerInstantiation(controllerFactory);
        builder.AddEndpointInvocation(awaitResult);
    }
}
