using System.Threading.Tasks;
using Kantaiko.Controllers.Tests.Shared;
using Xunit;

namespace Kantaiko.Controllers.Tests
{
    public class ParameterDeconstructionTest : IClassFixture<RequestHandlerProvider>
    {
        private readonly RequestHandlerProvider _requestHandlerProvider;

        public ParameterDeconstructionTest(RequestHandlerProvider requestHandlerProvider)
        {
            _requestHandlerProvider = requestHandlerProvider;
        }

        private class NumberGroup
        {
            public NumberPair NumberPair { get; set; } = null!;
            public int C { get; set; }
        }

        private class NumberPair
        {
            public int A { get; set; }
            public int B { get; set; }
        }

        private class DeconstructionTestController : TestController
        {
            [RegexPattern(@"sum pair (?<a>\w+) (?<b>\w+)")]
            public int SumPair(NumberPair pair) => pair.A + pair.B;

            [RegexPattern(@"sum group (?<a>\w+) (?<b>\w+) (?<c>\w+)")]
            public int SumGroup(NumberGroup numbers) => numbers.NumberPair.A + numbers.NumberPair.B + numbers.C;
        }

        [Fact]
        public async Task ShouldDeconstructClassParameter()
        {
            var context = new TestContext("sum pair 40 2");
            var result = await _requestHandlerProvider.RequestHandler.HandleAsync(context);

            Assert.True(result.HasReturnValue);
            Assert.Equal(42, result.ReturnValue);
        }

        [Fact]
        public async Task ShouldDeconstructClassParameterWithNestedParameters()
        {
            var context = new TestContext("sum group 20 20 2");
            var result = await _requestHandlerProvider.RequestHandler.HandleAsync(context);

            Assert.True(result.HasReturnValue);
            Assert.Equal(42, result.ReturnValue);
        }
    }
}
