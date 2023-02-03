using Kantaiko.Controllers.EndpointMatching;
using Kantaiko.Controllers.Execution;
using Kantaiko.Controllers.Tests.Shared;
using Xunit;

namespace Kantaiko.Controllers.Tests;

public class EndpointMatchingTest
{
    [Fact]
    public async Task ShouldProcessSimpleRequest()
    {
        var executor = CreateControllerExecutor();

        var context = new TestContext("hi");
        var result = await executor.HandleAsync(context);

        Assert.Equal("Hello!", result.EndpointResult);
    }

    [Fact]
    public async Task ShouldReportUnmatchedRequest()
    {
        var executor = CreateControllerExecutor();

        var context = new TestContext("кто я");
        var result = await executor.HandleAsync(context);

        Assert.Equal(ControllerErrorCodes.NotMatched, result.Error?.Code);
    }

    private class TestController : Controller
    {
        [Pattern("hi")]
        public string Greet() => "Hello!";
    }

    private static ControllerExecutor CreateControllerExecutor()
    {
        return TestUtils.CreateControllerExecutor<EndpointMatchingTest>(builder =>
        {
            builder.AddEndpointMatching();
            builder.AddDefaultHandlers();
        });
    }
}
