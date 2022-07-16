using System;
using System.Threading;
using Kantaiko.Routing.Context;

namespace Kantaiko.Controllers;

public abstract class ControllerBase<TContext> : IContextAcceptor<TContext>, IAutoRegistrableController<TContext>
    where TContext : IAsyncContext
{
    protected TContext Context { get; private set; } = default!;

    protected IServiceProvider ServiceProvider => Context.ServiceProvider;
    protected CancellationToken CancellationToken => Context.CancellationToken;

    void IContextAcceptor<TContext>.SetContext(TContext context)
    {
        Context = context;
    }
}
