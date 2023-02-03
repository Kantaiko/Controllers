using Kantaiko.Controllers.EndpointMatching.Text;
using Kantaiko.Properties;
using Xunit;

namespace Kantaiko.Controllers.Tests;

public class PatternTextMatcherTest
{
    [Fact]
    public void ShouldMatchRequestWithParametersUsingPatternMatcher()
    {
        var textPatternMatcher = new PatternTextMatcher("{a} + {b}");
        var result = textPatternMatcher.Match("40 + 2");

        Assert.True(result.Matched);

        var properties = result.MatchProperties.Get<TextMatchProperties>();

        Assert.Equal("40", properties?.Parameters["a"]);
        Assert.Equal("2", properties?.Parameters["b"]);
    }
}
