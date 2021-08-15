namespace Kantaiko.Controllers.Middleware
{
    internal interface IAutoRegistrableMiddleware { }

    internal interface IAutoRegistrableMiddleware<TContext> : IAutoRegistrableMiddleware { }
}
