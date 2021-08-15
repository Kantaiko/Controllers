using System;
using System.Collections.Generic;
using Kantaiko.Controllers.Introspection;
using Kantaiko.Controllers.Matchers;

namespace Kantaiko.Controllers.Internal
{
    internal class ControllerManager<TContext>
    {
        public ControllerManager(ControllerInfo info, IReadOnlyList<EndpointManager<TContext>> endpoints)
        {
            Info = info;
            Endpoints = endpoints;
        }

        public ControllerInfo Info { get; }
        public IReadOnlyList<EndpointManager<TContext>> Endpoints { get; }

        public ControllerMatchResult<TContext> MatchEndpoint(TContext requestContext, IServiceProvider provider)
        {
            foreach (var controllerEndpoint in Endpoints)
            {
                var context = new EndpointMatchContext<TContext>(requestContext, controllerEndpoint.Info, provider);
                var result = controllerEndpoint.Match(context);

                if (result.IsMatched)
                    return ControllerMatchResult<TContext>.Matched(this, controllerEndpoint, result);
            }

            return ControllerMatchResult<TContext>.NotMatched;
        }
    }
}
