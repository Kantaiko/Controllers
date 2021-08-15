using System.Threading.Tasks;
using Kantaiko.Controllers.Resources;
using Kantaiko.Controllers.Result;
using Kantaiko.Controllers.Tests.Shared;
using Kantaiko.Controllers.Validation;
using Xunit;

namespace Kantaiko.Controllers.Tests
{
    public class ParameterPostValidationTest : IClassFixture<RequestHandlerProvider>
    {
        private readonly RequestHandlerProvider _requestHandlerProvider;

        public ParameterPostValidationTest(RequestHandlerProvider requestHandlerProvider)
        {
            _requestHandlerProvider = requestHandlerProvider;
        }

        private class ParameterPostValidationTestController : TestController
        {
            [RegexPattern(@"sum (?<a>\w+) (?<b>\w+)")]
            public int Sum([MinValue(40)] int a, [MaxValue(2)] int b) => a + b;
        }

        [Fact]
        public async Task ShouldPostValidateParameters()
        {
            var context = new TestContext("sum 40 2");
            var result = await _requestHandlerProvider.RequestHandler.HandleAsync(context);

            Assert.True(result.HasReturnValue);
            Assert.Equal(42, result.ReturnValue);
        }

        [Fact]
        public async Task ShouldReportPostValidationError()
        {
            var context = new TestContext("sum 40 3");
            var result = await _requestHandlerProvider.RequestHandler.HandleAsync(context);

            Assert.True(result.IsExited);

            var errorExitReason = Assert.IsType<ErrorExitReason>(result.ExitReason);
            Assert.Equal(RequestErrorStage.ParameterPostValidation, errorExitReason.Stage);
            Assert.Equal(string.Format(Locale.ShouldBeNoMoreThan, 2), errorExitReason.ErrorMessage);
        }
    }
}
