using System;
using Kantaiko.Controllers.Converters;

namespace Kantaiko.Controllers.Design.Parameters
{
    public interface IParameterConverterFactoryProvider
    {
        Func<IParameterConverter> GetParameterConverterFactory(EndpointParameterDesignContext context);
    }
}
