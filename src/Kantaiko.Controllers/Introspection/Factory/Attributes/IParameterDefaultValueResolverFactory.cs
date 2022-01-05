using Kantaiko.Controllers.Introspection.Factory.Context;
using Kantaiko.Controllers.ParameterConversion.DefaultValue;

namespace Kantaiko.Controllers.Introspection.Factory.Attributes;

public interface IParameterDefaultValueResolverFactory
{
    IParameterDefaultValueResolver CreateParameterDefaultValueResolve(ParameterFactoryContext context);
}
