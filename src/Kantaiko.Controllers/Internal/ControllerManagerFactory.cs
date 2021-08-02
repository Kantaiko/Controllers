using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Kantaiko.Controllers.Design;
using Kantaiko.Controllers.Design.Endpoints;
using Kantaiko.Controllers.Design.Parameters;
using Kantaiko.Controllers.Exceptions;
using Kantaiko.Controllers.Introspection;
using Kantaiko.Controllers.Matchers;
using Kantaiko.Controllers.Middleware;
using Kantaiko.Controllers.Utils;
using Kantaiko.Controllers.Validation;

namespace Kantaiko.Controllers.Internal
{
    internal class ControllerManagerFactory<TRequest>
    {
        private readonly IDeconstructionValidator _deconstructionValidator;

        public ControllerManagerFactory(IDeconstructionValidator deconstructionValidator)
        {
            _deconstructionValidator = deconstructionValidator;
        }

        private static EndpointManager<TRequest> CreateEndpointManager(EndpointInfo endpointInfo,
            IServiceProvider provider)
        {
            var attributes = endpointInfo.MethodInfo.GetCustomAttributes();

            var matchers = new List<IEndpointMatcher<TRequest>>();
            var middlewares = new List<IEndpointMiddleware<TRequest>>();

            var designContext = new EndpointDesignContext(endpointInfo, provider);

            foreach (var attribute in attributes)
            {
                switch (attribute)
                {
                    case IEndpointMatcherFactory<TRequest> matcherFactory:
                        var matcher = matcherFactory.CreateEndpointMatcher(designContext);
                        matchers.Add(matcher);
                        break;
                    case IEndpointMiddlewareFactory<TRequest> middlewareFactory:
                        var middleware = middlewareFactory.CreateEndpointMiddleware(designContext);
                        middlewares.Add(middleware);
                        break;
                }
            }

            var parameterManagers = CreateParameterManagers(endpointInfo.ParameterTree.Children, provider);
            var parameterManagerTree = new ParameterManagerTree<TRequest>(parameterManagers);

            return new EndpointManager<TRequest>(endpointInfo, matchers, parameterManagerTree, middlewares);
        }

        private static IReadOnlyList<ParameterManager<TRequest>> CreateParameterManagers(
            IReadOnlyList<EndpointParameterInfo> parameterInfos,
            IServiceProvider provider,
            Type? parentType = null)
        {
            var parameters = parameterInfos.Select(info =>
            {
                Type? converterType = null;
                IParameterDefaultValueResolver? defaultValueResolver = null;

                var validators = new List<IParameterPostValidator>();
                var middlewares = new List<IEndpointMiddleware<TRequest>>();

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
                        case IParameterMiddlewareFactory<TRequest> middlewareFactory:
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

                var propertyInfo = parentType?.GetProperty(info.OriginalName);
                Debug.Assert(parentType is null || propertyInfo is not null);

                if (info is not EndpointParameterNode node)
                {
                    return new ParameterManager<TRequest>(info, middlewares, validators, converterType,
                        defaultValueResolver, propertyInfo);
                }

                var children = CreateParameterManagers(node.Children, provider, node.ParameterType);

                return new ParameterManager<TRequest>(info, middlewares, validators,
                    converterType, defaultValueResolver, propertyInfo, children);
            });

            return parameters.ToArray();
        }

        private static string? GetParameterName(IReadOnlyDictionary<string, object> properties)
        {
            return properties.TryGetValue(KantaikoParameterProperties.Name, out var property)
                ? property as string
                : null;
        }

        private static bool GetParameterNullability(IReadOnlyDictionary<string, object> properties)
        {
            return properties.TryGetValue(KantaikoParameterProperties.IsOptional, out var property) && property is true;
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
                    return new EndpointParameterInfo(endpointInfo, designProperties, propertyInfo.Name, name, realType,
                        isOptional, propertyInfo);
                }

                var existingProperty = properties.FirstOrDefault(
                    x => x != propertyInfo && x.PropertyType == propertyType);

                if (existingProperty != null)
                {
                    throw new DuplicateDeconstructedPropertyException(propertyInfo, existingProperty);
                }

                var children = GetClassParameters(endpointInfo, propertyType);
                return new EndpointParameterNode(endpointInfo, designProperties, propertyInfo.Name, name, propertyType,
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
                var (realType, isOptional) = ExtractParameterTypeAndNullability(parameterInfo);

                if (hasDefaultValueResolver)
                {
                    isOptional = true;
                }

                var canDeconstruct = !hasCustomConverter && _deconstructionValidator.CanDeconstruct(parameterType);
                if (parameterType.IsPrimitive || !parameterType.IsClass || !canDeconstruct)
                {
                    return new EndpointParameterInfo(endpointInfo, designProperties, parameterInfo.Name, name, realType,
                        isOptional, parameterInfo);
                }

                var existingParameter = parameters.FirstOrDefault(
                    x => x != parameterInfo && x.ParameterType == parameterType);

                if (existingParameter != null)
                {
                    throw new DuplicateDeconstructedParameterException(methodInfo, parameterInfo, existingParameter);
                }

                var children = GetClassParameters(endpointInfo, parameterType);
                return new EndpointParameterNode(endpointInfo, designProperties, parameterInfo.Name, name, realType,
                    isOptional,
                    parameterInfo, children);
            });

            return new EndpointParameterTree(result.ToList());
        }

        private static (Type, bool) ExtractPropertyTypeAndNullability(IReadOnlyDictionary<string, object> properties,
            PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType.IsValueType
                ? ExtractValueTypeInfo(propertyInfo.PropertyType)
                : (propertyInfo.PropertyType, GetParameterNullability(properties));
        }

        private static (Type, bool) ExtractParameterTypeAndNullability(ParameterInfo parameterInfo)
        {
            return parameterInfo.ParameterType.IsValueType
                ? ExtractValueTypeInfo(parameterInfo.ParameterType)
                : (parameterInfo.ParameterType, parameterInfo.IsOptional);
        }

        private static (Type, bool) ExtractValueTypeInfo(Type type)
        {
            var parameterType = Nullable.GetUnderlyingType(type);
            return parameterType is null ? (type, false) : (parameterType, true);
        }

        private EndpointInfo? CreateEndpointInfo(ControllerInfo controllerInfo, MethodInfo methodInfo)
        {
            var hasEndpointMatcher = methodInfo.GetCustomAttributes().Any(x => x is IEndpointMatcherFactory<TRequest>);
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

            var endpoints = type.GetMethods()
                .Select(methodInfo => CreateEndpointInfo(controllerInfo, methodInfo))
                .Where(endpoint => endpoint is not null)
                .ToArray();

            controllerInfo.SetEndpoints(endpoints!);
            return controllerInfo;
        }

        private ControllerManager<TRequest> CreateControllerManager(Type type, IServiceProvider provider)
        {
            var controllerInfo = CreateControllerInfo(type);

            var endpoints = controllerInfo.Endpoints
                .Select(endpointInfo => CreateEndpointManager(endpointInfo, provider))
                .ToArray();

            return new ControllerManager<TRequest>(controllerInfo, endpoints);
        }

        public ControllerManagerCollection<TRequest> CreateControllerManagerCollection(
            IControllerCollection controllerCollection, IServiceProvider serviceProvider)
        {
            var controllerManagers = controllerCollection.ControllerTypes
                .Select(type => CreateControllerManager(type, serviceProvider))
                .ToArray();

            return new ControllerManagerCollection<TRequest>(controllerManagers);
        }
    }
}
