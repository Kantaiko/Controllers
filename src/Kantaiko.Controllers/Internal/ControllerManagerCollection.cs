using System;
using System.Collections.Generic;

namespace Kantaiko.Controllers.Internal
{
    internal class ControllerManagerCollection<TContext>
    {
        public IReadOnlyList<ControllerManager<TContext>> ControllerManagers { get; }

        public ControllerManagerCollection(IReadOnlyList<ControllerManager<TContext>> controllerManagers)
        {
            ControllerManagers = controllerManagers;
        }

        public ControllerMatchResult<TContext> MatchController(TContext context, IServiceProvider serviceProvider)
        {
            foreach (var controllerManager in ControllerManagers)
            {
                var result = controllerManager.MatchEndpoint(context, serviceProvider);
                if (result.IsMatched) return result;
            }

            return ControllerMatchResult<TContext>.NotMatched;
        }
    }
}
