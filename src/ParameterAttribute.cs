using Kantaiko.Controllers.Introspection.Context;
using Kantaiko.Controllers.Introspection.Contracts;

namespace Kantaiko.Controllers;

/// <summary>
/// The attribute that overrides some parameter information like name, optionality and default value.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
public sealed class ParameterAttribute : Attribute, IParameterCustomizationProvider
{
    /// <summary>
    /// The name of the parameter.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Whether the parameter is optional.
    /// </summary>
    public bool IsOptional { get; init; }

    /// <summary>
    /// The default value of the parameter.
    /// </summary>
    public object? Default { get; init; }

    public ParameterAttribute(string? name = null)
    {
        Name = name;
    }

    ParameterCustomization IParameterCustomizationProvider.GetParameterCustomization(ParameterTransformationContext context)
    {
        return new ParameterCustomization
        {
            Name = Name,
            IsOptional = context.Parameter.IsOptional || IsOptional,
            DefaultValue = Default
        };
    }
}
