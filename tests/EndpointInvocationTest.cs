using Kantaiko.Controllers.EndpointMatching;
using Kantaiko.Controllers.Execution;
using Kantaiko.Controllers.Tests.Shared;
using Xunit;

#pragma warning disable CS4014

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

    [Fact]
    public async Task ShouldNotAwaitResultIfTheCorrespondingOptionIsDisabled()
    {
        var executor = CreateControllerExecutor(awaitResult: false);

        var context = new TestContext("result-task");
        var result = await executor.HandleAsync(context);

        result.ThrowOnError();
        Assert.IsType<Task<int>>(result.EndpointResult);
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

    private static ControllerExecutor CreateControllerExecutor(bool awaitResult = true)
    {
        return TestUtils.CreateControllerExecutor<EndpointInvocationTest>(builder =>
        {
            builder.AddEndpointMatching();
            builder.AddDefaultHandlers(awaitResult: awaitResult);
        });
    }
}
