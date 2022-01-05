using System.Linq;
using Kantaiko.Controllers.Introspection.Factory.Attributes;
using Kantaiko.Controllers.Introspection.Factory.Context;
using Kantaiko.Controllers.ParameterConversion.Validation;
using Kantaiko.Properties.Immutable;

namespace Kantaiko.Controllers.Introspection.Factory.Transformers;

public class PostValidationIntrospectionInfoTransformer : IntrospectionInfoTransformer
{
    protected override IImmutablePropertyCollection TransformParameterProperties(ParameterFactoryContext context)
    {
        var factories = context.Parameter.AttributeProvider.GetCustomAttributes(true)
            .OfType<IParameterPostValidatorFactory>()
            .ToArray();

        var postValidators = new IParameterPostValidator[factories.Length];

        for (var index = 0; index < factories.Length; index++)
        {
            postValidators[index] = factories[index].CreateParameterPostValidator(context);
        }

        return context.Parameter.Properties.Update<PostValidationParameterProperties>(props => props with
        {
            PostValidators = props.PostValidators.Concat(postValidators)
        });
    }
}
