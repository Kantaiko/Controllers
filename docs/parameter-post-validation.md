# Parameter post-validation

- [Getting started](getting-started.md)
- [Endpoint matching](endpoint-matching.md)
- [Parameter conversion](parameter-conversion.md)
- [Parameter deconstruction](parameter-deconstruction.md)
- Parameter post-validation
- [Middleware](middleware.md)
- [Another features](another-features.md)

Before resolved parameters will be passed to the endpoint method they go through the post-validation process. You can
apply multiple post-validation rules for each parameter using attributes. For example, there are built-in `MinValue`
and `MaxValue` attributes for number parameters.

Here the example how you can implement such attributes:

```c#
internal class MinValueValidator : IParameterPostValidator
{
    private readonly object _minValue;

    public MinValueValidator(object minValue)
    {
        _minValue = minValue;
    }

    public ValidationResult Validate(ParameterPostValidationContext context, object value)
    {
        return ((IComparable) value).CompareTo(_minValue) < 0
            ? ValidationResult.Error(string.Format(Locale.ShouldBeNoLessThan, _minValue))
            : ValidationResult.Success;
    }
}

[AttributeUsage(AttributeTargets.Parameter)]
public class MinValueAttribute : Attribute, IParameterPostValidatorFactory
{
    private readonly object _minValue;

    public MinValueAttribute(long minValue) => _minValue = minValue;
    public MinValueAttribute(int minValue) => _minValue = minValue;
    public MinValueAttribute(float minValue) => _minValue = minValue;
    public MinValueAttribute(double minValue) => _minValue = minValue;

    public IParameterPostValidator CreateParameterPostValidator(EndpointParameterDesignContext context)
    {
        ParameterHelper.CheckType(context.Info, typeof(long), typeof(int), typeof(float), typeof(double));
        return new MinValueValidator(_minValue);
    }
}
```

Note that a validator can be applied for multiple parameter types. You should manually restrict allowed types and throw
an exception if your attribute is used on wrong type. You can use `ParameterHelper` to do that.

Read next: [Middleware](middleware.md)
