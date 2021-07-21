using System;
using Kantaiko.Controllers.Converters;
using Kantaiko.Controllers.Design.Parameters;

namespace Kantaiko.Controllers
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class FromServicesAttribute : Attribute, IParameterConverterTypeProvider
    {
        public Type GetConverterType(EndpointParameterDesignContext context) => typeof(ServiceConverter);
    }
}
