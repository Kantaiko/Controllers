using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kantaiko.Controllers.Exceptions;
using Kantaiko.Controllers.Execution;
using Kantaiko.Controllers.Execution.Handlers;
using Kantaiko.Controllers.Introspection;
using Kantaiko.Controllers.Introspection.Factory;
using Kantaiko.Controllers.Introspection.Factory.Attributes;
using Kantaiko.Controllers.Introspection.Factory.Context;
using Kantaiko.Controllers.Result;
using Kantaiko.Controllers.Tests.Shared;
using Kantaiko.Routing;
using Xunit;

namespace Kantaiko.Controllers.Tests;

public class ExecutionHandlerTest
{
    [Fact]
    public async Task ShouldUseCustomExecutionHandler()
    {
        var controllerHandler = TestUtils.CreateControllerHandler<ExecutionHandlerTest>(
            introspectionBuilder => introspectionBuilder.AddEndpointMatching(),
            pipelineBuilder =>
            {
                pipelineBuilder.AddEndpointMatching();
                pipelineBuilder.AddHandler(new TestExecutionHandler());
                pipelineBuilder.AddHandler(new ConstructParametersHandler<TestContext>());
                pipelineBuilder.AddDefaultControllerHandling();
            }
        );

        var context = new TestContext("test 1");
        var result = await controllerHandler.Handle(context);

        Assert.Equal(42, result.ReturnValue);
    }

    [Theory]
    [InlineData("test 2")]
    [InlineData("test 3")]
    public async Task ShouldUseSubHandler(string input)
    {
        var controllerHandler = TestUtils.CreateControllerHandler<ExecutionHandlerTest>(
            introspectionBuilder =>
            {
                introspectionBuilder.AddEndpointMatching();
                introspectionBuilder.AddSubHandlerAttributes();
            },
            pipelineBuilder =>
            {
                pipelineBuilder.AddEndpointMatching();
                pipelineBuilder.AddSubHandlerExecution();
                pipelineBuilder.AddHandler(new ConstructParametersHandler<TestContext>());
                pipelineBuilder.AddDefaultControllerHandling();
            }
        );

        var result = await controllerHandler.Handle(new TestContext(input));

        Assert.Equal(42, result.ReturnValue);
    }

    private class TestController : Controller
    {
        [Pattern("test 1")]
        public int Test(int a) => a;
    }

    private class TestController2 : Controller
    {
        [Pattern("test 2"), SubHandler]
        public int Test(int a) => a;
    }

    [SubHandler]
    private class TestController3 : Controller
    {
        [Pattern("test 3")]
        public int Test(int a) => a;
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    private class SubHandlerAttribute : Attribute, IControllerExecutionHandlerFactory<TestContext>,
        IEndpointExecutionHandlerFactory<TestContext>
    {
        public IChainedHandler<ControllerExecutionContext<TestContext>, Task<ControllerExecutionResult>> CreateHandler(
            ControllerFactoryContext context)
        {
            return new TestExecutionHandler();
        }

        public IChainedHandler<ControllerExecutionContext<TestContext>, Task<ControllerExecutionResult>> CreateHandler(
            EndpointFactoryContext context)
        {
            return new TestExecutionHandler();
        }
    }

    private class TestExecutionHandler : ControllerExecutionHandler<TestContext>
    {
        protected override Task<ControllerExecutionResult> HandleAsync(ControllerExecutionContext<TestContext> context,
            NextAction next)
        {
            PropertyNullException.ThrowIfNull(context.Endpoint);

            context.ResolvedParameters = new Dictionary<EndpointParameterInfo, object?>();

            foreach (var parameterInfo in context.Endpoint.Parameters)
            {
                context.ResolvedParameters[parameterInfo] = 42;
            }

            return next();
        }
    }
}
