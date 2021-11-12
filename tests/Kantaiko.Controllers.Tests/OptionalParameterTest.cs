using System.Threading.Tasks;
using Kantaiko.Controllers.Tests.Shared;
using Xunit;

namespace Kantaiko.Controllers.Tests
{
    public class OptionalParameterTest : IClassFixture<RequestHandlerProvider>
    {
        private readonly RequestHandlerProvider _requestHandlerProvider;

        public OptionalParameterTest(RequestHandlerProvider requestHandlerProvider)
        {
            _requestHandlerProvider = requestHandlerProvider;
        }

        private class OptionalTestController : TestController
        {
            [Pattern(@"optional-nullable-value-type")]
            public int? OptionalNullableValueType(int? a) => a;

            [Pattern(@"optional-value-type-with-attribute")]
            public int OptionalValueTypeWithAttribute([Parameter(IsOptional = true)] int a) => a;

            [Pattern(@"optional-value-type-with-default")]
            public int OptionalValueTypeWithDefault(int a = 42) => a;

            [Pattern(@"optional-nullable-reference-type")]
            public string? OptionalNullableReferenceType(string? a) => a;

            [Pattern(@"optional-reference-type-with-attribute")]
            public string OptionalReferenceTypeWithAttribute([Parameter(IsOptional = true)] string a) => a;

            [Pattern(@"optional-reference-type-with-default")]
            public string OptionalReferenceTypeWithDefault(string a = null!) => a;
        }

        [Theory]
        [InlineData("optional-nullable-value-type", null)]
        [InlineData("optional-value-type-with-attribute", 0)]
        [InlineData("optional-value-type-with-default", 42)]
        [InlineData("optional-nullable-reference-type", null)]
        [InlineData("optional-reference-type-with-attribute", null)]
        [InlineData("optional-reference-type-with-default", null)]
        public async Task ShouldProcessRequestWithOptionalParameter(string pattern, object? expectedResult)
        {
            var context = new TestContext(pattern);
            var result = await _requestHandlerProvider.RequestHandler.HandleAsync(context);

            Assert.Equal(expectedResult, result.ReturnValue);
        }
    }
}
