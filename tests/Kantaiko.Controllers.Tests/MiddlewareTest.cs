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

        private class TestMiddleware : EndpointMiddleware<TestContext>
        {
            public override EndpointMiddlewareStage Stage => EndpointMiddlewareStage.BeforeInstanceCreation;

            public override Task HandleAsync(EndpointMiddlewareContext<TestContext> context,
                CancellationToken cancellationToken)
            {
                if (context.RequestContext.ShouldOverrideValueViaMiddleware)
                {
                    context.ExecutionContext.Parameters["a"].Value = 42;
                }

                if (context.RequestContext.ShouldInterruptViaMiddleware)
                {
                    context.ShouldProcess = false;
                }

                return Task.CompletedTask;
            }
        }

        private class BeforeExecutionTestMiddleware : EndpointMiddleware<TestContext>
        {
            public override EndpointMiddlewareStage Stage => EndpointMiddlewareStage.BeforeExecution;

            public override Task HandleAsync(EndpointMiddlewareContext<TestContext> context,
                CancellationToken cancellationToken)
            {
                if (context.RequestContext.Result is not null)
                {
                    context.RequestContext.Result["instance"] = context.ExecutionContext.ControllerInstance!;
                }

                return Task.CompletedTask;
            }
        }

        private class BeforeCompletionTestMiddleware : EndpointMiddleware<TestContext>
        {
            public override EndpointMiddlewareStage Stage => EndpointMiddlewareStage.BeforeCompletion;

            public override Task HandleAsync(EndpointMiddlewareContext<TestContext> context,
                CancellationToken cancellationToken)
            {
                if (context.RequestContext.Result is not null)
                {
                    context.RequestContext.Result["result"] = context.ExecutionContext.ProcessingResult!;
                }

                return Task.CompletedTask;
            }
        }

        private class TestEndpointMiddleware : IEndpointMiddleware<TestContext>
        {
            public EndpointMiddlewareStage Stage => EndpointMiddlewareStage.BeforeExecution;

            public Task HandleAsync(EndpointMiddlewareContext<TestContext> context, CancellationToken cancellationToken)
            {
                context.ExecutionContext.Parameters["a"].Value = 42;
                return Task.CompletedTask;
            }
        }

        private class TestMiddlewareAttribute : Attribute, IEndpointMiddlewareFactory<TestContext>,
            IParameterMiddlewareFactory<TestContext>
        {
            public IEndpointMiddleware<TestContext> CreateEndpointMiddleware(EndpointDesignContext context)
            {
                return new TestEndpointMiddleware();
            }

            public IEndpointMiddleware<TestContext> CreateParameterMiddleware(EndpointParameterDesignContext context)
            {
                return new TestEndpointMiddleware();
            }
        }

        private class MiddlewareTestController : TestController
        {
            [Pattern("empty")]
            public void Empty() { }

            [Pattern(@"global-middleware {a}")]
            public int Middleware(int a) => a;

            [Pattern(@"endpoint-middleware {a}")]
            [TestMiddleware]
            public int EndpointMiddleware(int a) => a;

            [Pattern(@"parameter-middleware {a}")]
            public int ParameterMiddleware([TestMiddleware] int a) => a;
        }

        [Fact]
        public async Task ShouldOverrideParameterValueViaGlobalMiddleware()
        {
            var context = new TestContext("global-middleware 25", true);
            var result = await _requestHandlerProvider.RequestHandler.HandleAsync(context);

            Assert.True(result.HasReturnValue);
            Assert.Equal(42, result.ReturnValue);
        }

        [Fact]
        public async Task ShouldInterruptExecutionViaGlobalMiddleware()
        {
            var context = new TestContext("global-middleware 25", ShouldInterruptViaMiddleware: true);
            var result = await _requestHandlerProvider.RequestHandler.HandleAsync(context);

            Assert.True(result.IsExited);

            var interruptionExitReason = Assert.IsType<InterruptionExitReason>(result.ExitReason);
            Assert.Equal(EndpointMiddlewareStage.BeforeInstanceCreation, interruptionExitReason.MiddlewareStage);
        }

        [Fact]
        public async Task ShouldOverrideParameterValueViaEndpointMiddleware()
        {
            var context = new TestContext("endpoint-middleware 25");
            var result = await _requestHandlerProvider.RequestHandler.HandleAsync(context);

            Assert.True(result.HasReturnValue);
            Assert.Equal(42, result.ReturnValue);
        }

        [Fact]
        public async Task ShouldOverrideParameterValueViaParameterMiddleware()
        {
            var context = new TestContext("parameter-middleware 25");
            var result = await _requestHandlerProvider.RequestHandler.HandleAsync(context);

            Assert.True(result.HasReturnValue);
            Assert.Equal(42, result.ReturnValue);
        }

        [Fact]
        public async Task ShouldExposeControllerInstanceSinceBeforeExecutionStage()
        {
            var context = new TestContext("empty", Result: new Dictionary<string, object>());
            var result = await _requestHandlerProvider.RequestHandler.HandleAsync(context);

            Assert.IsType<MiddlewareTestController>(context.Result!["instance"]);
        }

        [Fact]
        public async Task ShouldExposeProcessingResultSinceBeforeCompletionStage()
        {
            var context = new TestContext("empty", Result: new Dictionary<string, object>());
            var result = await _requestHandlerProvider.RequestHandler.HandleAsync(context);

            Assert.Same(result, context.Result!["result"]);
        }
    }
}
