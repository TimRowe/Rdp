using Rdp.Data.Entity;

namespace Rdp.Service
{
   public  interface IVersionControlService:IService<VersionControl>
    {
       bool GetVersionFlag(string key);
    }
}
