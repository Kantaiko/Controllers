using System;
using Kantaiko.Controllers.Resources;

namespace Kantaiko.Controllers.Validation
{
    internal class MaxValueValidator : IParameterPostValidator
    {
        private readonly object _maxValue;

        public MaxValueValidator(object maxValue)
        {
            _maxValue = maxValue;
        }

        public ValidationResult Validate(ParameterPostValidationContext context, object value)
        {
            return ((IComparable) value).CompareTo(_maxValue) > 0
                ? ValidationResult.Error(string.Format(Locale.ShouldBeNoMoreThan, _maxValue))
                : ValidationResult.Success;
        }
    }
}
