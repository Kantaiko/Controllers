using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Kantaiko.Controllers;
using Kantaiko.Controllers.Introspection.Factory;
using Kantaiko.Controllers.Introspection.Factory.Attributes;
using Kantaiko.Controllers.Introspection.Factory.Context;
using Kantaiko.Controllers.Introspection.Factory.Deconstruction;
using Kantaiko.Properties;
using Kantaiko.Properties.Immutable;
using Kantaiko.Routing.Context;

var deconstructionValidator = new TestDeconstructionValidator();

var introspectionBuilder = new IntrospectionBuilder<TestContext>();

introspectionBuilder.SetDeconstructionValidator(deconstructionValidator);
introspectionBuilder.AddEndpointMatching();
introspectionBuilder.AddDefaultTransformation();

var lookupTypes = Assembly.GetExecutingAssembly().GetTypes();
var introspectionInfo = introspectionBuilder.CreateIntrospectionInfo(lookupTypes);

// Get all endpoints
var endpoints = introspectionInfo.Controllers.SelectMany(x => x.Endpoints);

// Build help message
var helpMessage = new StringBuilder();
helpMessage.AppendLine("Available commands:");
helpMessage.AppendLine();

// Iterate endpoints
foreach (var endpointInfo in endpoints)
{
    if (CommandEndpointProperties.Of(endpointInfo)?.CommandName is not { } commandName)
    {
        continue;
    }

    helpMessage.Append($"/{commandName}");

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

internal record CommandEndpointProperties : ReadOnlyPropertiesBase<CommandEndpointProperties>
{
    public string? CommandName { get; init; }
}

[AttributeUsage(AttributeTargets.Method)]
internal class CommandAttribute : Attribute, IEndpointPropertyProvider
{
    private readonly string _name;

    public CommandAttribute(string name)
    {
        _name = name;
    }

    public IImmutablePropertyCollection UpdateEndpointProperties(EndpointFactoryContext context)
    {
        return context.Endpoint.Properties.Set(new CommandEndpointProperties { CommandName = _name });
    }
}

internal class TestDeconstructionValidator : IDeconstructionValidator
{
    public bool CanDeconstruct(Type type)
    {
        return type == typeof(InstallPackageInput);
    }
}


internal class TestContext : ContextBase { }
