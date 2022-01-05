using System;
using System.Threading;
using Kantaiko.Properties.Immutable;
using Kantaiko.Routing.Context;

namespace Kantaiko.Controllers;

public abstract class ControllerBase<TContext> : IContextAcceptor<TContext>, IAutoRegistrableController<TContext>
    where TContext : IContext
{
    protected TContext Context { get; private set; } = default!;

    protected IServiceProvider ServiceProvider => Context.ServiceProvider;
    protected IImmutablePropertyCollection Properties => Context.Properties;
    protected CancellationToken CancellationToken => Context.CancellationToken;

    void IContextAcceptor<TContext>.SetContext(TContext context)
    {
        Context = context;
    }
}
