using Kantaiko.Controllers.Exceptions;
using Kantaiko.Controllers.Introspection;
using Kantaiko.Controllers.Introspection.Context;
using Kantaiko.Controllers.Introspection.Contracts;
using Kantaiko.Controllers.ParameterConversion;
using Kantaiko.Properties.Immutable;

namespace Kantaiko.Controllers;

/// <summary>
/// The attribute that marks a parameter as a service that should be resolved from the service provider.
/// <br/>
/// If parameter is required and the service is not found, the <see cref="ServiceNotFoundException"/> will be thrown.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
public sealed class FromServicesAttribute : Attribute, IParameterConverter, IParameterPropertyProvider
{
    ValueTask<ConversionResult> IParameterConverter.ConvertAsync(ParameterConversionContext context)
    {
        var service = context.ServiceProvider.GetService(context.Parameter.ParameterType);

        if (service is null && !context.Parameter.IsOptional)
        {
            throw new ServiceNotFoundException(context.Parameter);
        }

        return ValueTask.FromResult(new ConversionResult(service));
    }

    IImmutablePropertyCollection IParameterPropertyProvider.UpdateParameterProperties(ParameterTransformationContext context)
    {
        return context.Parameter.Properties.Set(new VisibilityParameterProperties { IsHidden = true });
    }
}
