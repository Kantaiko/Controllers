using System.Threading.Tasks;
using Kantaiko.Controllers.Execution;
using Kantaiko.Controllers.Introspection.Factory;
using Kantaiko.Controllers.ParameterConversion.Text;
using Kantaiko.Controllers.Tests.Shared;
using Xunit;

namespace Kantaiko.Controllers.Tests;

public class AsyncEndpointTest
{
    [Fact]
    public async Task ShouldAwaitAsyncMethodResult()
    {
        var controllerHandler = TestUtils.CreateControllerHandler<AsyncEndpointTest>(
            (introspectionBuilder, lookupTypes) =>
            {
                var converterCollection = new TextParameterConverterCollection(lookupTypes);

                introspectionBuilder.AddEndpointMatching();
                introspectionBuilder.AddTextParameterConversion(converterCollection);
            },
            handlers =>
            {
                handlers.AddEndpointMatching();
                handlers.AddControllerInstantiation();
                handlers.AddEndpointInvocation();
                handlers.AddRequestCompletion();
            }
        );

        var result = await controllerHandler.HandleAsync(new TestContext("test"));

        Assert.Equal(42, result.ReturnValue);
    }

    private class TestController : Controller
    {
        [Pattern("test")]
        public async Task<int> GetResult()
        {
            await Task.Delay(10);

            return 42;
        }
    }
}
