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

            var result = await requestHandler.HandleAsync(new TestRequest("service 1"));

            Assert.Equal(42, result.ReturnValue);
        }

        [Fact]
        public async Task ShouldThrowWhenParameterServiceIsNotRegistered()
        {
            var requestHandler = CreateRequestHandler();

            async Task Action()
            {
                await requestHandler.HandleAsync(new TestRequest("service 2"));
            }

            await Assert.ThrowsAsync<ServiceNotFoundException>(Action);
        }

        [Fact]
        public async Task ShouldResolveNullWhenOptionalParameterServiceIsNotRegistered()
        {
            var requestHandler = CreateRequestHandler();

            var result = await requestHandler.HandleAsync(new TestRequest("service 3"));

            Assert.True(result.IsMatched);
        }

        private static RequestHandler<TestRequest> CreateRequestHandler()
        {
            var controllerCollection = new ControllerCollection(new[] {typeof(ServiceTestController)});
            return new RequestHandler<TestRequest>(controllerCollection, serviceProvider: new TestServiceProvider());
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
            [RegexPattern("service 1")]
            public int ResolveService1([FromServices] int a) => a;

            [RegexPattern("service 2")]
            public void ResolveService2([FromServices] string a) { }

            [RegexPattern("service 3")]
            public void ResolveService3([FromServices] string? a = null) { }
        }
    }
}
