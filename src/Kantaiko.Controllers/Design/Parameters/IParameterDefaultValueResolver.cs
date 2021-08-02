using Kantaiko.Controllers.Converters;

namespace Kantaiko.Controllers.Design.Parameters
{
    public interface IParameterDefaultValueResolver
    {
        object ResolveDefaultValue(ParameterConversionContext context);
    }
}
