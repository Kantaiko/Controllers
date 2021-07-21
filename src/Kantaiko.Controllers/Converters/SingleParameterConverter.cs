namespace Kantaiko.Controllers.Converters
{
    public abstract class SingleParameterConverter<TParameter> : ParameterConverter<TParameter>
    {
        public override ResolutionResult<TParameter> Convert(ParameterConversionContext context)
        {
            return Convert(context, context.Parameters[context.Info.Name]);
        }

        public override bool CheckValueExistence(ParameterConversionContext context)
        {
            return context.Parameters.ContainsKey(context.Info.Name);
        }

        public abstract ResolutionResult<TParameter> Convert(ParameterConversionContext context, string value);
    }
}
