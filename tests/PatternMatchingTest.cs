using Kantaiko.Controllers.Matching.Text;
using Kantaiko.Properties;
using Xunit;

namespace Kantaiko.Controllers.Tests;

public class TextPatternMatcherTest
{
    [Fact]
    public void ShouldMatchRequestWithParametersUsingPatternMatcher()
    {
        var textPatternMatcher = new PatternTextMatcher("{a} + {b}");
        var result = textPatternMatcher.Match("40 + 2");

        Assert.True(result.IsMatched);
        Assert.True(result.IsSuccess);

        var properties = result.Properties?.Get<MatchingTextParameterConversionProperties>();

        Assert.Equal("40", properties?.Parameters?["a"]);
        Assert.Equal("2", properties?.Parameters?["b"]);
    }
}
