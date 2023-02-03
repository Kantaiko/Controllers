using Kantaiko.Controllers.Execution.Handlers;
using Kantaiko.Controllers.Introspection;
using Kantaiko.Controllers.Introspection.Deconstruction;
using Kantaiko.Controllers.Introspection.Transformers;
using Kantaiko.Properties;

namespace Kantaiko.Controllers.Execution;

/// <summary>
/// The builder for <see cref="ControllerExecutor" />.
/// Can be extended with custom introspection transformers and execution handlers.
/// </summary>
public class ControllerExecutorBuilder : IPropertyContainer
{
    /// <summary>
    /// The deconstruction validator that will be used to detect composite parameters.
    /// </summary>
    public IDeconstructionValidator? DeconstructionValidator { get; set; }

    /// <summary>
    /// The list of introspection transformers that will be used to transform the introspection info.
    /// </summary>
    public ICollection<IIntrospectionInfoTransformer> Transformers { get; } = new List<IIntrospectionInfoTransformer>();

    /// <summary>
    /// The list of execution handlers that will be used to execute the controller.
    /// </summary>
    public ICollection<IControllerExecutionHandler> Handlers { get; } = new List<IControllerExecutionHandler>();

    /// <summary>
    /// The collection of user-defined properties that can be used to share data between different
    /// configuration methods.
    /// </summary>
    public IPropertyCollection Properties { get; } = new PropertyCollection();

    /// <summary>
    /// Builds the <see cref="ControllerExecutor" />.
    /// </summary>
    /// <param name="lookupTypes">The list of types that will be used to lookup the controllers.</param>
    /// <param name="serviceProvider">The service provider that will be used by introspection transformers.</param>
    /// <typeparam name="TController">The base type of the controllers.</typeparam>
    /// <returns>The <see cref="ControllerExecutor" />.</returns>
    public ControllerExecutor Build<TController>(IEnumerable<Type> lookupTypes,
        IServiceProvider? serviceProvider = null)
    {
        ArgumentNullException.ThrowIfNull(lookupTypes);

        var introspectionFactory = new IntrospectionInfoFactory(Transformers, DeconstructionValidator, serviceProvider);
        var introspectionInfo = introspectionFactory.CreateIntrospectionInfo<TController>(lookupTypes);

        return new ControllerExecutor(introspectionInfo, Handlers);
    }

    /// <summary>
    /// Creates a new instance of <see cref="ControllerExecutorBuilder" />
    /// with default introspection transformers configured.
    /// </summary>
    /// <returns>The <see cref="ControllerExecutorBuilder" />.</returns>
    public static ControllerExecutorBuilder CreateDefault()
    {
        var builder = new ControllerExecutorBuilder();
        builder.AddDefaultTransformers();

        return builder;
    }
}
