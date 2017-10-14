using Rdp.Data.Entity;

namespace Rdp.Service
{
    public interface IGlobalResourcesService : IService<GlobalResources>
    {
       string GetValue(string name, string strLan);
    }
}
