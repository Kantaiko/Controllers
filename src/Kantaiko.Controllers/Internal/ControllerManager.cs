using System;
using System.Collections.Generic;
using Kantaiko.Controllers.Introspection;
using Kantaiko.Controllers.Matchers;

namespace Kantaiko.Controllers.Internal
{
    internal class ControllerManager<TRequest>
    {
        public ControllerManager(ControllerInfo info, IReadOnlyList<EndpointManager<TRequest>> endpoints)
        {
            Info = info;
            Endpoints = endpoints;
        }

        public ControllerInfo Info { get; }
        public IReadOnlyList<EndpointManager<TRequest>> Endpoints { get; }

        public ControllerMatchResult<TRequest> MatchEndpoint(TRequest request, IServiceProvider provider)
        {
            foreach (var controllerEndpoint in Endpoints)
            {
                var context = new EndpointMatchContext<TRequest>(request, controllerEndpoint.Info, provider);
                var result = controllerEndpoint.Match(context);

                if (result.IsMatched)
                    return ControllerMatchResult<TRequest>.Matched(this, controllerEndpoint, result);
            }

            return ControllerMatchResult<TRequest>.NotMatched;
        }
    }
}
