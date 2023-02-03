using Kantaiko.Controllers.EndpointMatching;
using Kantaiko.Controllers.Execution;
using Kantaiko.Controllers.Tests.Shared;
using Xunit;

namespace Kantaiko.Controllers.Tests;

public class EndpointInvocationTest
{
    [Theory]
    [InlineData("result")]
    [InlineData("result-task")]
    public async Task ShouldCollectValueReturnedByEndpoint(string input)
    {
        var executor = CreateControllerExecutor();

        var context = new TestContext(input);
        var result = await executor.HandleAsync(context);

        result.ThrowOnError();
        Assert.Equal(42, result.EndpointResult);
    }

    [Theory]
    [InlineData("exception")]
    [InlineData("exception-async")]
    public async Task ShouldReportExceptionThrownByEndpoint(string input)
    {
        var executor = CreateControllerExecutor();

        var context = new TestContext(input);
        var result = await executor.HandleAsync(context);

        Assert.Equal(ControllerErrorCodes.InvocationException, result.Error?.Code);
        Assert.Equal("hi", result.Error?.Exception?.Message);
    }

    private class TestController : Controller
    {
        [Pattern("result-task")]
        public Task<int> TestValueTaskResult()
        {
            return Task.FromResult(42);
        }

        [Pattern("result")]
        public int TestResult()
        {
            return 42;
        }

        [Pattern("exception-async")]
        public async Task TestExceptionAsync()
        {
            await Task.Yield();
            throw new InvalidOperationException("hi");
        }

        [Pattern("exception")]
        public void TestException()
        {
            throw new InvalidOperationException("hi");
        }
    }

    private static ControllerExecutor CreateControllerExecutor()
    {
        return TestUtils.CreateControllerExecutor<EndpointInvocationTest>(builder =>
        {
            builder.AddEndpointMatching();
            builder.AddDefaultHandlers();
        });
    }
}
