using System;
using System.Threading.Tasks;
using Kantaiko.Controllers.Execution;
using Kantaiko.Controllers.Introspection.Factory;
using Kantaiko.Controllers.Result;
using Kantaiko.Controllers.Tests.Shared;
using Xunit;

namespace Kantaiko.Controllers.Tests;

public class EndpointInvocationTest
{
    [Theory]
    [InlineData("test 1")]
    [InlineData("test 2")]
    public async Task ShouldReportExceptionThrownByEndpoint(string input)
    {
        var controllerHandler = CreateControllerHandler();

        var context = new TestContext(input);
        var result = await controllerHandler.HandleAsync(context);

        var exceptionExitReason = Assert.IsType<ExceptionExitReason>(result.ExitReason);
        var invalidOperationException = Assert.IsType<InvalidOperationException>(exceptionExitReason.Exception);
        Assert.Equal("hi", invalidOperationException.Message);
    }

    private class TestController : Controller
    {
        [Pattern("test 1")]
        public void TestException()
        {
            throw new InvalidOperationException("hi");
        }

        [Pattern("test 2")]
        public async Task TestExceptionAsync()
        {
            await Task.Yield();
            throw new InvalidOperationException("hi");
        }
    }

    private static IControllerHandler<TestContext> CreateControllerHandler()
    {
        return TestUtils.CreateControllerHandler<EndpointInvocationTest>(
            introspectionBuilder => introspectionBuilder.AddEndpointMatching(),
            handlers =>
            {
                handlers.AddEndpointMatching();
                handlers.AddControllerInstantiation();
                handlers.AddEndpointInvocation();
            }
        );
    }
}
