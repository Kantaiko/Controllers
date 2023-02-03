using System.Text.RegularExpressions;
using Kantaiko.Properties.Immutable;

namespace Kantaiko.Controllers.EndpointMatching.Text;

/// <summary>
/// The helper class that matches the text patterns using regular expressions.
/// </summary>
public class RegexTextMatcher
{
    private readonly Regex _pattern;

    /// <summary>
    /// Creates a new instance of the <see cref="RegexTextMatcher"/> class.
    /// </summary>
    /// <param name="pattern">The regular expression pattern.</param>
    public RegexTextMatcher(Regex pattern)
    {
        _pattern = pattern;
    }

    /// <summary>
    /// Tries to match the text pattern against the specified text.
    /// </summary>
    /// <param name="text">The text to match.</param>
    /// <returns>The result of the matching.</returns>
    public EndpointMatchingResult Match(string text)
    {
        var match = _pattern.Match(text);

        if (!match.Success)
        {
            return false;
        }

        var parameters = new Dictionary<string, string>();

        foreach (Group matchGroup in match.Groups)
        {
            if (!string.IsNullOrEmpty(matchGroup.Name) && !string.IsNullOrEmpty(matchGroup.Value))
            {
                parameters[matchGroup.Name] = matchGroup.Value;
            }
        }

        return new EndpointMatchingResult(true)
        {
            MatchProperties = ImmutablePropertyCollection.Empty
                .Set(new TextMatchProperties { Parameters = parameters })
        };
    }
}
