using System;
using Kantaiko.Controllers.Converters;
using Kantaiko.Controllers.Design.Parameters;
using Kantaiko.Controllers.Design.Properties;

namespace Kantaiko.Controllers
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class FromServicesAttribute : Attribute, IParameterConverterTypeProvider, IParameterDesignPropertyProvider
    {
        public Type GetConverterType(EndpointParameterDesignContext context) => typeof(ServiceConverter);

        public DesignPropertyCollection GetParameterDesignProperties() => new()
        {
            [KantaikoParameterProperties.IsHidden] = true
        };
    }
}
