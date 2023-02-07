using Kantaiko.Controllers.Execution.Handlers;
using Kantaiko.Controllers.Introspection;
using Kantaiko.Controllers.Resources;

namespace Kantaiko.Controllers.Execution;

/// <summary>
/// The entry point for executing controller requests.
/// </summary>
public sealed class ControllerExecutor
{
    private readonly IEnumerable<IControllerExecutionHandler> _pipelineHandlers;

    /// <summary>
    /// The introspection info used by the executor.
    /// </summary>
    public IntrospectionInfo IntrospectionInfo { get; }

    /// <summary>
    /// Creates a new instance of <see cref="ControllerExecutor"/>.
    /// </summary>
    /// <param name="introspectionInfo">The introspection info to use for looking up controllers.</param>
    /// <param name="pipelineHandlers">The list of handlers to use for executing requests.</param>
    public ControllerExecutor(IntrospectionInfo introspectionInfo,
        IEnumerable<IControllerExecutionHandler> pipelineHandlers)
    {
        ArgumentNullException.ThrowIfNull(introspectionInfo);
        ArgumentNullException.ThrowIfNull(pipelineHandlers);

        IntrospectionInfo = introspectionInfo;
        _pipelineHandlers = pipelineHandlers;
    }

    /// <summary>
    /// Handles a specified request.
    /// </summary>
    /// <param name="context">
    /// The context of the request to handle.
    /// <br/>
    /// The type of the context does not make sense for the default pipeline handlers and executor itself,
    /// but it can be used by custom pipeline handlers, endpoint matchers, parameter resolvers, etc.
    /// </param>
    /// <param name="serviceProvider">
    /// The service provider to use for creating controllers and resolving dependencies.
    /// <br/>
    /// Note that even if the context contains a service provider, you must pass it explicitly to this method.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token to use by pipeline handlers.
    /// <br/>
    /// Note that even if the context contains a cancellation token, you must pass it explicitly to this method.
    /// </param>
    /// <returns></returns>
    public async Task<ControllerExecutionResult> HandleAsync(
        object context,
        IServiceProvider? serviceProvider = null,
        CancellationToken cancellationToken = default)
    {
        serviceProvider ??= EmptyServiceProvider.Instance;

        var executionContext = new ControllerExecutionContext(
            context,
            IntrospectionInfo,
            serviceProvider,
            cancellationToken
        );

        try
        {
            foreach (var pipelineHandler in _pipelineHandlers)
            {
                await pipelineHandler.HandleAsync(executionContext);

                if (executionContext.ExecutionError is not null)
                {
                    return new ControllerExecutionResult { Error = executionContext.ExecutionError };
                }
            }
        }
        catch (Exception exception)
        {
            return new ControllerExecutionResult
            {
                Error = new ControllerError(ControllerErrorCodes.PipelineException)
                {
                    Message = Strings.PipelineException,
                    Exception = exception
                }
            };
        }

        return new ControllerExecutionResult { EndpointResult = executionContext.InvocationResult };
    }
}
