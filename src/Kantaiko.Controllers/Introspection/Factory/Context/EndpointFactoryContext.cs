using System;

namespace Kantaiko.Controllers.Introspection.Factory.Context;

public readonly ref struct EndpointFactoryContext
{
    public EndpointFactoryContext(EndpointInfo endpoint, IServiceProvider serviceProvider)
    {
        Endpoint = endpoint;
        ServiceProvider = serviceProvider;
    }

    public EndpointInfo Endpoint { get; }
    public IServiceProvider ServiceProvider { get; }
}
