# Middleware

- [Getting started](getting-started.md)
- [Endpoint matching](endpoint-matching.md)
- [Parameter conversion](parameter-conversion.md)
- [Parameter deconstruction](parameter-deconstruction.md)
- [Parameter post-validation](parameter-post-validation.md)
- Middleware
- [Another features](another-features.md)

There is the middleware system allowing you to implement various use cases like request/response filters, authentication
and authorization.

To define the global middleware you should inherit it from the `EndpointMiddleware<TContext>` class and implement
abstract members.

```c#
internal class TestEndpointMiddleware : EndpointMiddleware<TestContext>
{
    public EndpointMiddlewareStage Stage => EndpointMiddlewareStage.BeforeExecution;

    public Task HandleAsync(EndpointMiddlewareContext<TestContext> context, CancellationToken cancellationToken)
    {
        context.ExecutionContext.Parameters["a"].Value = 42;
        return Task.CompletedTask;
    }
}
```

Middleware context provides two main possibilities: accessing `ExecutionContext` to get or override resolved parameters
and `ShouldProcess` property that can be set to `false` to interrupt the execution. You can also access the
`ControllerInstance` and `ProcessingResult` properties in the `ExecutionContext`, starting with the `BeforeExecution`
and `BeforeCompletion` stages, respectively.

If middleware interrupts execution, `ExitReason` of `RequestProcessingResult` will be of type `InterruptionExitReason`
and will contain the middleware stage where execution was interrupted.

You can also define middleware, which should be manually applied by attributes:

```c#
internal class AuthenticationMiddleware : EndpointMiddleware<TestContext>
{
    public EndpointMiddlewareStage Stage => EndpointMiddlewareStage.BeforeExecution;

    public Task HandleAsync(EndpointMiddlewareContext<TestContext> context, CancellationToken cancellationToken)
    {
        if (some auth check logic) {
            // Auth failed
            context.ShouldProcess = false;
            return Task.CompletedTask;
        }

        return Task.CompletedTask;
    }
}

private class AuthorizeAttribute : Attribute, IEndpointMiddlewareFactory<TestContext>,
    IParameterMiddlewareFactory<TestContext>
{
    public IEndpointMiddleware<TestContext> CreateEndpointMiddleware(EndpointDesignContext context)
    {
        return new AuthenticationMiddleware();
    }

    public IEndpointMiddleware<TestContext> CreateParameterMiddleware(EndpointParameterDesignContext context)
    {
        return new AuthenticationMiddleware();
    }
}
```

In this example `[Authorize]` attribute can be applied for both: endpoints and parameters.

> ⚠ Note that all middleware handlers are singletons. So don't store any request specific data in the middleware instances.

Read next: [Another features](another-features.md)
