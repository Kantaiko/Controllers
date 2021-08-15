using Kantaiko.Controllers.Design.Properties;

namespace Kantaiko.Controllers.Design.Endpoints
{
    public interface IEndpointDesignPropertyProvider
    {
        DesignPropertyCollection GetEndpointDesignProperties();
    }
}
