using Rdp.Data.Entity;

namespace Rdp.Service
{
   public  interface IVersionControlService:IService<VersionControl>
    {
       bool GetVersionFlag(string key);

        /// <summary>
        /// 根据Key获取Version
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        int GetVersion(string key);
    }
}
