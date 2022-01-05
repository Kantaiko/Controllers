using System;
using Kantaiko.Controllers.Introspection.Factory.Attributes;
using Kantaiko.Controllers.Introspection.Factory.Context;

namespace Kantaiko.Controllers;

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
public class ParameterAttribute : Attribute, IParameterCustomizationProvider
{
    public string? Name { get; }
    public bool IsOptional { get; init; }

    public ParameterAttribute(string? name = null)
    {
        Name = name;
    }

    public (string Name, bool IsOptional) GetParameterCustomization(ParameterFactoryContext context)
    {
        return (Name ?? context.Parameter.Name, IsOptional);
    }
}
