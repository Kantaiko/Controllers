using Kantaiko.Controllers.Matchers;

namespace Kantaiko.Controllers.Tests.Shared
{
    public class PatternMatcher : IEndpointMatcher<TestContext>
    {
        private readonly PatternTextMatcher _matcher;

        public PatternMatcher(string pattern)
        {
            _matcher = new PatternTextMatcher(pattern);
        }

        public EndpointMatchResult Match(EndpointMatchContext<TestContext> context)
        {
            return _matcher.Match(context.RequestContext.Text);
        }
    }
}
