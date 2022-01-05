using System;
using Kantaiko.Controllers.Introspection.Factory.Attributes;
using Kantaiko.Controllers.Introspection.Factory.Context;
using Kantaiko.Controllers.Matching;

namespace Kantaiko.Controllers.Tests.Shared;

[AttributeUsage(AttributeTargets.Method)]
internal class RegexAttribute : Attribute, IEndpointMatcherFactory<TestContext>
{
    private readonly string _pattern;

    public RegexAttribute(string pattern)
    {
        _pattern = pattern;
    }

    public IEndpointMatcher<TestContext> CreateEndpointMatcher(EndpointFactoryContext context)
    {
        return new RegexMatcher(_pattern);
    }
}
