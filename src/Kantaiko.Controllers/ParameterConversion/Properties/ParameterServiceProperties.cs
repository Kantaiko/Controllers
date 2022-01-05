using System;
using Kantaiko.Properties;

namespace Kantaiko.Controllers.ParameterConversion.Properties;

public record ParameterServiceProperties : ReadOnlyPropertiesBase<ParameterServiceProperties>
{
    public Type? ServiceType { get; init; }
}
