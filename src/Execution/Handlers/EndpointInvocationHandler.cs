using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using Kantaiko.Controllers.Introspection;
using Kantaiko.Controllers.Resources;
using Kantaiko.Controllers.Utils;

namespace Kantaiko.Controllers.Execution.Handlers;

/// <summary>
/// The one of the most important handlers in the execution pipeline.
/// It is responsible for constructing the endpoint invocation arguments, invoking the endpoint,
/// awaiting the result and setting it to <see cref="ControllerExecutionContext.InvocationResult"/>.
/// <br/>
/// Internally, it maintains a cache of compiled delegates that are used to invoke endpoints.
/// This allows to avoid the overhead of reflection on each request and improve the performance.
/// </summary>
public sealed class EndpointInvocationHandler : IControllerExecutionHandler
{
    private delegate object? InvocationDelegate(object controller, object?[] resolvedParameters);

    private delegate object? ResultAccessor(object? task);

    private readonly record struct ParameterDelegateSet(
        InvocationDelegate InvocationDelegate,
        ResultAccessor? ResultAccessor
    );

    private readonly ConcurrentDictionary<EndpointInfo, ParameterDelegateSet> _delegates = new();
    private readonly bool _awaitResult;

    /// <summary>
    /// Creates a new instance of <see cref="EndpointInvocationHandler"/>.
    /// </summary>
    /// <param name="awaitResult">Whether to automatically await the result of the endpoint invocation.</param>
    public EndpointInvocationHandler(bool awaitResult = true)
    {
        _awaitResult = awaitResult;
    }

    async Task IControllerExecutionHandler.HandleAsync(ControllerExecutionContext context)
    {
        var controller = Check.ContextProperty(context.ControllerInstance, nameof(EndpointInvocationHandler));
        var endpoint = Check.ContextProperty(context.Endpoint, nameof(EndpointInvocationHandler));
        var resolvedParameters = Check.ResolvedParameters(context);

        var delegateSet = _delegates.GetOrAdd(endpoint, CreateDelegateSet);

        object? result;

        try
        {
            result = delegateSet.InvocationDelegate(controller, resolvedParameters);

            if (_awaitResult && result is Task task)
            {
                await task;
            }
        }
        catch (Exception exception)
        {
            context.ExecutionError = new ControllerError(ControllerErrorCodes.InvocationException)
            {
                Message = Strings.InvocationException,
                Exception = exception,
            };

            return;
        }

        if (result is not null && delegateSet.ResultAccessor is not null)
        {
            result = delegateSet.ResultAccessor.Invoke(result);
        }

        context.InvocationResult = result;
    }

    private ParameterDelegateSet CreateDelegateSet(EndpointInfo endpoint)
    {
        var invocationDelegate = CreateInvocationDelegate(endpoint);
        var resultAccessor = CreateResultAccessor(endpoint);

        return new ParameterDelegateSet(invocationDelegate, resultAccessor);
    }

    private static InvocationDelegate CreateInvocationDelegate(EndpointInfo endpoint)
    {
        /*
         * This method creates a delegate that invokes the endpoint method with constructed parameters.
         *
         * It looks like this:
         * object (object controller, object?[]? resolvedParameters) => {
         *     controller.EndpointMethod(
         *         resolvedParameters[0],
         *         resolvedParameters[1],
         *         {
         *              var compositeParameter = new CompositeParameter();
         *              compositeParameter.Property1 = resolvedParameters[2];
         *              compositeParameter.Property2 = resolvedParameters[3];
         *              compositeParameter;
         *          },
         *          resolvedParameters[4]
         *     );
         * }
         *
         * Why? Just because it's fun. And because it's faster than reflection of course.
         */

        var controller = Expression.Parameter(typeof(object), "controller");
        var resolvedParameters = Expression.Parameter(typeof(object?[]), "resolvedParameters");
        var parameterExpressions = new List<Expression>();

        var parameterIndex = 0;

        foreach (var parameterInfo in endpoint.ParameterTree)
        {
            Expression parameterExpression;

            if (parameterInfo.HasChildren)
            {
                // If parameter has children, we need to construct it using resolved values:
                // {
                //     var parameter = new ParameterType();
                //     parameter.Property1 = resolvedParameters[parameterIndex++];
                //     parameter.Property2 = resolvedParameters[parameterIndex++];
                //     parameter;
                // }

                parameterExpression = CreateCompositeParameterBlock(parameterInfo,
                    resolvedParameters, ref parameterIndex);
            }
            else
            {
                // If parameter does not have children, we can just pass the resolved value directly:
                // resolvedParameters[parameterIndex++]

                parameterExpression = Expression.ArrayAccess(
                    resolvedParameters,
                    Expression.Constant(parameterIndex++)
                );

                Expression? defaultValue = parameterInfo.DefaultValue is not null
                    ? Expression.Convert(
                        Expression.Constant(parameterInfo.DefaultValue),
                        parameterInfo.RawParameterType
                    )
                    : null;

                if (defaultValue is not null || parameterInfo.RawParameterType.IsValueType)
                {
                    // If we have a default value, we need to check if the resolved value is null:
                    // resolvedParameters[parameterIndex++] == null ? defaultValue : resolvedParameters[parameterIndex++]
                    // If the parameter is a value type, we need to check it regardless of the default value,
                    // because we can't pass null to a value type
                    // P.S. don't worry about double increment, it's a constant expression

                    defaultValue ??= Expression.Default(parameterInfo.RawParameterType);

                    parameterExpression = Expression.Condition(
                        Expression.Equal(parameterExpression, Expression.Constant(null)),
                        defaultValue,
                        Expression.Convert(parameterExpression, parameterInfo.RawParameterType)
                    );
                }
                else
                {
                    parameterExpression = Expression.Convert(parameterExpression, parameterInfo.RawParameterType);
                }
            }

            parameterExpressions.Add(parameterExpression);
        }

        Expression body = Expression.Call(
            Expression.Convert(controller, endpoint.MethodInfo.DeclaringType!),
            endpoint.MethodInfo,
            parameterExpressions
        );

        if (endpoint.MethodInfo.ReturnType == typeof(void))
        {
            body = Expression.Block(body, Expression.Constant(null));
        }
        else
        {
            body = Expression.Convert(body, typeof(object));
        }

        var lambda = Expression.Lambda<InvocationDelegate>(
            body,
            controller,
            resolvedParameters
        );

        return lambda.Compile();
    }

    private static BlockExpression CreateCompositeParameterBlock(
        EndpointParameterInfo parameter,
        ParameterExpression resolvedParameters,
        ref int parameterIndex)
    {
        var instance = Expression.Variable(parameter.ParameterType, "instance");
        var instanceAssign = Expression.Assign(instance, Expression.New(parameter.ParameterType));
        var block = new List<Expression> { instanceAssign };

        foreach (var child in parameter.Children)
        {
            Expression constructedParameter;

            if (child.HasChildren)
            {
                // If the child parameter is a composite parameter, we need to create it recursively:
                // instance.Property = {
                //     var childParameter = new ChildParameterType();
                //     childParameter.Property1 = resolvedParameters[parameterIndex++];
                //     childParameter.Property2 = resolvedParameters[parameterIndex++];
                //     childParameter;
                // }

                constructedParameter = CreateCompositeParameterBlock(child, resolvedParameters, ref parameterIndex);
            }
            else
            {
                // If the child parameter is a simple parameter, we can just assign it:
                // instance.Property = resolvedParameters[parameterIndex++];

                constructedParameter = Expression.ArrayAccess(
                    resolvedParameters,
                    Expression.Constant(parameterIndex++)
                );

                constructedParameter = Expression.Convert(constructedParameter, child.RawParameterType);
            }

            var propertyAssign = Expression.Assign(
                Expression.Property(instance, (PropertyInfo) child.AttributeProvider),
                constructedParameter
            );

            block.Add(propertyAssign);
        }

        block.Add(instance);
        return Expression.Block(new[] { instance }, block);
    }

    private ResultAccessor? CreateResultAccessor(EndpointInfo endpoint)
    {
        if (!_awaitResult)
        {
            return null;
        }

        if (!endpoint.MethodInfo.ReturnType.IsGenericType)
        {
            return null;
        }

        var genericType = endpoint.MethodInfo.ReturnType.GetGenericTypeDefinition();

        if (genericType != typeof(Task<>))
        {
            return null;
        }

        // If endpoint method returns a Task<T>, we need to access the result:
        // object (object result) => ((Task<T>) result).Result

        var task = Expression.Parameter(typeof(object), "task");

        Expression result = Expression.Property(
            Expression.Convert(task, endpoint.MethodInfo.ReturnType),
            "Result"
        );

        result = Expression.Convert(result, typeof(object));
        return Expression.Lambda<ResultAccessor>(result, task).Compile();
    }
}
