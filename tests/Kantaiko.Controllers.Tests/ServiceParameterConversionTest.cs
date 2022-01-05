using System;
using System.Threading.Tasks;
using Kantaiko.Controllers.Exceptions;
using Kantaiko.Controllers.Execution;
using Kantaiko.Controllers.Introspection.Factory;
using Kantaiko.Controllers.ParameterConversion.Handlers;
using Kantaiko.Controllers.Result;
using Kantaiko.Controllers.Tests.Shared;
using Kantaiko.Routing;
using Xunit;

namespace Kantaiko.Controllers.Tests;

public class ServiceParameterConversionTest
{
    [Fact]
    public async Task ShouldResolveParameterAsService()
    {
        var controllerHandler = CreateControllerHandler();
        var serviceProvider = new TestServiceProvider();

        var context = new TestContext("service 1", serviceProvider);
        var result = await controllerHandler.Handle(context);

        Assert.Equal(42, result.ReturnValue);
    }

    [Fact]
    public async Task ShouldThrowWhenParameterServiceIsNotRegistered()
    {
        var controllerHandler = CreateControllerHandler();
        var serviceProvider = new TestServiceProvider();

        async Task Action()
        {
            var context = new TestContext("service 2", serviceProvider);
            await controllerHandler.Handle(context);
        }

        await Assert.ThrowsAsync<ServiceNotFoundException>(Action);
    }

    [Fact]
    public async Task ShouldResolveNullWhenOptionalParameterServiceIsNotRegistered()
    {
        var controllerHandler = CreateControllerHandler();
        var serviceProvider = new TestServiceProvider();

        var context = new TestContext("service 3", serviceProvider);
        var result = await controllerHandler.Handle(context);

        Assert.True(result.IsMatched);
    }

    private class TestController : Controller
    {
        [Pattern("service 1")]
        public int ResolveService1([FromServices] int a) => a;

        [Pattern("service 2")]
        public void ResolveService2([FromServices] string a) { }

        [Pattern("service 3")]
        public void ResolveService3([FromServices] string? a = null) { }
    }

    private static IHandler<TestContext, Task<ControllerExecutionResult>> CreateControllerHandler()
    {
        return TestUtils.CreateControllerHandler<ServiceParameterConversionTest>(
            introspectionBuilder =>
            {
                introspectionBuilder.AddEndpointMatching();
                introspectionBuilder.AddPropertyProviderAttributes();
            },
            pipelineBuilder =>
            {
                pipelineBuilder.AddEndpointMatching();
                pipelineBuilder.AddParameterConversion(new[] { new ResolveServiceParameterHandler<TestContext>() });
                pipelineBuilder.AddDefaultControllerHandling();
            }
        );
    }

    private class TestServiceProvider : IServiceProvider
    {
        public object? GetService(Type serviceType)
        {
            if (serviceType == typeof(int))
                return 42;

            return null;
        }
    }
}
