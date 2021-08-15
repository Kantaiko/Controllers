using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Kantaiko.Controllers.Introspection;
using Kantaiko.Controllers.Matchers;
using Kantaiko.Controllers.Middleware;
using Kantaiko.Controllers.Utils;

namespace Kantaiko.Controllers.Internal
{
    internal class EndpointManager<TContext>
    {
        private readonly bool _isAsync;

        public EndpointManager(EndpointInfo info,
            IReadOnlyList<IEndpointMatcher<TContext>> matchers,
            ParameterManagerTree<TContext> parameterTree,
            IReadOnlyList<IEndpointMiddleware<TContext>> middlewares)
        {
            Info = info;
            Matchers = matchers;
            Middlewares = middlewares;

            ParameterTree = parameterTree;
            Parameters = Flatten(ParameterTree.Children).ToArray();

            if (info.MethodInfo.ReturnType == typeof(Task))
                _isAsync = true;

            if (info.MethodInfo.ReturnType.IsGenericType &&
                info.MethodInfo.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
                _isAsync = true;
        }

        public EndpointInfo Info { get; }
        public IReadOnlyList<IEndpointMatcher<TContext>> Matchers { get; }
        public IReadOnlyList<IEndpointMiddleware<TContext>> Middlewares { get; }

        public IReadOnlyList<ParameterManager<TContext>> Parameters { get; }
        public ParameterManagerTree<TContext> ParameterTree { get; }

        private static IEnumerable<ParameterManager<TContext>> Flatten(
            IEnumerable<ParameterManager<TContext>> parameters)
        {
            return parameters.SelectMany(parameterInfo =>
            {
                if (parameterInfo.Children is not null)
                {
                    return Flatten(parameterInfo.Children);
                }

                return EnumerableUtils.Single(parameterInfo);
            });
        }

        public async Task<InvocationResult> Invoke(object controller, object[] parameters)
        {
            try
            {
                var result = Info.MethodInfo.Invoke(controller, parameters);
                if (result is null) return new InvocationResult();

                // TODO wtf
                if (_isAsync)
                {
                    var resultTask = (Task)result;
                    await resultTask.ConfigureAwait(false);
                    result = resultTask.GetType().GetProperty("Result")!.GetValue(resultTask);
                }

                return new InvocationResult(result);
            }
            catch (TargetInvocationException e)
            {
                Debug.Assert(e.InnerException is not null);
                return new InvocationResult(exception: e.InnerException);
            }
        }

        public EndpointMatchResult Match(EndpointMatchContext<TContext> context)
        {
            foreach (var endpointMatcher in Matchers)
            {
                var result = endpointMatcher.Match(context);
                if (result.IsMatched) return result;
            }

            return EndpointMatchResult.NotMatched;
        }

        public async Task InvokeMiddleware(EndpointMiddlewareContext<TContext> endpointMiddlewareContext,
            CancellationToken cancellationToken)
        {
            foreach (var middleware in Middlewares)
            {
                await middleware.HandleAsync(endpointMiddlewareContext, cancellationToken);
                if (!endpointMiddlewareContext.ShouldProcess) return;
            }

            foreach (var parameterManager in Parameters)
            {
                await parameterManager.InvokeMiddleware(endpointMiddlewareContext, cancellationToken);
                if (!endpointMiddlewareContext.ShouldProcess) return;
            }
        }
    }
}
