using System;
using System.Collections.Generic;
using Kantaiko.Controllers.Design.Controllers;
using Kantaiko.Controllers.Design.Properties;
using Kantaiko.Controllers.Internal;

namespace Kantaiko.Controllers.Introspection
{
    public class ControllerInfo
    {
        internal ControllerInfo(Type type)
        {
            Type = type;
            Properties = DesignPropertyExtractor.GetProperties<IControllerDesignPropertyProvider>(type,
                x => x.GetControllerDesignProperties());
        }

        public Type Type { get; }
        public IReadOnlyList<EndpointInfo> Endpoints { get; private set; } = null!;
        public IDesignPropertyCollection Properties { get; }

        internal void SetEndpoints(IReadOnlyList<EndpointInfo> endpoints)
        {
            Endpoints = endpoints;
        }
    }
}
