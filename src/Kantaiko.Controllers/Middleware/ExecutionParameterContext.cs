using Kantaiko.Controllers.Converters;
using Kantaiko.Controllers.Design.Parameters;

namespace Kantaiko.Controllers.Middleware
{
    public class ExecutionParameterContext
    {
        public ExecutionParameterContext(ParameterConversionContext context, IParameterConverter converter,
            IParameterDefaultValueResolver? defaultValueResolver)
        {
            Context = context;
            Converter = converter;
            DefaultValueResolver = defaultValueResolver;
        }

        public ParameterConversionContext Context { get; }
        public IParameterConverter Converter { get; }
        public IParameterDefaultValueResolver? DefaultValueResolver { get; }

        public object? Value { get; set; }
        public bool ValueExists { get; set; }
    }
}
