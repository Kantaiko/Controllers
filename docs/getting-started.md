# Getting started

- Getting started
- [Endpoint matching](endpoint-matching.md)
- [Parameter conversion](parameter-conversion.md)
- [Parameter deconstruction](parameter-deconstruction.md)
- [Parameter post-validation](parameter-post-validation.md)
- [Middleware](middleware.md)
- [Another features](another-features.md)

The entry service of the request processing pipeline is called `RequestHandler`.

To create request handler you should at least specify request type and provide to its constructor an instance
of `ControllerCollection`. The request type can be any reference type, even string, but we highly recommend to use
dedicated class. The controller collection contains controller types. You can construct it using `FromAssemblies`
factory method. In this case all suitable types from these assemblies will be used:

```c#
var controllerCollection = ControllerCollection.FromAssemblies(Assembly.GetExecutingAssembly());
var requestHandler = new RequestHandler<Request>(controllerCollection);
```

You should also define the base type for your controllers, inherited from `ControllerBase<TContext>`:

```c#
public class TestController : ControllerBase<TestContext> { }
```

Now you can define controllers. All controllers can access protected `Request` property to get a processing request:

```c#
internal class HelloController : TestController
{
    [Pattern("echo")]
    public string Echo() => Request.Text;
}
```

After you have instantiated request handler and defined some controllers, you can handle requests using
the `HandleAsync`
method:

```c#
var context = new Request("Hello, wolld!");
var result = await requestHandler.HandleAsync(request);
```

This library designed to support task cancellation, so you can provide a cancellation token as the last parameter. You
can also provide an instance of service provider that will be used to process a particular request.
(see [Dependency injection](another-features.md#dependency-injection))

This method returns a task with `RequestProcessingResult` that contains some information about how the request was
processed.

```c#
if (result.IsMatched)
{
    if (result.IsExited)
    {
        switch (result.ExitReason)
        {
            case ErrorExitReason errorReason:
                Console.WriteLine($"An error occurred on stage {errorReason.Stage}: {errorReason.ErrorMessage}");
                return;
            case InterruptionExitReason interruptionReason:
                Console.WriteLine($"Execution was interrupted at stage {interruptionReason.MiddlewareStage}");
                return;
            case ExceptionExitReason exceptionReason:
                Console.WriteLine($"An exception was thrown: {exceptionReason.Exception}");
                return;
        }
    }

    if (result.HasReturnValue)
    {
        Console.WriteLine($"The return value was: {result.ReturnValue}");
    }
}
```

There are also some optional services that allows you to customize request processing pipeline. They will be covered in
next sections.

Read next: [Endpoint matching](endpoint-matching.md)
