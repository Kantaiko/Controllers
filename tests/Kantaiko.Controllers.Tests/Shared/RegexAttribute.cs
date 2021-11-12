using System;
using Kantaiko.Controllers.Design.Endpoints;
using Kantaiko.Controllers.Matchers;

namespace Kantaiko.Controllers.Tests.Shared
{
    [AttributeUsage(AttributeTargets.Method)]
    internal class RegexAttribute : Attribute, IEndpointMatcherFactory<TestContext>
    {
        private readonly string _pattern;
        public RegexAttribute(string pattern) => _pattern = pattern;

        public IEndpointMatcher<TestContext> CreateEndpointMatcher(EndpointDesignContext context)
        {
            return new RegexMatcher(_pattern);
        }
    }
}
