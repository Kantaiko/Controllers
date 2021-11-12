using Kantaiko.Controllers.Converters;
using Kantaiko.Controllers.Design.Parameters;

namespace Kantaiko.Controllers.Middleware
{
    public class ExecutionParameterContext
    {
        public ExecutionParameterContext(ParameterConversionContext conversionContext,
            IParameterConverter resolvedConverter,
            IParameterDefaultValueResolver defaultValueResolver)
        {
            ConversionContext = conversionContext;
            ResolvedConverter = resolvedConverter;
            DefaultValueResolver = defaultValueResolver;
        }

        public ParameterConversionContext ConversionContext { get; }

        public IParameterConverter ResolvedConverter { get; }

        public IParameterDefaultValueResolver DefaultValueResolver { get; }

        public object? Value { get; set; }
        public bool ValueExists { get; set; }
    }
}
