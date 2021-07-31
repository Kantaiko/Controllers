using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kantaiko.Controllers.Design.Controllers;
using Kantaiko.Controllers.Design.Endpoints;
using Kantaiko.Controllers.Design.Parameters;
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

        private class TestAttribute : Attribute, IControllerDesignPropertyProvider,
            IEndpointDesignPropertyProvider, IParameterDesignPropertyProvider
        {
            public IReadOnlyDictionary<string, object> GetControllerDesignProperties() => new Dictionary<string, object>
            {
                ["TestControllerProperty"] = "TestControllerPropertyValue"
            };

            public IReadOnlyDictionary<string, object> GetEndpointDesignProperties() => new Dictionary<string, object>
            {
                ["TestEndpointProperty"] = "TestEndpointPropertyValue"
            };

            public IReadOnlyDictionary<string, object> GetParameterDesignProperties() => new Dictionary<string, object>
            {
                ["TestParameterProperty"] = "TestParameterPropertyValue"
            };
        }

        [Test]
        private class IntrospectionTestController : TestController
        {
            [Test]
            [RegexPattern("introspection-test")]
            public void IntrospectionTest([Test] int a) { }

            [RegexPattern("introspection-test-prevent-deconstruction")]
            public void IntrospectionTestPreventDeconstruction([FromServices] ControllerInfo controllerInfo) { }
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
