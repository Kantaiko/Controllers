using System;
using System.Collections.Generic;
using Kantaiko.Controllers.Design.Parameters;

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

        public IReadOnlyDictionary<string, object> GetParameterDesignProperties() => new Dictionary<string, object>
        {
            [KantaikoParameterProperties.Name] = Name!,
            [KantaikoParameterProperties.IsOptional] = IsOptional
        };
    }
}
