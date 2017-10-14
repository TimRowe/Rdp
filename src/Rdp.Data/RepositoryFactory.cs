using Rdp.Core.Dependency;
using Rdp.Core.Data;

namespace Rdp.Data
{
    /// <summary>
    /// Repository
    /// </summary>
    public  class RepositoryFactory
    {
        static public IRepository<T> Create<T>() where T : BaseEntity
        {
            return IocObjectManager.GetInstance().Resolve<IRepository<T>>();
        }
    }
}
