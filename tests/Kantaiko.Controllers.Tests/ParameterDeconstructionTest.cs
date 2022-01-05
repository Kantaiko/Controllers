using System;
using System.Threading.Tasks;
using Kantaiko.Controllers.Execution;
using Kantaiko.Controllers.Introspection.Factory;
using Kantaiko.Controllers.Introspection.Factory.Deconstruction;
using Kantaiko.Controllers.ParameterConversion.Text;
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
        var controllerHandler = TestUtils.CreateControllerHandler<ParameterDeconstructionTest>(
            introspectionBuilder =>
            {
                introspectionBuilder.SetDeconstructionValidator(new TestDeconstructionValidator());
                introspectionBuilder.AddEndpointMatching();
                introspectionBuilder.AddTextParameterConversion();
            },
            pipelineBuilder =>
            {
                pipelineBuilder.AddEndpointMatching();
                pipelineBuilder.AddTextParameterConversion();
                pipelineBuilder.AddDefaultControllerHandling();
            }
        );

        var context = new TestContext(input);
        var result = await controllerHandler.Handle(context);

        Assert.True(result.HasReturnValue);
        Assert.Equal(42, result.ReturnValue);
    }

    private class TestController : Controller
    {
        [Pattern("sum-pair {a} {b}")]
        public int SumPair(NumberPair pair) => pair.A + pair.B;

        [Pattern("sum-group {a} {b} {c}")]
        public int SumGroup(NumberGroup numbers) => numbers.NumberPair.A + numbers.NumberPair.B + numbers.C;
    }

    private class TestDeconstructionValidator : IDeconstructionValidator
    {
        public bool CanDeconstruct(Type type) => true;
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
}
