using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Kantaiko.Controllers.Design;
using Kantaiko.Controllers.Design.Endpoints;
using Kantaiko.Controllers.Design.Parameters;
using Kantaiko.Controllers.Design.Properties;
using Kantaiko.Controllers.Exceptions;
using Kantaiko.Controllers.Introspection;
using Kantaiko.Controllers.Matchers;
using Kantaiko.Controllers.Middleware;
using Kantaiko.Controllers.Utils;
using Kantaiko.Controllers.Validation;

namespace Kantaiko.Controllers.Internal
{
    internal class ControllerManagerFactory<TContext>
    {
        private readonly IDeconstructionValidator _deconstructionValidator;

        private readonly NullabilityInfoContext _nullabilityContext = new();

        public ControllerManagerFactory(IDeconstructionValidator deconstructionValidator)
        {
            _deconstructionValidator = deconstructionValidator;
        }

        private static EndpointManager<TContext> CreateEndpointManager(EndpointInfo endpointInfo,
            IServiceProvider provider)
        {
            var attributes = endpointInfo.MethodInfo.GetCustomAttributes();

            var matchers = new List<IEndpointMatcher<TContext>>();
            var middlewares = new List<IEndpointMiddleware<TContext>>();

            var designContext = new EndpointDesignContext(endpointInfo, provider);

            foreach (var attribute in attributes)
            {
                switch (attribute)
                {
                    case IEndpointMatcherFactory<TContext> matcherFactory:
                        var matcher = matcherFactory.CreateEndpointMatcher(designContext);
                        matchers.Add(matcher);
                        break;
                    case IEndpointMiddlewareFactory<TContext> middlewareFactory:
                        var middleware = middlewareFactory.CreateEndpointMiddleware(designContext);
                        middlewares.Add(middleware);
                        break;
                }
            }

            var parameterManagers = CreateParameterManagers(endpointInfo.ParameterTree.Children, provider);
            var parameterManagerTree = new ParameterManagerTree<TContext>(parameterManagers);

            return new EndpointManager<TContext>(endpointInfo, matchers, parameterManagerTree, middlewares);
        }

        private static IReadOnlyList<ParameterManager<TContext>> CreateParameterManagers(
            IReadOnlyList<EndpointParameterInfo> parameterInfos,
            IServiceProvider provider)
        {
            var parameters = parameterInfos.Select(info =>
            {
                Type? converterType = null;
                IParameterDefaultValueResolver? defaultValueResolver = null;

                var validators = new List<IParameterPostValidator>();
                var middlewares = new List<IEndpointMiddleware<TContext>>();

                var designContext = new EndpointParameterDesignContext(info, provider);

                foreach (var attribute in info.AttributeProvider.GetCustomAttributes(true))
                {
                    switch (attribute)
                    {
                        case IParameterConverterTypeProvider converterFactory:
                        {
                            converterType = converterFactory.GetConverterType(designContext);
                            break;
                        }
                        case IParameterDefaultValueResolverFactory resolverFactory:
                        {
                            defaultValueResolver = resolverFactory.CreateParameterDefaultValueResolver(designContext);
                            break;
                        }
                        case IParameterMiddlewareFactory<TContext> middlewareFactory:
                        {
                            var middleware = middlewareFactory.CreateParameterMiddleware(designContext);
                            middlewares.Add(middleware);
                            break;
                        }
                        case IParameterPostValidatorFactory validatorFactory:
                        {
                            var validator = validatorFactory.CreateParameterPostValidator(designContext);
                            validators.Add(validator);
                            break;
                        }
                    }
                }

                PropertyInfo? parameterPropertyInfo = null;

                /*
                 * TODO don't exploit AttributeProvider to access reflection member info
                 * or expose it with better name and type
                 */
                if (defaultValueResolver is null)
                {
                    defaultValueResolver = ConstantDefaultValueResolver.NullResolver;

                    if (info.AttributeProvider is ParameterInfo parameterInfo)
                    {
                        if (parameterInfo.ParameterType.IsValueType)
                        {
                            defaultValueResolver = parameterInfo.HasDefaultValue
                                ? new ConstantDefaultValueResolver(parameterInfo.DefaultValue!)
                                : new ValueTypeDefaultValueResolver(parameterInfo.ParameterType);
                        }
                    }

                    if (info.AttributeProvider is PropertyInfo propertyInfo)
                    {
                        parameterPropertyInfo = propertyInfo;

                        if (propertyInfo.PropertyType.IsValueType)
                        {
                            defaultValueResolver = new ValueTypeDefaultValueResolver(propertyInfo.PropertyType);
                        }
                    }
                }

                if (info is not EndpointParameterNode node)
                {
                    return new ParameterManager<TContext>(info, middlewares, validators, converterType,
                        defaultValueResolver, parameterPropertyInfo);
                }

                var children = CreateParameterManagers(node.Children, provider);

                return new ParameterManager<TContext>(info, middlewares, validators,
                    converterType, defaultValueResolver, parameterPropertyInfo, children);
            });

            return parameters.ToArray();
        }

        private static string? GetParameterName(IDesignPropertyCollection properties)
        {
            return properties.TryGetProperty<string>(KantaikoParameterProperties.Name, out var property)
                ? property
                : null;
        }

        private static bool GetParameterNullability(IDesignPropertyCollection properties)
        {
            return properties.TryGetProperty<bool>(KantaikoParameterProperties.IsOptional, out var property) &&
                   property is true;
        }

        private IReadOnlyList<EndpointParameterInfo> GetClassParameters(EndpointInfo endpointInfo, Type type)
        {
            var properties = type.GetProperties();

            var result = properties.Select(propertyInfo =>
            {
                var propertyType = propertyInfo.PropertyType;

                var designProperties = DesignPropertyExtractor.GetProperties<IParameterDesignPropertyProvider>(
                    propertyInfo,
                    x => x.GetParameterDesignProperties());

                var hasCustomConverter = propertyInfo.GetCustomAttributes().Any(x =>
                    x is IParameterConverterTypeProvider or IParameterConverterFactoryProvider);

                var hasDefaultValueResolver = propertyInfo.GetCustomAttributes()
                    .Any(x => x is IParameterDefaultValueResolverFactory);

                var name = GetParameterName(designProperties) ?? NamingUtils.ToCamelCase(propertyInfo.Name);
                var (realType, isOptional) = ExtractPropertyTypeAndNullability(designProperties, propertyInfo);

                if (hasDefaultValueResolver)
                {
                    isOptional = true;
                }

                var canDeconstruct = !hasCustomConverter && _deconstructionValidator.CanDeconstruct(propertyType);
                if (propertyType.IsPrimitive || !propertyType.IsClass || !canDeconstruct)
                {
                    return new EndpointParameterInfo(endpointInfo, designProperties, name,
                        realType, isOptional, propertyInfo);
                }

                var existingProperty = properties.FirstOrDefault(
                    x => x != propertyInfo && x.PropertyType == propertyType);

                if (existingProperty != null)
                {
                    throw new DuplicateDeconstructedPropertyException(propertyInfo, existingProperty);
                }

                var children = GetClassParameters(endpointInfo, propertyType);
                return new EndpointParameterNode(endpointInfo, designProperties, name, propertyType,
                    isOptional, propertyInfo, children);
            });

            return result.ToArray();
        }

        private EndpointParameterTree GetParameterTree(MethodInfo methodInfo, EndpointInfo endpointInfo)
        {
            var parameters = methodInfo.GetParameters();

            var result = parameters.Select(parameterInfo =>
            {
                var parameterType = parameterInfo.ParameterType;

                var designProperties = DesignPropertyExtractor.GetProperties<IParameterDesignPropertyProvider>(
                    parameterInfo,
                    x => x.GetParameterDesignProperties());

                Debug.Assert(parameterInfo.Name is not null);

                var hasCustomConverter = parameterInfo.GetCustomAttributes().Any(x =>
                    x is IParameterConverterTypeProvider or IParameterConverterFactoryProvider);

                var hasDefaultValueResolver = parameterInfo.GetCustomAttributes()
                    .Any(x => x is IParameterDefaultValueResolverFactory);

                var name = GetParameterName(designProperties) ?? parameterInfo.Name;
                var (realType, isOptional) = ExtractParameterTypeAndNullability(designProperties, parameterInfo);

                if (hasDefaultValueResolver)
                {
                    isOptional = true;
                }

                var canDeconstruct = !hasCustomConverter && _deconstructionValidator.CanDeconstruct(parameterType);
                if (parameterType.IsPrimitive || !parameterType.IsClass || !canDeconstruct)
                {
                    return new EndpointParameterInfo(endpointInfo, designProperties, name,
                        realType, isOptional, parameterInfo);
                }

                var existingParameter = parameters.FirstOrDefault(
                    x => x != parameterInfo && x.ParameterType == parameterType);

                if (existingParameter != null)
                {
                    throw new DuplicateDeconstructedParameterException(methodInfo, parameterInfo, existingParameter);
                }

                var children = GetClassParameters(endpointInfo, parameterType);
                return new EndpointParameterNode(endpointInfo, designProperties, name, realType,
                    isOptional,
                    parameterInfo, children);
            });

            return new EndpointParameterTree(result.ToList());
        }

        private (Type, bool) ExtractPropertyTypeAndNullability(IDesignPropertyCollection properties,
            PropertyInfo propertyInfo)
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
                var nullabilityInfo = _nullabilityContext.Create(propertyInfo);

                if (nullabilityInfo.ReadState == NullabilityState.Nullable)
                {
                    return (propertyInfo.PropertyType, true);
                }
            }

            return (propertyInfo.PropertyType, GetParameterNullability(properties));
        }

        private (Type, bool) ExtractParameterTypeAndNullability(IDesignPropertyCollection properties,
            ParameterInfo parameterInfo)
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
                var nullabilityInfo = _nullabilityContext.Create(parameterInfo);

                if (nullabilityInfo.ReadState == NullabilityState.Nullable)
                {
                    return (parameterInfo.ParameterType, true);
                }
            }

            return (parameterInfo.ParameterType, parameterInfo.IsOptional || GetParameterNullability(properties));
        }

        private static (Type, bool) ExtractValueTypeInfo(Type type)
        {
            var parameterType = Nullable.GetUnderlyingType(type);
            return parameterType is null ? (type, false) : (parameterType, true);
        }

        private EndpointInfo? CreateEndpointInfo(ControllerInfo controllerInfo, MethodInfo methodInfo)
        {
            var hasEndpointMatcher = methodInfo.GetCustomAttributes().Any(x => x is IEndpointMatcherFactory<TContext>);
            if (!hasEndpointMatcher) return null;

            var endpointInfo = new EndpointInfo(controllerInfo, methodInfo);

            var parameterTree = GetParameterTree(methodInfo, endpointInfo);
            endpointInfo.SetParameterTree(parameterTree);

            var duplicate = endpointInfo.Parameters
                .GroupBy(x => x.Name)
                .FirstOrDefault(x => x.Count() > 1);

            if (duplicate is not null)
                throw new DuplicateParameterNameException(methodInfo, duplicate.Key);

            return endpointInfo;
        }

        private ControllerInfo CreateControllerInfo(Type type)
        {
            var controllerInfo = new ControllerInfo(type);

            var endpoints = type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Select(methodInfo => CreateEndpointInfo(controllerInfo, methodInfo))
                .Where(endpoint => endpoint is not null)
                .ToArray();

            controllerInfo.SetEndpoints(endpoints!);
            return controllerInfo;
        }

        private ControllerManager<TContext> CreateControllerManager(Type type, IServiceProvider provider)
        {
            var controllerInfo = CreateControllerInfo(type);

            var endpoints = controllerInfo.Endpoints
                .Select(endpointInfo => CreateEndpointManager(endpointInfo, provider))
                .ToArray();

            return new ControllerManager<TContext>(controllerInfo, endpoints);
        }

        public ControllerManagerCollection<TContext> CreateControllerManagerCollection(
            IControllerCollection controllerCollection, IServiceProvider serviceProvider)
        {
            var controllerManagers = controllerCollection.ControllerTypes
                .Where(type => type.IsAssignableTo(typeof(IContextAcceptor<TContext>)))
                .Select(type => CreateControllerManager(type, serviceProvider))
                .ToArray();

            return new ControllerManagerCollection<TContext>(controllerManagers);
        }
    }
}
