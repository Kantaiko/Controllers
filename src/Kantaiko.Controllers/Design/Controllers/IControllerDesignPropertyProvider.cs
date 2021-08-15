using Kantaiko.Controllers.Design.Properties;

namespace Kantaiko.Controllers.Design.Controllers
{
    public interface IControllerDesignPropertyProvider
    {
        DesignPropertyCollection GetControllerDesignProperties();
    }
}
