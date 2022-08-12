using System.Collections.Generic;
using Kantaiko.Controllers.Execution.Handlers;

namespace Kantaiko.Controllers.Execution;

public class HandlerCollection<TContext> : List<IControllerExecutionHandler<TContext>>, IHandlerCollection<TContext> { }
