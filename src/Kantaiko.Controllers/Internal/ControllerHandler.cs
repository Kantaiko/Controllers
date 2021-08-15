﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Kantaiko.Controllers.Converters;
using Kantaiko.Controllers.Middleware;
using Kantaiko.Controllers.Resources;
using Kantaiko.Controllers.Result;
using Kantaiko.Controllers.Validation;

namespace Kantaiko.Controllers.Internal
{
    internal class ControllerHandler<TContext> where TContext : notnull
    {
        private readonly ControllerManagerCollection<TContext> _controllerManagerCollection;
        private readonly IConverterCollection _converterCollection;
        private readonly IInstanceFactory _instanceFactory;
        private readonly IMiddlewareCollection _middlewareCollection;
        private readonly IServiceProvider _serviceProvider;

        public ControllerHandler(ControllerManagerCollection<TContext> controllerManagerCollection,
            IConverterCollection converterCollection, IInstanceFactory instanceFactory,
            IMiddlewareCollection middlewareCollection, IServiceProvider serviceProvider)
        {
            _controllerManagerCollection = controllerManagerCollection;
            _converterCollection = converterCollection;
            _instanceFactory = instanceFactory;
            _middlewareCollection = middlewareCollection;
            _serviceProvider = serviceProvider;
        }

        public async Task<RequestProcessingResult> HandleAsync(TContext context,
            IServiceProvider? serviceProvider,
            CancellationToken cancellationToken)
        {
            serviceProvider ??= _serviceProvider;

            // 1. Resolve controller
            var controllerMatchResult = _controllerManagerCollection.MatchController(context, serviceProvider);
            if (!controllerMatchResult.IsMatched) return RequestProcessingResult.NotMatched;

            var (controllerManager, endpointManager, matchResult) = controllerMatchResult;

            if (!matchResult.IsSuccess)
            {
                var errorMessage = matchResult.ErrorMessage;
                Debug.Assert(errorMessage is not null);

                return RequestProcessingResult.Error(RequestErrorStage.EndpointMatching, errorMessage, null);
            }

            // 2. Prepare execution context

            var parameterContexts = endpointManager.Parameters.Select(x =>
            {
                var conversionContext = new ParameterConversionContext(x.Info, matchResult.Parameters, serviceProvider);
                var converterType = x.ConverterType ?? _converterCollection.ResolveConverterType(x.Info.ParameterType);
                var converter = _instanceFactory.CreateInstance(converterType, serviceProvider);

                Debug.Assert(converter is IParameterConverter);

                return new ExecutionParameterContext(conversionContext,
                    (IParameterConverter)converter, x.DefaultValueResolver);
            }).ToDictionary(k => k.ConversionContext.Info.Name, v => v);

            var executionContext = new RequestExecutionContext<TContext>(parameterContexts, endpointManager);

            // Creating middleware context and dispatcher
            var middlewareContext = new EndpointMiddlewareContext<TContext>(context, executionContext, serviceProvider);

            var middlewareDispatcher = new MiddlewareDispatcher<TContext>(_middlewareCollection, _instanceFactory,
                serviceProvider);

            // Invokes middleware stage and returns true if handler should stop the execution
            async Task<bool> MoveNextStage(EndpointMiddlewareStage stage)
            {
                middlewareContext.Stage = stage;

                await middlewareDispatcher.HandleAsync(middlewareContext, cancellationToken);
                await endpointManager.InvokeMiddleware(middlewareContext, cancellationToken);

                return !middlewareContext.ShouldProcess;
            }

            // 4. Check parameter existence
            if (await MoveNextStage(EndpointMiddlewareStage.BeforeExistenceCheck))
            {
                return RequestProcessingResult.Interrupted(EndpointMiddlewareStage.BeforeExistenceCheck);
            }

            var existenceCheckResult = CheckParameterValuesExistence(executionContext);
            if (!existenceCheckResult.IsSuccess)
            {
                return RequestProcessingResult.Error(RequestErrorStage.ParameterExistenceCheck,
                    existenceCheckResult.ErrorMessage, existenceCheckResult.Parameter);
            }

            // 5. Validate parameters
            if (await MoveNextStage(EndpointMiddlewareStage.BeforeValidation))
            {
                return RequestProcessingResult.Interrupted(EndpointMiddlewareStage.BeforeValidation);
            }

            var validationResult = ValidateParameters(executionContext);
            if (!validationResult.IsSuccess)
            {
                return RequestProcessingResult.Error(RequestErrorStage.ParameterValidation,
                    validationResult.ErrorMessage, validationResult.Parameter);
            }

            // 6. Resolve parameters
            if (await MoveNextStage(EndpointMiddlewareStage.BeforeResolution))
            {
                return RequestProcessingResult.Interrupted(EndpointMiddlewareStage.BeforeResolution);
            }

            var resolutionResult = await ResolveParameters(executionContext, cancellationToken);
            if (!resolutionResult.IsSuccess)
            {
                return RequestProcessingResult.Error(RequestErrorStage.ParameterResolution,
                    resolutionResult.ErrorMessage, resolutionResult.Parameter);
            }

            // 7. Post validate parameters
            if (await MoveNextStage(EndpointMiddlewareStage.BeforePostValidation))
            {
                return RequestProcessingResult.Interrupted(EndpointMiddlewareStage.BeforePostValidation);
            }

            var postValidationResult = PostValidateParameters(executionContext, serviceProvider);
            if (!postValidationResult.IsSuccess)
            {
                return RequestProcessingResult.Error(RequestErrorStage.ParameterPostValidation,
                    postValidationResult.ErrorMessage, postValidationResult.Parameter);
            }

            // 8. Instantiate controller
            if (await MoveNextStage(EndpointMiddlewareStage.BeforeInstanceCreation))
            {
                return RequestProcessingResult.Interrupted(EndpointMiddlewareStage.BeforeInstanceCreation);
            }

            var controller = (IContextAcceptor<TContext>)_instanceFactory.CreateInstance(controllerManager.Info.Type,
                serviceProvider);

            controller.SetContext(context);

            executionContext.ControllerInstance = controller;

            // 9. Invoke endpoint method
            if (await MoveNextStage(EndpointMiddlewareStage.BeforeExecution))
            {
                return RequestProcessingResult.Interrupted(EndpointMiddlewareStage.BeforeExecution);
            }

            var parameters = CreateParameters(executionContext);
            var result = await endpointManager.Invoke(controller, parameters);

            var processingResult = result.Exception is not null
                ? RequestProcessingResult.Exception(result.Exception)
                : RequestProcessingResult.Success(result.Result);

            executionContext.ProcessingResult = processingResult;

            // 10. Execute before completion stage and return processing result
            if (await MoveNextStage(EndpointMiddlewareStage.BeforeCompletion))
            {
                return RequestProcessingResult.Interrupted(EndpointMiddlewareStage.BeforeCompletion);
            }

            return processingResult;
        }

        private static object CreateDeconstructedParameter(Type type,
            IEnumerable<ParameterManager<TContext>> parameterManagers,
            RequestExecutionContext<TContext> context)
        {
            var instance = Activator.CreateInstance(type);
            Debug.Assert(instance is not null);

            foreach (var child in parameterManagers)
            {
                Debug.Assert(child.PropertyInfo is not null);

                var value = child.Children is not null
                    ? CreateDeconstructedParameter(child.Info.ParameterType, child.Children, context)
                    : context.Parameters[child.Info.Name].Value;

                child.PropertyInfo.SetValue(instance, value);
            }

            return instance;
        }

        private static object[] CreateParameters(RequestExecutionContext<TContext> context)
        {
            var parameters = context.EndpointManager.ParameterTree.Children.Select(parameterManager =>
            {
                if (parameterManager.Children is not null)
                {
                    return CreateDeconstructedParameter(parameterManager.Info.ParameterType,
                        parameterManager.Children, context);
                }

                return context.Parameters[parameterManager.Info.Name].Value;
            });

            return parameters.ToArray()!;
        }

        /// <summary>
        /// Checks that all required parameters have their values.
        /// </summary>
        /// <returns>successful operation result if all requirements are met</returns>
        private static OperationResult CheckParameterValuesExistence(RequestExecutionContext<TContext> context)
        {
            foreach (var parameterContext in context.Parameters.Values)
            {
                var exists = parameterContext.ResolvedConverter.CheckValueExistence(parameterContext.ConversionContext);

                if (exists)
                {
                    parameterContext.ValueExists = true;
                    continue;
                }

                if (parameterContext.ConversionContext.Info.IsOptional) continue;

                var errorMessage = string.Format(Locale.MissingRequiredParameter, parameterContext.ConversionContext.Info.Name);
                return OperationResult.Failure(errorMessage, parameterContext.ConversionContext.Info);
            }

            return OperationResult.Success;
        }

        /// <summary>
        /// Checks that all parameter validators reports no errors.
        /// </summary>
        /// <param name="context"></param>
        /// <returns>successful operation result if all parameters are valid</returns>
        private static OperationResult ValidateParameters(RequestExecutionContext<TContext> context)
        {
            foreach (var parameterContext in context.Parameters.Values)
            {
                if (!parameterContext.ValueExists) continue;

                var validationResult = parameterContext.ResolvedConverter.Validate(parameterContext.ConversionContext);
                if (validationResult.IsValid) continue;

                return OperationResult.Failure(validationResult.ErrorMessage, parameterContext.ConversionContext.Info);
            }

            return OperationResult.Success;
        }

        private static object? GetDefault(Type type) => type.IsValueType ? Activator.CreateInstance(type) : null;

        /// <summary>
        /// Resolvers parameter values.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>successful operation result if all parameters were successfully resolved</returns>
        private static async Task<OperationResult> ResolveParameters(RequestExecutionContext<TContext> context,
            CancellationToken cancellationToken)
        {
            foreach (var parameterContext in context.Parameters.Values)
            {
                if (!parameterContext.ValueExists)
                {
                    var value = parameterContext.DefaultValueResolver is not null
                        ? parameterContext.DefaultValueResolver.ResolveDefaultValue(parameterContext.ConversionContext)
                        : GetDefault(parameterContext.ConversionContext.Info.ParameterType);
                    parameterContext.Value = value;
                    continue;
                }

                var resolutionResult = await parameterContext.ResolvedConverter.Resolve(parameterContext.ConversionContext,
                    cancellationToken);

                if (!resolutionResult.Success)
                    return OperationResult.Failure(resolutionResult.ErrorMessage, parameterContext.ConversionContext.Info);

                parameterContext.Value = resolutionResult.Value;
            }

            return OperationResult.Success;
        }

        private static OperationResult PostValidateParameters(RequestExecutionContext<TContext> context,
            IServiceProvider serviceProvider)
        {
            foreach (var parameterManager in context.EndpointManager.Parameters)
            {
                var parameterValidationContext =
                    new ParameterPostValidationContext(parameterManager.Info, serviceProvider);

                var parameterContext = context.Parameters[parameterManager.Info.Name];
                if (parameterContext.Value is null) continue;

                var validationResult = parameterManager.Validate(parameterValidationContext, parameterContext.Value);
                if (validationResult.IsValid) continue;

                return OperationResult.Failure(validationResult.ErrorMessage, parameterManager.Info);
            }

            return OperationResult.Success;
        }
    }
}
