using System.Collections.Generic;

namespace Kantaiko.Controllers.Internal
{
    internal class ParameterManagerTree<TContext>
    {
        public ParameterManagerTree(IReadOnlyList<ParameterManager<TContext>> children)
        {
            Children = children;
        }

        public IReadOnlyList<ParameterManager<TContext>> Children { get; }
    }
}
