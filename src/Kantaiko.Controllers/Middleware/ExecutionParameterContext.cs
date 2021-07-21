using Kantaiko.Controllers.Converters;

namespace Kantaiko.Controllers.Middleware
{
    public class ExecutionParameterContext
    {
        public ExecutionParameterContext(ParameterConversionContext context, IParameterConverter converter)
        {
            Context = context;
            Converter = converter;
        }

        public ParameterConversionContext Context { get; }
        public IParameterConverter Converter { get; }

        public object? Value { get; set; }
        public bool ValueExists { get; set; }
    }
}
