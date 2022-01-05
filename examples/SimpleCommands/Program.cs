using System;
using System.Globalization;
using System.Reflection;
using System.Threading;
using Kantaiko.Controllers;
using Kantaiko.Controllers.Execution;
using Kantaiko.Controllers.Introspection.Factory;
using Kantaiko.Controllers.Introspection.Factory.Attributes;
using Kantaiko.Controllers.Introspection.Factory.Context;
using Kantaiko.Controllers.Matching;
using Kantaiko.Controllers.Matching.Text;
using Kantaiko.Controllers.ParameterConversion.Text;
using Kantaiko.Controllers.Result;
using Kantaiko.Properties;
using Kantaiko.Routing.Context;

// ReSharper disable LocalizableElement

// 1. Create introspection info
var lookupTypes = Assembly.GetExecutingAssembly().GetTypes();

var converterCollection = new TextParameterConverterCollection(lookupTypes);

var introspectionBuilder = new IntrospectionBuilder<TestContext>();

introspectionBuilder.AddEndpointMatching();
introspectionBuilder.AddTextParameterConversion(converterCollection);
introspectionBuilder.AddDefaultTransformation();

var introspectionInfo = introspectionBuilder.CreateIntrospectionInfo(lookupTypes);

// 2. Create controller execution pipeline
var pipelineBuilder = new PipelineBuilder<TestContext>();

pipelineBuilder.AddEndpointMatching();
pipelineBuilder.AddTextParameterConversion();
pipelineBuilder.AddDefaultControllerHandling();

var handlers = pipelineBuilder.Build();

// 3. Assemble controller handler
var controllerHandler = ControllerHandlerFactory.CreateControllerHandler(introspectionInfo, handlers);

// Invoke request handler
Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");
var result = await controllerHandler.Handle(new TestContext("Hello, world!"));

// Handle result
if (result.IsMatched)
{
    if (result.IsExited)
    {
        switch (result.ExitReason)
        {
            case ErrorExitReason errorReason:
                Console.WriteLine($"An error was occurred: {errorReason.ErrorMessage}");
                return;
            case InterruptionExitReason:
                Console.WriteLine("Execution was interrupted");
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

internal abstract class TestController : ControllerBase<TestContext> { }

internal class TestContext : ContextBase
{
    public TestContext(string text, IServiceProvider? serviceProvider = null,
        IReadOnlyPropertyCollection? properties = null, CancellationToken cancellationToken = default) :
        base(serviceProvider, properties, cancellationToken)
    {
        Text = text;
    }

    public string Text { get; }
}

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

    public IEndpointMatcher<TestContext> CreateEndpointMatcher(EndpointFactoryContext context)
    {
        return new PatternMatcher(_pattern);
    }
}
