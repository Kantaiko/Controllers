using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Kantaiko.Controllers;
using Kantaiko.Controllers.Design.Endpoints;
using Kantaiko.Controllers.Matchers;

var controllerCollection = ControllerCollection.FromAssemblies(Assembly.GetExecutingAssembly());
var requestHandler = new RequestHandler<TestContext>(controllerCollection);

// Get all endpoints
var endpoints = requestHandler.Info.Controllers.SelectMany(x => x.Endpoints).ToArray();

// Build help message
var helpMessage = new StringBuilder();
helpMessage.AppendLine("Available commands:");
helpMessage.AppendLine();

// Iterate endpoints
foreach (var endpointInfo in endpoints)
{
    if (!endpointInfo.Properties.TryGetValue(IntrospectionTestProperties.CommandName, out var commandName)) continue;

    helpMessage.AppendFormat("/{0}", commandName);

    foreach (var parameterInfo in endpointInfo.Parameters)
    {
        var template = parameterInfo.IsOptional ? " [{0} {1}]" : " {{{0} {1}}}";
        helpMessage.AppendFormat(template, parameterInfo.ParameterType.Name, parameterInfo.Name);
    }

    helpMessage.AppendLine();
}

Console.WriteLine(helpMessage.ToString());


internal class InstallPackageInput
{
    [Parameter("package")]
    public string PackageName { get; set; } = null!;

    [Parameter("version", IsOptional = true)]
    public string? PackageVersion { get; set; }
}

internal class TestController : ControllerBase<TestContext>
{
    [Command("sum")]
    public int Sum(int a, int b) => a + b;

    [Command("greet")]
    public string Greet(string? name = null) => name is null ? "Hi!" : $"Hi, {name}!";

    [Command("install")]
    public void InstallPackage(InstallPackageInput input) { }
}

internal static class IntrospectionTestProperties
{
    public const string CommandName = nameof(IntrospectionTestProperties) + ":" + nameof(CommandName);
}

[AttributeUsage(AttributeTargets.Method)]
internal class CommandAttribute : Attribute, IEndpointMatcherFactory<TestContext>, IEndpointDesignPropertyProvider
{
    private readonly string _name;

    public CommandAttribute(string name)
    {
        _name = name;
    }

    public IEndpointMatcher<TestContext> CreateEndpointMatcher(EndpointDesignContext context)
    {
        return null!;
    }

    public IReadOnlyDictionary<string, object> GetEndpointDesignProperties() => new Dictionary<string, object>
    {
        [IntrospectionTestProperties.CommandName] = _name
    };
}

internal record TestContext(string Text);
