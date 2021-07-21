using System;

namespace Kantaiko.Controllers.Design.Parameters
{
    public interface IParameterConverterTypeProvider
    {
        Type GetConverterType(EndpointParameterDesignContext context);
    }
}
