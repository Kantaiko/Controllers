using Kantaiko.Controllers.EndpointMatching;
using Kantaiko.Controllers.Exceptions;
using Kantaiko.Controllers.Execution;
using Kantaiko.Controllers.ParameterConversion;
using Kantaiko.Controllers.Tests.Shared;
using Xunit;

namespace Kantaiko.Controllers.Tests;

public class ServiceParameterConversionTest
{
    [Fact]
    public async Task ShouldResolveParameterAsService()
    {
        var controllerExecutor = CreateControllerExecutor();
        var serviceProvider = new TestServiceProvider();

        var context = new TestContext("service 1");
        var result = await controllerExecutor.HandleAsync(context, serviceProvider);

        result.ThrowOnError();
        Assert.Equal(42, result.EndpointResult);
    }

    [Fact]
    public async Task ShouldThrowWhenParameterServiceIsNotRegistered()
    {
        var controllerExecutor = CreateControllerExecutor();
        var serviceProvider = new TestServiceProvider();

        var context = new TestContext("service 2");
        var result = await controllerExecutor.HandleAsync(context, serviceProvider);

        Assert.Equal(ControllerErrorCodes.ParameterConversionException, result.Error?.Code);
        Assert.IsType<ServiceNotFoundException>(result.Error?.Exception);
    }

    [Fact]
    public async Task ShouldResolveNullWhenOptionalParameterServiceIsNotRegistered()
    {
        var controllerExecutor = CreateControllerExecutor();
        var serviceProvider = new TestServiceProvider();

        var context = new TestContext("service 3");
        var result = await controllerExecutor.HandleAsync(context, serviceProvider);

        result.ThrowOnError();
        Assert.True(result.Success);
    }

    private class TestController : Controller
    {
        [Pattern("service 1")]
        public int ResolveService1([FromServices] int a) => a;

        [Pattern("service 2")]
        public void ResolveService2([FromServices] string a) { }

        [Pattern("service 3")]
        public void ResolveService3([FromServices] string? a) { }
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

    private static ControllerExecutor CreateControllerExecutor()
    {
        return TestUtils.CreateControllerExecutor<ServiceParameterConversionTest>(builder =>
        {
            builder.AddEndpointMatching();
            builder.AddTextParameterConversion();
            builder.AddDefaultHandlers();
        });
    }
}
