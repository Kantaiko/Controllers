using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Kantaiko.Controllers.Converters;
using Kantaiko.Controllers.Design;
using Kantaiko.Controllers.Internal;
using Kantaiko.Controllers.Introspection;
using Kantaiko.Controllers.Middleware;
using Kantaiko.Controllers.Result;

namespace Kantaiko.Controllers
{
    public class RequestHandler<TContext> where TContext : notnull
    {
        private readonly ControllerHandler<TContext> _controllerHandler;

        public RequestHandlerInfo Info { get; }

        public RequestHandler(IControllerCollection controllerCollection,
            IConverterCollection? converterCollection = null,
            IMiddlewareCollection? middlewareCollection = null,
            IDeconstructionValidator? deconstructionValidator = null,
            IInstanceFactory? instanceFactory = null,
            IServiceProvider? serviceProvider = null)
        {
            if (controllerCollection is null) throw new ArgumentNullException(nameof(controllerCollection));

            converterCollection ??= ConverterCollection.Default;
            middlewareCollection ??= MiddlewareCollection.Empty;
            deconstructionValidator ??= new DefaultDeconstructionValidator(converterCollection);

            instanceFactory ??= DefaultInstanceFactory.Instance;
            serviceProvider ??= DefaultServiceProvider.Instance;

            var controllerManagerFactory = new ControllerManagerFactory<TContext>(deconstructionValidator);

            var controllerManagerCollection = controllerManagerFactory.CreateControllerManagerCollection(
                controllerCollection, serviceProvider);

            _controllerHandler = new ControllerHandler<TContext>(controllerManagerCollection,
                converterCollection, instanceFactory, middlewareCollection, serviceProvider);

            var controllerInfos = controllerManagerCollection.ControllerManagers.Select(x => x.Info).ToArray();
            Info = new RequestHandlerInfo(controllerInfos);
        }

        public Task<RequestProcessingResult> HandleAsync(TContext context,
            IServiceProvider? serviceProvider = null,
            CancellationToken cancellationToken = default)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));

            return _controllerHandler.HandleAsync(context, serviceProvider, cancellationToken);
        }
    }
}
