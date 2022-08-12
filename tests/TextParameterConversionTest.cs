using System;
using System.Threading.Tasks;
using Kantaiko.Controllers.Execution;
using Kantaiko.Controllers.Introspection.Factory;
using Kantaiko.Controllers.ParameterConversion;
using Kantaiko.Controllers.ParameterConversion.Text;
using Kantaiko.Controllers.Resources;
using Kantaiko.Controllers.Result;
using Kantaiko.Controllers.Tests.Shared;
using Xunit;

namespace Kantaiko.Controllers.Tests;

public class TextParameterConversionTest
{
    [Fact]
    public async Task ShouldReportParameterExistenceCheckError()
    {
        var controllerHandler = CreateControllerHandler();

        var context = new TestContext("test 40");
        var result = await controllerHandler.HandleAsync(context);

        Assert.True(result.IsExited);
        var errorExitReason = Assert.IsType<ErrorExitReason>(result.ExitReason);

        Assert.Equal(string.Format(Locale.MissingRequiredParameter, "b"), errorExitReason.ErrorMessage);
    }

    private class TestController : Controller
    {
        [Pattern("test {a}")]
        public void Test(int a, int b) => throw new InvalidOperationException();
    }

    private static IControllerHandler<TestContext> CreateControllerHandler()
    {
        return TestUtils.CreateControllerHandler<TextParameterConversionTest>(
            introspectionBuilder =>
            {
                introspectionBuilder.AddEndpointMatching();
                introspectionBuilder.AddTextParameterConversion();
            },
            handlers =>
            {
                handlers.AddEndpointMatching();
                handlers.AddParameterConversion(h => h.AddTextParameterConversion());
                handlers.AddDefaultControllerHandling();
            }
        );
    }
}
