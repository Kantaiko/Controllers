using System.Threading.Tasks;

namespace Kantaiko.Controllers.ParameterConversion.DefaultValue;

public interface IParameterDefaultValueResolver
{
    Task<object?> ResolveDefaultValueAsync(ParameterDefaultValueResolutionContext context);
}
