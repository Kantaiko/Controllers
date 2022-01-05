using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Kantaiko.Controllers;
using Kantaiko.Controllers.Execution;
using Kantaiko.Controllers.Introspection.Factory;
using Kantaiko.Controllers.ParameterConversion.Text;
using Kantaiko.Controllers.Result;
using Kantaiko.Properties;
using Kantaiko.Routing;
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

    var pipelineBuilder = new PipelineBuilder<TestContext>();
    var handlerFactory = new ServiceHandlerFactory();

    pipelineBuilder.AddEndpointMatching();
    pipelineBuilder.AddTextParameterConversion(handlerFactory);
    pipelineBuilder.AddDefaultControllerHandling(handlerFactory);

    var handlers = pipelineBuilder.Build();

    return ControllerHandlerFactory.CreateControllerHandler(introspectionInfo, handlers);
});

// Build service provider
var serviceProvider = serviceCollection.BuildServiceProvider();

// Resolve controller handler
var handler = serviceProvider.GetRequiredService<IHandler<TestContext, Task<ControllerExecutionResult>>>();

// Handle request
using var scope = serviceProvider.CreateScope();
var context = new TestContext("Hello, world!", serviceProvider);

var result = await handler.Handle(context);

internal class TestContext : ContextBase
{
    public TestContext(string text,
        IServiceProvider? serviceProvider = null,
        IReadOnlyPropertyCollection? properties = null,
        CancellationToken cancellationToken = default) :
        base(serviceProvider, properties, cancellationToken)
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
