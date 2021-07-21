# Parameter deconstruction

- [Getting started](getting-started.md)
- [Endpoint matching](endpoint-matching.md)
- [Parameter conversion](parameter-conversion.md)
- Parameter deconstruction
- [Parameter post-validation](parameter-post-validation.md)
- [Middleware](middleware.md)
- [Another features](another-features.md)

There is the pattern deconstruction mechanism to allow convenient parameter decomposition and reusing. You can define
classes with nested parameters and use them in endpoints:

```c#
internal class InstallPackageInput
{
    public string PackageName { get; set; } = null!;
    public bool Force { get; set; }
}

internal class PackageController : TestController
{
    private readonly PackageManager _packageManager;

    public PackageController(PackageManager packageManager)
    {
        _packageManager = packageManager;
    }

    [Pattern("install {packageName} {force}")]
    public Task InstallPackage(InstallPackageInput input)
    {
        return _packageManager.InstallPackage(input.PackageName, input.Force);
    }
}
```

You are probably interested in how the converted parameters differ from the deconstructed ones.
The `ParameterDeconstructionValidator` service decides which parameters should be deconstructed. The default
implementation relies on the existence of the appropriate converter. If a converter for the class parameter does not
exist, it will be deconstructed. You can implement your own strategy and pass the implementation of this service to
the `RequestHandler` constructor.

You can also define nested parameters in nested parameters at any depth level. The power of composition is in your
hands!

Read next: [Parameter post-validation](parameter-post-validation.md)
