using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Kantaiko.Controllers.Exceptions;
using Kantaiko.Controllers.Handlers;
using Kantaiko.Controllers.Introspection.Factory.Deconstruction;
using Kantaiko.Controllers.Introspection.Factory.Transformers;
using Kantaiko.Controllers.Utils;

namespace Kantaiko.Controllers.Introspection.Factory;

public class IntrospectionInfoFactory
{
    private readonly NullabilityInfoContext _nullabilityInfoContext = new();

    private readonly IDeconstructionValidator? _deconstructionValidator;
    private readonly IEnumerable<IIntrospectionInfoTransformer> _transformers;
    private readonly IServiceProvider _serviceProvider;

    public IntrospectionInfoFactory(
        IEnumerable<IIntrospectionInfoTransformer>? transformers = null,
        IServiceProvider? serviceProvider = null,
        IDeconstructionValidator? deconstructionValidator = null)
    {
        _transformers = transformers ?? Enumerable.Empty<IIntrospectionInfoTransformer>();
        _serviceProvider = serviceProvider ?? EmptyServiceProvider.Instance;
        _deconstructionValidator = deconstructionValidator;
    }

    private IReadOnlyList<EndpointParameterInfo> GetClassParameters(Type type)
    {
        var propertyInfos = type.GetProperties();

        var result = propertyInfos.Select(propertyInfo =>
        {
            var name = NamingUtils.ToCamelCase(propertyInfo.Name);
            var (realType, isOptional) = ExtractPropertyTypeAndNullability(propertyInfo);

            var propertyType = propertyInfo.PropertyType;

            if (!CanDeconstruct(propertyType) ||
                _deconstructionValidator?.CanDeconstruct(propertyType, propertyInfo) is not true)
            {
                return new EndpointParameterInfo(propertyInfo, name, isOptional, realType);
            }

            var existingProperty = propertyInfos.FirstOrDefault(
                x => x != propertyInfo && x.PropertyType == propertyType);

            if (existingProperty != null)
            {
                throw new DuplicateDeconstructedPropertyException(propertyInfo, existingProperty);
            }

            var children = GetClassParameters(propertyType);
            return new EndpointParameterInfo(propertyInfo, name, isOptional, propertyType, children);
        });

        return result.ToArray();
    }

    private IReadOnlyList<EndpointParameterInfo> GetParameterTree(MethodInfo methodInfo)
    {
        var parameters = methodInfo.GetParameters();

        var result = parameters.Select(parameterInfo =>
        {
            Debug.Assert(parameterInfo.Name is not null);

            var (realType, isOptional) = ExtractParameterTypeAndNullability(parameterInfo);

            var parameterType = parameterInfo.ParameterType;

            if (!CanDeconstruct(parameterType) ||
                _deconstructionValidator?.CanDeconstruct(parameterType, parameterInfo) is not true)
            {
                return new EndpointParameterInfo(parameterInfo, parameterInfo.Name, isOptional, realType);
            }

            var existingParameter = parameters.FirstOrDefault(
                x => x != parameterInfo && x.ParameterType == parameterType);

            if (existingParameter != null)
            {
                throw new DuplicateDeconstructedParameterException(methodInfo, parameterInfo, existingParameter);
            }

            var children = GetClassParameters(parameterType);
            return new EndpointParameterInfo(parameterInfo, parameterInfo.Name, isOptional, realType, children);
        });

        return result.ToArray();
    }

    private (Type, bool) ExtractPropertyTypeAndNullability(PropertyInfo propertyInfo)
    {
        if (propertyInfo.PropertyType.IsValueType)
        {
            var (underlyingType, isOptional) = ExtractValueTypeInfo(propertyInfo.PropertyType);

            if (isOptional)
            {
                return (underlyingType, true);
            }
        }
        else
        {
            var nullabilityInfo = _nullabilityInfoContext.Create(propertyInfo);

            if (nullabilityInfo.ReadState is NullabilityState.Nullable)
            {
                return (propertyInfo.PropertyType, true);
            }
        }

        return (propertyInfo.PropertyType, false);
    }

    private (Type, bool) ExtractParameterTypeAndNullability(ParameterInfo parameterInfo)
    {
        if (parameterInfo.ParameterType.IsValueType)
        {
            var (underlyingType, isOptional) = ExtractValueTypeInfo(parameterInfo.ParameterType);

            if (isOptional)
            {
                return (underlyingType, true);
            }
        }
        else
        {
            var nullabilityInfo = _nullabilityInfoContext.Create(parameterInfo);

            if (nullabilityInfo.ReadState is NullabilityState.Nullable)
            {
                return (parameterInfo.ParameterType, true);
            }
        }

        return (parameterInfo.ParameterType, parameterInfo.IsOptional);
    }

    private static (Type, bool) ExtractValueTypeInfo(Type type)
    {
        var parameterType = Nullable.GetUnderlyingType(type);

        return parameterType is null ? (type, false) : (parameterType, true);
    }

    private EndpointInfo CreateEndpointInfo(MethodInfo methodInfo)
    {
        var parameterTree = GetParameterTree(methodInfo);

        return new EndpointInfo(methodInfo, parameterTree);
    }

    private ControllerInfo CreateControllerInfo(Type type)
    {
        var endpoints = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            // Exclude object methods
            .Where(x => x.GetBaseDefinition().DeclaringType != typeof(object))
            // Exclude explicit implementation
            .Where(x => !x.Name.Contains('.'))
            .Select(CreateEndpointInfo)
            .ToArray();

        return new ControllerInfo(type, endpoints);
    }

    public IntrospectionInfo CreateIntrospectionInfo<TContext>(IEnumerable<Type> lookupTypes)
    {
        ArgumentNullException.ThrowIfNull(lookupTypes);

        var controllers = lookupTypes
            .Where(x => x.IsClass && !x.IsGenericType && !x.IsAbstract)
            .Where(x => x.IsAssignableTo(typeof(IAutoRegistrableController<TContext>)))
            .Select(CreateControllerInfo)
            .ToImmutableArray();

        var introspectionInfo = new IntrospectionInfo(controllers);

        return _transformers.Aggregate(introspectionInfo,
            (info, transformer) => transformer.Transform(info, _serviceProvider));
    }

    private static bool CanDeconstruct(Type type)
    {
        if (type.IsValueType && !type.IsPrimitive)
        {
            return true;
        }

        if (!type.IsClass || type.IsAbstract)
        {
            return false;
        }

        return type.GetConstructor(Type.EmptyTypes) is not null;
    }
}
