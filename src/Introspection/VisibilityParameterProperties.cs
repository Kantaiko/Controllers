using Kantaiko.Properties;

namespace Kantaiko.Controllers.Introspection;

public record VisibilityParameterProperties : ReadOnlyPropertiesBase<VisibilityParameterProperties>
{
    public bool IsHidden { get; init; }
}
