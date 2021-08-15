using System;
using Kantaiko.Controllers;
using Microsoft.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection();
serviceCollection.AddSingleton(provider =>
{
    var controllerCollection = ControllerCollection.FromAssemblies();

    return new RequestHandler<TestContext>(controllerCollection,
        instanceFactory: new ServiceInstanceFactory(),
        serviceProvider: provider);
});

// Build service provider
var serviceProvider = serviceCollection.BuildServiceProvider();
var requestHandler = serviceProvider.GetRequiredService<RequestHandler<TestContext>>();

// Handle request
var context = new TestContext("Hello, world!");
using var scope = serviceProvider.CreateScope();

var result = await requestHandler.HandleAsync(context, scope.ServiceProvider);

internal record TestContext(string Text);

internal class ServiceInstanceFactory : IInstanceFactory
{
    public object CreateInstance(Type type, IServiceProvider serviceProvider)
    {
        return ActivatorUtilities.CreateInstance(serviceProvider, type);
    }
}
