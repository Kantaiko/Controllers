using System.Collections.Immutable;
using System.Diagnostics;
using System.Reflection;
using Kantaiko.Controllers.Exceptions;
using Kantaiko.Controllers.Execution;
using Kantaiko.Controllers.Introspection.Deconstruction;
using Kantaiko.Controllers.Introspection.Transformers;
using Kantaiko.Controllers.Utils;

namespace Kantaiko.Controllers.Introspection;

/// <summary>
/// The factory that creates and transforms <see cref="IntrospectionInfo" />.
/// </summary>
public sealed class IntrospectionInfoFactory
{
    private readonly NullabilityInfoContext _nullabilityInfoContext = new();

    private readonly IDeconstructionValidator? _deconstructionValidator;
    private readonly IEnumerable<IIntrospectionInfoTransformer> _transformers;
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Creates a new instance of <see cref="IntrospectionInfoFactory" />.
    /// </summary>
    /// <param name="transformers">The list of transformers to apply to the introspection info.</param>
    /// <param name="deconstructionValidator">The deconstruction validator to use.</param>
    /// <param name="serviceProvider">The service provider to use.</param>
    public IntrospectionInfoFactory(
        IEnumerable<IIntrospectionInfoTransformer>? transformers = null,
        IDeconstructionValidator? deconstructionValidator = null,
        IServiceProvider? serviceProvider = null)
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
                return new EndpointParameterInfo(propertyInfo, name, isOptional, realType, propertyType, null);
            }

            var existingProperty = propertyInfos.FirstOrDefault(
                x => x != propertyInfo && x.PropertyType == propertyType);

            if (existingProperty != null)
            {
                throw new DuplicateCompositePropertyException(propertyInfo, existingProperty);
            }

            var children = GetClassParameters(propertyType);
            return new EndpointParameterInfo(propertyInfo, name, isOptional, propertyType,
                propertyType, null, children: children);
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
            var defaultValue = parameterInfo.HasDefaultValue ? parameterInfo.DefaultValue : null;

            if (!CanDeconstruct(parameterType) ||
                _deconstructionValidator?.CanDeconstruct(parameterType, parameterInfo) is not true)
            {
                return new EndpointParameterInfo(parameterInfo, parameterInfo.Name,
                    isOptional, realType, parameterType, defaultValue);
            }

            var existingParameter = parameters.FirstOrDefault(
                x => x != parameterInfo && x.ParameterType == parameterType);

            if (existingParameter != null)
            {
                throw new DuplicateCompositeParameterException(methodInfo, parameterInfo, existingParameter);
            }

            var children = GetClassParameters(parameterType);
            return new EndpointParameterInfo(parameterInfo, parameterInfo.Name, isOptional,
                realType, parameterType, defaultValue, children: children);
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

    /// <summary>
    /// Creates a new <see cref="IntrospectionInfo" /> from the given <paramref name="lookupTypes" />.
    /// </summary>
    /// <param name="lookupTypes">The types to lookup controllers from.</param>
    /// <typeparam name="TController">The base type of the controllers.</typeparam>
    /// <returns>The created <see cref="IntrospectionInfo" />.</returns>
    public IntrospectionInfo CreateIntrospectionInfo<TController>(IEnumerable<Type> lookupTypes)
    {
        ArgumentNullException.ThrowIfNull(lookupTypes);

        var controllers = lookupTypes
            .Where(x => x is { IsClass: true, IsGenericType: false, IsAbstract: false })
            .Where(x => x.IsAssignableTo(typeof(TController)))
            .Select(CreateControllerInfo)
            .ToImmutableArray();

        var introspectionInfo = new IntrospectionInfo(controllers);

        return _transformers.Aggregate(introspectionInfo,
            (info, transformer) => transformer.Transform(info, _serviceProvider));
    }

    private static bool CanDeconstruct(Type type)
    {
        if (type is { IsValueType: true, IsPrimitive: false })
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
