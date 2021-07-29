using System;
using System.Reflection;
using System.Threading.Tasks;
using Kantaiko.Controllers.Exceptions;
using Kantaiko.Controllers.Resources;
using Kantaiko.Controllers.Result;
using Kantaiko.Controllers.Tests.Shared;
using Xunit;

namespace Kantaiko.Controllers.Tests
{
    public class RequestHandlerTest : IClassFixture<RequestHandlerProvider>
    {
        private readonly RequestHandlerProvider _requestHandlerProvider;

        public RequestHandlerTest(RequestHandlerProvider requestHandlerProvider)
        {
            _requestHandlerProvider = requestHandlerProvider;
        }

        private class SimpleController : TestController
        {
            [RegexPattern("hi")]
            public string Greet() => "Hello!";

            [RegexPattern(@"(?<a>\w+) \+ (?<b>\w+)")]
            public int Sum(int a, int b) => a + b;

            [RegexPattern(@"sum (?<a>\w+)\s*(?<b>\w*)")]
            public int SumOptional(int a, int? b) => a + b ?? a;

            [RegexPattern(@"(?<a>\w*) \+ (?<b>\w*)")]
            public int SumFailed(int a, int b) => 0;

            [RegexPattern("test-1")]
            public void TestFailed(ICustomTypeProvider customTypeProvider) { }


            [RegexPattern("test-exception")]
            public void TestException() => throw new InvalidOperationException("hi");
        }

        [Fact]
        public async Task ShouldProcessSimpleRequest()
        {
            var request = new TestRequest("hi");
            var result = await _requestHandlerProvider.RequestHandler.HandleAsync(request);

            Assert.True(result.HasReturnValue);
            Assert.Equal("Hello!", result.ReturnValue);
        }

        [Fact]
        public async Task ShouldProcessRequestWithParameters()
        {
            var request = new TestRequest("40 + 2");
            var result = await _requestHandlerProvider.RequestHandler.HandleAsync(request);

            Assert.True(result.HasReturnValue);
            Assert.Equal(42, result.ReturnValue);
        }

        [Fact]
        public async Task ShouldProcessRequestWithOptionalParameter()
        {
            var request = new TestRequest("sum 42");
            var result = await _requestHandlerProvider.RequestHandler.HandleAsync(request);

            Assert.True(result.HasReturnValue);
            Assert.Equal(42, result.ReturnValue);
        }

        [Fact]
        public async Task ShouldReportParameterConversionError()
        {
            var request = new TestRequest("40 + test");
            var result = await _requestHandlerProvider.RequestHandler.HandleAsync(request);

            Assert.True(result.IsExited);
            var errorExitReason = Assert.IsType<ErrorExitReason>(result.ExitReason);

            Assert.Equal(RequestErrorStage.ParameterValidation, errorExitReason.Stage);
            Assert.Equal(Locale.IntegerRequired, errorExitReason.ErrorMessage);
        }

        [Fact]
        public async Task ShouldReportParameterExistenceCheckError()
        {
            var request = new TestRequest("40 + ");
            var result = await _requestHandlerProvider.RequestHandler.HandleAsync(request);

            Assert.True(result.IsExited);
            var errorExitReason = Assert.IsType<ErrorExitReason>(result.ExitReason);

            Assert.Equal(RequestErrorStage.ParameterExistenceCheck, errorExitReason.Stage);
            Assert.Equal(string.Format(Locale.MissingRequiredParameter, "b"), errorExitReason.ErrorMessage);
        }

        [Fact]
        public async Task ShouldIgnoreUnmatchedRequest()
        {
            var request = new TestRequest("who am i");
            var result = await _requestHandlerProvider.RequestHandler.HandleAsync(request);

            Assert.False(result.IsMatched);
        }

        [Fact]
        public async Task ShouldThrowConverterNotFoundExceptionWhenCannotDeconstructParameterType()
        {
            var request = new TestRequest("test-1");

            async Task Action()
            {
                await _requestHandlerProvider.RequestHandler.HandleAsync(request);
            }

            await Assert.ThrowsAsync<ConverterNotFoundException>(Action);
        }

        [Fact]
        public async Task ShouldReportExceptionThrowByEndpoint()
        {
            var request = new TestRequest("test-exception");

            var result = await _requestHandlerProvider.RequestHandler.HandleAsync(request);

            var exceptionExitReason = Assert.IsType<ExceptionExitReason>(result.ExitReason);
            var invalidOperationException = Assert.IsType<InvalidOperationException>(exceptionExitReason.Exception);
            Assert.Equal("hi", invalidOperationException.Message);
        }
    }
}
