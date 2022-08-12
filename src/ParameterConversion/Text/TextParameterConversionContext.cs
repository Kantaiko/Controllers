using System.Collections.Generic;
using Kantaiko.Controllers.Introspection;

namespace Kantaiko.Controllers.ParameterConversion.Text;

public class TextParameterConversionContext
{
    public TextParameterConversionContext(IReadOnlyDictionary<string, string> parameters,
        EndpointParameterInfo parameterInfo)
    {
        Parameters = parameters;
        ParameterInfo = parameterInfo;
    }

    public IReadOnlyDictionary<string, string> Parameters { get; }
    public EndpointParameterInfo ParameterInfo { get; }
}
