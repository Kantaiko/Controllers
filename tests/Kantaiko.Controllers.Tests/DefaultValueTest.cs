using System;
using System.Threading.Tasks;
using Kantaiko.Controllers.Converters;
using Kantaiko.Controllers.Design.Parameters;
using Kantaiko.Controllers.Tests.Shared;
using Xunit;

namespace Kantaiko.Controllers.Tests
{
    public class DefaultValueTest : IClassFixture<RequestHandlerProvider>
    {
        private readonly RequestHandlerProvider _requestHandlerProvider;

        public DefaultValueTest(RequestHandlerProvider requestHandlerProvider)
        {
            _requestHandlerProvider = requestHandlerProvider;
        }

        [AttributeUsage(AttributeTargets.Parameter)]
        private class DefaultWorldAttribute : Attribute, IParameterDefaultValueResolverFactory
        {
            public IParameterDefaultValueResolver CreateParameterDefaultValueResolver(
                EndpointParameterDesignContext context)
            {
                return new WorldDefaultValueResolver();
            }
        }

        private class WorldDefaultValueResolver : IParameterDefaultValueResolver
        {
            public object ResolveDefaultValue(ParameterConversionContext context)
            {
                return "world";
            }
        }

        private class DefaultValueTestController : TestController
        {
            [RegexPattern(@"greet\s?(?<name>\w*)")]
            public string Greet([DefaultWorld] string name) => $"Hello, {name}!";
        }

        [Theory]
        [InlineData("greet", "Hello, world!")]
        [InlineData("greet Alex", "Hello, Alex!")]
        public async Task ShouldHandleParameterWithDefaultValue(string req, string res)
        {
            var context = new TestContext(req);
            var result = await _requestHandlerProvider.RequestHandler.HandleAsync(context);

            Assert.Equal(res, result.ReturnValue);
        }
    }
}
