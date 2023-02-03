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
    public static void AddControllerInstantiation(this ControllerExecutorBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Handlers.Add(new ControllerInstantiationHandler());
    }

    /// <summary>
    /// Adds the <see cref="EndpointInvocationHandler"/> to the execution pipeline.
    /// </summary>
    /// <param name="builder">The <see cref="ControllerExecutorBuilder"/>.</param>
    public static void AddEndpointInvocation(this ControllerExecutorBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Handlers.Add(new EndpointInvocationHandler());
    }

    /// <summary>
    /// Adds the <see cref="ControllerInstantiationHandler"/> and <see cref="EndpointInvocationHandler"/>
    /// to the execution pipeline.
    /// <br/>
    /// In most cases should be called in the end of the configuration.
    /// </summary>
    /// <param name="builder">The <see cref="ControllerExecutorBuilder"/>.</param>
    public static void AddDefaultHandlers(this ControllerExecutorBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.AddControllerInstantiation();
        builder.AddEndpointInvocation();
    }
}
