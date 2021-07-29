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
    public class RequestHandler<TRequest> where TRequest : notnull
    {
        private readonly ControllerHandler<TRequest> _controllerHandler;

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

            var controllerManagerFactory = new ControllerManagerFactory<TRequest>(deconstructionValidator);

            var controllerManagerCollection = controllerManagerFactory.CreateControllerManagerCollection(
                controllerCollection, serviceProvider);

            _controllerHandler = new ControllerHandler<TRequest>(controllerManagerCollection,
                converterCollection, instanceFactory, middlewareCollection, serviceProvider);

            var controllerInfos = controllerManagerCollection.ControllerManagers.Select(x => x.Info).ToArray();
            Info = new RequestHandlerInfo(controllerInfos);
        }

        public Task<RequestProcessingResult> HandleAsync(TRequest request,
            IServiceProvider? serviceProvider = null,
            CancellationToken cancellationToken = default)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            return _controllerHandler.HandleAsync(request, serviceProvider, cancellationToken);
        }
    }
}
