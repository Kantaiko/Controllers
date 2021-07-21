using System;
using Kantaiko.Controllers.Design.Parameters;

namespace Kantaiko.Controllers.Validation
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
    public class MaxValueAttribute : Attribute, IParameterPostValidatorFactory
    {
        private readonly object _maxValue;

        public MaxValueAttribute(long maxValue) => _maxValue = maxValue;
        public MaxValueAttribute(int maxValue) => _maxValue = maxValue;
        public MaxValueAttribute(float maxValue) => _maxValue = maxValue;
        public MaxValueAttribute(double maxValue) => _maxValue = maxValue;

        public IParameterPostValidator CreateParameterPostValidator(EndpointParameterDesignContext context)
        {
            ParameterHelper.CheckType(context.Info, typeof(long), typeof(int), typeof(float), typeof(double));
            return new MaxValueValidator(_maxValue);
        }
    }
}
