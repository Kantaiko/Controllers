using System.Collections.Generic;
using Kantaiko.Controllers.ParameterConversion.Handlers;

namespace Kantaiko.Controllers.ParameterConversion;

public class ParameterHandlerCollection<TContext> : List<IParameterConversionHandler<TContext>>,
    IParameterHandlerCollection<TContext> { }
