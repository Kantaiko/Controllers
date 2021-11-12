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
            [Pattern("hi")]
            public string Greet() => "Hello!";

            [Pattern(@"{a} + {b}")]
            public int Sum(int a, int b) => a + b;

            [Pattern(@"{a} + ")]
            public int SumFailed(int a, int b) => 0;

            [Pattern("test-1")]
            public void TestFailed(ICustomTypeProvider customTypeProvider) { }

            [Pattern("test-exception")]
            public void TestException() => throw new InvalidOperationException("hi");
        }

        [Fact]
        public async Task ShouldProcessSimpleRequest()
        {
            var context = new TestContext("hi");
            var result = await _requestHandlerProvider.RequestHandler.HandleAsync(context);

            Assert.True(result.HasReturnValue);
            Assert.Equal("Hello!", result.ReturnValue);
        }

        [Fact]
        public async Task ShouldProcessRequestWithParameters()
        {
            var context = new TestContext("40 + 2");
            var result = await _requestHandlerProvider.RequestHandler.HandleAsync(context);

            Assert.True(result.HasReturnValue);
            Assert.Equal(42, result.ReturnValue);
        }

        [Fact]
        public async Task ShouldReportParameterConversionError()
        {
            var context = new TestContext("40 + test");
            var result = await _requestHandlerProvider.RequestHandler.HandleAsync(context);

            Assert.True(result.IsExited);
            var errorExitReason = Assert.IsType<ErrorExitReason>(result.ExitReason);

            Assert.Equal(RequestErrorStage.ParameterValidation, errorExitReason.Stage);
            Assert.Equal(Locale.IntegerRequired, errorExitReason.ErrorMessage);
        }

        [Fact]
        public async Task ShouldReportParameterExistenceCheckError()
        {
            var context = new TestContext("40 + ");
            var result = await _requestHandlerProvider.RequestHandler.HandleAsync(context);

            Assert.True(result.IsExited);
            var errorExitReason = Assert.IsType<ErrorExitReason>(result.ExitReason);

            Assert.Equal(RequestErrorStage.ParameterExistenceCheck, errorExitReason.Stage);
            Assert.Equal(string.Format(Locale.MissingRequiredParameter, "b"), errorExitReason.ErrorMessage);
        }

        [Fact]
        public async Task ShouldIgnoreUnmatchedRequest()
        {
            var context = new TestContext("who am i");
            var result = await _requestHandlerProvider.RequestHandler.HandleAsync(context);

            Assert.False(result.IsMatched);
        }

        [Fact]
        public async Task ShouldThrowConverterNotFoundExceptionWhenCannotDeconstructParameterType()
        {
            var context = new TestContext("test-1");

            async Task Action()
            {
                await _requestHandlerProvider.RequestHandler.HandleAsync(context);
            }

            await Assert.ThrowsAsync<ConverterNotFoundException>(Action);
        }

        [Fact]
        public async Task ShouldReportExceptionThrownByEndpoint()
        {
            var context = new TestContext("test-exception");

            var result = await _requestHandlerProvider.RequestHandler.HandleAsync(context);

            var exceptionExitReason = Assert.IsType<ExceptionExitReason>(result.ExitReason);
            var invalidOperationException = Assert.IsType<InvalidOperationException>(exceptionExitReason.Exception);
            Assert.Equal("hi", invalidOperationException.Message);
        }
    }
}
