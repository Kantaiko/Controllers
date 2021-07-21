using System;
using Kantaiko.Controllers.Resources;

namespace Kantaiko.Controllers.Validation
{
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
}
