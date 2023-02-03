using Kantaiko.Controllers.EndpointMatching;
using Kantaiko.Controllers.Execution;
using Kantaiko.Controllers.ParameterConversion;
using Kantaiko.Controllers.Tests.Shared;
using Xunit;

namespace Kantaiko.Controllers.Tests;

public class OptionalParameterTest
{
    [Theory]
    [InlineData("optional-nullable-value-type", null)]
    [InlineData("optional-value-type-with-attribute", 0)]
    [InlineData("optional-value-type-with-default", 42)]
    [InlineData("optional-value-type-with-default-via-attribute", 42)]
    [InlineData("optional-nullable-reference-type", null)]
    [InlineData("optional-reference-type-with-attribute", null)]
    [InlineData("optional-reference-type-with-default", "hi")]
    [InlineData("optional-reference-type-with-default-via-attribute", "hi")]
    public async Task ShouldProcessRequestWithOptionalParameter(string pattern, object? expectedResult)
    {
        var executor = TestUtils.CreateControllerExecutor<OptionalParameterTest>(builder =>
        {
            builder.AddEndpointMatching();
            builder.AddTextParameterConversion();
            builder.AddDefaultHandlers();
        });

        var context = new TestContext(pattern);
        var result = await executor.HandleAsync(context);

        result.ThrowOnError();
        Assert.Equal(expectedResult, result.EndpointResult);
    }

    private class TestController : Controller
    {
        [Pattern("optional-nullable-value-type")]
        public int? OptionalNullableValueType(int? a) => a;

        [Pattern("optional-value-type-with-attribute")]
        public int OptionalValueTypeWithAttribute([Parameter(IsOptional = true)] int a) => a;

        [Pattern("optional-value-type-with-default")]
        public int OptionalValueTypeWithDefault(int a = 42) => a;

        [Pattern("optional-nullable-reference-type")]
        public string? OptionalNullableReferenceType(string? a) => a;

        [Pattern("optional-reference-type-with-attribute")]
        public string OptionalReferenceTypeWithAttribute([Parameter(IsOptional = true)] string a) => a;

        [Pattern("optional-reference-type-with-default")]
        public string OptionalReferenceTypeWithDefault(string a = "hi") => a;

        [Pattern("optional-reference-type-with-default-via-attribute")]
        public string OptionalReferenceTypeWithDefaultViaAttribute(
            [Parameter(IsOptional = true, Default = "hi")]
            string a) => a;

        [Pattern("optional-value-type-with-default-via-attribute")]
        public int OptionalValueTypeWithDefaultViaAttribute(
            [Parameter(IsOptional = true, Default = 42)]
            int a) => a;
    }
}
