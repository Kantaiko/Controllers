using System;
using Kantaiko.Controllers.Design.Parameters;
using Kantaiko.Controllers.Design.Properties;

namespace Kantaiko.Controllers
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
    public class ParameterAttribute : Attribute, IParameterDesignPropertyProvider
    {
        public string? Name { get; }
        public bool IsOptional { get; init; }

        public ParameterAttribute(string? name = null)
        {
            Name = name;
        }

        public DesignPropertyCollection GetParameterDesignProperties() => new()
        {
            [KantaikoParameterProperties.Name] = Name,
            [KantaikoParameterProperties.IsOptional] = IsOptional
        };
    }
}
