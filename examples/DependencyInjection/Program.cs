using System;
using System.Reflection;
using System.Threading;
using Kantaiko.Controllers;
using Kantaiko.Controllers.Execution;
using Kantaiko.Controllers.Introspection.Factory;
using Kantaiko.Controllers.ParameterConversion;
using Kantaiko.Controllers.ParameterConversion.Text;
using Kantaiko.Routing.Abstractions;
using Kantaiko.Routing.Context;
using Microsoft.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection();

serviceCollection.AddSingleton(provider =>
{
    var lookupTypes = Assembly.GetExecutingAssembly().GetTypes();

    var converterCollection = new TextParameterConverterCollection(lookupTypes);

    var introspectionBuilder = new IntrospectionBuilder<TestContext>();

    introspectionBuilder.SetServiceProvider(provider);
    introspectionBuilder.AddEndpointMatching();
    introspectionBuilder.AddTextParameterConversion(converterCollection);
    introspectionBuilder.AddDefaultTransformation();

    var introspectionInfo = introspectionBuilder.CreateIntrospectionInfo(lookupTypes);

    var handlers = new HandlerCollection<TestContext>();
    var handlerFactory = new ServiceHandlerFactory();

    handlers.AddEndpointMatching();
    handlers.AddParameterConversion(h => h.AddTextParameterConversion(handlerFactory));
    handlers.AddDefaultControllerHandling(handlerFactory);

    return ControllerHandlerFactory.CreateControllerHandler(introspectionInfo, handlers);
});

// Build service provider
var serviceProvider = serviceCollection.BuildServiceProvider();

// Resolve controller handler
var handler = serviceProvider.GetRequiredService<IControllerHandler<TestContext>>();

// Handle request
using var scope = serviceProvider.CreateScope();
var context = new TestContext("Hello, world!", serviceProvider);

var result = await handler.HandleAsync(context);

internal class TestContext : AsyncContextBase
{
    public TestContext(string text,
        IServiceProvider? serviceProvider = null,
        CancellationToken cancellationToken = default
    ) : base(serviceProvider, cancellationToken)
    {
        Text = text;
    }

    public string Text { get; }
}

internal class ServiceHandlerFactory : IHandlerFactory
{
    public object CreateHandler(Type handlerType, IServiceProvider serviceProvider)
    {
        return ActivatorUtilities.CreateInstance(serviceProvider, handlerType);
    }
}
