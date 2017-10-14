using Microsoft.Extensions.DependencyInjection;

namespace Rdp.Web.Framework.Runtime
{
    public class IocContainerManager
    {
        private static ServiceProvider _container;
        
        

        public static ServiceProvider GetInstance()
        {
            return _container;
        }

        public static void SetInstance(ServiceProvider container)
        {
            _container = container;
        }
    }
}
