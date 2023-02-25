using Kantaiko.Controllers.EndpointMatching;
using Kantaiko.Controllers.Execution;
using Kantaiko.Controllers.Tests.Shared;
using Xunit;

namespace Kantaiko.Controllers.Tests;

public class ErrorEndpointMatchingTest
{
    [Fact]
    public async Task ShouldReturnMatchingErrorImmediately()
    {
        var executor = TestUtils.CreateControllerExecutor<ErrorEndpointMatchingTest>(builder =>
        {
            builder.AddEndpointMatching();
            builder.AddDefaultHandlers();
        });

        var context = new TestContext("hi");
        var result = await executor.HandleAsync(context);

        Assert.Equal(ControllerErrorCodes.MatchingFailed, result.Error?.Code);
        Assert.Equal("Test", result.Error?.InnerError?.Code);
    }

    [Fact]
    public async Task ShouldMatchEndpointAfterError()
    {
        var executor = TestUtils.CreateControllerExecutor<ErrorEndpointMatchingTest>(builder =>
        {
            builder.AddEndpointMatching(false);
            builder.AddDefaultHandlers();
        });

        var context = new TestContext("hi");
        var result = await executor.HandleAsync(context);

        Assert.Equal("Hello!", result.EndpointResult);
    }

    private class TestController : Controller
    {
        [ErrorEndpointMatcher]
        [Pattern("hi")]
        public string Greet() => "Hello!";
    }

    private class ErrorEndpointMatcherAttribute : Attribute, IEndpointMatcher
    {
        public EndpointMatchingResult Match(EndpointMatchingContext context)
        {
            return new ControllerError("Test");
        }
    }
}
