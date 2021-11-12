using System;
using Kantaiko.Controllers.Design.Endpoints;
using Kantaiko.Controllers.Matchers;

namespace Kantaiko.Controllers.Tests.Shared
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PatternAttribute : Attribute, IEndpointMatcherFactory<TestContext>
    {
        private readonly string _pattern;
        public PatternAttribute(string pattern) => _pattern = pattern;

        public IEndpointMatcher<TestContext> CreateEndpointMatcher(EndpointDesignContext context)
        {
            return new PatternMatcher(_pattern);
        }
    }
}
