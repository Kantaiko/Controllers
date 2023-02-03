using Kantaiko.Controllers.EndpointMatching;
using Kantaiko.Controllers.EndpointMatching.Text;

namespace Kantaiko.Controllers.Tests.Shared;

[AttributeUsage(AttributeTargets.Method)]
internal class PatternAttribute : Attribute, IEndpointMatcher
{
    private readonly PatternTextMatcher _matcher;

    public PatternAttribute(string pattern)
    {
        _matcher = new PatternTextMatcher(pattern);
    }

    public EndpointMatchingResult Match(EndpointMatchingContext context)
    {
        if (context.RequestContext is not TestContext testContext)
        {
            throw new InvalidOperationException("Request context is not a test context.");
        }

        return _matcher.Match(testContext.Message);
    }
}
