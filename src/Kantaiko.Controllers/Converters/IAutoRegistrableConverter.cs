namespace Kantaiko.Controllers.Converters
{
    internal interface IAutoRegistrableConverter { }

    internal interface IAutoRegistrableConverter<TParameter> : IAutoRegistrableConverter { }
}
