namespace Kantaiko.Controllers.Execution.Handlers;

/// <summary>
/// Represents a handler that can be used to perform some operation on a <see cref="ControllerExecutionContext"/>.
/// <br/>
/// All handlers are executed in the order they are registered until one of them sets <see cref="ControllerExecutionContext.ExecutionError"/>.
/// If all handlers are executed and <see cref="ControllerExecutionContext.ExecutionError"/> is still null,
/// the pipeline will be considered as successfully completed.
/// </summary>
public interface IControllerExecutionHandler
{
    /// <summary>
    /// Performs some operation on the <paramref name="context"/>.
    /// </summary>
    /// <param name="context">The context to perform the operation on.</param>
    /// <returns>A task that represents the operation.</returns>
    Task HandleAsync(ControllerExecutionContext context);
}
