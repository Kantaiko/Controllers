# Parameter conversion

- [Getting started](getting-started.md)
- [Endpoint matching](endpoint-matching.md)
- Parameter conversion
- [Parameter deconstruction](parameter-deconstruction.md)
- [Parameter post-validation](parameter-post-validation.md)
- [Middleware](middleware.md)
- [Another features](another-features.md)

Each endpoint can define various parameters with different types:

```c#
internal class PackageController : TestController
{
    private readonly PackageManager _packageManager;

    public PackageController(PackageManager packageManager)
    {
        _packageManager = packageManager;
    }

    [Pattern("install {packageName} {force}")]
    public Task InstallPackage(string packageName, bool force)
    {
        return _packageManager.InstallPackage(packageName, force);
    }
}
```

As a parameter type, you can use not only primitives, but also used-defined types. There is the powerful conversion
system for transforming an input string to these types.

The main unit of this system is a parameter converter. In the main, all parameter converters are classes inherited
from `ParameterConverter<TParameter>` or `AsyncParameterConverter<TParameter>` or some of their subclasses. Such classes
can be automatically collected by `FromAssemblies` method of `ConverterCollection`:

```c#
var converterCollection = ConverterCollection.FromAssemblies(Assembly.GetExecutingAssembly());
var requestHandler = new RequestHandler<Request>(controllerCollection, converterCollection: converterCollection);
```

Each sync parameter converter must implement the `CheckValueExistence` and `Convert` methods. They both receive
a `ParameterConversionContext` which contains the `Parameters` dictionary returned by the endpoint matcher. For example:

```c#
public class IntConverter : ParameterConverter<int>
{
    public override bool CheckValueExistence(ParameterConversionContext context)
    {
        return context.Parameters.ContainsKey(context.Info.Name);
    }

    public override ResolutionResult<int> Convert(ParameterConversionContext context)
    {
        return int.TryParse(context.Parameters[context.Info.Name], out var result)
            ? ResolutionResult.Success(result)
            : ResolutionResult.Error(Locale.IntegerRequired);
    }
}
```

Note that a parameter converter can use multiple values from the `Parameters` dictionary as well as an endpoint matcher
can store multiple values associated with one parameter. However, in the most cases each parameter will match the single
entry of `Parameters` dictionary. Then you can use `SingleParameterConverter<TParameter>` to automatically check
existence of the parameter value and extract it:

```c#
public class IntConverter : SingleParameterConverter<int>
{
    public override ResolutionResult<int> Convert(ParameterConversionContext context, string value)
    {
        return int.TryParse(value, out var result)
            ? ResolutionResult.Success(result)
            : ResolutionResult.Error(Locale.IntegerRequired);
    }
}
```

Parameter converters also can be asynchronous. Such converters work in two stages: validation and resolution. Each async
converter should implement the `Resolve` and `CheckValueExistence` methods. The `Validate` method can be ommited and by
default will always pass the validation. There is the example of the converter resolving an instance of `User` entity by
its identifier:

```c#
internal class UserConverter : AsyncParameterConverter<User>
{
    private int _userId;

    private readonly IUserRepository _userRepository;

    public UserConverter(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public override bool CheckValueExistence(ParameterConversionContext context)
    {
        return context.Parameters.ContainsKey(context.Info.Name);
    }

    public override ValidationResult Validate(ParameterConversionContext context)
    {
        if (!int.TryParse(context.Parameters[context.Info.Name], out var result))
            return ValidationResult.Error("Identifier required");

        _userId = result;
        return ValidationResult.Success;
    }

    public override async Task<ResolutionResult<User>> Resolve(ParameterConversionContext context,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindById(_userId, cancellationToken);
        return user is not null ? ResolutionResult.Success(user) : ResolutionResult.Error("User not found");
    }
}
```

Note that unlike endpoint matchers, parameter converters are not singletons. They can safely share state between
resolution stages.

Like sync converters async ones can also inherit `AsyncSingleParameterConverter<TParameter>`. You can also create helper
classes for implementing converters easier. For example, you can create `EntityParameterConverter` class and implement
validation logic here:

```c#
internal abstract class EntityConverter<TParameter> : AsyncSingleParameterConverter<User>
{
    protected int EntityId { get; private set; }

    public override ValidationResult Validate(ParameterConversionContext context, string value)
    {
        if (!int.TryParse(value, out var result))
            return ValidationResult.Error("Identifier required");

        EntityId = result;
        return ValidationResult.Success;
    }
}

internal class UserConverter : EntityConverter<User>
{
    private readonly IUserRepository _userRepository;

    public UserConverter(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public override async Task<ResolutionResult<User>> Resolve(ParameterConversionContext context, string value,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindById(EntityId, cancellationToken);
        return user is not null ? ResolutionResult.Success(user) : ResolutionResult.Error("User not found");
    }
}
```

There is the another option to define and use parameter converters. You can bind the specific converter to the specific
parameter using attributes. Such converters must implement the non-generic `IParameterConverter` interface. You can use
the `IParameterConverterTypeProvider` interface to provide a custom converter type.

For example, built-in `FromServices` attribute is just a converter type provider:

```c#
internal class ServiceConverter : IParameterConverter
{
    private object? _service;

    public bool CheckValueExistence(ParameterConversionContext context)
    {
        _service = context.ServiceProvider.GetService(context.Info.ParameterType);
        return _service is not null;
    }

    public ValidationResult Validate(ParameterConversionContext context) => ValidationResult.Success;

    public Task<IResolutionResult> Resolve(ParameterConversionContext context, CancellationToken cancellationToken)
    {
        return Task.FromResult<IResolutionResult>(ResolutionResult.Success(_service));
    }
}

[AttributeUsage(AttributeTargets.Parameter)]
public class FromServicesAttribute : Attribute, IParameterConverterTypeProvider
{
    public Type GetConverterType(EndpointParameterDesignContext context) => typeof(ServiceConverter);
}
```

Alternatively, attribute can implement the `IParameterConverterFactoryProvider` interface to provide custom converter
factory. It might be useful when you need to pass to the converter additional parameters defined in the attribute
constructor.

Read next: [Parameter deconstruction](parameter-deconstruction.md)
