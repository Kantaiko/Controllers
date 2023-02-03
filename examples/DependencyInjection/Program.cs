using System.Reflection;
using DependencyInjection;
using Kantaiko.Controllers;
using Kantaiko.Controllers.Execution;
using Microsoft.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection();

serviceCollection.AddSingleton(provider =>
{
    var executorBuilder = new ControllerExecutorBuilder();
    // configure controller executor here

    var lookupTypes = Assembly.GetExecutingAssembly().GetTypes();
    return executorBuilder.Build<TestController>(lookupTypes, provider);
});

// Build service provider
var serviceProvider = serviceCollection.BuildServiceProvider();

// Resolve controller handler
var handler = serviceProvider.GetRequiredService<ControllerExecutor>();

// Handle request
using var scope = serviceProvider.CreateScope();
var context = new TestContext("Hello, world!", serviceProvider);

// Note: you should pass service provider to both the context and the HandleAsync call
// The execution pipeline makes no assumptions about whether the context contains a service provider or not
// The same applies to the CancellationToken parameter
var result = await handler.HandleAsync(context, serviceProvider);

namespace DependencyInjection
{
    internal class TestController : ControllerBase<TestContext> { }

    internal record TestContext(string Text, IServiceProvider ServiceProvider);

    internal class ServiceControllerFactory : IControllerFactory
    {
        public object CreateHandler(Type handlerType, IServiceProvider serviceProvider)
        {
            return ActivatorUtilities.CreateInstance(serviceProvider, handlerType);
        }
    }
}
