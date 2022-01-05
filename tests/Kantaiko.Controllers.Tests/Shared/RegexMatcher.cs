using Kantaiko.Controllers.Matching;
using Kantaiko.Controllers.Matching.Text;

namespace Kantaiko.Controllers.Tests.Shared;

internal class RegexMatcher : IEndpointMatcher<TestContext>
{
    private readonly RegexTextMatcher _matcher;

    public RegexMatcher(string pattern)
    {
        _matcher = new RegexTextMatcher(pattern);
    }

    public EndpointMatchResult Match(EndpointMatchContext<TestContext> context)
    {
        return _matcher.Match(context.RequestContext.Message);
    }
}
