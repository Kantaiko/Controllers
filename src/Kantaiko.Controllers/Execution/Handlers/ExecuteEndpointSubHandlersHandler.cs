﻿using System.Threading.Tasks;
using Kantaiko.Controllers.Exceptions;
using Kantaiko.Controllers.Execution.Properties;
using Kantaiko.Controllers.Result;

namespace Kantaiko.Controllers.Execution.Handlers;

public class ExecuteEndpointSubHandlersHandler<TContext> : ControllerExecutionHandler<TContext>
{
    protected override Task<ControllerResult> HandleAsync(ControllerContext<TContext> context, NextAction next)
    {
        PropertyNullException.ThrowIfNull(context.Endpoint);

        if (EndpointExecutionProperties<TContext>.Of(context.Endpoint)?.Handler is { } handler)
        {
            return handler.Handle(context, _ => next());
        }

        return next();
    }
}