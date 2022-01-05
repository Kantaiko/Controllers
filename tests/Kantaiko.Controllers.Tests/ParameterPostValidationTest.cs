using System.Threading.Tasks;
using Kantaiko.Controllers.Execution;
using Kantaiko.Controllers.Introspection.Factory;
using Kantaiko.Controllers.ParameterConversion.Text;
using Kantaiko.Controllers.ParameterConversion.Validation;
using Kantaiko.Controllers.Resources;
using Kantaiko.Controllers.Result;
using Kantaiko.Controllers.Tests.Shared;
using Kantaiko.Routing;
using Xunit;

namespace Kantaiko.Controllers.Tests;

public class ParameterPostValidationTest
{
    [Fact]
    public async Task ShouldPostValidateParameters()
    {
        var controllerHandler = CreateControllerHandler();

        var context = new TestContext("sum-validation 40 2");
        var result = await controllerHandler.Handle(context);

        Assert.True(result.HasReturnValue);
        Assert.Equal(42, result.ReturnValue);
    }

    [Fact]
    public async Task ShouldReportPostValidationError()
    {
        var controllerHandler = CreateControllerHandler();

        var context = new TestContext("sum-validation 40 3");
        var result = await controllerHandler.Handle(context);

        Assert.True(result.IsExited);

        var errorExitReason = Assert.IsType<ErrorExitReason>(result.ExitReason);
        Assert.Equal(string.Format(Locale.ShouldBeNoMoreThan, 2), errorExitReason.ErrorMessage);
    }

    private class TestController : Controller
    {
        [Pattern(@"sum-validation {a} {b}")]
        public int Sum([MinValue(40)] int a, [MaxValue(2)] int b) => a + b;
    }

    private static IHandler<TestContext, Task<ControllerExecutionResult>> CreateControllerHandler()
    {
        return TestUtils.CreateControllerHandler<ParameterPostValidationTest>(
            introspectionBuilder =>
            {
                introspectionBuilder.AddEndpointMatching();
                introspectionBuilder.AddTextParameterConversion();
                introspectionBuilder.AddParameterPostValidation();
            },
            pipelineBuilder =>
            {
                pipelineBuilder.AddEndpointMatching();
                pipelineBuilder.AddTextParameterConversion();
                pipelineBuilder.AddDefaultControllerHandling();
            }
        );
    }
}
