using System;
using System.Threading.Tasks;
using Kantaiko.Controllers.Exceptions;
using Kantaiko.Controllers.Tests.Shared;
using Xunit;

namespace Kantaiko.Controllers.Tests
{
    public class ServiceConverterTest
    {
        [Fact]
        public async Task ShouldResolveParameterAsService()
        {
            var requestHandler = CreateRequestHandler();

            var result = await requestHandler.HandleAsync(new TestContext("service 1"));

            Assert.Equal(42, result.ReturnValue);
        }

        [Fact]
        public async Task ShouldThrowWhenParameterServiceIsNotRegistered()
        {
            var requestHandler = CreateRequestHandler();

            async Task Action()
            {
                await requestHandler.HandleAsync(new TestContext("service 2"));
            }

            await Assert.ThrowsAsync<ServiceNotFoundException>(Action);
        }

        [Fact]
        public async Task ShouldResolveNullWhenOptionalParameterServiceIsNotRegistered()
        {
            var requestHandler = CreateRequestHandler();

            var result = await requestHandler.HandleAsync(new TestContext("service 3"));

            Assert.True(result.IsMatched);
        }

        private static RequestHandler<TestContext> CreateRequestHandler()
        {
            var controllerCollection = new ControllerCollection(new[] { typeof(ServiceTestController) });
            return new RequestHandler<TestContext>(controllerCollection, serviceProvider: new TestServiceProvider());
        }

        private class TestServiceProvider : IServiceProvider
        {
            public object? GetService(Type serviceType)
            {
                if (serviceType == typeof(int))
                    return 42;

                return null;
            }
        }

        private class ServiceTestController : TestController
        {
            [Pattern("service 1")]
            public int ResolveService1([FromServices] int a) => a;

            [Pattern("service 2")]
            public void ResolveService2([FromServices] string a) { }

            [Pattern("service 3")]
            public void ResolveService3([FromServices] string? a = null) { }
        }
    }
}
