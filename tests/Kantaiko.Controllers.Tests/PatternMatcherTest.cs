using Kantaiko.Controllers.Matchers;
using Xunit;

namespace Kantaiko.Controllers.Tests
{
    public class PatternMatcherTest
    {
        [Fact]
        public void ShouldMatchRequestWithParametersUsingPatternMatcher()
        {
            var textPatternMatcher = new PatternTextMatcher("{a} + {b}");
            var result = textPatternMatcher.Match("40 + 2");

            Assert.True(result.IsMatched);
            Assert.True(result.IsSuccess);

            Assert.Equal("40", result.Parameters!["a"]);
            Assert.Equal("2", result.Parameters!["b"]);
        }
    }
}
