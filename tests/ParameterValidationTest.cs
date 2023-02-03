using System.ComponentModel.DataAnnotations;
using Kantaiko.Controllers.EndpointMatching;
using Kantaiko.Controllers.Execution;
using Kantaiko.Controllers.ParameterConversion;
using Kantaiko.Controllers.ParameterValidation;
using Kantaiko.Controllers.Tests.Shared;
using Xunit;
using ValidationResult = Kantaiko.Controllers.ParameterValidation.ValidationResult;

namespace Kantaiko.Controllers.Tests;

public class ParameterValidationTest
{
    [Fact]
    public async Task ShouldValidateParameters()
    {
        var executor = CreateControllerExecutor();

        var context = new TestContext("sum-validation 40 2");
        var result = await executor.HandleAsync(context);

        result.ThrowOnError();
        Assert.Equal(42, result.EndpointResult);
    }

    [Fact]
    public async Task ShouldReportValidationAttributeError()
    {
        var controllerHandler = CreateControllerExecutor();

        var context = new TestContext("sum-validation 40 3");
        var result = await controllerHandler.HandleAsync(context);

        Assert.Equal(ControllerErrorCodes.ParameterValidationFailed, result.Error?.Code);
        Assert.Equal(ParameterErrorCodes.ValidationAttributeFailed, result.Error?.InnerError?.Code);

        var validationAttribute = ValidationErrorProperties.Of(result.Error!)?.ValidationAttribute;
        Assert.Equal(typeof(RangeAttribute), validationAttribute?.GetType());
    }

    [Fact]
    public async Task ShouldReportCustomValidatorError()
    {
        var controllerExecutor = CreateControllerExecutor();

        var context = new TestContext("sum-validation 41 2");
        var result = await controllerExecutor.HandleAsync(context);

        Assert.Equal(ControllerErrorCodes.ParameterValidationFailed, result.Error?.Code);
        Assert.Equal(EvenAttribute.ErrorCode, result.Error?.InnerError?.Code);
    }

    private class TestController : Controller
    {
        [Pattern(@"sum-validation {a} {b}")]
        public int Sum([Even] int a, [Range(0, 2)] int b) => a + b;
    }

    [AttributeUsage(AttributeTargets.Parameter)]
    private class EvenAttribute : Attribute, IParameterValidator<int>
    {
        public const string ErrorCode = "Test:NumberNotEven";

        public ValidationResult Validate(ParameterValidationContext<int> context)
        {
            return context.Value % 2 == 0 ? true : new ControllerError(ErrorCode);
        }
    }

    private static ControllerExecutor CreateControllerExecutor()
    {
        return TestUtils.CreateControllerExecutor<ParameterValidationTest>(builder =>
        {
            builder.AddEndpointMatching();
            builder.AddTextParameterConversion();
            builder.AddParameterValidation();
            builder.AddDefaultHandlers();
        });
    }
}
