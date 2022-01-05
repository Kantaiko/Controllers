using System;
using Kantaiko.Controllers.Introspection.Factory.Attributes;
using Kantaiko.Controllers.Introspection.Factory.Context;
using Kantaiko.Controllers.ParameterConversion.Properties;
using Kantaiko.Properties.Immutable;

namespace Kantaiko.Controllers;

[AttributeUsage(AttributeTargets.Parameter)]
public class FromServicesAttribute : Attribute, IParameterPropertyProvider
{
    public IImmutablePropertyCollection UpdateParameterProperties(ParameterFactoryContext context)
    {
        return context.Parameter.Properties.Set(new ParameterServiceProperties
        {
            ServiceType = context.Parameter.ParameterType
        });
    }
}
