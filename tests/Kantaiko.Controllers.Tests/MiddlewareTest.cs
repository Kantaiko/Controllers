using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Kantaiko.Controllers.Design.Endpoints;
using Kantaiko.Controllers.Design.Parameters;
using Kantaiko.Controllers.Middleware;
using Kantaiko.Controllers.Result;
using Kantaiko.Controllers.Tests.Shared;
using Xunit;

namespace Kantaiko.Controllers.Tests
{
    public class MiddlewareTest : IClassFixture<RequestHandlerProvider>
    {
        private readonly RequestHandlerProvider _requestHandlerProvider;

        public MiddlewareTest(RequestHandlerProvider requestHandlerProvider)
        {
            _requestHandlerProvider = requestHandlerProvider;
        }

        private class TestMiddleware : EndpointMiddleware<TestRequest>
        {
            public override EndpointMiddlewareStage Stage => EndpointMiddlewareStage.BeforeInstanceCreation;

            public override Task HandleAsync(EndpointMiddlewareContext<TestRequest> context,
                CancellationToken cancellationToken)
            {
                if (context.Request.ShouldOverrideValueViaMiddleware)
                {
                    context.ExecutionContext.Parameters["a"].Value = 42;
                }

                if (context.Request.ShouldInterruptViaMiddleware)
                {
                    context.ShouldProcess = false;
                }

                return Task.CompletedTask;
            }
        }

        private class BeforeExecutionTestMiddleware : EndpointMiddleware<TestRequest>
        {
            public override EndpointMiddlewareStage Stage => EndpointMiddlewareStage.BeforeExecution;

            public override Task HandleAsync(EndpointMiddlewareContext<TestRequest> context,
                CancellationToken cancellationToken)
            {
                if (context.Request.Result is not null)
                {
                    context.Request.Result["instance"] = context.ExecutionContext.ControllerInstance!;
                }

                return Task.CompletedTask;
            }
        }

        private class BeforeCompletionTestMiddleware : EndpointMiddleware<TestRequest>
        {
            public override EndpointMiddlewareStage Stage => EndpointMiddlewareStage.BeforeCompletion;

            public override Task HandleAsync(EndpointMiddlewareContext<TestRequest> context,
                CancellationToken cancellationToken)
            {
                if (context.Request.Result is not null)
                {
                    context.Request.Result["result"] = context.ExecutionContext.ProcessingResult!;
                }

                return Task.CompletedTask;
            }
        }

        private class TestEndpointMiddleware : IEndpointMiddleware<TestRequest>
        {
            public EndpointMiddlewareStage Stage => EndpointMiddlewareStage.BeforeExecution;

            public Task HandleAsync(EndpointMiddlewareContext<TestRequest> context, CancellationToken cancellationToken)
            {
                context.ExecutionContext.Parameters["a"].Value = 42;
                return Task.CompletedTask;
            }
        }

        private class TestMiddlewareAttribute : Attribute, IEndpointMiddlewareFactory<TestRequest>,
            IParameterMiddlewareFactory<TestRequest>
        {
            public IEndpointMiddleware<TestRequest> CreateEndpointMiddleware(EndpointDesignContext context)
            {
                return new TestEndpointMiddleware();
            }

            public IEndpointMiddleware<TestRequest> CreateParameterMiddleware(EndpointParameterDesignContext context)
            {
                return new TestEndpointMiddleware();
            }
        }

        private class MiddlewareTestController : TestController
        {
            [RegexPattern("empty")]
            public void Empty() { }

            [RegexPattern(@"global-middleware (?<a>\w+)")]
            public int Middleware(int a) => a;

            [RegexPattern(@"endpoint-middleware (?<a>\w+)")]
            [TestMiddleware]
            public int EndpointMiddleware(int a) => a;

            [RegexPattern(@"parameter-middleware (?<a>\w+)")]
            public int ParameterMiddleware([TestMiddleware] int a) => a;
        }

        [Fact]
        public async Task ShouldOverrideParameterValueViaGlobalMiddleware()
        {
            var request = new TestRequest("global-middleware 25", true);
            var result = await _requestHandlerProvider.RequestHandler.HandleAsync(request);

            Assert.True(result.HasReturnValue);
            Assert.Equal(42, result.ReturnValue);
        }

        [Fact]
        public async Task ShouldInterruptExecutionViaGlobalMiddleware()
        {
            var request = new TestRequest("global-middleware 25", ShouldInterruptViaMiddleware: true);
            var result = await _requestHandlerProvider.RequestHandler.HandleAsync(request);

            Assert.True(result.IsExited);

            var interruptionExitReason = Assert.IsType<InterruptionExitReason>(result.ExitReason);
            Assert.Equal(EndpointMiddlewareStage.BeforeInstanceCreation, interruptionExitReason.MiddlewareStage);
        }

        [Fact]
        public async Task ShouldOverrideParameterValueViaEndpointMiddleware()
        {
            var request = new TestRequest("endpoint-middleware 25");
            var result = await _requestHandlerProvider.RequestHandler.HandleAsync(request);

            Assert.True(result.HasReturnValue);
            Assert.Equal(42, result.ReturnValue);
        }

        [Fact]
        public async Task ShouldOverrideParameterValueViaParameterMiddleware()
        {
            var request = new TestRequest("parameter-middleware 25");
            var result = await _requestHandlerProvider.RequestHandler.HandleAsync(request);

            Assert.True(result.HasReturnValue);
            Assert.Equal(42, result.ReturnValue);
        }

        [Fact]
        public async Task ShouldExposeControllerInstanceSinceBeforeExecutionStage()
        {
            var request = new TestRequest("empty", Result: new Dictionary<string, object>());
            var result = await _requestHandlerProvider.RequestHandler.HandleAsync(request);

            Assert.IsType<MiddlewareTestController>(request.Result!["instance"]);
        }

        [Fact]
        public async Task ShouldExposeProcessingResultSinceBeforeCompletionStage()
        {
            var request = new TestRequest("empty", Result: new Dictionary<string, object>());
            var result = await _requestHandlerProvider.RequestHandler.HandleAsync(request);

            Assert.Same(result, request.Result!["result"]);
        }
    }
}
