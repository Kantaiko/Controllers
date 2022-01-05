using System.Text.RegularExpressions;

namespace Kantaiko.Controllers.Matching.Text;

public class PatternTextMatcher
{
    private readonly RegexTextMatcher _regexTextMatcher;

    private static readonly Regex PatternGroupRegex = new(@"{(\w+)}", RegexOptions.Multiline);

    public PatternTextMatcher(string pattern)
    {
        var escapedPattern = Regex.Escape(pattern).Replace(@"\{", "{");
        var patternRegex = PatternGroupRegex.Replace(escapedPattern, @"(?<$1>\S+)");

        _regexTextMatcher = new RegexTextMatcher(patternRegex);
    }

    public EndpointMatchResult Match(string text)
    {
        return _regexTextMatcher.Match(text);
    }
}
