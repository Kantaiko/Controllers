using System.Collections.Generic;
using Kantaiko.Controllers.ParameterConversion.Handlers;

namespace Kantaiko.Controllers.ParameterConversion;

public interface IParameterHandlerCollection<TContext> : IList<IParameterConversionHandler<TContext>> { }
