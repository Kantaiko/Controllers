using Kantaiko.Controllers.Introspection;

namespace Kantaiko.Controllers.ParameterValidation;

/// <summary>
/// The context passed to parameter validators.
/// </summary>
public readonly ref struct ParameterValidationContext
{
    /// <summary>
    /// Creates a new instance of <see cref="ParameterValidationContext"/>.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="parameter">The parameter that is being validated.</param>
    /// <param name="serviceProvider">The service provider.</param>
    public ParameterValidationContext(object? value, EndpointParameterInfo parameter, IServiceProvider serviceProvider)
    {
        Value = value;
        Parameter = parameter;
        ServiceProvider = serviceProvider;
    }

    /// <summary>
    /// The parameter that is being validated.
    /// </summary>
    public object? Value { get; }

    /// <summary>
    /// The parameter that is being validated.
    /// </summary>
    public EndpointParameterInfo Parameter { get; }

    /// <summary>
    /// The service provider.
    /// </summary>
    public IServiceProvider ServiceProvider { get; }
}

/// <summary>
/// The generic version of <see cref="ParameterValidationContext"/>.
/// </summary>
/// <typeparam name="TValue">The type of the value to validate.</typeparam>
public readonly ref struct ParameterValidationContext<TValue>
{
    /// <summary>
    /// Creates a new instance of <see cref="ParameterValidationContext"/>.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="parameter">The parameter that is being validated.</param>
    /// <param name="serviceProvider">The service provider.</param>
    public ParameterValidationContext(TValue value, EndpointParameterInfo parameter, IServiceProvider serviceProvider)
    {
        Value = value;
        Parameter = parameter;
        ServiceProvider = serviceProvider;
    }

    /// <summary>
    /// The value to validate.
    /// </summary>
    public TValue Value { get; }

    /// <summary>
    /// The parameter that is being validated.
    /// </summary>
    public EndpointParameterInfo Parameter { get; }

    /// <summary>
    /// The service provider.
    /// </summary>
    public IServiceProvider ServiceProvider { get; }
}
