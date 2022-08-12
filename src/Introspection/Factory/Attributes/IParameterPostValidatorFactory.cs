using Kantaiko.Controllers.Introspection.Factory.Context;
using Kantaiko.Controllers.ParameterConversion.Validation;

namespace Kantaiko.Controllers.Introspection.Factory.Attributes;

public interface IParameterPostValidatorFactory
{
    IParameterPostValidator CreateParameterPostValidator(ParameterFactoryContext context);
}
