using Kantaiko.Controllers.Matchers;

namespace Kantaiko.Controllers.Tests.Shared
{
    internal class PatternMatcher : IEndpointMatcher<TestContext>
    {
        private readonly RegexTextMatcher _matcher;

        public PatternMatcher(string pattern)
        {
            _matcher = new RegexTextMatcher(pattern);
        }

        public EndpointMatchResult Match(EndpointMatchContext<TestContext> context)
        {
            return _matcher.Match(context.RequestContext.Text);
        }
    }
}
