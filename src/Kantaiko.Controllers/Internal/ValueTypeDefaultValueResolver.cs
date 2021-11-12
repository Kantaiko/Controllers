using System;
using System.Diagnostics;
using Kantaiko.Controllers.Converters;
using Kantaiko.Controllers.Design.Parameters;

namespace Kantaiko.Controllers.Internal
{
    internal class ValueTypeDefaultValueResolver : IParameterDefaultValueResolver
    {
        private readonly Type _type;

        public ValueTypeDefaultValueResolver(Type type)
        {
            Debug.Assert(type.IsValueType);

            _type = type;
        }

        public object? ResolveDefaultValue(ParameterConversionContext context)
        {
            return Activator.CreateInstance(_type);
        }
    }
}
