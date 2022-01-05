using System;

namespace Kantaiko.Controllers.Introspection.Factory.Context;

public readonly ref struct ControllerFactoryContext
{
    public ControllerFactoryContext(ControllerInfo controller, IServiceProvider serviceProvider)
    {
        Controller = controller;
        ServiceProvider = serviceProvider;
    }

    public ControllerInfo Controller { get; }
    public IServiceProvider ServiceProvider { get; }
}
