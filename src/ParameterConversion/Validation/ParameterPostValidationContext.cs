using System;
using Kantaiko.Controllers.Introspection;

namespace Kantaiko.Controllers.ParameterConversion.Validation;

public readonly ref struct ParameterPostValidationContext
{
    public ParameterPostValidationContext(object value, EndpointParameterInfo parameter,
        IServiceProvider serviceProvider)
    {
        Value = value;
        Parameter = parameter;
        ServiceProvider = serviceProvider;
    }

    public object Value { get; }
    public EndpointParameterInfo Parameter { get; }
    public IServiceProvider ServiceProvider { get; }
}
