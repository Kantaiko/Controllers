using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Kantaiko.Controllers.Exceptions;
using Kantaiko.Controllers.Introspection;

namespace Kantaiko.Controllers.Execution.Handlers;

public class ConstructParametersHandler<TContext> : IControllerExecutionHandler<TContext>
{
    public Task HandleAsync(ControllerExecutionContext<TContext> context)
    {
        PropertyNullException.ThrowIfNull(context.Endpoint);
        PropertyNullException.ThrowIfNull(context.ResolvedParameters);

        var constructedParameters = new object?[context.Endpoint.ParameterTree.Count];

        for (var index = 0; index < context.Endpoint.ParameterTree.Count; index++)
        {
            var parameter = context.Endpoint.ParameterTree[index];
            var value = ConstructParameter(parameter, context.ResolvedParameters);

            if (value is null && parameter.AttributeProvider is ParameterInfo { HasDefaultValue: true } parameterInfo)
            {
                value = parameterInfo.DefaultValue;
            }

            constructedParameters[index] = value;
        }

        context.ConstructedParameters = constructedParameters;

        return Task.CompletedTask;
    }

    private static object? ConstructParameter(EndpointParameterInfo parameter,
        IReadOnlyDictionary<EndpointParameterInfo, object?> resolvedParameters)
    {
        if (!parameter.HasChildren)
        {
            return resolvedParameters.GetValueOrDefault(parameter);
        }

        var instance = Activator.CreateInstance(parameter.ParameterType)!;

        foreach (var child in parameter.Children)
        {
            var property = (PropertyInfo) child.AttributeProvider;

            property.SetValue(instance, ConstructParameter(child, resolvedParameters));
        }

        return instance;
    }
}
