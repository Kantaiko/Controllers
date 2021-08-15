# Endpoint matching

- [Getting started](getting-started.md)
- Endpoint matching
- [Parameter conversion](parameter-conversion.md)
- [Parameter deconstruction](parameter-deconstruction.md)
- [Parameter post-validation](parameter-post-validation.md)
- [Middleware](middleware.md)
- [Another features](another-features.md)

Endpoint is a controller method decorated with one or multiple matcher attributes:

```c#
public class HelloController : TestController
{
    [Pattern("hello")]
    public void Greet()
    {
        Console.WriteLine("Hi!");
    }
}
```

Such attributes implement `IEndpointMatcherFactory` interface to create the instance of `IEndpointMatcher`:

```c#
internal class PatternMatcher : IEndpointMatcher<TexTContext>
{
    private readonly Regex _pattern;
    public PatternMatcher(string pattern) => _pattern = new Regex(pattern);

    public EndpointMatchResult Match(EndpointMatchContext<TexTContext> context)
    {
        return _pattern.IsMatch(context.RequestContext.Text)
            ? EndpointMatchResult.Success(new Dictionary<string, object>())
            : EndpointMatchResult.NotMatched;
    }
}

[AttributeUsage(AttributeTargets.Method)]
internal class PatternAttribute : Attribute, IEndpointMatcherFactory<TexTContext>
{
    private readonly string _pattern;
    public PatternAttribute(string pattern) => _pattern = pattern;

    public IEndpointMatcher<TexTContext> CreateEndpointMatcher(EndpointDesignContext context)
    {
        return new PatternMatcher(_pattern);
    }
}
```

Each endpoint matcher should implement the `Match` method returning `EndpointMatchResult` which can be in three states:

- `Success` indicates that request should be handled by the endpoint. It must also contain `Parameters` dictionary that
  will be used by parameter converters.
- `Error` reports that an error has occured during endpoint matching.
- `NotMatched` means that next endpoint matcher should be invoked. If all matchers return not matched results, request
  will be ignored and `IsMatched` property of `RequestProcessingResult` will be false.

> ⚠ Note that all endpoint matchers are singletons. So don't store any request specific data in the matcher instances.

To implement common use cases like simple patterns and regexes you can use built-in helper classes: `PatternTextMatcher`
and `RegexTextMatcher`.

Read next: [Parameter conversion](parameter-conversion.md)
