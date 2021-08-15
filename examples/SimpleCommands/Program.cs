using System;
using System.Globalization;
using System.Reflection;
using System.Threading;
using Kantaiko.Controllers;
using Kantaiko.Controllers.Design.Endpoints;
using Kantaiko.Controllers.Matchers;
using Kantaiko.Controllers.Result;

// ReSharper disable LocalizableElement

var controllerCollection = ControllerCollection.FromAssemblies(Assembly.GetExecutingAssembly());
var requestHandler = new RequestHandler<TestContext>(controllerCollection);

// Invoke request handler
Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");
var result = await requestHandler.HandleAsync(new TestContext("Hello, world!"));

// Handle result
if (result.IsMatched)
{
    if (result.IsExited)
    {
        switch (result.ExitReason)
        {
            case ErrorExitReason errorReason:
                Console.WriteLine($"An error occurred on stage {errorReason.Stage}: {errorReason.ErrorMessage}");
                return;
            case InterruptionExitReason interruptionReason:
                Console.WriteLine($"Execution was interrupted at stage {interruptionReason.MiddlewareStage}");
                return;
            case ExceptionExitReason exceptionReason:
                Console.WriteLine($"An exception was thrown: {exceptionReason.Exception}");
                return;
        }
    }

    if (result.HasReturnValue)
    {
        Console.WriteLine($"The return value was: {result.ReturnValue}");
    }
}

internal class HelloController : TestController
{
    [Pattern("Hello, {name}!")]
    public string Test(string name) => $"Hi, {name}!";
}

internal class TestController : ControllerBase<TestContext> { }

internal record TestContext(string Text);

internal class PatternMatcher : IEndpointMatcher<TestContext>
{
    private readonly PatternTextMatcher _patternTextMatcher;
    public PatternMatcher(string pattern) => _patternTextMatcher = new PatternTextMatcher(pattern);

    public EndpointMatchResult Match(EndpointMatchContext<TestContext> context)
    {
        return _patternTextMatcher.Match(context.RequestContext.Text);
    }
}

[AttributeUsage(AttributeTargets.Method)]
internal class PatternAttribute : Attribute, IEndpointMatcherFactory<TestContext>
{
    private readonly string _pattern;
    public PatternAttribute(string pattern) => _pattern = pattern;

    public IEndpointMatcher<TestContext> CreateEndpointMatcher(EndpointDesignContext context)
    {
        return new PatternMatcher(_pattern);
    }
}
