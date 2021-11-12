using Kantaiko.Controllers.Converters;
using Kantaiko.Controllers.Design.Parameters;

namespace Kantaiko.Controllers.Internal
{
    public class ConstantDefaultValueResolver : IParameterDefaultValueResolver
    {
        private readonly object? _value;

        public ConstantDefaultValueResolver(object? value)
        {
            _value = value;
        }

        public object? ResolveDefaultValue(ParameterConversionContext context)
        {
            return _value;
        }

        public static ConstantDefaultValueResolver NullResolver { get; } = new(null);
    }
}
