using System;
using System.Linq;
using System.Threading.Tasks;
using Kantaiko.Controllers.Converters;
using Kantaiko.Controllers.Design.Controllers;
using Kantaiko.Controllers.Design.Endpoints;
using Kantaiko.Controllers.Design.Parameters;
using Kantaiko.Controllers.Design.Properties;
using Kantaiko.Controllers.Introspection;
using Kantaiko.Controllers.Tests.Shared;
using VerifyXunit;
using Xunit;

namespace Kantaiko.Controllers.Tests
{
    [UsesVerify]
    public class IntrospectionTest : IClassFixture<RequestHandlerProvider>
    {
        private readonly RequestHandlerProvider _requestHandlerProvider;

        public IntrospectionTest(RequestHandlerProvider requestHandlerProvider)
        {
            _requestHandlerProvider = requestHandlerProvider;
        }

        private class TestDefaultValueResolver : IParameterDefaultValueResolver
        {
            public object? ResolveDefaultValue(ParameterConversionContext context)
            {
                throw new NotImplementedException();
            }
        }

        private class DefaultAttribute : Attribute, IParameterDefaultValueResolverFactory
        {
            public IParameterDefaultValueResolver CreateParameterDefaultValueResolver(
                EndpointParameterDesignContext context)
            {
                return new TestDefaultValueResolver();
            }
        }

        private class TestAttribute : Attribute, IControllerDesignPropertyProvider,
            IEndpointDesignPropertyProvider, IParameterDesignPropertyProvider
        {
            public DesignPropertyCollection GetControllerDesignProperties() => new()
            {
                ["TestControllerProperty"] = "TestControllerPropertyValue"
            };

            public DesignPropertyCollection GetEndpointDesignProperties() => new()
            {
                ["TestEndpointProperty"] = "TestEndpointPropertyValue"
            };

            public DesignPropertyCollection GetParameterDesignProperties() => new()
            {
                ["TestParameterProperty"] = "TestParameterPropertyValue"
            };
        }

        [Test]
        private class IntrospectionTestController : TestController
        {
            [Test]
            [Pattern("introspection-test")]
            public void IntrospectionTest([Test] int a) { }

            [Pattern("introspection-test-prevent-deconstruction")]
            public void IntrospectionTestPreventDeconstruction([FromServices] ControllerInfo controllerInfo) { }

            [Pattern("introspection-test-default")]
            public void IntrospectionTestDefault([Default] int a) { }
        }

        [Fact]
        public async Task ShouldConstructCorrectControllerInfo()
        {
            var requestHandlerInfo = _requestHandlerProvider.RequestHandler.Info;
            var controllerInfo = requestHandlerInfo.Controllers.First(
                x => x.Type == typeof(IntrospectionTestController));

            await Verifier.Verify(controllerInfo).UseDirectory("__snapshots__");
        }
    }
}
