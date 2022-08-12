using Kantaiko.Controllers.Introspection.Factory.Context;

namespace Kantaiko.Controllers.Introspection.Factory.Attributes;

public interface IParameterCustomizationProvider
{
    (string Name, bool IsOptional) GetParameterCustomization(ParameterFactoryContext context);
}
