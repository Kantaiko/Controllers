using Kantaiko.Controllers.EndpointMatching;
using Kantaiko.Controllers.Execution;
using Kantaiko.Controllers.ParameterConversion;
using Kantaiko.Controllers.Tests.Shared;
using Xunit;

namespace Kantaiko.Controllers.Tests;

public class ParameterDeconstructionTest
{
    [Theory]
    [InlineData("sum-pair 40 2")]
    [InlineData("sum-group 20 20 2")]
    public async Task ShouldDeconstructClassParameter(string input)
    {
        var executor = TestUtils.CreateControllerExecutor<ParameterDeconstructionTest>(builder =>
        {
            builder.AddEndpointMatching();
            builder.AddTextParameterConversion();
            builder.AddDefaultHandlers();
        });

        var context = new TestContext(input);
        var result = await executor.HandleAsync(context);

        result.ThrowOnError();
        Assert.Equal(42, result.EndpointResult);
    }

    private class TestController : Controller
    {
        [Pattern("sum-pair {a} {b}")]
        public int SumPair(NumberPair pair) => pair.A + pair.B;

        [Pattern("sum-group {a} {b} {c}")]
        public int SumGroup(NumberGroup numbers) => numbers.NumberPair.A + numbers.NumberPair.B + numbers.C;
    }

    [CompositeParameter]
    private class NumberGroup
    {
        public required NumberPair NumberPair { get; init; }
        public int C { get; init; }
    }

    [CompositeParameter]
    private class NumberPair
    {
        public int A { get; init; }
        public int B { get; init; }
    }
}
