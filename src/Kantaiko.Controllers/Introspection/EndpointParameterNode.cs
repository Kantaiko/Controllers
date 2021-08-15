using System;
using System.Collections.Generic;
using System.Reflection;
using Kantaiko.Controllers.Design.Properties;

namespace Kantaiko.Controllers.Introspection
{
    public class EndpointParameterNode : EndpointParameterInfo
    {
        internal EndpointParameterNode(EndpointInfo endpoint, IDesignPropertyCollection properties,
            string originalName, string name, Type type,
            bool isOptional, ICustomAttributeProvider attributeProvider, IReadOnlyList<EndpointParameterInfo> children)
            : base(endpoint, properties, originalName, name, type, isOptional, attributeProvider)
        {
            Children = children;
        }

        public IReadOnlyList<EndpointParameterInfo> Children { get; }
    }
}
