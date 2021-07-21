using System;
using Kantaiko.Controllers.Converters;
using Kantaiko.Controllers.Design;

namespace Kantaiko.Controllers.Internal
{
    internal class DefaultDeconstructionValidator : IDeconstructionValidator
    {
        private readonly IConverterCollection _converterCollection;

        public DefaultDeconstructionValidator(IConverterCollection converterCollection)
        {
            _converterCollection = converterCollection;
        }

        public bool CanDeconstruct(Type parameterType)
        {
            return !_converterCollection.ConverterTypes.ContainsKey(parameterType);
        }
    }
}
