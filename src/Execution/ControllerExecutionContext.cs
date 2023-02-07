using Kantaiko.Controllers.Execution.Handlers;
using Kantaiko.Controllers.Introspection;
using Kantaiko.Properties;
using Kantaiko.Properties.Immutable;

namespace Kantaiko.Controllers.Execution;

/// <summary>
/// The context that represents the execution of a controller request.
/// <br/>
/// From one side, it defines some kind of a contract between the controller execution stages.
/// From the other side, it allows user code to customize the execution process.
/// </summary>
public sealed class ControllerExecutionContext : IPropertyContainer
{
    internal ControllerExecutionContext(
        object requestContext,
        IntrospectionInfo introspectionInfo,
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken)
    {
        RequestContext = requestContext;
        IntrospectionInfo = introspectionInfo;

        ServiceProvider = serviceProvider;
        CancellationToken = cancellationToken;
    }

    /// <summary>
    /// The request context that is processed by the controller.
    /// </summary>
    public object RequestContext { get; }

    /// <summary>
    /// The introspection info.
    /// </summary>
    public IntrospectionInfo IntrospectionInfo { get; }

    private IPropertyCollection? _properties;

    /// <summary>
    /// The user-defined data that can be used between different execution stages.
    /// </summary>
    public IPropertyCollection Properties => _properties ??= new PropertyCollection();

    /// <summary>
    /// The endpoint that was selected to process the request.
    /// <br/>
    /// Will be null if the endpoint is not selected yet.
    /// </summary>
    public EndpointInfo? Endpoint { get; set; }

    /// <summary>
    /// The property collection that contains the additional data provided by the endpoint matching stage.
    /// </summary>
    public IImmutablePropertyCollection MatchProperties { get; set; } = ImmutablePropertyCollection.Empty;

    /// <summary>
    /// The controller instance that will be used to process the request.
    /// <br/>
    /// Will be null if the controller is not created yet.
    /// </summary>
    public object? ControllerInstance { get; set; }

    /// <summary>
    /// The array of parameter values that was created by the parameter conversion stage.
    /// <br/>
    /// The order of the values corresponds to the order of the <see cref="EndpointInfo.Parameters"/> list.
    /// <br/>
    /// An empty array by default.
    /// </summary>
    public object?[] ResolvedParameters { get; set; } = Array.Empty<object>();

    /// <summary>
    /// The normalized result of the method call that was executed to process the request.
    /// <br/>
    /// In most cases, this value will be set by the <see cref="EndpointInvocationHandler"/>.
    /// This handler produces the following values:
    /// <list type="bullet">
    /// <item>If the method return type is void, the value will be null.</item>
    /// <item>If the method return type is <see cref="Task"/> or <see cref="ValueTask"/> the value will be null.</item>
    /// <item>
    /// If the method return type is <see cref="Task{TResult}"/> or <see cref="ValueTask{TResult}"/> the value will be
    /// the result of the task.
    /// </item>
    /// <item>Otherwise, the value will be the result of the method call.</item>
    /// <item>The value will always be the result of the method call if the <c>awaitResult</c> parameter of the
    /// handler is set to <c>false</c>.</item>
    /// </list>
    /// This property will be used to produce the final result after the execution pipeline is finished.
    /// </summary>
    public object? InvocationResult { get; set; }

    /// <summary>
    /// An error that occurred during the execution of the execution pipeline or controller method invocation.
    /// <br/>
    /// This property will be used to produce the final result after the execution pipeline is finished.
    /// </summary>
    public ControllerError? ExecutionError { get; set; }

    /// <summary>
    /// The service provider that can be used to resolve services.
    /// </summary>
    public IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// The cancellation token that can be used to listen for request cancellation.
    /// </summary>
    public CancellationToken CancellationToken { get; }
}
