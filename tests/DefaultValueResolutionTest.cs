using System;
using System.Threading.Tasks;
using Kantaiko.Controllers.Execution;
using Kantaiko.Controllers.Introspection.Factory;
using Kantaiko.Controllers.Introspection.Factory.Attributes;
using Kantaiko.Controllers.Introspection.Factory.Context;
using Kantaiko.Controllers.ParameterConversion;
using Kantaiko.Controllers.ParameterConversion.DefaultValue;
using Kantaiko.Controllers.ParameterConversion.Text;
using Kantaiko.Controllers.Tests.Shared;
using Xunit;

namespace Kantaiko.Controllers.Tests;

public class DefaultValueResolutionTest
{
    [Theory]
    [InlineData("greet", "Hello, world!")]
    [InlineData("greet Alex", "Hello, Alex!")]
    public async Task ShouldHandleParameterWithDefaultValue(string request, string response)
    {
        var controllerHandler = TestUtils.CreateControllerHandler<DefaultValueResolutionTest>(
            introspectionInfo =>
            {
                introspectionInfo.AddEndpointMatching();
                introspectionInfo.AddTextParameterConversion();
                introspectionInfo.AddParameterDefaultValueResolution();
            },
            handlers =>
            {
                handlers.AddEndpointMatching();
                handlers.AddParameterConversion(h => h.AddTextParameterConversion());
                handlers.AddDefaultControllerHandling();
            }
        );

        var context = new TestContext(request);
        var result = await controllerHandler.HandleAsync(context);

        Assert.True(result.HasReturnValue);
        Assert.Equal(response, result.ReturnValue);
    }

    private class TestController : Controller
    {
        [Regex(@"greet\s?(?<name>\w*)")]
        public string Greet([DefaultWorld] string name) => $"Hello, {name}!";
    }

    [AttributeUsage(AttributeTargets.Parameter)]
    private class DefaultWorldAttribute : Attribute, IParameterDefaultValueResolverFactory
    {
        public IParameterDefaultValueResolver CreateParameterDefaultValueResolve(ParameterFactoryContext context)
        {
            return new WorldDefaultValueResolver();
        }
    }

    private class WorldDefaultValueResolver : IParameterDefaultValueResolver
    {
        public Task<object?> ResolveDefaultValueAsync(ParameterDefaultValueResolutionContext context)
        {
            return Task.FromResult<object?>("world");
        }
    }
}
