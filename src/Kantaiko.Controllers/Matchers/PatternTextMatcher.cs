using System.Text.RegularExpressions;

namespace Kantaiko.Controllers.Matchers
{
    public class PatternTextMatcher
    {
        private readonly RegexTextMatcher _regexTextMatcher;

        private static readonly Regex PatternGroupRegex = new(@"{(\w+)}", RegexOptions.Multiline);

        public PatternTextMatcher(string pattern)
        {
            var patternRegex = PatternGroupRegex.Replace(pattern, @"(?<$1>\w+)");

            _regexTextMatcher = new RegexTextMatcher(patternRegex);
        }

        public EndpointMatchResult Match(string text)
        {
            return _regexTextMatcher.Match(text);
        }
    }
}
