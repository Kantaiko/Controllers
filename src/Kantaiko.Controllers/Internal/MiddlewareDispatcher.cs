using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Kantaiko.Controllers.Middleware;

namespace Kantaiko.Controllers.Internal
{
    internal class MiddlewareDispatcher<TContext>
    {
        private readonly IReadOnlyDictionary<EndpointMiddlewareStage, IEndpointMiddleware<TContext>[]> _middleware;

        public MiddlewareDispatcher(IMiddlewareCollection middlewareCollection, IInstanceFactory instanceFactory,
            IServiceProvider serviceProvider)
        {
            IEndpointMiddleware<TContext> CreateMiddlewareInstance(Type type)
            {
                var middlewareInstance = instanceFactory.CreateInstance(type, serviceProvider);

                Debug.Assert(middlewareInstance is IEndpointMiddleware<TContext>);

                return (IEndpointMiddleware<TContext>) middlewareInstance;
            }

            _middleware = middlewareCollection.MiddlewareTypes
                .Where(x => x.IsAssignableTo(typeof(IAutoRegistrableMiddleware<TContext>)))
                .Select(CreateMiddlewareInstance)
                .GroupBy(x => x.Stage)
                .ToDictionary(k => k.Key, v => v.ToArray());
        }

        public async Task HandleAsync(EndpointMiddlewareContext<TContext> context, CancellationToken cancellationToken)
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
