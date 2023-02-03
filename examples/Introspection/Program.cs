using System.Reflection;
using System.Text;
using Introspection;
using Kantaiko.Controllers;
using Kantaiko.Controllers.Introspection;
using Kantaiko.Controllers.Introspection.Context;
using Kantaiko.Controllers.Introspection.Contracts;
using Kantaiko.Controllers.Introspection.Deconstruction;
using Kantaiko.Controllers.Introspection.Transformers;
using Kantaiko.Properties;
using Kantaiko.Properties.Immutable;

var lookupTypes = Assembly.GetExecutingAssembly().GetTypes();

// Here we create IntrospectionInfo directly in order to demonstrate its independent usage
var introspectionInfoFactory = new IntrospectionInfoFactory(
    transformers: new IIntrospectionInfoTransformer[]
    {
        new PropertyProviderTransformer(),
        new ParameterCustomizationTransformer()
    },
    deconstructionValidator: new DefaultDeconstructionValidator()
);

var introspectionInfo = introspectionInfoFactory.CreateIntrospectionInfo<TestController>(lookupTypes);

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

namespace Introspection
{
    [CompositeParameter]
    internal class InstallPackageInput
    {
        [Parameter("package")]
        public string PackageName { get; set; } = null!;

        [Parameter("version", IsOptional = true)]
        public string PackageVersion { get; set; } = null!;
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

    internal record CommandEndpointProperties : PropertyRecord<CommandEndpointProperties>
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

        public IImmutablePropertyCollection UpdateEndpointProperties(EndpointTransformationContext context)
        {
            return context.Endpoint.Properties.Set(new CommandEndpointProperties { CommandName = _name });
        }
    }

    internal class TestContext { }
}
