using Kantaiko.Controllers.Matchers;

namespace Kantaiko.Controllers.Tests.Shared
{
    internal class PatternMatcher : IEndpointMatcher<TestRequest>
    {
        private readonly RegexTextMatcher _matcher;

        public PatternMatcher(string pattern)
        {
            _matcher = new RegexTextMatcher(pattern);
        }

        public EndpointMatchResult Match(EndpointMatchContext<TestRequest> context)
        {
            return _matcher.Match(context.Request.Text);
        }
    }
}
