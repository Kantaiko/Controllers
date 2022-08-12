using System;
using Kantaiko.Controllers.Introspection.Factory.Attributes;
using Kantaiko.Controllers.Introspection.Factory.Context;

namespace Kantaiko.Controllers.ParameterConversion.Validation;

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
public class MinValueAttribute : Attribute, IParameterPostValidatorFactory
{
    private readonly object _minValue;

    public MinValueAttribute(long minValue) => _minValue = minValue;
    public MinValueAttribute(int minValue) => _minValue = minValue;
    public MinValueAttribute(float minValue) => _minValue = minValue;
    public MinValueAttribute(double minValue) => _minValue = minValue;

    public IParameterPostValidator CreateParameterPostValidator(ParameterFactoryContext context)
    {
        ParameterHelper.CheckType(context.Parameter, typeof(long), typeof(int), typeof(float), typeof(double));
        return new MinValueValidator(_minValue);
    }
}
