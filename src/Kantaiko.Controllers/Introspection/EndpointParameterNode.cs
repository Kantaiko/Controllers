using System;
using System.Collections.Generic;
using System.Reflection;

namespace Kantaiko.Controllers.Introspection
{
    public class EndpointParameterNode : EndpointParameterInfo
    {
        internal EndpointParameterNode(EndpointInfo endpoint, IReadOnlyDictionary<string, object> properties,
            string originalName, string name, Type type,
            bool isOptional, ICustomAttributeProvider attributeProvider, IReadOnlyList<EndpointParameterInfo> children)
            : base(endpoint, properties, originalName, name, type, isOptional, attributeProvider)
        {
            Children = children;
        }

        public IReadOnlyList<EndpointParameterInfo> Children { get; }
    }
}
