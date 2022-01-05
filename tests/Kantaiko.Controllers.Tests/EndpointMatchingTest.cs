using System.Threading.Tasks;
using Kantaiko.Controllers.Execution;
using Kantaiko.Controllers.Introspection.Factory;
using Kantaiko.Controllers.Result;
using Kantaiko.Controllers.Tests.Shared;
using Kantaiko.Routing;
using Xunit;

namespace Kantaiko.Controllers.Tests;

public class EndpointMatchingTest
{
    [Fact]
    public async Task ShouldProcessSimpleRequest()
    {
        var controllerHandler = CreateControllerHandler();

        var context = new TestContext("hi");
        var result = await controllerHandler.Handle(context);

        Assert.True(result.HasReturnValue);
        Assert.Equal("Hello!", result.ReturnValue);
    }

    [Fact]
    public async Task ShouldIgnoreUnmatchedRequest()
    {
        var controllerHandler = CreateControllerHandler();

        var context = new TestContext("кто я");
        var result = await controllerHandler.Handle(context);

        Assert.False(result.IsMatched);
    }

    private class TestController : Controller
    {
        [Pattern("hi")]
        public string Greet() => "Hello!";
    }

    private static IHandler<TestContext, Task<ControllerExecutionResult>> CreateControllerHandler()
    {
        return TestUtils.CreateControllerHandler<EndpointMatchingTest>(
            introspectionBuilder => introspectionBuilder.AddEndpointMatching(),
            pipelineBuilder =>
            {
                pipelineBuilder.AddEndpointMatching();
                pipelineBuilder.AddDefaultControllerHandling();
            }
        );
    }
}
