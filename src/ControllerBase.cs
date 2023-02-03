namespace Kantaiko.Controllers;

/// <summary>
/// The base class for controller base classes.
/// </summary>
/// <typeparam name="TContext">The type of the context object.</typeparam>
public abstract class ControllerBase<TContext> : IContextAcceptor
{
    /// <summary>
    /// The current context object.
    /// </summary>
    protected TContext Context { get; private set; } = default!;

    void IContextAcceptor.SetContext(object context)
    {
        if (context is not TContext typedContext)
        {
            throw new InvalidOperationException(
                $"Expected context of type {typeof(TContext).FullName}, but got {context.GetType().FullName}");
        }

        Context = typedContext;
    }
}
