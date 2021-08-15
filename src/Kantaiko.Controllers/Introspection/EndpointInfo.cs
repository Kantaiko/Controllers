using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kantaiko.Controllers.Design.Endpoints;
using Kantaiko.Controllers.Design.Properties;
using Kantaiko.Controllers.Internal;
using Kantaiko.Controllers.Utils;

namespace Kantaiko.Controllers.Introspection
{
    public class EndpointInfo
    {
        internal EndpointInfo(ControllerInfo controller, MethodInfo methodInfo)
        {
            Controller = controller;
            MethodInfo = methodInfo;
            Properties = DesignPropertyExtractor.GetProperties<IEndpointDesignPropertyProvider>(methodInfo,
                x => x.GetEndpointDesignProperties());
        }

        public ControllerInfo Controller { get; }
        public MethodInfo MethodInfo { get; }

        public IReadOnlyList<EndpointParameterInfo> Parameters { get; private set; } = null!;
        public EndpointParameterTree ParameterTree { get; private set; } = null!;

        public IDesignPropertyCollection Properties { get; }

        private static IEnumerable<EndpointParameterInfo> Flatten(IEnumerable<EndpointParameterInfo> parameters)
        {
            return parameters.SelectMany(parameterInfo =>
            {
                if (parameterInfo is EndpointParameterNode node)
                {
                    return Flatten(node.Children);
                }

                return EnumerableUtils.Single(parameterInfo);
            });
        }

        internal void SetParameterTree(EndpointParameterTree parameterTree)
        {
            ParameterTree = parameterTree;
            Parameters = Flatten(parameterTree.Children).ToArray();
        }
    }
}
