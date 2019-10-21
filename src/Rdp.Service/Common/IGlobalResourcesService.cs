using Rdp.Data.Entity;
using System.Collections.Generic;

namespace Rdp.Service
{
    public interface IGlobalResourcesService : IService<GlobalResources>
    {

       string GetValue(string name, string strLan);

       List<GlobalResources> GetList(string strLan);
    }
}
