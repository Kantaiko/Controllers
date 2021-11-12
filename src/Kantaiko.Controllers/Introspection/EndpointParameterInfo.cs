using System;
using System.Reflection;
using Kantaiko.Controllers.Design.Properties;

namespace Kantaiko.Controllers.Introspection
{
    public class EndpointParameterInfo
    {
        internal EndpointParameterInfo(EndpointInfo endpoint, IDesignPropertyCollection properties,
            string name, Type type, bool isOptional, ICustomAttributeProvider attributeProvider)
        {
            Endpoint = endpoint;
            AttributeProvider = attributeProvider;
            Properties = properties;
            Name = name;
            ParameterType = type;
            IsOptional = isOptional;
        }

        public EndpointInfo Endpoint { get; }
        public ICustomAttributeProvider AttributeProvider { get; }

        public string Name { get; }

        public bool IsOptional { get; }
        public Type ParameterType { get; }

        public IDesignPropertyCollection Properties { get; }
    }
}
