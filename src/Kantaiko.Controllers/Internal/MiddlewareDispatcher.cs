using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Kantaiko.Controllers.Middleware;

namespace Kantaiko.Controllers.Internal
{
    internal class MiddlewareDispatcher<TRequest>
    {
        private readonly IReadOnlyDictionary<EndpointMiddlewareStage, IEndpointMiddleware<TRequest>[]> _middleware;

        public MiddlewareDispatcher(IMiddlewareCollection middlewareCollection, IInstanceFactory instanceFactory,
            IServiceProvider serviceProvider)
        {
            IEndpointMiddleware<TRequest> CreateMiddlewareInstance(Type type)
            {
                var middlewareInstance = instanceFactory.CreateInstance(type, serviceProvider);

                Debug.Assert(middlewareInstance is IEndpointMiddleware<TRequest>);

                return (IEndpointMiddleware<TRequest>) middlewareInstance;
            }

            _middleware = middlewareCollection.MiddlewareTypes
                .Where(x => x.IsAssignableTo(typeof(IAutoRegistrableMiddleware<TRequest>)))
                .Select(CreateMiddlewareInstance)
                .GroupBy(x => x.Stage)
                .ToDictionary(k => k.Key, v => v.ToArray());
        }

        public async Task HandleAsync(EndpointMiddlewareContext<TRequest> context, CancellationToken cancellationToken)
        {
            if (!_middleware.TryGetValue(context.Stage, out var middlewareInstances)) return;

            foreach (var middleware in middlewareInstances)
            {
                await middleware.HandleAsync(context, cancellationToken);
                if (!context.ShouldProcess) return;
            }
        }
    }
}
