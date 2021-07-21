using System.Collections.Generic;

namespace Kantaiko.Controllers.Internal
{
    internal class ParameterManagerTree<TRequest>
    {
        public ParameterManagerTree(IReadOnlyList<ParameterManager<TRequest>> children)
        {
            Children = children;
        }

        public IReadOnlyList<ParameterManager<TRequest>> Children { get; }
    }
}
