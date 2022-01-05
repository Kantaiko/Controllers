using System;
using System.Threading;
using Kantaiko.Properties;
using Kantaiko.Routing.Context;

namespace Kantaiko.Controllers.Tests.Shared;

public class TestContext : ContextBase
{
    public TestContext(string message,
        IServiceProvider? serviceProvider = null,
        IReadOnlyPropertyCollection? properties = null,
        CancellationToken cancellationToken = default) :
        base(serviceProvider, properties, cancellationToken)
    {
        Message = message;
    }

    public string Message { get; }
}
