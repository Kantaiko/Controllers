using System.Globalization;
using System.Reflection;
using Kantaiko.Controllers;
using Kantaiko.Controllers.EndpointMatching;
using Kantaiko.Controllers.EndpointMatching.Text;
using Kantaiko.Controllers.Execution;
using Kantaiko.Controllers.ParameterConversion;
using SimpleCommands;

// ReSharper disable LocalizableElement

// 1. Configure introspection transformers and execution handlers
var executorBuilder = new ControllerExecutorBuilder();

executorBuilder.AddEndpointMatching();
executorBuilder.AddTextParameterConversion();
executorBuilder.AddDefaultHandlers();

// 2. Build controller executor
var lookupTypes = Assembly.GetExecutingAssembly().GetTypes();
var executor = executorBuilder.Build<TestController>(lookupTypes);

// 3. Set request locale and invoke request handler
Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");
var result = await executor.HandleAsync(new TestContext("Hello, world!"));

// Handle result
if (result.EndpointResult is not null)
{
    Console.WriteLine($"Endpoint return value: {result.EndpointResult}");
}

if (result.Error is { } error)
{
    switch (error.Code)
    {
        case ControllerErrorCodes.NotMatched:
        {
            Console.WriteLine("Request not matched");
            break;
        }
        case ControllerErrorCodes.InvocationException:
        {
            Console.WriteLine("An exception was thrown by the endpoint");
            Console.WriteLine(error.Exception);
            break;
        }
        case ControllerErrorCodes.ParameterConversionFailed:
        {
            Console.WriteLine($"Failed to convert parameter of type {error.Parameter!.ParameterType}");
            Console.WriteLine($"Conversion error: [{error.InnerError!.Code}] {error.InnerError!.Message}");
            break;
        }
        default:
        {
            Console.WriteLine($"Unexpected error: [{error.Code}] {error.Message}");
            break;
        }
    }
}

namespace SimpleCommands
{
    internal class HelloController : TestController
    {
        [Pattern("Hello, {name}!")]
        public string Test(string name) => $"Hi, {name}!";
    }

    internal abstract class TestController : ControllerBase<TestContext> { }

    internal record TestContext(string Text);

    [AttributeUsage(AttributeTargets.Method)]
    internal class PatternAttribute : Attribute, IEndpointMatcher<TestContext>
    {
        private readonly PatternTextMatcher _matcher;

        public PatternAttribute(string pattern, PatternOptions options = PatternOptions.None)
        {
            _matcher = new PatternTextMatcher(pattern, options);
        }

        EndpointMatchingResult IEndpointMatcher<TestContext>.Match(EndpointMatchingContext<TestContext> context)
        {
            return _matcher.Match(context.RequestContext.Text);
        }
    }
}
