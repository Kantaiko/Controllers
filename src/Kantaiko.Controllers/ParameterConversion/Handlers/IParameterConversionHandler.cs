using System.Threading.Tasks;
using Kantaiko.Routing.Handlers;

namespace Kantaiko.Controllers.ParameterConversion.Handlers;

public interface IParameterConversionHandler<TContext> : IHandler<ParameterConversionContext<TContext>, Task> { }
