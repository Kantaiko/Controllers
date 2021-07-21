using System.Collections.Generic;

namespace Kantaiko.Controllers.Introspection
{
    public class EndpointParameterTree
    {
        internal EndpointParameterTree(IReadOnlyList<EndpointParameterInfo> children)
        {
            Children = children;
        }

        public IReadOnlyList<EndpointParameterInfo> Children { get; }
    }
}
