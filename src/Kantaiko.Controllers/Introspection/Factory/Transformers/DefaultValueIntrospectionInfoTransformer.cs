using System.Linq;
using Kantaiko.Controllers.Introspection.Factory.Attributes;
using Kantaiko.Controllers.Introspection.Factory.Context;
using Kantaiko.Controllers.ParameterConversion.DefaultValue;
using Kantaiko.Properties.Immutable;

namespace Kantaiko.Controllers.Introspection.Factory.Transformers;

public class DefaultValueIntrospectionInfoTransformer : IntrospectionInfoTransformer
{
    protected override IImmutablePropertyCollection TransformParameterProperties(ParameterFactoryContext context)
    {
        var factory = context.Parameter.AttributeProvider.GetCustomAttributes(true)
            .OfType<IParameterDefaultValueResolverFactory>()
            .FirstOrDefault();

        if (factory is null)
        {
            return context.Parameter.Properties;
        }

        return context.Parameter.Properties.Set(new DefaultValueResolutionParameterProperties
        {
            DefaultValueResolver = factory.CreateParameterDefaultValueResolve(context)
        });
    }
}
