namespace Kantaiko.Controllers;

public abstract class ControllerBase<TContext> : IContextAcceptor<TContext>, IAutoRegistrableController<TContext>
{
    protected TContext Context { get; private set; } = default!;

    void IContextAcceptor<TContext>.SetContext(TContext context)
    {
        Context = context;
    }
}
