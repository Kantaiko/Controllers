using Kantaiko.Properties;

namespace Kantaiko.Controllers.ParameterConversion.DefaultValue;

public record DefaultValueResolutionProperties : ReadOnlyPropertiesBase<DefaultValueResolutionProperties>
{
    public IParameterDefaultValueResolver? DefaultValueResolver { get; init; }
}
