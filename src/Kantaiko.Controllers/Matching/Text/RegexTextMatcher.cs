using System.Collections.Generic;
using System.Text.RegularExpressions;
using Kantaiko.Properties.Immutable;

namespace Kantaiko.Controllers.Matching.Text;

public class RegexTextMatcher
{
    private readonly Regex _pattern;

    public RegexTextMatcher(string pattern, RegexOptions regexOptions = RegexOptions.None)
    {
        _pattern = new Regex(pattern, regexOptions);
    }

    public EndpointMatchResult Match(string text)
    {
        var match = _pattern.Match(text);
        if (!match.Success) return EndpointMatchResult.NotMatched;

        var parameters = new Dictionary<string, string>();

        foreach (Group matchGroup in match.Groups)
        {
            if (!string.IsNullOrEmpty(matchGroup.Name) && !string.IsNullOrEmpty(matchGroup.Value))
            {
                parameters[matchGroup.Name] = matchGroup.Value;
            }
        }

        var properties = new MatchingTextParameterConversionProperties { Parameters = parameters };

        return EndpointMatchResult.Success(ImmutablePropertyCollection.Empty.Set(properties));
    }
}
