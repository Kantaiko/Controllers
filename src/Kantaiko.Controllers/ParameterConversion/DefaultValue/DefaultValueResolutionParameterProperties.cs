using Kantaiko.Properties;

namespace Kantaiko.Controllers.ParameterConversion.DefaultValue;

public record DefaultValueResolutionParameterProperties :
    ReadOnlyPropertiesBase<DefaultValueResolutionParameterProperties>
{
    public IParameterDefaultValueResolver? DefaultValueResolver { get; init; }
}
