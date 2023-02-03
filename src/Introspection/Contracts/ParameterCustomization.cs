namespace Kantaiko.Controllers.Introspection.Contracts;

/// <summary>
/// The model of a parameter customization.
/// </summary>
public readonly struct ParameterCustomization
{
    public string? Name { get; init; }
    public bool? IsOptional { get; init; }
    public object? DefaultValue { get; init; }
}
