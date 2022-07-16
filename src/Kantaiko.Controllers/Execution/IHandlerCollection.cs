using System.Collections.Generic;
using Kantaiko.Controllers.Execution.Handlers;

namespace Kantaiko.Controllers.Execution;

public interface IHandlerCollection<TContext> : IList<IControllerExecutionHandler<TContext>> { }
