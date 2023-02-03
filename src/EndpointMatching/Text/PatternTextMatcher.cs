using System.Text.RegularExpressions;

namespace Kantaiko.Controllers.EndpointMatching.Text;

/// <summary>
/// The helper class that matches the endpoint with the text pattern.
/// The pattern can contain parameters in the form of <c>{parameterName}</c>.
/// </summary>
public partial class PatternTextMatcher : RegexTextMatcher
{
    /// <summary>
    /// Creates a new instance of the <see cref="PatternTextMatcher"/> class.
    /// </summary>
    /// <param name="pattern">The pattern to match.</param>
    /// <param name="options">The options of the pattern matching.</param>
    public PatternTextMatcher(string pattern, PatternOptions options = PatternOptions.None) :
        base(GetPatternRegex(pattern, options)) { }

    [GeneratedRegex("{(\\w+)}", RegexOptions.Multiline)]
    private static partial Regex PatternGroupRegex();

    private static Regex GetPatternRegex(string pattern, PatternOptions options)
    {
        var escapedPattern = Regex.Escape(pattern).Replace(@"\{", "{");
        var patternRegex = PatternGroupRegex().Replace(escapedPattern, @"(?<$1>\S+)");

        var regexOptions = RegexOptions.None;

        if ((options & PatternOptions.MultiLine) != 0)
        {
            regexOptions |= RegexOptions.Multiline;
        }

        if ((options & PatternOptions.IgnoreCase) != 0)
        {
            regexOptions |= RegexOptions.IgnoreCase;
        }

        if ((options & PatternOptions.CompleteMatch) != 0)
        {
            patternRegex = $"^{patternRegex}$";
        }

        return new Regex(patternRegex, regexOptions);
    }
}
