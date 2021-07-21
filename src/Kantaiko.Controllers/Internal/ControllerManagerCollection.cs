using System;
using System.Collections.Generic;

namespace Kantaiko.Controllers.Internal
{
    internal class ControllerManagerCollection<TRequest>
    {
        public IReadOnlyList<ControllerManager<TRequest>> ControllerManagers { get; }

        public ControllerManagerCollection(IReadOnlyList<ControllerManager<TRequest>> controllerManagers)
        {
            ControllerManagers = controllerManagers;
        }

        public ControllerMatchResult<TRequest> MatchController(TRequest request, IServiceProvider serviceProvider)
        {
            foreach (var controllerManager in ControllerManagers)
            {
                var result = controllerManager.MatchEndpoint(request, serviceProvider);
                if (result.IsMatched) return result;
            }

            return ControllerMatchResult<TRequest>.NotMatched;
        }
    }
}
