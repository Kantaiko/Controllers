using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Kantaiko.Controllers.Matchers
{
    public class RegexTextMatcher
    {
        private readonly Regex _pattern;

        public RegexTextMatcher(string pattern)
        {
            _pattern = new Regex(pattern);
        }

        public EndpointMatchResult Match(string text)
        {
            var match = _pattern.Match(text);
            if (!match.Success) return EndpointMatchResult.NotMatched;

            var result = new Dictionary<string, string>();

            foreach (Group matchGroup in match.Groups)
            {
                if (!string.IsNullOrEmpty(matchGroup.Name) && !string.IsNullOrEmpty(matchGroup.Value))
                {
                    result[matchGroup.Name] = matchGroup.Value;
                }
            }

            return EndpointMatchResult.Success(result);
        }
    }
}
