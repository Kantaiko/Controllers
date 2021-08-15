using System;
using System.Collections.Generic;
using System.Reflection;
using Kantaiko.Controllers.Design.Properties;

namespace Kantaiko.Controllers.Introspection
{
    public class EndpointParameterInfo
    {
        internal EndpointParameterInfo(EndpointInfo endpoint, IDesignPropertyCollection properties,
            string originalName, string name, Type type, bool isOptional, ICustomAttributeProvider attributeProvider)
        {
            Endpoint = endpoint;
            AttributeProvider = attributeProvider;
            Properties = properties;
            OriginalName = originalName;
            Name = name;
            ParameterType = type;
            IsOptional = isOptional;
        }

        public EndpointInfo Endpoint { get; }
        public ICustomAttributeProvider AttributeProvider { get; }

        public string OriginalName { get; }
        public string Name { get; }

        public bool IsOptional { get; }
        public Type ParameterType { get; }

        public IDesignPropertyCollection Properties { get; }
    }
}
