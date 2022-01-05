using Kantaiko.Routing.Context;

namespace Kantaiko.Controllers;

public interface IAutoRegistrableController<TContext> where TContext : IContext { }
