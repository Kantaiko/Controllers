# Another features

- [Getting started](getting-started.md)
- [Endpoint matching](endpoint-matching.md)
- [Parameter conversion](parameter-conversion.md)
- [Parameter deconstruction](parameter-deconstruction.md)
- [Parameter post-validation](parameter-post-validation.md)
- [Middleware](middleware.md)
- Another features

## Dependency injection

This library was designed to support dependency injection. In almost every context you can find the `ServiceProvider`
property of built-in type `System.IServiceProvider`. It allows you not to depend on dependency injection library, so you
can bring any DI container you like. You can pass a global `IServiceProvider` instance to the `RequestHandler` and
additional scoped instances to the each `HandleAsync` call.

There is also the `IInstanceFactory` service that is used to instantiate controllers, converters and middleware
handlers. The default implementation just creates new instances using `Activator.CreateInstance()`, so you need to
reimplement it to support dependency injection.

You can find an example of using this library with `Microsoft.Extensions.DependencyInjection`
in [`examples/DependencyInjection`](/libraries/Kantaiko.Extensions.Controllers/examples/DependencyInjection)
project.

## Localization

The request execution pipeline and some built-in converters use localized strings. The default locale is depends on the
host language, but you can set a specific culture for the current thread before calling the `HandleAsync` method.

```c#
Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");

var result = await requestHandler.HandleAsync(context);
```

## Design properties

Each attribute can implement `IControllerDesignPropertyProvider`, `IEndpointDesignPropertyProvider`
or `IParameterDesignPropertyProvider` interfaces to provide some metadata that will be available in the `Properties`
dictionary-like object at design stage (when endpoint matcher and middleware factories are called)
.

There are also some auxiliary properties that can override the parameter name and nullability. Keys of these properties
are defined in the `KantaikoParameterProperties` static class. There is the built-in `[Parameter]` attribute that uses
these properties, but you can define your own implementation.

There is also the `KantaikoParameterProperties.IsHidden` property, which does not have any special effect on the request
pipeline, but is some kind of convention that the parameter should be hidden from user. For example,
the `[FromServices]`
attribute marks the parameters with this property.

## Introspection

Another powerful mechanism providing by this library is introspection. After creating the `RequestHandler` instance you
can access its `Info` property. It allows you to traverse all controllers, endpoints and parameters to build various
representations about them. For example, you can create the `/help` commands with list of all available commands and
their parameters.

You can find an example of using introspection
in [`examples/Introspection`](/libraries/Kantaiko.Extensions.Controllers/examples/Introspection) project.
