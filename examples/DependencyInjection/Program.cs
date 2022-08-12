using System;
using System.Reflection;
using Kantaiko.Controllers;
using Kantaiko.Controllers.Execution;
using Kantaiko.Controllers.Handlers;
using Kantaiko.Controllers.Introspection.Factory;
using Kantaiko.Controllers.ParameterConversion;
using Kantaiko.Controllers.ParameterConversion.Text;
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

// Note: you should pass service provider to both the context and the HandleAsync call
// The execution pipeline makes no assumptions about whether the context contains a service provider or not
// The same applies to the CancellationToken parameter
var result = await handler.HandleAsync(context, serviceProvider);

internal record TestContext(string Text, IServiceProvider ServiceProvider);

internal class ServiceHandlerFactory : IHandlerFactory
{
    public object CreateHandler(Type handlerType, IServiceProvider serviceProvider)
    {
        return ActivatorUtilities.CreateInstance(serviceProvider, handlerType);
    }
}
