using System;
using Kantaiko.Controllers;
using Microsoft.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection();
serviceCollection.AddSingleton(provider =>
{
    var controllerCollection = ControllerCollection.FromAssemblies();

    return new RequestHandler<TestRequest>(controllerCollection,
        instanceFactory: new ServiceInstanceFactory(),
        serviceProvider: provider);
});

// Build service provider
var serviceProvider = serviceCollection.BuildServiceProvider();
var requestHandler = serviceProvider.GetRequiredService<RequestHandler<TestRequest>>();

// Handle request
var request = new TestRequest("Hello, world!");
using var scope = serviceProvider.CreateScope();

var result = await requestHandler.HandleAsync(request, scope.ServiceProvider);

internal record TestRequest(string Text);

internal class ServiceInstanceFactory : IInstanceFactory
{
    public object CreateInstance(Type type, IServiceProvider serviceProvider)
    {
        return ActivatorUtilities.CreateInstance(serviceProvider, type);
    }
}
