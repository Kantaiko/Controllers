using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Kantaiko.Controllers.Design.Parameters;
using Kantaiko.Controllers.Introspection;
using Kantaiko.Controllers.Middleware;
using Kantaiko.Controllers.Validation;

namespace Kantaiko.Controllers.Internal
{
    internal class ParameterManager<TRequest>
    {
        public EndpointParameterInfo Info { get; }

        public ParameterManager(EndpointParameterInfo info,
            IReadOnlyList<IEndpointMiddleware<TRequest>> middlewares,
            IReadOnlyList<IParameterPostValidator> validators,
            Type? converterType,
            IParameterDefaultValueResolver? defaultValueResolver = null,
            PropertyInfo? propertyInfo = null,
            IReadOnlyList<ParameterManager<TRequest>>? children = null)
        {
            Info = info;
            Middlewares = middlewares;
            Validators = validators;
            ConverterType = converterType;
            DefaultValueResolver = defaultValueResolver;
            PropertyInfo = propertyInfo;
            Children = children;
        }

        public IReadOnlyList<IEndpointMiddleware<TRequest>> Middlewares { get; }
        public IReadOnlyList<IParameterPostValidator> Validators { get; }
        public Type? ConverterType { get; }
        public IParameterDefaultValueResolver? DefaultValueResolver { get; }
        public PropertyInfo? PropertyInfo { get; }
        public IReadOnlyList<ParameterManager<TRequest>>? Children { get; }

        public ValidationResult Validate(ParameterPostValidationContext context, object value)
        {
            foreach (var validator in Validators)
            {
                var result = validator.Validate(context, value);
                if (!result.IsValid) return result;
            }

            return ValidationResult.Success;
        }

        public async Task InvokeMiddleware(EndpointMiddlewareContext<TRequest> endpointMiddlewareContext,
            CancellationToken cancellationToken)
        {
            foreach (var middleware in Middlewares)
            {
                await middleware.HandleAsync(endpointMiddlewareContext, cancellationToken);
                if (!endpointMiddlewareContext.ShouldProcess)
                    return;
            }
        }
    }
}
